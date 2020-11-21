using BatchDataAccessLibrary.Enums;
using BatchDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchDataAccessLibrary.Interfaces
{
    public interface IPcsActiveTempParameters
    {
        PcsTempTargets GetTargetsFor(string recipeName);
        Dictionary<decimal, List<PcsTempTargets>> GetListOfTargetsSeperatedByTargetTemps(RecipeTypes recipeType);
        Task<List<PcsTempTargets>> GetAllTempParameters();
    }
}
