using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RosemountDiagnosticsV2.View_Models
{
    public class BatchesMadePerWeek
    {
        public int WeekNo { get; set; }
        public int BatchesMade { get; set; }
        public int ConcBatchesCount { get; set; }
        public int BigBangBatchesCount { get; set; }
        public int RegBatchesCount { get; set; }
    }
}
