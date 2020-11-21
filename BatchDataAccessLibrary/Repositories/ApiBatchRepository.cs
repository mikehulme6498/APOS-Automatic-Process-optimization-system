using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BatchDataAccessLibrary.Repositories
{
    public class ApiBatchRepository : IApiBatchRepository
    {
        private readonly List<BatchReport> reports;
        private readonly IBatchRepository _batchRepository;

        public ApiBatchRepository(IBatchRepository batchRepository)
        {
            reports = new List<BatchReport>();
            _batchRepository = batchRepository;
        }
        public BatchReport AddBatchToDb(BatchReport report)
        {
            //bool batchExists = await _batchRepository.BatchExists(report.Campaign, report.BatchNo, report.StartTime);
            _batchRepository.Add(report);
            _batchRepository.SaveChanges();
            //int id = report.BatchReportId;
            return report;

        }

        public List<BatchReport> AllBatches()
        {
            return _batchRepository.AllBatches.ToList();
        }

        public List<string> GetBatchInfoListForSync()
        {
            List<BatchReport> reports = _batchRepository.AllBatches.ToList();
            List<String> batchInfo = new List<string>();
            foreach (var report in reports)
            {
                batchInfo.Add($"{report.Campaign}-{report.BatchNo}-{report.StartTime}");
            }
            return batchInfo;
        }


    }
}
