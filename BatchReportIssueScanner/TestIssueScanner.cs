using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BatchReports.IssueScanner
{
    public class TestIssueScanner : IssueScannerBase
    {

        public TestIssueScanner(IMaterialDetailsRepository materialDetailsRepository) : base(materialDetailsRepository)
        {
            IssueDescriptor = "Test Issues";
        }
        public override void ScanForIssues(BatchReport report)
        {
            report.BatchIssues.Add(new BatchIssue
            {
                FaultType = BatchIssue.FaultTypes.Quality,
                Message = "Test Fault",
                MaterialName = "Puresse",
                TimeLost = 0,
                IssueCreatedBy = IssueDescriptor
            });
            SetIssueScannedFor(report);
        }
    }
}
