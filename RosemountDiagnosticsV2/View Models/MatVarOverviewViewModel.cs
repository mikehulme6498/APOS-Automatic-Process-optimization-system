using RosemountDiagnosticsV2.Models;
using System.Collections.Generic;

namespace RosemountDiagnosticsV2.View_Models
{
    public class MatVarOverviewViewModel
    {
        public List<MaterialTotals> Gains { get; set; } = new List<MaterialTotals>();
        public List<MaterialTotals> Losses { get; set; } = new List<MaterialTotals>();
        public List<MaterialTotals> UnsortedTotals { get; set; } = new List<MaterialTotals>();
        public DateSelectorModal DateSelectorModal { get; set; }
        public double TotalGainForPeriod { get; set; }
        public double TotalLossForPeriod { get; set; }
        public double CombinedTotal { get; set; }

        public MatVarOverviewViewModel()
        {
            DateSelectorModal = new DateSelectorModal();
        }
    }


}
