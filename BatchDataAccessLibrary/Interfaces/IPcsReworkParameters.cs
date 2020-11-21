using BatchDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BatchDataAccessLibrary.Interfaces
{
    public interface IPcsReworkParameters
    {
        List<PcsReworkParameters> GetListOfAllReworkParemeters();
        Task<List<PcsReworkParameters>> ToListAsync();
        void Add(PcsReworkParameters pcsReworkParameters);
        Task<int> SaveChangesAsync();
        Task<PcsReworkParameters> FindAsync(int? id);
        EntityEntry<PcsReworkParameters> Update(PcsReworkParameters pcsReworkParameters);
        EntityEntry<PcsReworkParameters> Remove(PcsReworkParameters pcsReworkParameters);
        bool Any(int id);

    }
}
