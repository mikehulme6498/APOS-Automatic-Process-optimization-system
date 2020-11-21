using System;

namespace BatchDataAccessLibrary.Models.ShiftLog
{
    public class OperatorShiftLog
    {
        public int OperatorShiftLogId { get; set; }
        public DateTime Date { get; set; }
        public string DaysNights { get; set; }
        public string ShiftColour { get; set; }
        public string Operators { get; set; }
        public double EffluentAtStartOfShift { get; set; }
        public int ReworkTotes { get; set; }
        public int BigBangReworkTotes { get; set; }
        public string Comments { get; set; }

    }
}
