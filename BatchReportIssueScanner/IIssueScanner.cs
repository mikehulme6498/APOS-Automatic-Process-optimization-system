using BatchDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static BatchReports.IssueScanner.IssueScannerBase;

namespace BatchReports.IssueScanner
{
    interface IIssueScanner
    {
       void ScanForIssues(BatchReport report);
       ScanTypes GetScanType();

       string Descriptor();
    }
}
