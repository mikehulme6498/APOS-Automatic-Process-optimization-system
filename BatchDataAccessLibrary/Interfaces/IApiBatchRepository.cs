using BatchDataAccessLibrary.Models;
using System.Collections.Generic;

namespace BatchDataAccessLibrary.Interfaces
{
    public interface IApiBatchRepository
    {
        BatchReport AddBatchToDb(BatchReport report);
        List<string> GetBatchInfoListForSync();
        List<BatchReport> AllBatches();

    }
}
