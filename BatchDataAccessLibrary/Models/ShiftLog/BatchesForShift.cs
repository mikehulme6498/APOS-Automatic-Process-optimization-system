namespace BatchDataAccessLibrary.Models.ShiftLog
{
    public class BatchesForShift
    {
        public int BatchesForShiftID { get; set; }
        public int ShiftId { get; set; }
        public int BatchId { get; set; }
        public bool StreamWash { get; set; }
        public bool StockTankWash { get; set; }
        public string StreamWashVesselName { get; set; }
        public string StockTankWashVesselName { get; set; }
    }
}
