using System.Collections.Generic;
using BatchDataAccessLibrary.Models;

namespace BatchDataAccessLibrary.Interfaces
{
    public interface IBatchDataFileManager
    {
        List<BatchReport> ProcessStringIntoBatchReports(string textfromAllfiles);
    }
}