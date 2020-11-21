using BatchDataAccessLibrary.DataAccess;
using BatchDataAccessLibrary.Enums;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchDataAccessLibrary.Repositories
{
    public class PcsParametersRepository : IPcsWeightParameterRepository
    {
        private readonly BatchContext _batchContext;

        public PcsParametersRepository(BatchContext batchContext)
        {
            _batchContext = batchContext;
        }

        public void Add(PcsWeightParameters parameter)
        {
            _batchContext.Add(parameter);
        }

       

        public List<PcsWeightParameters> GetAllParameters()
        {
            return _batchContext.PcsParameters.ToList();
        }

        public List<PcsWeightParameters> GetParametersForRecipeType(RecipeTypes recipeType)
        {
            return _batchContext.PcsParameters.Where(x => x.RecipeType == recipeType).ToList();
        }

        public async Task SaveChangesAsync()
        {
            await _batchContext.SaveChangesAsync();
        }
        public async Task<PcsWeightParameters> FindAsync(int? id)
        {
            return await _batchContext.PcsParameters.FindAsync(id);
        }
        public EntityEntry<PcsWeightParameters> Update(PcsWeightParameters pcsWeightParameters)
        {
            return _batchContext.Update(pcsWeightParameters);
        }

        public EntityEntry<PcsWeightParameters> Remove(PcsWeightParameters pcsWeightParameters)
        {
            return _batchContext.PcsParameters.Remove(pcsWeightParameters);
        }

        public bool Any(int id)
        {
            return _batchContext.PcsParameters.Any(param => param.PcsWeightParametersId == id);
        }
    }
}
