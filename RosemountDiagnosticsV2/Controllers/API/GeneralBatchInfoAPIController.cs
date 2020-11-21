using BatchDataAccessLibrary.Helpers;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RosemountDiagnosticsV2.Controllers.API
{
    [Route("batchinfo/")]
    [ApiController]
    public class GeneralBatchInfoAPIController : ControllerBase
    {
        private readonly IBatchRepository _batchRepository;
        private readonly IRecipeLimitRepository _recipeLimitRepository;

        public GeneralBatchInfoAPIController(IBatchRepository batchRepository, IRecipeLimitRepository recipeLimitRepository)
        {
            _batchRepository = batchRepository;
            _recipeLimitRepository = recipeLimitRepository;
        }

        [HttpGet]
        [Route("GetBatchesPerWeek")]
        public Dictionary<int, int> GetBatchPerWeekCount(int year)
        {
            
            if (year == 0)
            {
                return null;
            }
            Dictionary<int, int> weeklyCount = new Dictionary<int, int>();

            var batches = _batchRepository.AllBatches.Where(x => x.StartTime.Year == year).ToList();
            int currentWeek;

            if (year == DateTime.Now.Year)
            {
                currentWeek = HelperMethods.GetWeekNumber(DateTime.Now);
            }
            else
            {
                currentWeek = 52;
            }

            for (int i = 1; i <= currentWeek; i++)
            {
                weeklyCount.Add(i, 0);
            }

            foreach (var batch in batches)
            {
                try
                {
                    weeklyCount[batch.WeekNo]++;
                }
                catch
                {
                    weeklyCount.Add(batch.WeekNo, 1);
                }
            }
            var test = JsonConvert.SerializeObject(weeklyCount);
            return weeklyCount;
        }

        // CURRENTLY NOT USED - WAS NOT NEEDED BUT KEPT FOR NOW INCASE THAT CHANGES 
        //
        //[HttpGet]
        //[Route("GetRecipeLimits")]
        //public RecipeLimits GetRecipeLimits(BatchReport.RecipeTypes recipeType, RecipeLimits.LimitType limitType)
        //{
        //    return _recipeLimitRepository.GetLimitInfo(recipeType, limitType);
        //}
        
    }
}
