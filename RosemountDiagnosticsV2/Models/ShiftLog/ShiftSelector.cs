using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RosemountDiagnosticsV2.Models.ShiftLog
{
    public class ShiftSelector
    {
        public string GetShiftDayNight(DateTime time)
        {
            if(time.Hour >= 06 && time.Hour < 18)
            {
                return "Days";
            }
            return "Nights";
        }
    }
}
