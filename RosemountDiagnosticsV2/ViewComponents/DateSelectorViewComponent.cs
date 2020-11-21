using BatchDataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Newtonsoft.Json;
using RosemountDiagnosticsV2.Models;
using System.Linq;

namespace RosemountDiagnosticsV2.ViewComponents
{
    public class DateSelectorViewComponent : ViewComponent
    {
        private readonly IBatchRepository _BatchRepository;

        public DateSelectorViewComponent(IBatchRepository batchRepository)
        {
            _BatchRepository = batchRepository;
        }
        public IViewComponentResult Invoke(DateSelectorModal dateSelectorModal)
        {
            foreach (var year in _BatchRepository.GetYearsInSystemForDropDown())
            {
                dateSelectorModal.YearsAvailable.Add(new SelectListItem { Text = year.ToString(), Value = year.ToString() });
            }

            foreach (var week in _BatchRepository.GetWeeksInSystemForDropDown(dateSelectorModal.YearForWeek))
            {
                dateSelectorModal.WeeksAvailable.Add(new SelectListItem { Text = week.ToString(), Value = week.ToString() });
            }

            return View(dateSelectorModal);
        }

        public IViewComponentResult GetWeeksAvailableInYear(int year)
        {
            var weeks = _BatchRepository.GetWeeksInSystemForDropDown(year).ToList();
            return new ContentViewComponentResult(JsonConvert.SerializeObject(weeks));
        }


    }
}
