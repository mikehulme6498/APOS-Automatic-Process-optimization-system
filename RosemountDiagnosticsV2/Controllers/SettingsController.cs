using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BatchDataAccessLibrary.DataAccess;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using BatchDataAccessLibrary.Repositories;
using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RosemountDiagnosticsV2.Models.Settings;

namespace RosemountDiagnosticsV2.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IMaterialDetailsRepository _materialDetailsRepository;

        public SettingsController(IMaterialDetailsRepository materialDetailsRepository)
        {
            _materialDetailsRepository = materialDetailsRepository;
        }
        [HttpGet]
        public IActionResult Index()
        {
            //var temp = GetActiveTemps("KING-FT JUST TEMPS.xlsx");
            ViewBag.NewMaterialsFound = _materialDetailsRepository.GetMaterialsThatNeedDetailsEntering().Any();
            return View();
        }

        //private List<string> GetActiveTemps(string fName)
        //{
        //    List<PcsActiveDropTemps> dropTemps = new List<PcsActiveDropTemps>();
        //    List<string> seedData = new List<string>();
        //    DataSet dataSet = new DataSet();
        //    var fileName = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + fName;
        //    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        //    using (var stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read))
        //    {

        //        using (var reader = ExcelReaderFactory.CreateOpenXmlReader(stream))
        //        {
        //            dataSet = reader.AsDataSet();

        //            int columns = dataSet.Tables[0].Columns.Count;

        //            for (int i=0; i<columns; i++)
        //            {
        //                dropTemps.Add(new PcsActiveDropTemps()
        //                {
        //                    Recipe = dataSet.Tables[0].Rows[0][i].ToString(),
        //                    UpperLimit = Convert.ToDecimal(dataSet.Tables[0].Rows[1][i]),
        //                    LowerLimit = Convert.ToDecimal(dataSet.Tables[0].Rows[2][i]),
        //                    Target = Convert.ToDecimal(dataSet.Tables[0].Rows[3][i]),
        //                });                   
        //            }
        //            //modelBuilder.Entity<PcsTempTargets>().HasData(new PcsTempTargets { PcsTempTargetsId = 1, Recipe = "BULCON", UpperLimit = 12m, LowerLimit = 12m });

        //                var test = JsonConvert.SerializeObject(dropTemps);
        //            for(int i=0; i< dropTemps.Count; i++)
        //            {
        //                int id = i + 1;
        //                string data = "modelBuilder.Entity<PcsTempTargets>().HasData(new PcsTempTargets { PcsTempTargetsId = " + id + " ,Recipe = " + "'" + dropTemps[i].Recipe + "'" + ", UpperLimit = "+ dropTemps[i].UpperLimit +"M, LowerLimit = " + dropTemps[i].LowerLimit +"M, Target = " + dropTemps[i].Target +"M });";
        //                seedData.Add(data);
        //            }

        //        }
        //    }

        //    return seedData;
        //}      


    }
}