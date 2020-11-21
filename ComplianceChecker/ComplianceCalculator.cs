using BatchDataAccessLibrary.Enums;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using BatchReports.ComplianceChecker.Models;
using ComplianceChecker;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BatchReports.ComplianceChecker
{
    public class ComplianceCalculator
    {
        private readonly IPcsReworkParameters _pcsReworkParameters;
        private readonly IPcsScoringRepository _pcsScoringRepository;
        private readonly IPcsToleranceParameterRepository _pcsToleranceParameter;
        private readonly IPcsWeightParameterRepository _pcsParameterRepository;
        private readonly IPcsActiveTempParameters _pcsActiveTempParameters;
        private readonly IHelperMethods _helperMethods;
        private readonly IMaterialDetailsRepository _materialDetailsRepository;

        public ComplianceCalculator(IPcsReworkParameters pcsReworkParameters, IPcsScoringRepository pcsScoringRepository,
            IPcsToleranceParameterRepository pcsToleranceParameterRepository, IPcsWeightParameterRepository pcsParameterRepository,
            IPcsActiveTempParameters pcsActiveTempParameters, IHelperMethods helperMethods, IMaterialDetailsRepository materialDetailsRepository)
        {
            _pcsReworkParameters = pcsReworkParameters;
            _pcsScoringRepository = pcsScoringRepository;
            _pcsToleranceParameter = pcsToleranceParameterRepository;
            _pcsParameterRepository = pcsParameterRepository;
            _pcsActiveTempParameters = pcsActiveTempParameters;
            _helperMethods = helperMethods;
            _materialDetailsRepository = materialDetailsRepository;
        }
        public List<PcsDailyResults> GetResultsForEachDay(List<BatchReport> reportsThisWeek)
        {
            List<PcsDailyResults> output = new List<PcsDailyResults>();
            DateTime currentDay = GetFirstDateOfCurrentWeek(reportsThisWeek);
            int amountOfDayToLookThrough = GetDaysToLoopThrough(currentDay);

            for (int i = 0; i < amountOfDayToLookThrough; i++)
            {
                List<BatchReport> currentDayReports = reportsThisWeek.Where(x => x.StartTime.Date == currentDay.Date).ToList();
                PcsDailyResults todaysFigures = GetTodaysFigures(currentDayReports);
                if (todaysFigures != null)
                {
                    output.Add(todaysFigures);
                }
                currentDay = currentDay.AddDays(1);
            }
            return output;
        }

        private PcsDailyResults GetTodaysFigures(List<BatchReport> currentDayReports)
        {
            PcsRework rework = new PcsRework(_pcsReworkParameters, _helperMethods);
            PcsDailyResults output;

            if (currentDayReports.Count != 0)
            {
                output = new PcsDailyResults(currentDayReports.Select(x => x.StartTime).First(), _pcsScoringRepository);
                List<PcsParameterTotals> parameterTotals = new List<PcsParameterTotals>();

                parameterTotals.AddRange(GetWeightsForRecipeType(currentDayReports, RecipeTypes.Conc));
                parameterTotals.AddRange(GetWeightsForRecipeType(currentDayReports, RecipeTypes.Reg));
                parameterTotals.AddRange(GetWeightsForRecipeType(currentDayReports, RecipeTypes.BigBang));
                parameterTotals.Add(GetAllPerfumeWeights(currentDayReports));

                var dyes = GetAllDyeWeights(currentDayReports);
                if (dyes != null)
                {
                    parameterTotals.Add(dyes);
                }

                var activeTemps = GetAllActiveDropTemps(currentDayReports);
                if (activeTemps != null)
                {
                    parameterTotals.Add(activeTemps);
                }

                output.MaterialsChecked.AddRange(parameterTotals);
                output.DailyRework.AddRange(rework.CheckForReworkCompliance(currentDayReports));
                output.ProcessCompliance();
                return output;
            }
            return null;
        }

        private PcsParameterTotals GetAllDyeWeights(List<BatchReport> todaysReports)
        {
            List<IPcsIndividualParameters> dyes = new List<IPcsIndividualParameters>();

            foreach (var report in todaysReports)
            {
                Vessel dyeVessel = report.AllVessels.Where(x => x.VesselType == Vessel.VesselTypes.MainMixer).First();
                List<Material> dyeList = dyeVessel.Materials.Where(x => x.Name.ToLower().Contains("dye") && !x.Name.ToLower().Contains("flush")).ToList();

                if (dyeList.Count != 0)
                {
                    foreach (var dye in dyeList)
                    {
                        dyes.Add(new PcsDyes("Dyes", report.Campaign.ToString() + "-" + report.BatchNo.ToString(), report.Recipe,
                        Convert.ToDecimal(dye.TargetWeight), Convert.ToDecimal(dye.ActualWeight), report.RecipeType, _pcsToleranceParameter));
                    }
                }
            }
            if (dyes.Count == 0)
            {
                return null;
            }
            else
            {
                decimal tolerance = dyes.Select(x => x.Tolerance).FirstOrDefault();
                return new PcsParameterTotals("Dyes (+/- " + tolerance.ToString() + "%)", dyes, _pcsScoringRepository);
            }
        }

        private PcsParameterTotals GetAllPerfumeWeights(List<BatchReport> todaysReports)
        {
            List<IPcsIndividualParameters> perfumes = new List<IPcsIndividualParameters>();

            foreach (var report in todaysReports)
            {
                Vessel perfumeVessel = report.AllVessels.Where(x => x.VesselType == Vessel.VesselTypes.PerfumePreWeigher).First();
                perfumeVessel.Materials = perfumeVessel.Materials.OrderBy(x => x.StartTime).ToList();
                if (perfumeVessel.Materials.Count > 0)
                {
                    Material perfume = perfumeVessel.Materials[0];


                    if (perfume.WeighTime > 9)
                    {
                        perfume.ActualWeight = perfume.TargetWeight;
                    }

                    perfumes.Add(new PcsWeights("Perfume", report.Campaign.ToString() + "-" + report.BatchNo.ToString(), report.Recipe,
                        Convert.ToDecimal(perfume.TargetWeight), Convert.ToDecimal(perfume.ActualWeight), report.RecipeType, _pcsToleranceParameter));
                }
            }
            decimal tolerance = perfumes.Select(x => x.Tolerance).FirstOrDefault();
            return new PcsParameterTotals("Perfumes (+/- " + tolerance.ToString() + "%)", perfumes, _pcsScoringRepository);
        }

        private PcsParameterTotals GetAllActiveDropTemps(List<BatchReport> todaysReports)
        {
            List<IPcsIndividualParameters> temps = new List<IPcsIndividualParameters>();
            var activeTempLimits = _pcsActiveTempParameters.GetAllTempParameters().Result;

            foreach (var report in todaysReports)
            {
                Vessel mainMixerVessel = report.AllVessels.Where(x => x.VesselType == Vessel.VesselTypes.MainMixer).First();
                decimal dropTemp = _helperMethods.GetTemperatureOfActiveDrop(mainMixerVessel);
                if (activeTempLimits.Any(x => x.Recipe == report.Recipe))
                {
                    temps.Add(new PcsActiveTemps("Active Drop Temp", report.Campaign.ToString() + "-" + report.BatchNo.ToString(), report.Recipe, dropTemp, report.RecipeType, _pcsActiveTempParameters, _pcsToleranceParameter));
                }
                else
                {
                    Debug.WriteLine($"Compliance checker (function GetAllActiveDropTemps) could not retrieve limits for {report.Recipe} therefore it was ignored");
                }

            }

            return new PcsParameterTotals("Actives Drop Temp", temps, _pcsScoringRepository);
        }

        private List<PcsParameterTotals> GetWeightsForRecipeType(List<BatchReport> dayReports, RecipeTypes recipeType)
        {
            List<PcsWeightParameters> parametersToLookFor = _pcsParameterRepository.GetParametersForRecipeType(recipeType);
            List<PcsParameterTotals> materials = new List<PcsParameterTotals>();

            foreach (var parameter in parametersToLookFor)
            {
                List<IPcsIndividualParameters> weights = GetWeights(dayReports.Where(x => x.RecipeType == recipeType).ToList(), parameter.Parameter);
                string parameterShortName = _materialDetailsRepository.GetSingleMaterial(parameter.Parameter).ShortName;

                if (weights.Count != 0)
                {
                    decimal tolerance = weights.Select(x => x.Tolerance).FirstOrDefault();
                    materials.Add(new PcsParameterTotals(parameterShortName + " (+/- " + tolerance.ToString() + "%)", weights, _pcsScoringRepository));
                }
            }
            return materials;
        }


        private List<IPcsIndividualParameters> GetWeights(List<BatchReport> reports, string name)
        {
            List<IPcsIndividualParameters> weights = new List<IPcsIndividualParameters>();

            if (reports.Count != 0)
            {
                foreach (var report in reports)
                {
                    Material currentMaterial = FindMaterialInBatch(report.AllVessels, name);
                    if (currentMaterial != null)
                    {
                        weights.Add(new PcsWeights(currentMaterial.Name, report.Campaign.ToString() + "-" + report.BatchNo.ToString(), report.Recipe,
                            Convert.ToDecimal(currentMaterial.TargetWeight), Convert.ToDecimal(currentMaterial.ActualWeight), report.RecipeType, _pcsToleranceParameter));
                    }
                }
            }
            return weights;
        }



        private DateTime GetFirstDateOfCurrentWeek(List<BatchReport> reportsThisWeek)
        {
            return reportsThisWeek.Select(x => x.StartTime).Last();
        }

        private int GetDaysToLoopThrough(DateTime startOfWeek)
        {
            int days = DateTime.Now.Subtract(startOfWeek).Days;
            return days > 7 ? 7 : days;
        }

        private Material FindMaterialInBatch(List<Vessel> vessels, string name)
        {
            Material material = new Material();

            foreach (var vessel in vessels)
            {
                material = vessel.Materials.Find(x => x.Name == name);
                if (material != null) { break; }
            }
            return material;
        }
    }
}
