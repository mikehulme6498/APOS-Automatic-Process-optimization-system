using BatchDataAccessLibrary.Enums;
using BatchDataAccessLibrary.Helpers;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using BatchReports.IssueScanner;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RosemountDiagnosticsV2.Hubs;
using RosemountDiagnosticsV2.View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RosemountDiagnosticsV2.Controllers.API
{
    [Route("v1/")]
    [ApiController]
    public class BatchUploadAPIController : ControllerBase
    {
        private readonly IApiBatchRepository _apiBatchRepository;
        private readonly IHubContext<BatchCompletedHub> _hubContext;
        private readonly IGapInTimeReasons _gapInTimeReasons;
        private readonly IMaterialDetailsRepository _materialDetailsRepository;
        private readonly IssueScannerManager issueScannerManager;

        public BatchUploadAPIController(IApiBatchRepository apiBatchRepository, IHubContext<BatchCompletedHub> hubcontext, IGapInTimeReasons gapInTimeReasons, IMaterialDetailsRepository materialDetailsRepository)
        {
            _apiBatchRepository = apiBatchRepository;
            _hubContext = hubcontext;
            _gapInTimeReasons = gapInTimeReasons;
            _materialDetailsRepository = materialDetailsRepository;
            issueScannerManager = new IssueScannerManager(_gapInTimeReasons, _materialDetailsRepository);
        }

        [HttpPost]
        [Route("AddToDb")]
        public ActionResult<BatchReport> AddBatchToDatabase([FromBody] BatchReport batch)
        {
            var request = Request;
            var headers = request.Headers;

            var isSync = Convert.ToBoolean(headers["Sync"]);

            if (batch == null)
            {
                return NotFound();
            }

            if (headers.ContainsKey("Authorization"))
            {
                // The authorization key will be the computers MAC address when software goes live
                // it was set to this for testing purposes

                if (headers["Authorization"] == "Auth 123456")
                {
                    if (batch.IsValidBatch)
                    {
                        issueScannerManager.ScanForIssues(batch);
                        var batchReport = _apiBatchRepository.AddBatchToDb(batch);

                        if (!isSync)
                        {
                            var batchJson = JsonConvert.SerializeObject(batchReport);
                            batchJson = AddAdditionalInfoToJson(batchJson, batch, isSync);
                            var batchesMadeJson = JsonConvert.SerializeObject(BatchesMadePerWeekByCategory());
                            _hubContext.Clients.All.SendAsync("LastBatch", batchJson);
                            _hubContext.Clients.All.SendAsync("BatchesMadeByCategory", batchesMadeJson);
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }

                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return Unauthorized();
            }


            return Ok();
        }

        private string AddAdditionalInfoToJson(string batchJson, BatchReport report, bool sync)
        {
            batchJson = batchJson.Substring(0, batchJson.Length - 1);

            StringBuilder sb = new StringBuilder();
            var qualityIssues = BatchHelperMethods.GetTotalQualityIssues(report.BatchIssues);
            var stoppageTime = BatchHelperMethods.GetTotalStoppageTime(report.BatchIssues);
            var matVarCost = BatchHelperMethods.GetTotalMatVarCost(report.BatchIssues, _materialDetailsRepository);
            sb.Append(@"," + Convert.ToChar(34) + "QualityIssues" + Convert.ToChar(34) + ":" + Convert.ToChar(34) + qualityIssues + Convert.ToChar(34));
            sb.Append(@"," + Convert.ToChar(34) + "Stoppages" + Convert.ToChar(34) + ":" + Convert.ToChar(34) + stoppageTime + Convert.ToChar(34));
            sb.Append(@"," + Convert.ToChar(34) + "MatVarCost" + Convert.ToChar(34) + ":" + Convert.ToChar(34) + matVarCost + Convert.ToChar(34));
            sb.Append(@"," + Convert.ToChar(34) + "Sync" + Convert.ToChar(34) + ":" + Convert.ToChar(34) + sync + Convert.ToChar(34) + "}");
            batchJson += sb.ToString();
            return batchJson;
        }

        [HttpGet]
        [Route("GetListOfBatchNumbers")]
        public ActionResult<List<string>> GetListOfBatchNumbers()
        {
            return _apiBatchRepository.GetBatchInfoListForSync();
        }

        private BatchesMadePerWeek BatchesMadePerWeekByCategory()
        {
            //BatchesMadePerWeek batchesMadePerWeek = new BatchesMadePerWeek();
            var batchesMadePerWeek = _apiBatchRepository.AllBatches().Where(x => x.StartTime.Year == DateTime.Now.Year).GroupBy(b => b.WeekNo)
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
            return batchesMadePerWeek.Where(x => x.WeekNo == HelperMethods.GetWeekNumber(DateTime.Now)).FirstOrDefault();
        }
    }
}
