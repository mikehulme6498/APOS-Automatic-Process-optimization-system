using BatchDataAccessLibrary.Enums;
using BatchDataAccessLibrary.Helpers;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using BatchReports.ComplianceChecker;
using Microsoft.AspNetCore.Mvc;
using RosemountDiagnosticsV2.Excel;
using RosemountDiagnosticsV2.Helper_Methods;
using RosemountDiagnosticsV2.Interfaces;
using RosemountDiagnosticsV2.Models;
using RosemountDiagnosticsV2.View_Models;
using RosemountDiagnosticsV2.View_Models.Quality;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RosemountDiagnosticsV2.Controllers
{
    public class QualityController : Controller
    {
        private readonly IBatchRepository _batchRepository;
        private readonly IPcsWeightParameterRepository _pcsParameterRepository;
        private readonly IPcsReworkParameters _pcsReworkParameters;
        private readonly IPcsActiveTempParameters _pcsActiveTempParameters;
        private readonly IPcsScoringRepository _pcsScoringRepository;
        private readonly IPcsToleranceParameterRepository _pcsToleranceParameter;
        private readonly IRecipeLimitRepository _recipeLimitRepository;
        private readonly IHelperMethods _helperMethods;
        private readonly IApplicationData _applicationData;
        private readonly IMaterialDetailsRepository _materialDetailsRepository;
        private readonly GeneralHelperMethods _generalHelperMethods;
        private readonly XLCreator _xLCreator = new XLCreator();

        public QualityController(IBatchRepository batchRepository,
            IPcsWeightParameterRepository pcsParameterRepository, IPcsReworkParameters pcsReworkParameters,
            IPcsActiveTempParameters pcsActiveTempParameters, IPcsScoringRepository pcsScoringRepository,
            IPcsToleranceParameterRepository pcsToleranceParameter, IRecipeLimitRepository recipeLimitRepository,
            IHelperMethods helperMethods, IApplicationData applicationData, IMaterialDetailsRepository materialDetailsRepository)
        {
            _batchRepository = batchRepository;
            _pcsParameterRepository = pcsParameterRepository;
            _pcsReworkParameters = pcsReworkParameters;
            _pcsActiveTempParameters = pcsActiveTempParameters;
            _pcsScoringRepository = pcsScoringRepository;
            _pcsToleranceParameter = pcsToleranceParameter;
            _recipeLimitRepository = recipeLimitRepository;
            _helperMethods = helperMethods;
            _applicationData = applicationData;
            _materialDetailsRepository = materialDetailsRepository;
            _generalHelperMethods = new GeneralHelperMethods(_batchRepository);

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PcsCompliance(int week = -1, int year = -1)
        {
            PcsComplianceViewModel pcsComplianceViewModel = new PcsComplianceViewModel();
            ComplianceCalculator complianceChecker = new ComplianceCalculator(_pcsReworkParameters, _pcsScoringRepository, _pcsToleranceParameter, _pcsParameterRepository, _pcsActiveTempParameters, _helperMethods, _materialDetailsRepository);
            List<BatchReport> reportsThisWeek;

            pcsComplianceViewModel.ShouldShowNextWeekLink = ShouldShowLinkForNextWeek(week);
            pcsComplianceViewModel.ShouldShowNextPrviousLink = ShouldShowLinkForPreviousWeek(week);
            pcsComplianceViewModel.RecipesWithRework = _pcsReworkParameters.GetListOfAllReworkParemeters().Select(x => x.RecipeName).ToList();
            pcsComplianceViewModel.CurrentWeek = SetCurrentWeekIfNotSetByUser(week);

            reportsThisWeek = GetBatchesForSelectedWeek(week, year);

            if (reportsThisWeek.Count != 0)
            {
                pcsComplianceViewModel.DailyResults = complianceChecker.GetResultsForEachDay(reportsThisWeek);
                pcsComplianceViewModel.GenerateDataSet();
            }
            else
            {
                pcsComplianceViewModel.NoReportsThisWeek = true;
            }
            return View(pcsComplianceViewModel);
        }

        public IActionResult ShowControlChart(QualityControlChartViewModel qualityControlChartViewModel)
        {
            List<BatchReport> reports = _generalHelperMethods.GetBatchReportsForDateSelector(qualityControlChartViewModel.DateSelectorModal);

            switch (qualityControlChartViewModel.ParameterId)
            {
                case "1":
                    qualityControlChartViewModel.ChartData.AddRange(GetViscoChartData(reports));
                    break;
                case "2":
                    qualityControlChartViewModel.ChartData.AddRange(GetPhChartData(reports));
                    break;
                case "3":
                    qualityControlChartViewModel.ChartData.AddRange(GetSoftquatChartData(reports));
                    break;
                case "4":
                    qualityControlChartViewModel.ChartData.AddRange(GetStenolDropChartData(reports));
                    break;
                case "5":
                    qualityControlChartViewModel.ChartData.AddRange(GetActiveDropChartData(reports));
                    break;
                case "6":
                    qualityControlChartViewModel.ChartData.AddRange(GetHCLChartData(reports));
                    break;
            }

            return ViewComponent("QualityControlChart", qualityControlChartViewModel);
        }
        public IActionResult ControlCharts()
        {
            QualityControlChartViewModel qualityControlChartViewModel = new QualityControlChartViewModel()
            {
                DateSelectorModal = new DateSelectorModal { 
                    Year=DateTime.Now.Year, 
                    YearForWeek=DateTime.Now.Year
                }
            };
            
            if (_applicationData.ApplicationMode == "demo") 
            {
                qualityControlChartViewModel.SetDemoMode();
            } 
            else 
            { 
                qualityControlChartViewModel.SetFullAppMode();
            }
            return View(qualityControlChartViewModel);
        }

        private List<ControlChartData> GetViscoChartData(List<BatchReport> reports)
        {
            List<ControlChartData> output = new List<ControlChartData>();
            var BatchesGroupedByRecipeType = BatchHelperMethods.GroupBatchesByRecipeType(reports);

            foreach (var recipeType in BatchesGroupedByRecipeType.Keys)
            {
                List<decimal> values = new List<decimal>();
                List<string> xAxisLabels = new List<string>();

                foreach (var report in BatchesGroupedByRecipeType[recipeType].OrderBy(x => x.StartTime.Date).ThenBy(x => x.StartTime.TimeOfDay))
                {
                    values.Add(Convert.ToDecimal(report.Visco));
                    xAxisLabels.Add($"{report.Campaign}-{report.BatchNo}");
                }
                RecipeLimits limits = _recipeLimitRepository.GetLimitInfo(recipeType, LimitType.Visco);

                ControlChartData data = new ControlChartData
                {
                    Target = limits.Target,
                    Max = limits.Max,
                    Min = limits.Min,
                    Values = values,
                    XAxisLabels = xAxisLabels,
                    SeriesName = "Visco cP",
                    Title = $"Viscos Of {ChangeRecipeTypeNameForDemo(recipeType)} batches",
                    YAxisSuffix = "cP",
                    ChartId = recipeType.ToString()
                };
                data.ProcessCpkValues();
                output.Add(data);
                _xLCreator.AddToWorkBook<decimal>($"Visco-{recipeType}", values, data.Min, data.Max);
            }
            return output;

        }

        private string ChangeRecipeTypeNameForDemo(RecipeTypes recipeTypes)
        {
            if (_applicationData.ApplicationMode != "demo")
            {
                return recipeTypes.ToString();
            }
            else
            {
                switch (recipeTypes)
                {
                    case RecipeTypes.Conc:
                        return "Recipe Type 1";
                    case RecipeTypes.Reg:
                        return "Recipe Type 2";
                    case RecipeTypes.BigBang:
                        return "Recipe Type 3";
                    default:
                        return recipeTypes.ToString();
                }
            }
        }
        private List<ControlChartData> GetPhChartData(List<BatchReport> reports)
        {
            List<ControlChartData> output = new List<ControlChartData>();
            var BatchesGroupedByRecipeType = BatchHelperMethods.GroupBatchesByRecipeType(reports);
            List<decimal> values = new List<decimal>();
            List<string> xAxisLabels = new List<string>();

            foreach (var report in reports.OrderBy(x => x.StartTime.Date).ThenBy(x => x.StartTime.TimeOfDay))
            {
                values.Add(Convert.ToDecimal(report.Ph));
                xAxisLabels.Add($"{report.Campaign}-{report.BatchNo}");
            }
            ControlChartData data = new ControlChartData
            {
                Target = 2.6M,
                Max = 2.9M,
                Min = 2.3M,
                Values = values,
                XAxisLabels = xAxisLabels,
                SeriesName = "Ph",
                Title = $"pH Of all Batches",
                YAxisSuffix = "pH",
                ChartId = "phchart"
            };
            data.ProcessCpkValues();
            output.Add(data);

            _xLCreator.AddToWorkBook<decimal>("PH", values, data.Min, data.Max);


            return output;
        }
        private List<ControlChartData> GetHCLChartData(List<BatchReport> reports)
        {
            List<ControlChartData> output = new List<ControlChartData>();
            var BatchesGroupedByRecipeType = BatchHelperMethods.GroupBatchesByRecipeType(reports);
            
            string hclName = _applicationData.ApplicationMode == "demo" ? "Material 59" : "HCL";
            
            foreach (var recipeType in BatchesGroupedByRecipeType.Keys)
            {
                List<decimal> values = new List<decimal>();
                List<string> xAxisLabels = new List<string>();

                foreach (var report in BatchesGroupedByRecipeType[recipeType].OrderBy(x => x.StartTime.Date).ThenBy(x => x.StartTime.TimeOfDay))
                {
                    decimal hcl = Convert.ToDecimal(BatchHelperMethods.GetSingleMaterialFromVessel(report, hclName).ActualWeight);
                    values.Add(hcl);
                    xAxisLabels.Add($"{report.Campaign}-{report.BatchNo}");
                }
                RecipeLimits limits = _recipeLimitRepository.GetLimitInfo(recipeType, LimitType.HCL);

                ControlChartData data = new ControlChartData
                {
                    Target = limits.Target,
                    Max = limits.Target + (0.05M * limits.Target),
                    Min = limits.Target - (0.05M * limits.Target),
                    Values = values,
                    XAxisLabels = xAxisLabels,
                    SeriesName = $"{hclName} Quantity",
                    Title = $"{hclName} Quantity Of {recipeType} Batches",
                    YAxisSuffix = "Kg",
                    ChartId = recipeType.ToString()
                };
                data.ProcessCpkValues();
                output.Add(data);
                _xLCreator.AddToWorkBook<decimal>($"{hclName}-{recipeType}", values, data.Min, data.Max);
            }
            return output;
        }
        private List<ControlChartData> GetActiveDropChartData(List<BatchReport> reports)
        {
            List<ControlChartData> output = new List<ControlChartData>();
            List<BatchReport> reportsToCheck = new List<BatchReport>();
            var batchesGroupedByRecipeType = BatchHelperMethods.GroupBatchesByRecipeType(reports);

            foreach (var recipeType in batchesGroupedByRecipeType.Keys)
            {
                List<decimal> values = new List<decimal>();
                List<string> xAxisLabels = new List<string>();
                PcsTempTargets limits = new PcsTempTargets();
                string nameOfRecipe = "";

                foreach (var report in batchesGroupedByRecipeType[recipeType].OrderBy(x => x.StartTime.Date).ThenBy(x => x.StartTime.TimeOfDay))
                {

                    if (recipeType == RecipeTypes.Conc)
                    {
                        if (report.Recipe == "WHTCON" || report.Recipe == "Recipe 38")
                        {
                            limits = _pcsActiveTempParameters.GetTargetsFor(report.Recipe);
                            limits.UpperLimit = limits.Target + 1.5M;
                            limits.LowerLimit = limits.Target - 1.5M;
                            decimal tempValue = Convert.ToDecimal(_helperMethods.GetTemperatureOfActiveDrop(report.AllVessels.Where(x => x.VesselType == Vessel.VesselTypes.MainMixer).First()));
                            values.Add(CheckTemperatureForAdjustments(tempValue, limits.UpperLimit, limits.LowerLimit));
                            xAxisLabels.Add($"{report.Campaign}-{report.BatchNo}");
                            nameOfRecipe = report.Recipe;
                        }
                    }
                    if (recipeType == RecipeTypes.Reg)
                    {
                        if (report.Recipe == "WHTREG" || report.Recipe == "RE-Recipe 39")
                        {
                            limits = _pcsActiveTempParameters.GetTargetsFor(report.Recipe);
                            limits.UpperLimit = limits.Target + 1M;
                            limits.LowerLimit = limits.Target - 1M;
                            decimal tempValue = Convert.ToDecimal(_helperMethods.GetTemperatureOfActiveDrop(report.AllVessels.Where(x => x.VesselType == Vessel.VesselTypes.MainMixer).First()));
                            values.Add(CheckTemperatureForAdjustments(tempValue, limits.UpperLimit, limits.LowerLimit));
                            xAxisLabels.Add($"{report.Campaign}-{report.BatchNo}");
                            nameOfRecipe = report.Recipe;
                        }
                    }
                    if (recipeType == RecipeTypes.BigBang)
                    {
                        if (report.Recipe == "BB-SKY" || report.Recipe == "BB-Recipe 15")
                        {
                            limits = _pcsActiveTempParameters.GetTargetsFor(report.Recipe);
                            limits.UpperLimit = limits.Target + 1.5M;
                            limits.LowerLimit = limits.Target - 1.5M;
                            decimal tempValue = Convert.ToDecimal(_helperMethods.GetTemperatureOfActiveDrop(report.AllVessels.Where(x => x.VesselType == Vessel.VesselTypes.MainMixer).First()));
                            values.Add(CheckTemperatureForAdjustments(tempValue, limits.UpperLimit, limits.LowerLimit));
                            xAxisLabels.Add($"{report.Campaign}-{report.BatchNo}");
                            nameOfRecipe = report.Recipe;
                        }
                    }
                }

                ControlChartData data = new ControlChartData
                {
                    Target = limits.Target,
                    Max = limits.UpperLimit,
                    Min = limits.LowerLimit,
                    Values = values,
                    XAxisLabels = xAxisLabels,
                    SeriesName = "Active Drop Temp",
                    Title = $"Active Drop Temps Of {nameOfRecipe} Batches",
                    YAxisSuffix = " C",
                    ChartId = recipeType.ToString()
                };
                data.ProcessCpkValues();
                output.Add(data);
                _xLCreator.AddToWorkBook<decimal>($"Active-temp-{nameOfRecipe}", values, data.Min, data.Max);
            }
            
            return output;
        }
        private List<ControlChartData> GetSoftquatChartData(List<BatchReport> reports)
        {
            List<ControlChartData> output = new List<ControlChartData>();
            var BatchesGroupedByRecipeType = BatchHelperMethods.GroupBatchesByRecipeType(reports);


            foreach (var recipeType in BatchesGroupedByRecipeType.Keys)
            {
                List<decimal> values = new List<decimal>();
                List<string> xAxisLabels = new List<string>();

                string quatName = _applicationData.ApplicationMode == "demo" ? "Material 51" : "SOFTQUAT";

                foreach (var report in BatchesGroupedByRecipeType[recipeType].OrderBy(x => x.StartTime.Date).ThenBy(x => x.StartTime.TimeOfDay))
                {
                    double softquatWeight = BatchHelperMethods.GetSingleMaterialFromVessel(report, quatName).ActualWeight;
                    values.Add(Convert.ToDecimal(softquatWeight));
                    xAxisLabels.Add($"{report.Campaign}-{report.BatchNo}");
                }
                RecipeLimits limits = _recipeLimitRepository.GetLimitInfo(recipeType, LimitType.Softquat);

                ControlChartData data = new ControlChartData
                {
                    Target = limits.Target,
                    Max = limits.Target + (0.05M * limits.Target),
                    Min = limits.Target - (0.05M * limits.Target),
                    Values = values,
                    XAxisLabels = xAxisLabels,
                    SeriesName = quatName + " Quantity Kg",
                    Title = $"{quatName} Quantity Of {recipeType} Batches",
                    YAxisSuffix = "Kg",
                    ChartId = recipeType.ToString()
                };
                data.ProcessCpkValues();
                output.Add(data);
                _xLCreator.AddToWorkBook<decimal>($"{quatName}-{recipeType}", values, data.Min, data.Max);

            }

            return output;
        }
        private List<ControlChartData> GetStenolDropChartData(List<BatchReport> reports)
        {
            List<ControlChartData> output = new List<ControlChartData>();
            List<BatchReport> regBatches = reports.Where(x => x.RecipeType == RecipeTypes.Reg).ToList();
            List<decimal> values = new List<decimal>();
            List<string> xAxisLabels = new List<string>();

            string stenolName = _applicationData.ApplicationMode == "demo" ? "Material 21" : "FATTY ALC";

            foreach (var report in regBatches)
            {
                decimal stenol = Convert.ToDecimal(BatchHelperMethods.GetSingleMaterialFromVessel(report, stenolName).ActualWeight);
                values.Add(stenol);
                xAxisLabels.Add(report.Campaign.ToString() + "-" + report.BatchNo.ToString());
            }

            ControlChartData data = new ControlChartData
            {
                Target = 58.5M,
                Max = 58.5M + (0.05M * 58.5M),
                Min = 58.5M - (0.05M * 58.5M),
                Values = values,
                XAxisLabels = xAxisLabels,
                SeriesName = $"{stenolName} Quantity Kg",
                Title = $"{stenolName} Quantity Of Reg Batches",
                YAxisSuffix = "Kg",
                ChartId = "Reg"
            };
            data.ProcessCpkValues();
            output.Add(data);
            _xLCreator.AddToWorkBook<decimal>($"{stenolName}-Regs", values, data.Min, data.Max);

            return output;
        }
        private decimal CheckTemperatureForAdjustments(decimal temperature, decimal upperLimit, decimal lowerLimit)
        {
            decimal output = 0;
            if (temperature > upperLimit || temperature < lowerLimit)
            {
                output = temperature;
            }
            if (temperature + 0.2M <= upperLimit && temperature + 0.2M >= lowerLimit)
            {
                output = temperature + 0.2M;
            }
            if (temperature - 0.2M <= upperLimit && temperature + 0.2M >= lowerLimit)
            {
                output = temperature - 0.2M;
            }
            if (temperature < upperLimit && temperature > lowerLimit)
            {
                output = temperature;
            }
            return output;
        }
        private List<BatchReport> GetBatchesForSelectedWeek(int week, int year)
        {
            if (week == -1 && year == -1)
            {
                return _batchRepository.GetBatchesByWeek(HelperMethods.GetWeekNumber(DateTime.Now), DateTime.Now.Year).OrderByDescending(x => x.StartTime).ToList();
            }
            else
            {
                return _batchRepository.GetBatchesByWeek(week, year).OrderByDescending(x => x.StartTime).ToList();
            }
        }
        private int SetCurrentWeekIfNotSetByUser(int week)
        {
            return week == -1 ? HelperMethods.GetWeekNumber(DateTime.Now) : week;
        }
        private bool ShouldShowLinkForPreviousWeek(int week)
        {
            return week != 1;
        }
        private static bool ShouldShowLinkForNextWeek(int week)
        {
            return HelperMethods.GetWeekNumber(DateTime.Now) != week && week != -1;
        }












    }
}