using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchDataAccessLibrary.Repositories.PcsCompliance.DemoMocks
{
    public class DemoMockPcsReworkRepository : IPcsReworkParameters
    {
        readonly List<PcsReworkParameters> reworkParams;

        public DemoMockPcsReworkRepository()
        {
            reworkParams = new List<PcsReworkParameters>
            {
                new PcsReworkParameters { PcsReworkParametersId = 1, RecipeName = "Recipe 17", TargetReworkAmount = 1200M },
                new PcsReworkParameters { PcsReworkParametersId = 2, RecipeName = "Recipe 26", TargetReworkAmount = 400M },
                new PcsReworkParameters { PcsReworkParametersId = 3, RecipeName = "BB-Recipe 15", TargetReworkAmount = 400M },
                new PcsReworkParameters { PcsReworkParametersId = 4, RecipeName = "BB-Recipe 11", TargetReworkAmount = 400M },
                new PcsReworkParameters { PcsReworkParametersId = 5, RecipeName = "RE-Recipe 18", TargetReworkAmount = 750M }
            };
        }

        public void Add(PcsReworkParameters pcsReworkParameters)
        {
            reworkParams.Add(pcsReworkParameters);
        }

        public bool Any(int id)
        {
            return reworkParams.Any(x => x.PcsReworkParametersId == id);
        }

        public async Task<PcsReworkParameters> FindAsync(int? id)
        {
            Task<PcsReworkParameters> task = Task.Run(() =>
            {
                return reworkParams.Find(x => x.PcsReworkParametersId == id);
            });

            return await task;
        }

        public List<PcsReworkParameters> GetListOfAllReworkParemeters()
        {
            return reworkParams;
        }

        public EntityEntry<PcsReworkParameters> Remove(PcsReworkParameters pcsReworkParameters)
        {
            reworkParams.Remove(pcsReworkParameters);
            return null;
        }

        public Task<int> SaveChangesAsync()
        {
            return Task.Run(() => 1);
        }

        public Task<List<PcsReworkParameters>> ToListAsync()
        {
            throw new NotImplementedException();
        }

        public EntityEntry<PcsReworkParameters> Update(PcsReworkParameters pcsReworkParameters)
        {
            PcsReworkParameters existingParam = reworkParams.Find(param => param.PcsReworkParametersId == pcsReworkParameters.PcsReworkParametersId);
            existingParam.RecipeName = pcsReworkParameters.RecipeName;
            existingParam.TargetReworkAmount = pcsReworkParameters.TargetReworkAmount;
            return null;
        }
    }
}
