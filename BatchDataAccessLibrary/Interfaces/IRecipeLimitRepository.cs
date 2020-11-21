using BatchDataAccessLibrary.Enums;
using BatchDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BatchDataAccessLibrary.Interfaces
{
    public interface IRecipeLimitRepository
    {
        RecipeLimits GetLimitInfo(RecipeTypes recipeType, LimitType limitType);
    }
}
