using BatchDataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RosemountDiagnosticsV2.Models;
using System;
using System.Diagnostics;
using System.Linq;

namespace RosemountDiagnosticsV2.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBatchRepository _BatchRepository;
        private readonly IMaterialDetailsRepository _materialDetailsRepository;

        public HomeController(IBatchRepository batchRepository, IMaterialDetailsRepository materialDetailsRepository)
        {
            _BatchRepository = batchRepository;
            _materialDetailsRepository = materialDetailsRepository;

            if (!_materialDetailsRepository.GetMaterialNamesThatAreIncludedInMatVar().Any())
            {
                _materialDetailsRepository.LoadMaterialData();
            }
        }

        public IActionResult Index()
        {
            ViewData["currentWeek"] = BatchDataAccessLibrary.Helpers.HelperMethods.GetWeekNumber(DateTime.Now);
            return View();
        }

        ////CHECKS AND ADDS NEW MATERIALS TO DATABASE IF FOUND IN REPORTS

        //private void CheckForMissingMaterials()
        //{
        //    List<BatchReport> reports = _BatchRepository.GetBatchesByDates(new DateTime(2020, 4, 1), new DateTime(2020, 7, 1));
        //    MaterialsFound materialsFound = new MaterialsFound(_materialDetailsRepository);

        //    foreach (var report in reports)
        //    {
        //        foreach (var vessel in report.AllVessels)
        //        {
        //            foreach (var material in vessel.Materials)
        //            {
        //                materialsFound.AddNewMaterial(material.Name);
        //            }
        //        }
        //    }
        //    materialsFound.AddNewMaterialsToDB();
        //}

        public IActionResult LiveFeed()
        {
            ViewData["currentWeek"] = BatchDataAccessLibrary.Helpers.HelperMethods.GetWeekNumber(DateTime.Now);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public JsonResult GetWeeksAvailableInYear(int year)
        {
            var weeks = _BatchRepository.GetWeeksInSystemForDropDown(year).ToList();

            return Json(weeks);
        }

        //[Route("create-guage")]
        public IActionResult CreateGuage(int recipetype, string guageTitle, double guageValue, int limittype)
        {
            return ViewComponent("CreateGuage", new { recipeType = recipetype, title = guageTitle, value = guageValue, limitType = limittype, height = 75, width = 120 });
        }


    }
}
