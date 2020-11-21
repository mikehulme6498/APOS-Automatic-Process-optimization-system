using System;
using System.Collections.Generic;
using System.Text;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;

namespace BatchReports.IssueScanner
{
    public class WaitTimeIssueScanner : IssueScannerBase
    {
        const double waitTimeLimit = 2;
        private readonly IMaterialDetailsRepository _materialDetailsRepository;

        public WaitTimeIssueScanner(IMaterialDetailsRepository materialDetailsRepository) : base(materialDetailsRepository)
        {
            IssueDescriptor = "Wait Time Issues";
            ScanType = ScanTypes.Issue;
            _materialDetailsRepository = materialDetailsRepository;
        }

        public override void ScanForIssues(BatchReport report)
        {
            CheckWaitTimes(report);
            SetIssueScannedFor(report);
        }

        private void CheckWaitTimes(BatchReport report)
        {
            foreach (var vessel in report.AllVessels)
            {
                foreach (var material in vessel.Materials)
                {
                    if (material.WaitTime > waitTimeLimit)
                    {
                        BatchIssue issue = new BatchIssue
                        {
                            FaultType = BatchIssue.FaultTypes.WaitTime,
                            MaterialName = material.Name,
                            MaterialShortName = _materialDetailsRepository.GetSingleMaterial(material.Name).ShortName ?? material.Name,
                            TimeLost = Math.Round(material.WaitTime, 2),
                            WeightDiffference = 0,
                            Message = $"The batch had to wait { material.WaitTime } minutes for { material.Name }. " +
                                      $"This is usually because it is being used by another stream.",
                            IssueCreatedBy = IssueDescriptor
                        };
                        report.BatchIssues.Add(issue);
                    }

                }
            }
        }
    }
}
