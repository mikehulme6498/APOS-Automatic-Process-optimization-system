using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models.ShiftLog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RosemountDiagnosticsV2.View_Models;
using RosemountDiagnosticsV2.View_Models.OperatorLog;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using BatchDataAccessLibrary.Models;

namespace RosemountDiagnosticsV2.Controllers
{
    public class OperatorLogController : Controller
    {
        private readonly IMaterialDetailsRepository _materialDetailsRepository;
        private readonly IBatchRepository _batchRepository;
        private readonly IShiftLogRepository _shiftLogRepository;

        public OperatorLogController(IMaterialDetailsRepository maaterialDetailsRepository, IBatchRepository batchRepository, IShiftLogRepository shiftLogRepository)
        {
            _materialDetailsRepository = maaterialDetailsRepository;
            _batchRepository = batchRepository;
            _shiftLogRepository = shiftLogRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult AddOperators(int shiftId, string shiftColour)
        {
            _shiftLogRepository.AddShiftColourOperatorsToShiftLog(shiftId, shiftColour);
            return Json(shiftId);
        }

        [HttpGet]
        public IActionResult ViewPreviousShifts()
        {
            Dictionary<OperatorShiftLog, int> BatchesWithBatchCount = new Dictionary<OperatorShiftLog, int>();
            var shiftLogs = _shiftLogRepository.GetAllShifts();
            foreach (var shift in shiftLogs)
            {
                BatchesWithBatchCount.Add(shift, _shiftLogRepository.GetBatchesForShift(shift.OperatorShiftLogId).Count());
            }
            return View(BatchesWithBatchCount);
        }

        public IActionResult AddAllBatchesToShiftLog()
        {
            var batches = _batchRepository.AllBatches.Where(x => x.StartTime.Month == 10 && x.StartTime.Year==2020 && x.StartTime.Day > 16).ToList();
            _shiftLogRepository.AddBatchesToShiftLog(batches);
            return RedirectToAction("Index", "Settings");
        }

        public IActionResult RemoveAllShifts()
        {
            
            
            return RedirectToAction("Index", "Settings");
        }

        [HttpGet, HttpPost]
        public IActionResult ViewCurrentShift(int shiftId, string shift)
        {
            OperatorLogViewModel operatorLogViewModel = new OperatorLogViewModel();
            OperatorShiftLog shiftLog;
            if (shift == "current")
            {
                 shiftLog = _shiftLogRepository.GetOrCreateShiftLog(DateTime.Now);
            }
            else
            {
                shiftLog = _shiftLogRepository.GetShiftLogById(shiftId);
            }

            operatorLogViewModel.CurrentShift = shiftLog;
            operatorLogViewModel.NeedOperators = shiftLog.Operators == null || shiftLog.Operators == "";
            operatorLogViewModel.BatchReports = GetBatchesForShift(shiftLog.OperatorShiftLogId);
            operatorLogViewModel.ToteChanges = _shiftLogRepository.GetToteChanges(shiftLog.OperatorShiftLogId);
            operatorLogViewModel.TotalReworkToteChanges = _shiftLogRepository.GetTotalReworkCount(shiftLog.OperatorShiftLogId);
           

            ViewBag.Totes = _materialDetailsRepository.GetListOfActiveMaterialsForDropDown();
            ViewBag.Recipes = _batchRepository.GetRecipeNamesForDropDown();
            ViewBag.ShiftColours = _shiftLogRepository.GetShiftColoursForDropDown();

            return View(operatorLogViewModel);
        }

        private List<BatchWithCIPViewModel> GetBatchesForShift(int shiftId)
        {
            List<BatchWithCIPViewModel> output = new List<BatchWithCIPViewModel>();

            foreach (var report in _shiftLogRepository.GetBatchesForShift(shiftId))
            {
                output.Add(new BatchWithCIPViewModel
                {
                    Report = _batchRepository.GetBatchById(report.BatchId),
                    StockTankCip = report.StockTankWash,
                    StreamCip = report.StreamWash
                });
            }
            return output
                .OrderByDescending(x => x.Report.StartTime.Date)
                .ThenByDescending(x => x.Report.StartTime.TimeOfDay)
                .ToList();
        }

       

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddReworkTote(int toteCount, string toteType, int shiftId)
        {
            int currentToteCount = 0;
            int totesadded = 0;
            string addRemove = "";

            switch (toteType)
            {
                case "#normal-tote-count":
                    currentToteCount = _shiftLogRepository.GetReworkToteCount(shiftId);
                    totesadded = toteCount - currentToteCount;
                    addRemove = toteCount > currentToteCount ? "added" : "removed";
                    _shiftLogRepository.UpdateReworkTotes(shiftId, toteCount);
                    break;
                case "#bb-tote-count":
                    currentToteCount = _shiftLogRepository.GetBBReworkToteCount(shiftId);
                    totesadded = toteCount - currentToteCount;
                    addRemove = toteCount > currentToteCount ? "added" : "removed";
                    _shiftLogRepository.UpdateBigBangReworkTotes(shiftId, toteCount);
                    break;
                default:
                    break;

            }

            var totalToteCount = _shiftLogRepository.GetTotalReworkCount(shiftId);
            return Json(new { totesAdded = Math.Abs(totesadded), addOrRemove = addRemove, totalRework = totalToteCount });
        }

        public IActionResult AddOperatorToShift(int shiftId, string name)
        {
            _shiftLogRepository.AddOperatorToShiftLog(shiftId, name);
            return Json(new { operators = _shiftLogRepository.GetOperators(shiftId) });
        }

        public IActionResult RemoveOperatorFromShift(int shiftId, string name)
        {
            _shiftLogRepository.RemoveOperatorFromShiftLog(shiftId, name);
            return Json(new { operators = _shiftLogRepository.GetOperators(shiftId) });
        }

        public IActionResult RemoveToteFromShift(int shiftId, string toteName)
        {
            _shiftLogRepository.RemoveToteFromShift(shiftId, toteName);

            return Json(new { toteRemoved = toteName, toteCount = _shiftLogRepository.GetToteChangesCount(shiftId) });
        }

        public IActionResult EditEffluent(int shiftId, double effluent)
        {
            _shiftLogRepository.AddEffluentToShiftLog(shiftId, effluent);
            return Json(new { effluentLevel = effluent });
        }

        public IActionResult UpdateCip(int shiftId, int batchId, bool washed, string vesselType, string vesselName)
        {
            _shiftLogRepository.UpdateBatchesForShift(shiftId, batchId, washed, vesselType, vesselName);         
            return Json("Ok");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult AddToteToShift(int shiftId, string toteName)
        {
            _shiftLogRepository.AddToteChange(shiftId, toteName);
            var toteChanges = _shiftLogRepository.GetToteChanges(shiftId);
            return Json(new { totes = toteChanges, toteCount = toteChanges.Count() });
        }

        public IActionResult ListOperators(int shiftid)
        {
            return ViewComponent("ListOperators", new { shiftId = shiftid });
        }

        public IActionResult ListToteChanges(int shiftid)
        {
            return ViewComponent("ListToteChanges", new { shiftId = shiftid });
        }

        [HttpPost]
        public IActionResult AddGoodStockToWaste(OperatorLogViewModel model)
        {

            _shiftLogRepository.AddGoodStockToWaste(model.GoodStockToWaste);
            return Json(new { recipe = model.GoodStockToWaste.RecipeName, amount = model.GoodStockToWaste.Amount });
            //return RedirectToAction("ViewCurrentShift", new { shiftId = model.CurrentShift.OperatorShiftLogId });
        }
    }
}