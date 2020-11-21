using Microsoft.CodeAnalysis.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RosemountDiagnosticsV2.Models.Settings
{
    public class PcsActiveDropTemps
    {
        public string Recipe { get; set; }
        public decimal UpperLimit { get; set; }
        public decimal LowerLimit { get; set; }
        public decimal Target{ get; set; }
    }
}
