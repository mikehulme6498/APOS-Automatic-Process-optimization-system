using BatchDataAccessLibrary.Enums;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BatchDataAccessLibrary.Repositories
{
    public class MockRecipeLimitRepository : IRecipeLimitRepository
    {
        List<RecipeLimits> Recipe_Limits;
        public MockRecipeLimitRepository()
        {
            Recipe_Limits = new List<RecipeLimits>();
            CreateMockData();
        }

        private void CreateMockData()
        {
            Recipe_Limits.Add(new RecipeLimits { RecipeLimitsId = 1, RecipeType = RecipeTypes.Conc, LimitTypes= LimitType.MakeTime, Min = 40, Max = 120, Target = 65, Tolerance = 10, GuageMax = 300 });
            Recipe_Limits.Add(new RecipeLimits { RecipeLimitsId = 2, RecipeType = RecipeTypes.BigBang, LimitTypes = LimitType.MakeTime, Min = 40, Max = 120, Target = 65, Tolerance = 10, GuageMax = 300 });
            Recipe_Limits.Add(new RecipeLimits { RecipeLimitsId = 3, RecipeType = RecipeTypes.Reg, LimitTypes = LimitType.MakeTime, Min = 60, Max = 120, Target = 80, Tolerance = 10, GuageMax = 300 });
            Recipe_Limits.Add(new RecipeLimits { RecipeLimitsId = 4, RecipeType = RecipeTypes.Conc, LimitTypes = LimitType.Visco, Min = 25, Max = 45, Target = 35, Tolerance = 5, GuageMax = 60 });
            Recipe_Limits.Add(new RecipeLimits { RecipeLimitsId = 5, RecipeType = RecipeTypes.BigBang, LimitTypes = LimitType.Visco, Min = 30, Max = 70, Target = 50, Tolerance = 5, GuageMax = 80 });
            Recipe_Limits.Add(new RecipeLimits { RecipeLimitsId = 6, RecipeType = RecipeTypes.Reg, LimitTypes = LimitType.Visco, Min = 15, Max = 35, Target = 25, Tolerance = 5, GuageMax = 60 });
            Recipe_Limits.Add(new RecipeLimits { RecipeLimitsId = 7, RecipeType = RecipeTypes.Conc, LimitTypes = LimitType.Softquat, Min = 0, Max = 0, Target = 1333, Tolerance = 0, GuageMax = 0 });
            Recipe_Limits.Add(new RecipeLimits { RecipeLimitsId = 8, RecipeType = RecipeTypes.BigBang, LimitTypes = LimitType.Softquat, Min = 0, Max = 0, Target = 3167, Tolerance = 0, GuageMax = 0 });
            Recipe_Limits.Add(new RecipeLimits { RecipeLimitsId = 9, RecipeType = RecipeTypes.Reg, LimitTypes = LimitType.Softquat, Min = 0, Max = 0, Target = 392, Tolerance = 0, GuageMax = 0 });
            Recipe_Limits.Add(new RecipeLimits { RecipeLimitsId = 10, RecipeType = RecipeTypes.Conc, LimitTypes = LimitType.HCL, Min = 0, Max = 0, Target = 12.4M, Tolerance = 0, GuageMax = 0 });
            Recipe_Limits.Add(new RecipeLimits { RecipeLimitsId = 11, RecipeType = RecipeTypes.BigBang, LimitTypes = LimitType.HCL, Min = 0, Max = 0, Target = 33.8M, Tolerance = 0, GuageMax = 0 });
            Recipe_Limits.Add(new RecipeLimits { RecipeLimitsId = 12, RecipeType = RecipeTypes.Reg, LimitTypes = LimitType.HCL, Min = 0, Max = 0, Target = 14.8M, Tolerance = 0, GuageMax = 0 });
        }

        public RecipeLimits GetLimitInfo(RecipeTypes recipeType, LimitType limitType)
        {
            return Recipe_Limits.Where(x => x.RecipeType == recipeType && x.LimitTypes == limitType).FirstOrDefault();
        }
    }
}
