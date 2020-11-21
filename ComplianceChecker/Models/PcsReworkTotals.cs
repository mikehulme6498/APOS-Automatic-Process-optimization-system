namespace BatchReports.ComplianceChecker.Models
{
    public class PcsReworkTotals
    {
        public string RecipeName { get; set; }
        public decimal ExpectedReworkAmount { get; set; }
        public decimal ActualReworkAmount { get; set; }
        public int BatchesMade { get; set; }
        public int BatchesWithRework { get; set; }
        public int BatchesWithCorrectAmountOrOver { get; set; }
    }
}