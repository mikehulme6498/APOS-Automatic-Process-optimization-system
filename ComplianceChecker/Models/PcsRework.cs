using BatchDataAccessLibrary.Helpers;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BatchReports.ComplianceChecker.Models
{
    public class PcsRework
    {
        private readonly IPcsReworkParameters _pcsReworkParameters;
        private readonly IHelperMethods _helperMethods;
        readonly List<PcsReworkParameters> Parameters = new List<PcsReworkParameters>();
        public PcsRework(IPcsReworkParameters pcsReworkParameters, IHelperMethods helperMethods)
        {
            _pcsReworkParameters = pcsReworkParameters;
            _helperMethods = helperMethods;
            Parameters = _pcsReworkParameters.GetListOfAllReworkParemeters();
        }

        public List<PcsReworkTotals> CheckForReworkCompliance(List<BatchReport> reports)
        {
            List<PcsReworkTotals> reworkTotals = new List<PcsReworkTotals>();

            foreach (var recipe in Parameters.Select(x => x.RecipeName))
            {
                reworkTotals.Add(new PcsReworkTotals { RecipeName = recipe });
            }

            foreach (var report in reports)
            {
                if (CurrentReportShouldHaveRework(report))
                {
                    PcsReworkTotals currentTotal = new PcsReworkTotals();
                    Material rework = _helperMethods.FindReworkInBatch(report);

                    if (rework != null)
                    {
                        currentTotal.ActualReworkAmount = Convert.ToDecimal(rework.ActualWeight);
                        currentTotal.BatchesMade = 1;
                        currentTotal.ExpectedReworkAmount = GetExpectedReworkAmount(report.Recipe);
                        currentTotal.BatchesWithRework = 1;
                        currentTotal.RecipeName = report.Recipe;
                    }
                    else
                    {
                        currentTotal.ActualReworkAmount = 0;
                        currentTotal.BatchesMade = 1;
                        currentTotal.ExpectedReworkAmount = GetExpectedReworkAmount(report.Recipe);
                        currentTotal.BatchesWithRework = 0;
                        currentTotal.RecipeName = report.Recipe;
                    }

                    AddToReworkTotals(reworkTotals, currentTotal);
                }
            }
            return reworkTotals;
        }
        private void AddToReworkTotals(List<PcsReworkTotals> reworkTotals, PcsReworkTotals currentTotal)
        {
            if (reworkTotals.Exists(x => x.RecipeName == currentTotal.RecipeName))
            {
                PcsReworkTotals temp = reworkTotals.Find(x => x.RecipeName == currentTotal.RecipeName);
                temp.ActualReworkAmount += currentTotal.ActualReworkAmount;
                temp.BatchesMade += currentTotal.BatchesMade;
                temp.BatchesWithRework += currentTotal.BatchesWithRework;
                temp.ExpectedReworkAmount += currentTotal.ExpectedReworkAmount;
            }
            else
            {
                reworkTotals.Add(new PcsReworkTotals
                {
                    RecipeName = currentTotal.RecipeName,
                    ActualReworkAmount = currentTotal.ActualReworkAmount,
                    BatchesMade = currentTotal.BatchesMade,
                    BatchesWithRework = currentTotal.BatchesWithRework,
                    ExpectedReworkAmount = currentTotal.ExpectedReworkAmount
                });
            }

        }
        private bool CurrentReportShouldHaveRework(BatchReport report)
        {
            return Parameters.Exists(x => x.RecipeName == report.Recipe);
        }

        private decimal GetExpectedReworkAmount(string recipeName)
        {
            return Parameters.Where(x => x.RecipeName == recipeName).Select(x => x.TargetReworkAmount).First();
        }
    }
}
