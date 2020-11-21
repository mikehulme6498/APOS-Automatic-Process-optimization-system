using BatchDataAccessLibrary.Enums;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using RosemountDiagnosticsV2.View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static BatchDataAccessLibrary.Models.BatchIssue;

namespace RosemountDiagnosticsV2.Controllers
{
    public class BatchReportsController : Controller
    {
        private readonly IBatchRepository _BatchRepository;
        private readonly IRecipeLimitRepository _recipeLimitRepository;

        public BatchReportsController(IBatchRepository batchRepository, IRecipeLimitRepository recipeLimitRepository)
        {
            _BatchRepository = batchRepository;
            _recipeLimitRepository = recipeLimitRepository;
        }
        public IActionResult Index()
        {

            return View();
        }

        public ActionResult ViewAllBatches(bool loadAll=false)
        {
            List<BatchReport> reports;

            if (loadAll)
            {
                reports = _BatchRepository.AllBatches.OrderByDescending(x => x.StartTime).ToList();
            }
            else
            {
                reports = _BatchRepository.AllBatches.OrderByDescending(x => x.StartTime).ToList();
            }
           return View(reports);
        }

        public IActionResult ViewSingleBatch(int batchId)
        {
            BatchReport report = _BatchRepository.GetBatchById(batchId);

            SingleBatchViewModel singleBatchViewModel = new SingleBatchViewModel
            {
                Report = report
            };

            singleBatchViewModel.RecipeViscoLimits = _recipeLimitRepository.GetLimitInfo(report.RecipeType, LimitType.Visco);
            singleBatchViewModel.BatchTimeLimits = _recipeLimitRepository.GetLimitInfo(report.RecipeType, LimitType.MakeTime);
            GetIssuesForViewModel(singleBatchViewModel);
            singleBatchViewModel.TotalTimeLost = singleBatchViewModel.TimeIssues.Select(x => x.TimeLost).Sum();
            singleBatchViewModel.TotalMatvarIssues = singleBatchViewModel.MatVarIssues.Count();
            singleBatchViewModel.TotalQualityIssues = singleBatchViewModel.QualityIssues.Count();

            foreach (var vessel in singleBatchViewModel.Report.AllVessels)
            {              
                vessel.Materials = vessel.Materials.OrderBy(m => m.StartTime.Date).ThenBy(x => x.StartTime.TimeOfDay).ToList();
            }
            return View(singleBatchViewModel);
        }
        private void GetIssuesForViewModel(SingleBatchViewModel singleBatchViewModel)
        {
            singleBatchViewModel.QualityIssues = singleBatchViewModel.Report.BatchIssues
                .Where(x => IsAQualityIssues(x.FaultType) && x.RemoveIssue == false)
                .ToList();

            singleBatchViewModel.TimeIssues = singleBatchViewModel.Report.BatchIssues
                .Where(x => IsATimeIssue(x.FaultType) && x.RemoveIssue == false)
                .OrderByDescending(x => x.TimeLost)
                .ToList();

            singleBatchViewModel.MatVarIssues = singleBatchViewModel.Report.BatchIssues
                .Where(x => IsAMatVarIssue(x.FaultType) && x.RemoveIssue == false)
                .OrderByDescending(x => x.PercentOut)
                .ToList();
        }
        private bool IsATimeIssue(FaultTypes faultType)
        {
            return (faultType == FaultTypes.AcquireTime || faultType == FaultTypes.WaitTime || faultType == FaultTypes.WeighTime);
        }

        private bool IsAQualityIssues(FaultTypes faultType) 
        {
            return (faultType == FaultTypes.Quality || faultType == FaultTypes.TemperatureHigh || faultType == FaultTypes.TemperatureLow);
        }

        private bool IsAMatVarIssue(FaultTypes faultType)
        {
            return (faultType == FaultTypes.Underweigh || faultType == FaultTypes.Overweigh);
        }
        public IActionResult ViewSingleBatchByNumber(string batchNum, int year)
        {

            SingleBatchViewModel singleBatchViewModel = new SingleBatchViewModel
            {
                Report = _BatchRepository.GetBatchByBatchNumber(batchNum, year)
            };

            singleBatchViewModel.RecipeViscoLimits = _recipeLimitRepository.GetLimitInfo(singleBatchViewModel.Report.RecipeType, LimitType.Visco);
            singleBatchViewModel.BatchTimeLimits = _recipeLimitRepository.GetLimitInfo(singleBatchViewModel.Report.RecipeType, LimitType.MakeTime);

            foreach (var vessel in singleBatchViewModel.Report.AllVessels)
            {
                foreach (var material in vessel.Materials)
                {
                    vessel.Materials = vessel.Materials.OrderBy(m => m.StartTime).ToList();
                }
            }

            GetIssuesForViewModel(singleBatchViewModel);
            return View("ViewSingleBatch", singleBatchViewModel);
        }
    }
}