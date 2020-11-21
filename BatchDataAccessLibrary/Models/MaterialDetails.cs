using BatchDataAccessLibrary.DataAccess;
using BatchDataAccessLibrary.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BatchDataAccessLibrary.Models
{
    public class MaterialDetails
    {
        [DebuggerDisplay("{ParallelWeighGroup}-{ParallelGroupOrder} : {Name}")]
        public int MaterialDetailsId { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        [MaxLength(50)]
        [Display(Name="Short Name")]
        public string ShortName { get; set; }
        [Required]
        [Display(Name = "Product Code")]
        public int ProductCode { get; set; }
        [Required]
        [Display(Name = "Average Weigh Time")]
        public double AvgWeighTime { get; set; }
        [Required]
        [Display(Name = "Average Wait Time")]
        public double AvgWaitTime { get; set; }
        [Display(Name= "Maximum Raw Temp")]
        public double MaxRawTemp { get; set; }
        [Display(Name = "Minimum Raw Temp")]
        public double MinRawTemp { get; set; }
        [Display(Name = "Minimum Drop Temp")]
        public double MinDropTemp { get; set; }
        [Display(Name = "Maximum Drop Temp")]
        public double MaxDropTemp { get; set; }
        [Display(Name = "Cost Per Ton")]
        public double CostPerTon { get; set; }
        [Required]
        [Display(Name = "Include In Mat Var")]
        public bool IncludeInMatVar { get; set; }
        [Display(Name = "Parallel Weigh Group")]
        public int ParallelWeighGroup { get; set; }
        [Display(Name = "Parallel Group Order")]
        public int ParallelGroupOrder { get; set; }
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        [Display(Name = "Needs Details Input")]
        public bool NeedsDetailsInput { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
    }
}
