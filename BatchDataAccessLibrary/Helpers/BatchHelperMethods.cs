using BatchDataAccessLibrary.Enums;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static BatchDataAccessLibrary.Models.Vessel;

namespace BatchDataAccessLibrary.Helpers
{
    public class BatchHelperMethods : IHelperMethods
    {
        public static double GetTotalStoppageTime(List<BatchIssue> issues)
        {
            if (issues == null || issues.Count == 0)
            {
                return 0;
            }

            double time = 0;
            foreach (var issue in issues)
            {
                switch (issue.FaultType)
                {
                    case BatchIssue.FaultTypes.WeighTime:
                        time += issue.TimeLost;
                        break;
                    case BatchIssue.FaultTypes.WaitTime:
                        time += issue.TimeLost;
                        break;
                    case BatchIssue.FaultTypes.AcquireTime:
                        time += issue.TimeLost;
                        break;
                    default:
                        break;
                }
            }
            return time;
        }
        public static int GetTotalQualityIssues(List<BatchIssue> issues)
        {
            if (issues == null)
            {
                return 0;
            }

            int issueCount = 0;
            foreach (var issue in issues)
            {
                switch (issue.FaultType)
                {
                    case BatchIssue.FaultTypes.TemperatureHigh:
                        issueCount++;
                        break;
                    case BatchIssue.FaultTypes.TemperatureLow:
                        issueCount++;
                        break;
                    case BatchIssue.FaultTypes.Quality:
                        issueCount++;
                        break;
                    default:
                        break;
                }
            }
            return issueCount;
        }
        public static double GetTotalMatVarCost(List<BatchIssue> issues, IMaterialDetailsRepository materialDetailsRepository)
        {
            double cost = 0;
            foreach (var issue in issues)
            {
                switch (issue.FaultType)
                {
                    case BatchIssue.FaultTypes.Overweigh:
                        cost += materialDetailsRepository.GetCostMaterialLoss(issue.MaterialName, issue.WeightDiffference);
                        break;
                    case BatchIssue.FaultTypes.Underweigh:
                        cost += materialDetailsRepository.GetCostMaterialLoss(issue.MaterialName, issue.WeightDiffference);
                        break;
                    default:
                        break;
                }
            }
            return cost;
        }
        public static Material GetSingleMaterialFromVessel(BatchReport report, VesselTypes vesselType, string materialName)
        {
            Vessel tempVessel;

            try
            {
                tempVessel = report.AllVessels.Find((x) => x.VesselType == vesselType);
            }
            catch
            {
                return null;
            }

            try
            {
                foreach (var material in tempVessel.Materials)
                {
                    if (material.Name == materialName)
                    {
                        return material;
                    }
                }
            }
            catch
            {
                Console.WriteLine();
            }

            return null;
        }
        public static Material GetSingleMaterialFromVessel(BatchReport report, VesselTypes vesselType, int listPosition)
        {
            Vessel tempVessel;

            try
            {
                tempVessel = report.AllVessels.Find((x) => x.VesselType == vesselType);
            }
            catch
            {
                return null;
            }

            return tempVessel.Materials[listPosition];
        }
        public static Material GetSingleMaterialFromVessel(BatchReport report, string materialName)
        {
            foreach (var vessel in report.AllVessels)
            {
                foreach (var material in vessel.Materials)
                {
                    if (material.Name == materialName)
                    {
                        return material;
                    }
                }
            }

            return null;
        }
        public Material FindReworkInBatch(BatchReport report)
        {
            Vessel vessel = report.AllVessels.Where(x => x.VesselType == VesselTypes.MainMixer).First();
            Material output = vessel.Materials.Where(x => x.Name == "WASHINGS RWK" || x.Name == "BB CIP").FirstOrDefault();
            return output;
        }
        public static double CountActualWeightOfAllMaterialsInVessel(Vessel vessel)
        {
            double output = 0;
            foreach (var material in vessel.Materials)
            {
                output += material.ActualWeight;
            }
            return output;
        }
        public static BatchReport CreateBlankBatchReport()
        {
            BatchReport report = new BatchReport()
            {
                Recipe = "N/A",
                Campaign = 0,
                BatchNo = 0,
                MakingTime = 0,
                NewMakeTime = 0,
                Appearance = "N/A",
                RecipeType = 0,
                WeekNo = 0,
                DropTime = 0,
                FileName = "N/A",
                IsValidBatch = false,
                MeasuredColour = "N/A",
                AllocatedTo = "0",
                Visco = 0
            };

            return report;
        }
        public decimal GetTemperatureOfActiveDrop(Vessel vessel)
        {
            // Temperarture is taken from the material before Empty Vxx3 as temperature on report is
            // registered once the material has finished dropping.

            vessel.Materials = vessel.Materials.OrderBy(x => x.StartTime.Date).ThenBy(x => x.StartTime.TimeOfDay).ToList();

            for (int i = 0; i < vessel.Materials.Count; i++)
            {
                if (vessel.Materials[i].Name.ToLower().Contains("empty v") && vessel.Materials[i].Name.EndsWith("3"))
                {
                    //if (i == 0)
                    //{
                    //    return Convert.ToDecimal(vessel.Materials[i].VesselTemp);
                    //}

                    return Convert.ToDecimal(vessel.Materials[i - 1].VesselTemp);
                }
            }
            return 0;
        }
        public static Dictionary<RecipeTypes, List<BatchReport>> GroupBatchesByRecipeType(List<BatchReport> reports)
        {
            return reports
                 .GroupBy(x => x.RecipeType)
                 .ToDictionary(group => group.Key, group => group
                 .ToList());
        }
        public static bool DoesBatchHaveRework(BatchReport report)
        {
            bool output = false;

            Vessel vessel = report.AllVessels.Where(x => x.VesselType == VesselTypes.MainMixer).First();
            foreach (var material in vessel.Materials)
            {
                if (material.Name == "WASHINGS RWK" || material.Name == "BB-CIP")
                {
                    return true;
                }
            }
            return output;
        }
    }
}

