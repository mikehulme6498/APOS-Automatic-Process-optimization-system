

using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using RosemountDiagnosticsV2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RosemountDiagnosticsV2.View_Models
{
    public class MaterialUsageViewModel
    {
        private readonly IMaterialDetailsRepository _materialDetailsRepository;
        private readonly IBatchRepository _batchReportRepository;

        [DebuggerDisplay("{Name}")]
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int ProductCode { get; set; }
        public double CostPerTon { get; set; }
        public int TotalBatchesMadeWithMaterial { get; set; }
        public List<SingleMaterialWeeklyUsage> WeeklyUsage { get; set; } = new List<SingleMaterialWeeklyUsage>();
        public List<StreamBreakdownInfo> streamInfo { get; set; } = new List<StreamBreakdownInfo>();
        public int Year { get; set; }
        public int Week { get; set; }
        //public int YearForWeek { get; set; }       
        public double PeriodTotalUsed { get; set; }
        public double PeriodTotalGainLossEuro { get; set; }
        public double PeriodTotalGainLossKg { get; set; }
        public double PercentageVariance { get; set; }
        public string GainLoss { get; set; }
        public List<SelectListItem> MaterialsForDropDown { get; private set; } = new List<SelectListItem>();

        public DateSelectorModal DateSelectorModal { get; set; } 

        public MaterialUsageViewModel()
        {
            
        }
        public MaterialUsageViewModel(IMaterialDetailsRepository MaterialDetailsRepository, IBatchRepository batchReportRepository)
        { 
            _materialDetailsRepository = MaterialDetailsRepository;
            _batchReportRepository = batchReportRepository;
            DateSelectorModal = new DateSelectorModal();

            foreach (var material in _materialDetailsRepository.GetAllMaterialDetails())
            {
                if (material.IncludeInMatVar)
                {
                    MaterialsForDropDown.Add(new SelectListItem() { Text = material.ShortName, Value = material.Name });
                }
            }

            MaterialsForDropDown = MaterialsForDropDown.OrderBy(x => x.Text).ToList();
        }

        public void CalculateTotals()
        {
            CalculateLossGainForEachWeek();
            CalculateTotalLossGainForPeriodSelected();
        }
        private void CalculateLossGainForEachWeek()
        {
            foreach (var usage in WeeklyUsage)
            {
                usage.CalculateWeeklyFigures();
            }
        }

        private void CalculateTotalLossGainForPeriodSelected()
        {
            var target = 0.00;

            foreach (var week in WeeklyUsage)
            {
                PeriodTotalGainLossEuro += Math.Round(week.GiveawayGainEuro, 2);
                PeriodTotalGainLossKg += Math.Round(week.GiveawayGainKg, 2);
                PeriodTotalUsed += Math.Round(week.Actual, 2);
                target += week.Target;
            }

            
            PercentageVariance = Math.Round(100 - ((PeriodTotalUsed / target) * 100), 2);

            GainLoss = target - PeriodTotalUsed > 0.00 ? "Gain" : "Giveaway";
            
        }


    }
}
