using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using System;
using System.Collections.Generic;

namespace BatchReports.IssueScanner
{
    public class MatVarIssueScanner : IssueScannerBase
    {
        private readonly IMaterialDetailsRepository _MaterialDetailsrepository;
        private readonly List<string> MaterialNamesIncludedInMatVar = new List<string>();
        public MatVarIssueScanner(IMaterialDetailsRepository materialDetailsRepository) : base(materialDetailsRepository)
        {
            _MaterialDetailsrepository = materialDetailsRepository;
            MaterialNamesIncludedInMatVar = _MaterialDetailsrepository.GetMaterialNamesThatAreIncludedInMatVar();
            IssueDescriptor = "MatVar Issues";
            ScanType = ScanTypes.MatVar;
        }

        public override void ScanForIssues(BatchReport report)
        {
            CheckTargetVsActualWeights(report);
            SetIssueScannedFor(report);
        }

        private void CheckTargetVsActualWeights(BatchReport report)
        {
            foreach (Material material in GetAllMaterialsUsedInBatch(report))
            {
                if (MaterialNamesIncludedInMatVar.Contains(material.Name))
                {
                    double difference = Math.Round(material.TargetWeight - material.ActualWeight, 2);
                    double percentage = Math.Round(100 - ((material.ActualWeight / material.TargetWeight) * 100), 2);

                    if (Math.Abs(percentage) >= 5)
                    {
                        BatchIssue issue = new BatchIssue()
                        {
                            MaterialName = material.Name,
                            MaterialShortName = _MaterialDetailsrepository.GetSingleMaterial(material.Name).ShortName ?? material.Name,
                            FaultType = BatchIssue.FaultTypes.Overweigh,
                            TimeLost = 0,
                            PercentOut = percentage,
                            WeightDiffference = difference,
                            Message = $"{material.Name} {GetUnderOverMessage(difference)} by {difference} kg",
                            IssueCreatedBy = IssueDescriptor
                        };
                        report.BatchIssues.Add(issue);
                    }
                }
            }
        }

        private string GetUnderOverMessage(double difference)
        {
            if (difference < 0)
            {
                return "Underweighed";
            }
            return "Overweighed";
        }

        private List<Material> GetAllMaterialsUsedInBatch(BatchReport report)
        {
            List<Material> allMaterials = new List<Material>();

            foreach (var vessel in report.AllVessels)
            {
                allMaterials.AddRange(vessel.Materials);
            }

            return allMaterials;
        }

    }
}
