using BatchDataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BatchDataAccessLibrary.Models
{
    [DebuggerDisplay("{ MaterialName } - { FaultType }")]
    public class BatchIssue
    {
        public enum FaultTypes
        {
            WeighTime,
            WaitTime,
            Overweigh,
            AcquireTime,
            Underweigh,
            TemperatureHigh,
            TemperatureLow,
            Quality,
            BatchAdjusted,
            NoIssue,
            Other
        };
        public int BatchIssueId { get; set; }
       
        [Required]
        [MaxLength(200)]
        public string MaterialName { get; set; }
        
        public string MaterialShortName { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public double TimeLost { get; set; }
        public double PercentOut { get; set; }
        public double ActualReading { get; set; }
        public double WeightDiffference { get; set; }
        [Required]
        public FaultTypes FaultType { get; set; }
        public bool RemoveIssue { get; set; } = false;
        public string ReasonRemoved { get; set; }
        public string IssueRemovedBy { get; set; }
        public string IssueCreatedBy { get; set; }

    }
}
