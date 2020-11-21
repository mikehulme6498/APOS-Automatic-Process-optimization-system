using BatchDataAccessLibrary.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BatchDataAccessLibrary.Models
{
    public class RecipeLimits
    {       
        public int RecipeLimitsId { get; set; }
        public RecipeTypes RecipeType { get; set; }
        public LimitType LimitTypes { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public decimal Min { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public decimal Max { get; set; }
        public int GuageMax { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public decimal Target { get; set; }
        public int Tolerance { get; set; }
    }
}
