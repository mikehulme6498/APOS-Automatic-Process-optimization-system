using BatchDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using RosemountDiagnosticsV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RosemountDiagnosticsV2.Interfaces
{
    public interface IDateSelector
    {
        string TimeFrame { get; set; }
        DateTime DateFrom { get; set; }
        DateTime DateTo { get; set; }
        string TimeFrameTitle { get; set; }
        int Year { get; set; }
        int Week { get; set; }
        int YearForWeek { get; set; }
        bool TimeSet { get; set; }
        List<SelectListItem> YearsAvailable { get; set; }
        List<SelectListItem> WeeksAvailable { get; set; }
        List<SelectListItem> MaterialsForDropDown { get; }
        void SetDateSelecter(DateSelectorModal dateSelector);
        DateSelectorModal GetDateSelectorDetails();
        //List<BatchReport> GetBatchesForSelectedDates(); 
    }
}
