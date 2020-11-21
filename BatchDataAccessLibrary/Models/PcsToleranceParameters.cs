using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BatchDataAccessLibrary.Models
{
    public class PcsToleranceParameters
    {
        public int PcsToleranceParametersId { get; set; }
        [Display(Name ="Tolerance %")]
        [Column(TypeName = "decimal(6,2)")]
        public decimal TolerancePercent { get; set; }
        [Display(Name = "Out Of Range %")]
        [Column(TypeName = "decimal(6,2)")]
        public decimal OutOfRangePercent { get; set; }
    }
}
