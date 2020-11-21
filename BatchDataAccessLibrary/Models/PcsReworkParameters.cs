using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BatchDataAccessLibrary.Models
{
    public class PcsReworkParameters
    {
        public int PcsReworkParametersId { get; set; }
        [Display(Name ="Recipe Name")]
        public string RecipeName { get; set; }
        [Display(Name = "Target Rework Amount (kg)")]
        [Column(TypeName = "decimal(6,2)")]
        public decimal TargetReworkAmount { get; set; }
    }
}
