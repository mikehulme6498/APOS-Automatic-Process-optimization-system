using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using System;
using System.Collections.Generic;

namespace BatchReports.IssueScanner
{
    public class IssueScannerManager
    {
        private readonly List<IIssueScanner> issueScanners = new List<IIssueScanner>();
        private readonly IGapInTimeReasons _gapInTimeReasons;
        private readonly IMaterialDetailsRepository _materialDetailsRepository;

        public IssueScannerManager(IGapInTimeReasons gapInTimeReasons, IMaterialDetailsRepository materialDetailsRepository)
        {
            _gapInTimeReasons = gapInTimeReasons;
            _materialDetailsRepository = materialDetailsRepository;
            RegisterIssueScanners();
        }

        private void RegisterIssueScanners()
        {
            issueScanners.Add(new AdjustmentsFirstIssueScanner(_materialDetailsRepository));
            issueScanners.Add(new MatVarIssueScanner(_materialDetailsRepository));
            issueScanners.Add(new WaitTimeIssueScanner(_materialDetailsRepository));
            issueScanners.Add(new WeighTimeIssueScanner(_materialDetailsRepository));            
            //issueScanners.Add(new GapInTimesIssueScanner(_gapInTimeReasons, _materialDetailsRepository));
            issueScanners.Add(new QualityIssueScanner(_materialDetailsRepository));
        }

        public void ScanForIssues(BatchReport report)
        {
            foreach (var scanner in issueScanners)
            {
                if(report.BatchIssues.Exists(x => x.IssueCreatedBy == scanner.Descriptor()))
                {
                    continue;
                }
                switch (scanner.GetScanType())
                {
                    case IssueScannerBase.ScanTypes.Quality:                   
                   
                    case IssueScannerBase.ScanTypes.Adjustment:
                        
                    case IssueScannerBase.ScanTypes.MatVar:
                        scanner.ScanForIssues(report);
                        break;
                        
                    case IssueScannerBase.ScanTypes.Issue:
                        if (report.MakingTime > 65 && report.NewMakeTime > 65)
                        {
                            if (!report.IssuesScannedFor.Exists((x) => x.IssueClassName == scanner.GetType().Name))
                            {
                                scanner.ScanForIssues(report);
                                SetNewMakeTime(report);
                            }
                        }
                        break;
                }
                
            }
        }

        private void SetNewMakeTime(BatchReport report)
        {
            double timeLost = 0;
            foreach (var issue in report.BatchIssues)
            {
                if (!issue.RemoveIssue)
                {
                    timeLost += issue.TimeLost;
                }
            }

            report.NewMakeTime = Math.Round(report.MakingTime - timeLost,2);
        }
    }
}
