using System;
using System.Diagnostics;

namespace RosemountDiagnosticsV2.View_Models
{
    [DebuggerDisplay("{ Year } - { WeekNo }")]
    public class SingleMaterialWeeklyUsage
    {
        public int WeekNo { get; set; }
        public int Year { get; set; }
        public double Target { get; set; }
        public double TargetCost { get; set; }
        public double Actual { get; set; }
        public double ActualCost { get; set; }
        public double CostPerTon { get; set; }
        public double GiveawayGainKg { get;  private set; }
        public double GiveawayGainEuro { get; private set; }

        public SingleMaterialWeeklyUsage(int week, int year, double target, double actual, double costPerTon)
        {
            WeekNo = week;
            Year = year;
            Target = Math.Round(target,2);
            Actual = Math.Round(actual,2);
            CostPerTon = Math.Round(costPerTon,2);
            
        }

        public void CalculateWeeklyFigures()
        {
            TargetCost = Math.Round((Target * CostPerTon) / 1000, 2);
            ActualCost = Math.Round((Actual * CostPerTon) / 1000, 2);
            GiveawayGainKg = Math.Round(Target - Actual, 2);
            GiveawayGainEuro = Math.Round(TargetCost - ActualCost, 2);
        }
    }

}
