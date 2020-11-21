using BatchDataAccessLibrary.Enums;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchDataAccessLibrary.Repositories.PcsCompliance.DemoMocks
{
    public class DemoMockPcsParameterRepository : IPcsWeightParameterRepository
    {
        readonly List<PcsWeightParameters> parameters;

        public DemoMockPcsParameterRepository()
        {
            parameters = new List<PcsWeightParameters>
            {
               new PcsWeightParameters { PcsWeightParametersId = 1, RecipeType = RecipeTypes.Conc, Parameter = "Material 51" },
               new PcsWeightParameters { PcsWeightParametersId = 2, RecipeType = RecipeTypes.BigBang, Parameter = "Material 51" },
               new PcsWeightParameters { PcsWeightParametersId = 3, RecipeType = RecipeTypes.Reg, Parameter = "Material 51"  } ,
               new PcsWeightParameters { PcsWeightParametersId = 4, RecipeType = RecipeTypes.Conc, Parameter = "Material 59"  } ,
               new PcsWeightParameters { PcsWeightParametersId = 5, RecipeType = RecipeTypes.BigBang, Parameter = "Material 59"  },
               new PcsWeightParameters { PcsWeightParametersId = 6, RecipeType = RecipeTypes.Reg, Parameter = "Material 59"  },
               new PcsWeightParameters { PcsWeightParametersId = 7, RecipeType = RecipeTypes.Reg, Parameter = "Material 21" }
            };
        }

        public void Add(PcsWeightParameters parameter)
        {
            parameters.Add(parameter);
        }

        public bool Any(int id)
        {
            return parameters.Any(param => param.PcsWeightParametersId == id);
        }


        public async Task<PcsWeightParameters> FindAsync(int? id)
        {
            Task<PcsWeightParameters> task = Task.Run(() =>
            {
                return parameters.Find(x => x.PcsWeightParametersId == id);
            });

            return await task;
        }

        public List<PcsWeightParameters> GetAllParameters()
        {
            return parameters;
        }

        public List<PcsWeightParameters> GetParametersForRecipeType(RecipeTypes recipeType)
        {
            return parameters.Where(x => x.RecipeType == recipeType).ToList();
        }

        public EntityEntry<PcsWeightParameters> Remove(PcsWeightParameters pcsWeightParameters)
        {
            parameters.Remove(pcsWeightParameters);
            return null;
        }

        public Task SaveChangesAsync()
        {
            return Task.Run(() => 1);
        }

        public EntityEntry<PcsWeightParameters> Update(PcsWeightParameters pcsWeightParameter)
        {
            PcsWeightParameters existingParam = parameters.Find(param => param.PcsWeightParametersId == pcsWeightParameter.PcsWeightParametersId);
            existingParam.Parameter = pcsWeightParameter.Parameter;
            existingParam.RecipeType = pcsWeightParameter.RecipeType;
            return null;
        }
    }
}
