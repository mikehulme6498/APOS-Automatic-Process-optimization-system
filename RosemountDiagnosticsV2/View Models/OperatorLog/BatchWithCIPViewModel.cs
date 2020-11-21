using BatchDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RosemountDiagnosticsV2.View_Models.OperatorLog
{
    public class BatchWithCIPViewModel
    {
        public BatchReport Report { get; set; }
        public bool StreamCip { get; set; }
        public bool StockTankCip { get; set; }
    }
}
