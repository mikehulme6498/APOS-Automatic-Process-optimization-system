using BatchDataAccessLibrary.DataAccess;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BatchDataAccessLibrary.Helpers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BatchDataAccessLibrary.Repositories
{
    public class BatchReportRepository : IBatchRepository
    {
        private readonly BatchContext _context;

        public BatchReportRepository(BatchContext context)
        {
            _context = context;
        }

        public IEnumerable<BatchReport> AllBatches =>       
            
            
            _context.BatchReports
                .Include(x => x.BatchIssues)
                .Include(x => x.IssuesScannedFor)
                .Include(x => x.AllVessels)
                .ThenInclude(x => x.Materials)
                .ToList();


        public BatchReport GetBatchById(int id)
        {
            return _context.BatchReports
                .Include(x => x.BatchIssues)
                .Include(x => x.IssuesScannedFor)
                .Include(x => x.AllVessels)
                .ThenInclude(x => x.Materials)
                .Where(x => x.BatchReportId == id)
                .FirstOrDefault();
        }
        public BatchReport GetBatchByBatchNumber(string batchNum, int year)
        {
            int campaign = Convert.ToInt32(batchNum.Substring(0, batchNum.IndexOf('-')));
            int batch = Convert.ToInt32(batchNum.Substring(batchNum.IndexOf('-')+1));

            return _context.BatchReports
                .Include(x => x.BatchIssues)
                .Include(x => x.IssuesScannedFor)
                .Include(x => x.AllVessels)
                .ThenInclude(x => x.Materials)
                .Where(x => x.Campaign == campaign && x.BatchNo == batch && x.StartTime.Year == year)
                .FirstOrDefault();
        }

        public List<BatchReport> GetBatchesByWeek(int weekNum, int year)
        {
            return _context.BatchReports
                .Include(x => x.BatchIssues)
                .Include(x => x.IssuesScannedFor)
                .Include(x => x.AllVessels)
                .ThenInclude(x => x.Materials)
                .Where(x => x.WeekNo == weekNum && x.StartTime.Year == year)
                .ToList();
        }

        public async Task<bool> BatchExists(int campaign, int batch, DateTime startDateTime)
        {
            BatchReport report = await _context.BatchReports.FirstOrDefaultAsync(x => x.Campaign == campaign && x.BatchNo == batch && x.StartTime == startDateTime);

            return report != null;
        }

        public void Add(BatchReport report)
        {
            _context.Add(report);
        }

        public void AddRange(List<BatchReport> reports)
        {
            _context.AddRange(reports);
        }

        public void AddConversionFaults(List<BatchConversionFault> faults)
        {
            _context.ConversionFaults.AddRange(faults);
        }


        public void SaveChanges(BatchReport report = null)
        {
            _context.SaveChanges();
        }

        public List<int> GetYearsInSystemForDropDown()
        {
            var years = _context.BatchReports.OrderByDescending(x => x.StartTime.Year).Select(x => x.StartTime.Year).Distinct().ToList();
            List<int> yearsAvailable = new List<int>();
            foreach (var year in years)
            {
                yearsAvailable.Add(year);
            }
            return yearsAvailable;
        }
        public List<int> GetWeeksInSystemForDropDown(int year)
        {
            var weeks = _context.BatchReports.Where(x => x.StartTime.Year == year)
                .OrderByDescending(x => x.WeekNo)
                .Select(x => x.WeekNo)
                .Distinct()
                .ToList();
            return weeks;
        }

        public List<BatchReport> GetBatchesByYear(int year)
        {
            return _context.BatchReports
                .Include(x => x.BatchIssues)
                .Include(x => x.IssuesScannedFor)
                .Include(x => x.AllVessels)
                .ThenInclude(x => x.Materials)
                .Where(x => x.StartTime.Year == year)
                .ToList();
        }

        public List<BatchReport> GetBatchesByDates(DateTime dateFrom, DateTime dateTo)
        {
            return _context.BatchReports
                .Include(x => x.BatchIssues)
                .Include(x => x.IssuesScannedFor)
                .Include(x => x.AllVessels)
                .ThenInclude(x => x.Materials)
                .Where(x => x.StartTime >= dateFrom && x.StartTime <= dateTo)
                .ToList();
        }

        public List<string> GetAllRecipeNames()
        {
            return _context.BatchReports
                .Select(x => x.Recipe)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
        }

        public List<SelectListItem> GetRecipeNamesForDropDown()
        {
            List<SelectListItem> recipeNames = new List<SelectListItem>();
            foreach (var recipeName in GetAllRecipeNames())
            {
                recipeNames.Add(new SelectListItem { Text = recipeName, Value = recipeName });
            }
           return recipeNames;
        }
    }

}
