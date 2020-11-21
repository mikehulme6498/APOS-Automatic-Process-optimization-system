using BatchDataAccessLibrary.Enums;
using BatchDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BatchDataAccessLibrary.Interfaces
{
    public interface IPcsWeightParameterRepository
    {
        List<PcsWeightParameters> GetParametersForRecipeType(RecipeTypes recipeType);
        void Add(PcsWeightParameters parameter);
        Task SaveChangesAsync();
        Task<PcsWeightParameters> FindAsync(int? id);
        EntityEntry<PcsWeightParameters> Update(PcsWeightParameters pcsWeightParameter);
        List<PcsWeightParameters> GetAllParameters();
        EntityEntry<PcsWeightParameters> Remove(PcsWeightParameters pcsWeightParameters);
        bool Any(int id);

    }
}
