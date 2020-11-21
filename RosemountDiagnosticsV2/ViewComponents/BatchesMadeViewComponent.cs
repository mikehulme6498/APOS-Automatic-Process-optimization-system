using BatchDataAccessLibrary.Enums;
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
    public class BatchesMadeViewComponent : ViewComponent
    {
        private readonly IBatchRepository _batchRepository;

        public BatchesMadeViewComponent(IBatchRepository batchRepository)
        {
            _batchRepository = batchRepository;
        }

        public IViewComponentResult Invoke()
        {
            BatchesMadePerWeek batchesMadePerWeek = new BatchesMadePerWeek();
            var batches = _batchRepository.AllBatches.Where(x => x.StartTime.Year == DateTime.Now.Year).GroupBy(b => b.WeekNo)
                            .Select(x => new BatchesMadePerWeek()
                            {
                                WeekNo = x.Key,
                                BatchesMade = x.Count(),
                                ConcBatchesCount = x.Count(t => t.RecipeType == RecipeTypes.Conc),
                                BigBangBatchesCount = x.Count(t => t.RecipeType == RecipeTypes.BigBang),
                                RegBatchesCount = x.Count(t => t.RecipeType == RecipeTypes.Reg),
                            })
                            .OrderBy(x => x.WeekNo)
                            .ToList();

            for(int i=1; i<=HelperMethods.GetWeekNumber(DateTime.Now); i++)
            {
                if(!batches.Any(x => x.WeekNo == i))
                {
                    batches.Add(new BatchesMadePerWeek { WeekNo = i, BatchesMade = 0, BigBangBatchesCount = 0, ConcBatchesCount = 0, RegBatchesCount=0 });
                }
            }

            return View(batches.OrderBy(x => x.WeekNo));
        }
    }
}
