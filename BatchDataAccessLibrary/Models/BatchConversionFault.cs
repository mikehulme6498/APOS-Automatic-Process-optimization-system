using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BatchDataAccessLibrary.Models
{
    public class BatchConversionFault
    {
        [DebuggerDisplay("Fault : { Message }")]
        public int BatchConversionFaultId { get; set; }
        public string Campaign { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public string FileName { get; set; }
    }
}
