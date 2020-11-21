using BatchDataAccessLibrary.FileReader;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using BatchReports.IssueScanner;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace RosemountDiagnosticsV2.Controllers
{
    public class UploadDataController : Controller
    {
        private readonly IBatchRepository _batchRepository;
        private readonly IGapInTimeReasons _gapInTimeReasons;
        private readonly IMaterialDetailsRepository _materialDetailsRepository;
        private readonly IShiftLogRepository _shiftLogRepository;
        private readonly IBatchDataFileManager _batchDataFileManager;
        private readonly IssueScannerManager issueScannerManager;

        public UploadDataController(IBatchRepository batchRepository, IGapInTimeReasons gapInTimeReasons, IMaterialDetailsRepository materialDetailsRepository, IShiftLogRepository shiftLogRepository, IBatchDataFileManager batchDataFileManager)
        {
            _batchRepository = batchRepository;
            _gapInTimeReasons = gapInTimeReasons;
            _materialDetailsRepository = materialDetailsRepository;
            _shiftLogRepository = shiftLogRepository;
            _batchDataFileManager = batchDataFileManager;
            issueScannerManager = new IssueScannerManager(_gapInTimeReasons, _materialDetailsRepository);
        }
        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> UploadBatches(string textfromAllfiles)
        {
            List<BatchReport> batchReports = _batchDataFileManager.ProcessStringIntoBatchReports(textfromAllfiles);
            int goodBatches = 0;
            int badBatches = 0;

            await CheckForbatchesThatAlreadyExist(batchReports);

            foreach (var report in batchReports)
            {
                if (report.IsValidBatch)
                {
                    goodBatches++;
                    issueScannerManager.ScanForIssues(report);
                    _batchRepository.Add(report);
                }
                else
                {
                    badBatches++;
                    _batchRepository.AddConversionFaults(report.ConversionFaults);
                }
            }
            _batchRepository.SaveChanges();
            _shiftLogRepository.AddBatchesToShiftLog(batchReports);

            ViewData["goodBatches"] = goodBatches;
            ViewData["badBatches"] = badBatches;
            ViewData["totalBatches"] = batchReports.Count;

            return View("ViewResults", batchReports);
        }

        private async Task CheckForbatchesThatAlreadyExist(List<BatchReport> reports)
        {
            foreach (var report in reports)
            {
                bool batchDoesExist = await _batchRepository.BatchExists(report.Campaign, report.BatchNo, report.StartTime);

                if (batchDoesExist)
                {
                    report.ConversionFaults.Clear();
                    report.IsValidBatch = false;
                    report.ConversionFaults.Add(new BatchConversionFault
                    {
                        Message = "Batch is already in the system",
                        Campaign = report.Campaign + " - " + report.BatchNo,
                        Date = DateTime.Now,
                        ExceptionMessage = null
                    });
                }
            }
        }


    }
}