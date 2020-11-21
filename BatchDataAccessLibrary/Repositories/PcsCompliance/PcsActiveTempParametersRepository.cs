using BatchDataAccessLibrary.DataAccess;
using BatchDataAccessLibrary.Enums;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace BatchDataAccessLibrary.Repositories
{
    public class PcsActiveTempParametersRepository : IPcsActiveTempParameters
    {
        private readonly BatchContext _batchContext;

        public PcsActiveTempParametersRepository(BatchContext batchContext)
        {
            _batchContext = batchContext;
        }

        public PcsTempTargets GetTargetsFor(string recipeName)
        {
            return _batchContext.PcsTempsTargets.Where(x => x.Recipe == recipeName).FirstOrDefault();
        }

        //public List<decimal> GetListOfDistinctTempsForRecipe(RecipeTypes recipeType)
        //{
        //    return _batchContext.PcsTempsTargets.Where(x => x.RecipeType == recipeType).Select(x => x.Target).Distinct().ToList();
        //}

        public Dictionary<decimal, List<PcsTempTargets>> GetListOfTargetsSeperatedByTargetTemps(RecipeTypes recipeType)
        {
            var recipeByType = _batchContext.PcsTempsTargets.Where(x => x.RecipeType == recipeType).ToList();
            var recipesSeperatedByTargets = recipeByType.GroupBy(x => x.Target).ToDictionary(group => group.Key, group => group.ToList());
            return recipesSeperatedByTargets;
        }

        public async Task<List<PcsTempTargets>> GetAllTempParameters()
        {
            return await _batchContext.PcsTempsTargets.ToListAsync();
        }
    }
}
