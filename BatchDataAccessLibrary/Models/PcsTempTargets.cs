using BatchDataAccessLibrary.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BatchDataAccessLibrary.Models
{
    public class PcsTempTargets
    {
        public int PcsTempTargetsId { get; set; }
        public RecipeTypes RecipeType { get; set; }
        public string Recipe { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public decimal Target { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public decimal UpperLimit { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public decimal LowerLimit { get; set; }
    }
}
