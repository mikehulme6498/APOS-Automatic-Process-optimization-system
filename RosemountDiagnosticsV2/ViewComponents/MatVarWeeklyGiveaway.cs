using BatchDataAccessLibrary.Helpers;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using RosemountDiagnosticsV2.Interfaces;
using RosemountDiagnosticsV2.Models;
using RosemountDiagnosticsV2.View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RosemountDiagnosticsV2.ViewComponents
{
    public class MatVarWeeklyGiveawayChart : ViewComponent
    {
        private readonly IBatchRepository _batchRepository;
        private readonly IMaterialDetailsRepository _materialDetails;
        
        public MatVarWeeklyGiveawayChart(IBatchRepository batchRepository, IMaterialDetailsRepository materialDetails)
        {
            _batchRepository = batchRepository;
            _materialDetails = materialDetails;
        }



        //public IViewComponentResult Invoke(int week, int year, DateTime dateFrom, DateTime dateTo, string timeFrame)
        public IViewComponentResult Invoke(DateSelectorModal dateSelectorModal, bool homePage = false)
        {

            List<BatchReport> reports = new List<BatchReport>();

            if (homePage)
            {
                reports = _batchRepository.GetBatchesByYear(DateTime.Now.Year);
            }
            else
            {
                switch (dateSelectorModal.TimeFrame)
                {
                    case "year":
                        reports = _batchRepository.GetBatchesByYear(dateSelectorModal.Year);
                        break;
                    case "week":
                        reports = _batchRepository.GetBatchesByWeek(dateSelectorModal.Week, dateSelectorModal.Year);
                        break;
                    case "dates":
                        reports = _batchRepository.GetBatchesByDates(dateSelectorModal.DateFrom, dateSelectorModal.DateTo);
                        break;
                }
            }
            List<MatVarWeeklyGiveawayTotals> totals = CalculateWeeklyTotals(reports);
            return View(totals);
        }

        private List<MatVarWeeklyGiveawayTotals> CalculateWeeklyTotals(List<BatchReport> reports)
        {
            List<MatVarWeeklyGiveawayTotals> totals = new List<MatVarWeeklyGiveawayTotals>();
            var allMaterialsDetailsIncludedInMatVar = _materialDetails.GetAllMaterialDetails().Where(x => x.IncludeInMatVar == true).ToList();
            
            CreateTotalsUptoCurrentWeek(totals);

            foreach (var report in reports)
            {
                foreach (var vessel in report.AllVessels)
                {
                    foreach (var material in vessel.Materials)
                    {
                        if (allMaterialsDetailsIncludedInMatVar.Any(x => x.Name == material.Name))
                        {
                            if (totals.Any(x => x.Week == report.WeekNo))
                            {
                                double costPerTon = allMaterialsDetailsIncludedInMatVar
                                    .Where(x => x.Name == material.Name)
                                    .Select(x => x.CostPerTon)
                                    .FirstOrDefault() / 1000;
                                double amountUsed = material.TargetWeight - material.ActualWeight;
                                double totalCost = Math.Round(costPerTon * amountUsed, 2);
                                totals.Find(x => x.Week == report.WeekNo).Total += totalCost;
                            }
                        }
                    }
                }
            }

            return totals.OrderBy(x => x.Week).ToList();
        }

        private static void CreateTotalsUptoCurrentWeek(List<MatVarWeeklyGiveawayTotals> totals)
        {
            for (int i = 1; i <= HelperMethods.GetWeekNumber(DateTime.Now); i++)
            {
                totals.Add(new MatVarWeeklyGiveawayTotals
                {
                    Week = i,
                    Total = 0
                });
            }
        }
    }
}
