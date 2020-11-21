using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BatchDataAccessLibrary.Models
{
    public class PcsScoring
    {
        public int PcsScoringId { get; set; }
        [Display(Name = "Score 2 % Target")]
        [Column(TypeName = "decimal(6,2)")]
        public decimal Score2Target { get; set; }
        [Display(Name = "Score 1 % Target")]
        [Column(TypeName = "decimal(6,2)")]
        public decimal Score1Lower { get; set; }
    }
}
