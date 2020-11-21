using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;

namespace BatchReports.IssueScanner
{
    public class GapInTimesIssueScanner : IssueScannerBase
    {
        private readonly IGapInTimeReasons _gapReasons;

        public GapInTimesIssueScanner(IGapInTimeReasons gapReasons, IMaterialDetailsRepository materialDetailsRepository) : base(materialDetailsRepository)
        {
            IssueDescriptor = "Time Gap Between Material Issues";
            ScanType = ScanTypes.Issue;
            _gapReasons = gapReasons;
        }
        public override void ScanForIssues(BatchReport report)
        {
            ScanForGapsInMaterials(report);
            SetIssueScannedFor(report);
        }

        private void ScanForGapsInMaterials(BatchReport report)
        {
            Vessel mainMixer = report.AllVessels.Where(x => x.VesselType == Vessel.VesselTypes.MainMixer).FirstOrDefault();
            
            if(mainMixer != null)
            {
                List<Material> materials = mainMixer.Materials.OrderBy(x => x.StartTime).ToList();
                for (int i = 1; i < materials.Count; i++)
                {
                    double timeDifference = GetDifferenceInTime(materials[i], materials[i-1]);

                    if(timeDifference != 0 && timeDifference > GapInMaterialTimeThreshold)
                    {
                        if (!CheckIfWaitIssueAlreadyExists(report, materials[i-1].Name, materials[i].Name))
                        {
                            BatchIssue issue = new BatchIssue()
                            {
                                FaultType = BatchIssue.FaultTypes.WeighTime,
                                MaterialName = materials[i].Name + " & " + materials[i - 1].Name,
                                IssueCreatedBy = IssueDescriptor,
                                TimeLost = Math.Round(timeDifference, 2),
                                Message = GetMessageForIssue(materials[i - 1].Name, materials[i].Name) + " Time lost : " + timeDifference + " minutes."
                            };
                            report.BatchIssues.Add(issue);
                        }
                    }
                }
            }
        }

        private bool CheckIfWaitIssueAlreadyExists(BatchReport report, string material, string material2)
        {
            return report.BatchIssues.Where(x => x.FaultType == BatchIssue.FaultTypes.WaitTime).Any(x => x.MaterialName == material || x.MaterialName == material2);
            
        }

        private string GetMessageForIssue(string material1, string material2)
        {
            List<string> reasons = _gapReasons.GetReasonForGap(material1, material2);
            StringBuilder sb = new StringBuilder();
                        
            foreach (var reason in reasons)
            {                
                sb.Append(reason);
            }
            return sb.ToString();
        }

        private double GetDifferenceInTime(Material firstMaterial, Material secondMaterial)
        {
            if (GetCurrentMaterialWeighGroup(firstMaterial.Name) == 0)
            {
                TimeSpan timeDifference = firstMaterial.StartTime.AddMinutes(-firstMaterial.WeighTime).Subtract(secondMaterial.StartTime.AddMinutes(secondMaterial.WeighTime));
                return Math.Round(timeDifference.TotalMinutes,2);
            }
            return 0;
        }
    }
}
