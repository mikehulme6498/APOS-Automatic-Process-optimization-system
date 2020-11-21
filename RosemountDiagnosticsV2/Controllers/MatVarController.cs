using BatchDataAccessLibrary.Helpers;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using RosemountDiagnosticsV2.Helper_Methods;
using RosemountDiagnosticsV2.Interfaces;
using RosemountDiagnosticsV2.Models;
using RosemountDiagnosticsV2.View_Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RosemountDiagnosticsV2.Controllers
{
    public class MatVarController : Controller
    {
        private readonly IBatchRepository _BatchRepository;
        private readonly IMaterialDetailsRepository _materialDetailsRepository;
        private readonly GeneralHelperMethods helper;

        public MatVarController(IBatchRepository batchRepository, IMaterialDetailsRepository materialDetailsRepository)
        {
            _BatchRepository = batchRepository;
            _materialDetailsRepository = materialDetailsRepository;
            helper = new GeneralHelperMethods(_BatchRepository);
        }
        public IActionResult Index()
        {
            return View();

        }
        [HttpGet, HttpPost]
        public IActionResult Overview(DateSelectorModal dateSelectorModal)
        {
            MatVarOverviewViewModel overview = new MatVarOverviewViewModel();
            if (!dateSelectorModal.TimeSet)
            {
                overview.DateSelectorModal.TimeFrame = "year";
                overview.DateSelectorModal.Week = HelperMethods.GetWeekNumber(DateTime.Now);
                overview.DateSelectorModal.Year = DateTime.Now.Year;
                
            }
            else
            {
                overview.DateSelectorModal = dateSelectorModal;
            }
            GetGainsAndLosses(overview);
            return View(overview);
        }


        private MatVarOverviewViewModel GetGainsAndLosses(MatVarOverviewViewModel overview)
        {
           
            List<BatchReport> reports = helper.GetBatchReportsForDateSelector(overview.DateSelectorModal);
            List<MaterialDetails> allMaterialDetails = _materialDetailsRepository.GetAllMaterialDetails();
            overview.UnsortedTotals = GetUnsortedTotals(reports, allMaterialDetails);
            SortTotals(overview);
            overview.CombinedTotal = overview.TotalGainForPeriod + overview.TotalLossForPeriod;
            return overview;
        }

        private void SortTotals(MatVarOverviewViewModel overview)
        {
            foreach (var total in overview.UnsortedTotals)
            {
                total.TotalKg = Math.Round(total.TotalKg, 2);
                //if(total.Total > -1 && total.Total < 1 ) { continue; }
                total.Cost = Math.Round(total.TotalKg * total.CostPerTon / 1000, 2);

                if (total.Cost > 1)
                {
                    overview.Gains.Add(total);
                    overview.TotalGainForPeriod += total.Cost;
                }
                else if (total.Cost < -1)
                {                    
                    overview.Losses.Add(total);
                    overview.TotalLossForPeriod += total.Cost;
                }
            }
            overview.Gains = overview.Gains.OrderByDescending(x => x.Cost).ToList();
            overview.Losses = overview.Losses.OrderBy(x => x.Cost).ToList();
        }
        private List<MaterialTotals> GetUnsortedTotals(List<BatchReport> reports, List<MaterialDetails> allMaterialDetails)
        {

            List<MaterialTotals> totals = new List<MaterialTotals>();
            var materialsNamesIncludedInMatVar = _materialDetailsRepository.GetMaterialNamesThatAreIncludedInMatVar();
            
            foreach (var report in reports)
            {
                foreach (var vessel in report.AllVessels)
                {
                    foreach (var material in vessel.Materials)
                    {
                        if (materialsNamesIncludedInMatVar.Contains(material.Name))
                        {
                            var tempMaterialDetail = allMaterialDetails.Where(x => x.Name == material.Name).FirstOrDefault();
                            double totalKg = material.TargetWeight - material.ActualWeight;

                            if (material.Name.Contains("DYE"))
                            {
                                totalKg = helper.CalculateDyeAmountInSolution(material.Name, material.TargetWeight - material.ActualWeight);
                            }

                            if (totals.Any(x => x.Name == material.Name))
                            {
                                totals.Find(x => x.Name == material.Name).TotalKg += totalKg;
                            }
                            else
                            {
                                totals.Add(new MaterialTotals
                                {
                                    Name = material.Name,
                                    ShortName = ShortNameTo13Charactors(tempMaterialDetail.ShortName),
                                    ProductCode = tempMaterialDetail.ProductCode,
                                    TotalKg = Math.Round(totalKg, 2),
                                    Cost = Math.Round(_materialDetailsRepository.GetCostMaterialLoss(material.Name, totalKg), 2),
                                    CostPerTon = tempMaterialDetail.CostPerTon
                                });
                            }
                        }
                    }
                }
            }
            return totals;
        }

        private string ShortNameTo13Charactors(string name)
        {
            int length = name.Length;
            if (length <= 13)
            {
                return name;
            }
            else
            {
                return name.Substring(0, 13);
            }
        }
       [HttpGet]
        public IActionResult MaterialUsage()
        {
            
            MaterialUsageViewModel materials = new MaterialUsageViewModel(_materialDetailsRepository, _BatchRepository)
            {
                //DateSelectorModal = _dateSelector.GetDateSelectorDetails()
            };

            materials.DateSelectorModal.TimeFrame = "year";
            materials.DateSelectorModal.Week = 35;
            materials.DateSelectorModal.Year = 2020;
            return View(materials);
            
        }

        [HttpPost]
        public IActionResult MaterialUsageSingle(string name, DateSelectorModal dateSelectorModal)
        {
            List<BatchReport> reports = new List<BatchReport>();
            MaterialDetails details = _materialDetailsRepository.GetSingleMaterial(name);
            MaterialUsageViewModel materialUsageViewModel = new MaterialUsageViewModel();

            materialUsageViewModel.Name = details.Name;
            materialUsageViewModel.ProductCode = details.ProductCode;
            materialUsageViewModel.ShortName = details.ShortName;
            materialUsageViewModel.CostPerTon = details.CostPerTon;
            materialUsageViewModel.DateSelectorModal = dateSelectorModal;
            reports = helper.GetBatchReportsForDateSelector(materialUsageViewModel.DateSelectorModal);
            materialUsageViewModel.streamInfo = GetStreamMatVarBreakdown(reports, details.Name);
            CalculateWeeklyUsageForSetTimePeriod(materialUsageViewModel, reports, details);
            materialUsageViewModel.WeeklyUsage = materialUsageViewModel.WeeklyUsage.OrderBy(x => x.Year).ThenBy(x => x.WeekNo).ToList();

            return View("MaterialUsageSingle", materialUsageViewModel);
        }

        private void CalculateWeeklyUsageForSetTimePeriod(MaterialUsageViewModel materialUsageViewModel, List<BatchReport> reports, MaterialDetails details)
        {
            foreach (var report in reports)
            {
                foreach (var vessel in report.AllVessels)
                {
                    foreach (var material in vessel.Materials)
                    {
                        if (material.Name == details.Name)
                        {
                            double target = material.TargetWeight;
                            double actual = material.ActualWeight;

                            if (material.Name.Contains("DYE"))
                            {
                                target = helper.CalculateDyeAmountInSolution(material.Name, material.TargetWeight);
                                actual = helper.CalculateDyeAmountInSolution(material.Name, material.ActualWeight);
                            }

                            if (materialUsageViewModel.WeeklyUsage.Any(x => x.WeekNo == report.WeekNo && x.Year == report.StartTime.Year))
                            {
                                materialUsageViewModel.WeeklyUsage.Find(x => x.WeekNo == report.WeekNo && x.Year == report.StartTime.Year).Target += target;
                                materialUsageViewModel.WeeklyUsage.Find(x => x.WeekNo == report.WeekNo && x.Year == report.StartTime.Year).Actual += actual;

                            }
                            else
                            {
                                materialUsageViewModel.WeeklyUsage.Add(
                                    new SingleMaterialWeeklyUsage(report.WeekNo,
                                                            report.StartTime.Year,
                                                            target,
                                                            actual,
                                                            materialUsageViewModel.CostPerTon));
                            }
                            materialUsageViewModel.TotalBatchesMadeWithMaterial++;
                        }
                    }
                }
            }
            materialUsageViewModel.CalculateTotals();

        }

        private List<StreamBreakdownInfo> GetStreamMatVarBreakdown(List<BatchReport> reports, string materialName)
        {

            List<StreamBreakdownInfo> streams = new List<StreamBreakdownInfo>();

            foreach (var report in reports)
            {
                foreach (var vessel in report.AllVessels)
                {
                    foreach (var material in vessel.Materials)
                    {
                        if (material.Name == materialName)
                        {
                            double gainLoss = material.TargetWeight - material.ActualWeight;

                            if (material.Name.Contains("DYE"))
                            {
                                gainLoss = helper.CalculateDyeAmountInSolution(material.Name, material.TargetWeight - material.ActualWeight);
                            }
                            if (streams.Any(x => x.StreamName == report.StreamName))
                            {
                                streams.Find(x => x.StreamName == report.StreamName).LossInfo += gainLoss;
                                streams.Find(x => x.StreamName == report.StreamName).Occurances++;
                            }
                            else
                            {
                                streams.Add(new StreamBreakdownInfo { StreamName = report.StreamName, Occurances = 1, LossInfo = gainLoss });
                            }
                        }
                    }
                }
            }

            foreach (var stream in streams)
            {
                stream.LossInfo = Math.Round(stream.LossInfo, 2);
                if (stream.LossInfo > 0)
                {
                    stream.LossType = "Gain";
                }
                else
                {
                    stream.LossType = "Giveaway";
                }
            }

            double totalLoss = GetTotalForLossType(streams, "Giveaway");
            double totalGain = GetTotalForLossType(streams, "Gain");

            foreach (var stream in streams)
            {
                if (stream.LossType == "Giveaway")
                {
                    stream.Percentage = Math.Round((stream.LossInfo / totalLoss) * 100, 2);
                }
                else
                {
                    stream.Percentage = Math.Round((stream.LossInfo / totalGain) * 100, 2);
                }
            }
            return streams.OrderBy(x => x.StreamName).ToList();
        }

        private double GetTotalForLossType(List<StreamBreakdownInfo> streams, string lossType)
        {
            double total = 0;

            foreach (var stream in streams)
            {
                if (stream.LossType == lossType)
                {
                    total += stream.LossInfo;
                }
            }

            return total;
        }
    }
}


