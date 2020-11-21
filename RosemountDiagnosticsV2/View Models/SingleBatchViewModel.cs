using BatchDataAccessLibrary.Models;
using System.Collections.Generic;

namespace RosemountDiagnosticsV2.View_Models
{
    public class SingleBatchViewModel
    {
        public BatchReport Report { get; set; }
        public RecipeLimits RecipeViscoLimits { get; set; }
        public RecipeLimits BatchTimeLimits { get; set; }
        public List<BatchIssue> TimeIssues { get; set; }
        public List<BatchIssue> QualityIssues { get; set; }
        public List<BatchIssue> MatVarIssues { get; set; }
        public double TotalTimeLost { get; set; }
        public int TotalQualityIssues { get; set; }
        public int TotalMatvarIssues { get; set; }
    }
}
