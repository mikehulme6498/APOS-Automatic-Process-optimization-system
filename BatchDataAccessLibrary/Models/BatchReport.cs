using BatchDataAccessLibrary.Enums;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BatchDataAccessLibrary.Models
{
    [DebuggerDisplay("Batch { Campaign } - { BatchNo } : { Recipe }")]
    public class BatchReport
    {

        public int BatchReportId { get; set; }
        
        public string OriginalReport { get; set; }
        [MaxLength(15)]
        public string StreamName { get; set; }
        [Required]
        [MaxLength(100)]
        public string Recipe { get; set; }
        [Required]
        public int Campaign { get; set; }
        [Required]
        public int BatchNo { get; set; }
        [Required]
        public int WeekNo { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public double MakingTime { get; set; }
        [Required]
        public double QATime { get; set; }
        public double PreQaTemp { get; set; }
        [Required]
        public double Visco { get; set; }
        [Required]
        public double Ph { get; set; }

        [Required]
        public double SG { get; set; }
        [Required]
        [MaxLength(10)]
        public string Appearance { get; set; }
        [Required]
        [MaxLength(10)]
        public string VisualColour { get; set; }
        [Required]
        [MaxLength(10)]
        public string MeasuredColour { get; set; }
        [Required]
        [MaxLength(10)]
        public string Odour { get; set; }
        [Required]
        [MaxLength(10)]
        public string OverallQAStatus { get; set; }
        [Required]
        public double StockTankAllocationTime { get; set; }
        [MaxLength(30)]
        public string AllocatedTo { get; set; }
        [Required]
        public double DropTime { get; set; }
        [Required]
        public double TotalRecipeWeight { get; set; }
        [Required]
        public double TotalActualWeight { get; set; }
        [Required]
        public double VesselWeightIncrease { get; set; }
        public RecipeTypes RecipeType { get; set; }
        [NotMapped]
        public bool IsBatchAdjusted { get; set; } = false;
        [NotMapped]
        public bool IsValidBatch { get; set; } = true;
        [NotMapped]
        [MaxLength(300)]
        public string FileName { get; set; }
        public double NewMakeTime { get; set; }
        public List<Vessel> AllVessels { get; set; }
        public List<BatchIssue> BatchIssues { get; set; }
        public List<IssuesScannedFor> IssuesScannedFor { get; set; }
        public List<BatchConversionFault> ConversionFaults { get; set; }

        public BatchReport()
        {
            BatchIssues = new List<BatchIssue>();
            AllVessels = new List<Vessel>();
            ConversionFaults = new List<BatchConversionFault>();
            IssuesScannedFor = new List<IssuesScannedFor>();
        }

        public double GetReworkAmount()
        {
            Vessel vessel = AllVessels.Where(x => x.VesselType == Vessel.VesselTypes.MainMixer).FirstOrDefault();
            foreach (var material in vessel.Materials)
            {
                if(material.Name == "WASHINGS RWK" || material.Name == "BB CIP")
                {
                    return material.TargetWeight;
                }
            }
            return 0;
        }

        public double GetTotalBatchWeightInKg()
        {
            return TotalActualWeight * 1000;
        }
    }
}
