using BatchDataAccessLibrary.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace BatchDataAccessLibrary.Models.ShiftLog
{
    public class GoodStockToWaste
    {
        public int GoodStockToWasteId { get; set; }
        public int ShiftLogId { get; set; }
        public DateTime Date { get; set; }
        [Display(Name = "Amount Kg")]
        public double Amount { get; set; }
        [Display(Name = "Recipe Name")]
        public string RecipeName { get; set; }
        public WasteReason Reason { get; set; }
        public string Comment { get; set; }

    }
}
