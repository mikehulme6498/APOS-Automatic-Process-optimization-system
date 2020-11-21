using Microsoft.AspNetCore.Mvc.Rendering;
using RosemountDiagnosticsV2.Interfaces;
using System;
using System.Collections.Generic;

namespace RosemountDiagnosticsV2.Models
{
    public class DateSelectorModal : IDateSelector
    {
        //private readonly IBatchRepository _batchRepository;

        public string TimeFrame { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string TimeFrameTitle { get; set; }

        public int Year { get; set; }
        public int Week { get; set; }
        public int YearForWeek { get; set; }
        //public string Name { get; set; }
        public List<SelectListItem> YearsAvailable { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> WeeksAvailable { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> MaterialsForDropDown { get; private set; } = new List<SelectListItem>();
        public bool TimeSet { get; set; } = false;

        public DateSelectorModal()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
        }
        public DateSelectorModal GetDateSelectorDetails()
        {
            return this;
        }

        public void SetDateSelecter(DateSelectorModal dateSelector)
        {
            TimeFrame = dateSelector.TimeFrame;
            DateFrom = dateSelector.DateFrom;
            DateTo = dateSelector.DateTo;
            TimeFrameTitle = dateSelector.TimeFrameTitle;
            Year = dateSelector.Year;
            Week = dateSelector.Week;
            YearForWeek = dateSelector.YearForWeek;
            YearsAvailable = dateSelector.YearsAvailable;
            WeeksAvailable = dateSelector.WeeksAvailable;
            MaterialsForDropDown = dateSelector.MaterialsForDropDown;
            SetTimeFrameTitle();
        }

        private void SetTimeFrameTitle()
        {

            switch (TimeFrame)
            {
                case "year":
                    TimeFrameTitle = Year.ToString();
                    break;
                case "week":
                    TimeFrameTitle = YearForWeek.ToString() + " Week : " + Week;
                    break;
                case "dates":
                    TimeFrameTitle = DateFrom.ToShortDateString() + " To " + DateTo.ToShortDateString();
                    break;
                default:
                    break;
            }
        }

        //public List<BatchReport> GetBatchesForSelectedDates()
        //{
        //    switch (TimeFrame)
        //    {
        //        case "year":
        //            return _batchRepository.GetBatchesByYear(Year);
        //        case "week":
        //            return _batchRepository.GetBatchesByWeek(Week, Year);
        //        case "dates":
        //            return _batchRepository.GetBatchesByDates(DateFrom, DateTo);
        //        default:
        //            return null;
        //    }
        //}
    }
}
