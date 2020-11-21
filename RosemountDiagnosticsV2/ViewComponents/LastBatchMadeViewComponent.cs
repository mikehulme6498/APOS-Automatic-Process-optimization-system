using BatchDataAccessLibrary.Helpers;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using RosemountDiagnosticsV2.View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RosemountDiagnosticsV2.ViewComponents
{
    public class LastBatchMadeViewComponent : ViewComponent
    {
        private readonly IBatchRepository _batchRepository;
        private readonly IMaterialDetailsRepository _materialDetailsRepository;

        public LastBatchMadeViewComponent(IBatchRepository batchRepository, IMaterialDetailsRepository materialDetailsRepository)
        {
            _batchRepository = batchRepository;
            _materialDetailsRepository = materialDetailsRepository;
        }

        public IViewComponentResult Invoke(BatchReport report = null)
        {
            BatchReport lastBatchMade = _batchRepository.AllBatches.OrderByDescending(x => x.StartTime).FirstOrDefault(); ;

            if (lastBatchMade == null)
            {
                lastBatchMade = BatchHelperMethods.CreateBlankBatchReport();
            }

            ViewData["QualityIssues"] = BatchHelperMethods.GetTotalQualityIssues(lastBatchMade.BatchIssues);
            ViewData["StoppagesTime"] = BatchHelperMethods.GetTotalStoppageTime(lastBatchMade.BatchIssues);
            ViewData["MatVarCost"] = BatchHelperMethods.GetTotalMatVarCost(lastBatchMade.BatchIssues, _materialDetailsRepository);

            return View(lastBatchMade);
        }


    }
}
