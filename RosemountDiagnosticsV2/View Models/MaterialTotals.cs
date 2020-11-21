using System.Diagnostics;

namespace RosemountDiagnosticsV2.View_Models
{
    [DebuggerDisplay("Name { Name } - { TotalKg } Kg - { Cost }")]
    public class MaterialTotals
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int ProductCode { get; set; }
        public double TotalKg { get; set; }
        public double Cost { get; set; }
        public double CostPerTon { get; set; }
    }
}