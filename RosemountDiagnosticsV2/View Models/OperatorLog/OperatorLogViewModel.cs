using BatchDataAccessLibrary.Models;
using BatchDataAccessLibrary.Models.ShiftLog;
using System.Collections.Generic;

namespace RosemountDiagnosticsV2.View_Models.OperatorLog
{
    public class OperatorLogViewModel
    {
        public OperatorShiftLog CurrentShift { get; set; }
        public List<BatchWithCIPViewModel> BatchReports { get; set; }
        public List<string> ToteChanges { get; set; } = new List<string>();
        public GoodStockToWaste GoodStockToWaste { get; set; }
        public int TotalReworkToteChanges { get; set; }
        public bool NeedOperators { get; set; }


        public OperatorLogViewModel()
        {
            CurrentShift = new OperatorShiftLog();
            BatchReports = new List<BatchWithCIPViewModel>();
        }
    }
}
