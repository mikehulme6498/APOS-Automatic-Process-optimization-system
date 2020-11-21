using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RosemountDiagnosticsV2.View_Models.Quality
{
    public class PcsSummary
    {
        public string Title { get; set; }
        public List<PcsSummaryParameter> ParameterSummary = new List<PcsSummaryParameter>();
        public DateTime Date { get; set; }
        public decimal Percentage { get; set; }
        public string Icon { get; set; }
        public string IconColourClass { get; set; }
        public string TitleColourClass { get; set; }
    }
}
