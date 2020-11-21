using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static BatchDataAccessLibrary.Helpers.BatchHelperMethods;

namespace BatchReports.IssueScanner
{
    class WeighTimeIssueScanner : IssueScannerBase
    {
        private readonly IMaterialDetailsRepository _materialDetailsRepository;

        public WeighTimeIssueScanner(IMaterialDetailsRepository materialDetailsRepository) : base(materialDetailsRepository)
        {
            IssueDescriptor = "Weigh Time Issues";
            ScanType = ScanTypes.Issue;
            _materialDetailsRepository = materialDetailsRepository;
        }
        public override void ScanForIssues(BatchReport report)
        {
            CheckWeighTimeOfMaterials(report);
            RemoveIssuesWithTimeLossLessThanThreshold(report);
            CheckIfPefumeWeighIssueNeedsRemoving(report);
            SetIssueScannedFor(report);
        }

        private void CheckIfPefumeWeighIssueNeedsRemoving(BatchReport report)
        {
            Material perfume = GetSingleMaterialFromVessel(report, Vessel.VesselTypes.PerfumePreWeigher, 0);

            BatchIssue perfumeIssue = report.BatchIssues
                    .Where(x => x.MaterialName == perfume.Name && x.FaultType == BatchIssue.FaultTypes.WeighTime)
                    .FirstOrDefault();

            if (perfumeIssue != null)
            {
                DateTime batchReadyForPerfumeTime = GetTimeBatchWasReadyForPreweigher(report, Vessel.VesselTypes.PerfumePreWeigher);

                if (perfume.StartTime.AddMinutes(perfume.WeighTime) < batchReadyForPerfumeTime)
                {
                    perfumeIssue.RemoveIssue = true;
                    perfumeIssue.IssueRemovedBy = IssueDescriptor;
                    perfumeIssue.ReasonRemoved = $"Although {perfume.Name} took {perfume.WeighTime} minutes" +
                                                 $"to weigh the batch did require the perfume until after that. " +
                                                 $"So no time was lost.";
                }
            }
        }

        private void CheckActivePrewigherWeighTimes(BatchReport report, Material material)
        {
            Material activeMaterial = GetSingleMaterialFromVessel(report, Vessel.VesselTypes.ActivePreWeigher, material.Name);

            DateTime batchReadyForPerfumeTime = GetTimeBatchWasReadyForPreweigher(report, Vessel.VesselTypes.ActivePreWeigher);

            if (activeMaterial.StartTime.AddMinutes(activeMaterial.WeighTime) > batchReadyForPerfumeTime)
            {
                double timeLost = activeMaterial.StartTime.AddMinutes(activeMaterial.WeighTime).Subtract(batchReadyForPerfumeTime).TotalMinutes;

                BatchIssue issue = new BatchIssue()
                {
                    MaterialName = activeMaterial.Name,
                    MaterialShortName = _materialDetailsRepository.GetSingleMaterial(material.Name).ShortName ?? material.Name,
                    IssueCreatedBy = IssueDescriptor,
                    FaultType = BatchIssue.FaultTypes.WeighTime,
                    Message = $"{activeMaterial.Name} took {activeMaterial.WeighTime} minute to weigh but the mixer " +
                            $"did not need it until {batchReadyForPerfumeTime.ToShortTimeString() } so only {timeLost} " +
                            $"minutes where lost.",
                    TimeLost = timeLost
                };
            }
        }

        private void CheckWeighTimeOfMaterials(BatchReport report)
        {
            foreach (var vessel in report.AllVessels)
            {
                foreach (var material in vessel.Materials)
                {
                    if (vessel.VesselType == Vessel.VesselTypes.MainMixer)
                    {
                        CheckMainMixerWeighTimes(report, material);
                    }
                    if (vessel.VesselType == Vessel.VesselTypes.ActivePreWeigher)
                    {
                        CheckActivePrewigherWeighTimes(report, material);
                    }
                }
            }
        }

        private void CheckMainMixerWeighTimes(BatchReport report, Material material)
        {
            var materialInfo = materialDetails.Find(x => x.Name.ToLower().Replace(" ", "") == material.Name.ToLower().Replace(" ", ""));
            BatchIssue weighIssue = null;

            if (materialInfo != null)
            {
                if (materialInfo.ParallelWeighGroup >= 1)
                {
                    weighIssue = CheckWeighTimeAgainstLastMaterialInParallelWeighGroup(report, material, materialInfo);
                }
                else
                {
                    weighIssue = CheckWeighTimeAgainstAverageTime(report, material, materialInfo);
                }

            }
            if (weighIssue != null)
            {
                report.BatchIssues.Add(weighIssue);
            }
        }


