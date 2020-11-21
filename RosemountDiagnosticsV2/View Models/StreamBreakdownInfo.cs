using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RosemountDiagnosticsV2.View_Models
{
    public class StreamBreakdownInfo
    {
        public string StreamName { get; set; }
        public string LossType { get; set; }
        public double LossInfo { get; set; }
        public int Occurances { get; set; }
        public double Percentage { get; set; }

    }
}
