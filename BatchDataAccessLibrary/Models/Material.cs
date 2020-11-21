using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BatchDataAccessLibrary.Models
{
    [DebuggerDisplay("Material : { Name }")]
    public class Material
    {
        public int MaterialId { get; set; }
        //[ForeignKey("BatchReportId")]
        [MaxLength(100)]
        public string Name { get; set; }
        public double TargetWeight { get; set; }
        public double ActualWeight { get; set; }
        public DateTime StartTime { get; set; }
        public double WaitTime { get; set; }
        public double WeighTime { get; set; }
        public double VesselBefore { get; set; }
        public double WeightAfter { get; set; }
        public double VesselTemp { get; set; }
        public double AgitatorSpeed { get; set; }
        public double RawMatTemp { get; set; }
        public DateTime MillingFinishTime { get; set; }
        public double MillingRunTime { get; set; }
        //public bool Issue { get; set; } = false;
        public static string[] properties { get; } = { "Actual Weight", "Agitator Speed", "Milling Runtime", "Raw Material Temp", "Target Weight", "Vessel Temp", "Wait Time", "Weigh Time" };
    }
}