        private BatchIssue CheckWeighTimeAgainstAverageTime(BatchReport report, Material material, MaterialDetails materialInfo)
        {
            BatchIssue issue = null;

            if (material.WeighTime > materialInfo.AvgWeighTime)
            {
                double timeLost = GetTimeLost(report, material, materialInfo.AvgWeighTime);


                issue = new BatchIssue()
                {
                    FaultType = BatchIssue.FaultTypes.WeighTime,
                    MaterialShortName = _materialDetailsRepository.GetSingleMaterial(material.Name).ShortName ?? material.Name,
                    MaterialName = material.Name,
                    TimeLost = timeLost,
                    WeightDiffference = 0,
                    Message = $"{material.Name} took {material.WeighTime} minutes to weigh. " +
                          $"The average weigh time is {materialInfo.AvgWeighTime} minutes giving " +
                          $"a loss of {timeLost} minutes",
                    IssueCreatedBy = IssueDescriptor
                };
            }
            return issue;
        }

        private double GetTimeLost(BatchReport report, Material material, double avgWeighTime)
        {
            int currentMaterialGroup = GetCurrentMaterialWeighGroup(material.Name);
            if(report.Campaign == 737)
            {
                Console.WriteLine();
            }
            if (currentMaterialGroup != 0)
            {
                Material lastMaterialInGroup = GetLastMaterialFromGroupInBatch(report, currentMaterialGroup);
                if (lastMaterialInGroup.Name == material.Name) { return material.WeighTime - avgWeighTime; }


                if (material.StartTime.AddMinutes(material.WeighTime) > lastMaterialInGroup.StartTime)
                {
                    TimeSpan span = material.StartTime.AddMinutes(material.WeighTime).Subtract(lastMaterialInGroup.StartTime);
                    return Math.Round(span.TotalMinutes, 2);
                }
                else
                {
                    return 0;
                }
            }
            return Math.Round(material.WeighTime - avgWeighTime, 2);
        }

        private Material GetLastMaterialFromGroupInBatch(BatchReport report, int currentMaterialGroup)
        {
            List<string> materialsInGroup = GetListOfMaterialsInGroup(currentMaterialGroup);
            Vessel mainMixer = report.AllVessels.Where(x => x.VesselType == Vessel.VesselTypes.MainMixer).FirstOrDefault();

            for (int i = materialsInGroup.Count - 1; i > 0; i--)
            {
                if (mainMixer.Materials.Any(x => x.Name == materialsInGroup[i]))
                {
                    return mainMixer.Materials.Where(x => x.Name == materialsInGroup[i]).First();
                }
            }
            return null;
        }

        private void RemoveIssuesWithTimeLossLessThanThreshold(BatchReport report)
        {
            foreach (var issue in report.BatchIssues)
            {
                if (issue.IssueCreatedBy == IssueDescriptor && issue.TimeLost < WeighTimeLossThreshold)
                {
                    issue.RemoveIssue = true;
                    issue.ReasonRemoved = $"Time lost weighing {issue.MaterialName} is below the threshold of " + WeighTimeLossThreshold + " Minutes";
                    issue.IssueRemovedBy = IssueDescriptor;
                }
            }
        }

        private BatchIssue CheckWeighTimeAgainstLastMaterialInParallelWeighGroup(BatchReport report, Material material, MaterialDetails materialInfo)
        {
            BatchIssue issue = null;

            Material lastMaterialInGroup = GetSingleMaterialFromVessel(report, GetLastMaterialNameFromGroup(materialInfo.ParallelWeighGroup));
            double timeLost = GetTimeLost(report, material, materialInfo.AvgWeighTime);
            
            if (timeLost > WeighTimeLossThreshold && lastMaterialInGroup != null)
            {
                issue = new BatchIssue()
                {
                    FaultType = BatchIssue.FaultTypes.WeighTime,
                    MaterialName = material.Name,
                    MaterialShortName = _materialDetailsRepository.GetSingleMaterial(material.Name).ShortName ?? material.Name,
                    TimeLost = timeLost,
                    WeightDiffference = 0,
                    Message = $"{material.Name} took {material.WeighTime} minutes to weigh. " +
                          $"However it weighs in parallel with other materials. The last material " +
                          $"to weigh in the group is {lastMaterialInGroup.Name} which did not weigh " +
                          $"until {lastMaterialInGroup.StartTime.ToShortTimeString()} meaning the time lost was {timeLost} minutes.",
                    IssueCreatedBy = IssueDescriptor
                };
            }
            return issue;

        }

        private DateTime GetTimeBatchWasReadyForPreweigher(BatchReport report, Vessel.VesselTypes vesselType)
        {

            Vessel preweigherVessel = report.AllVessels.Where(x => x.VesselType == vesselType).FirstOrDefault();
            Vessel mainMixerVessel = report.AllVessels.Where(x => x.VesselType == Vessel.VesselTypes.MainMixer).FirstOrDefault();

            if (preweigherVessel != null && mainMixerVessel != null)
            {
                for (int i = 0; i < mainMixerVessel.Materials.Count; i++)
                {
                    if (mainMixerVessel.Materials[i].Name == "Empty " + preweigherVessel.VesselName)
                    {
                        return mainMixerVessel.Materials[i - 1].StartTime.AddMinutes(5);
                    }
                }
            }
            return new DateTime(2000, 1, 1);
        }



    }
}
