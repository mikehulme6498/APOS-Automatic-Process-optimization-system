using BatchDataAccessLibrary.DataAccess;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BatchDataAccessLibrary.Repositories
{
    public class PcsReworkRepository : IPcsReworkParameters
    {
        private readonly BatchContext _batchContext;

        public PcsReworkRepository(BatchContext batchContext)
        {
            _batchContext = batchContext;
        }

        public void Add(PcsReworkParameters pcsReworkParameters)
        {
            _batchContext.Add(pcsReworkParameters);
        }

        public bool Any(int id)
        {
            return _batchContext.PcsReworkParameters.Any(x => x.PcsReworkParametersId == id);
        }

        public async Task<PcsReworkParameters> FindAsync(int? id)
        {
            return await _batchContext.PcsReworkParameters.FindAsync(id);
        }

        public List<PcsReworkParameters> GetListOfAllReworkParemeters()
        {
            return _batchContext.PcsReworkParameters.ToList();
        }

        public EntityEntry<PcsReworkParameters> Remove(PcsReworkParameters pcsReworkParameters)
        {
            return _batchContext.Remove(pcsReworkParameters);
        }

        public Task<int> SaveChangesAsync()
        {
            return _batchContext.SaveChangesAsync();
        }

        public Task<List<PcsReworkParameters>> ToListAsync()
        {
            return _batchContext.PcsReworkParameters.ToListAsync();
        }

        public EntityEntry<PcsReworkParameters> Update(PcsReworkParameters pcsReworkParameters)
        {
            return _batchContext.Update(pcsReworkParameters);
        }
    }
}
