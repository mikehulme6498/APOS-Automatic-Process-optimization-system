using BatchDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace BatchDataAccessLibrary.Interfaces
{
    public interface IBatchRepository
    {
        IEnumerable<BatchReport> AllBatches { get; }
        BatchReport GetBatchById(int id);
        BatchReport GetBatchByBatchNumber(string batchNum, int year);
        List<BatchReport> GetBatchesByWeek(int weekNum, int year);
        List<BatchReport> GetBatchesByYear(int year);
        List<BatchReport> GetBatchesByDates(DateTime dateFrom, DateTime dateTo);
        Task<bool> BatchExists(int campaign, int batch, DateTime startDateTime);
        List<int> GetYearsInSystemForDropDown();
        List<int> GetWeeksInSystemForDropDown(int year);
        List<SelectListItem> GetRecipeNamesForDropDown();
        List<string> GetAllRecipeNames();
        void Add(BatchReport report);
        void AddRange(List<BatchReport> reports);
        void AddConversionFaults(List<BatchConversionFault> faults);
        void SaveChanges(BatchReport report = null);

    }
}
