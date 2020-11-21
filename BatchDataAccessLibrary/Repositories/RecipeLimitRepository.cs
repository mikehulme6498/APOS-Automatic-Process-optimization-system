using BatchDataAccessLibrary.DataAccess;
using BatchDataAccessLibrary.Enums;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BatchDataAccessLibrary.Repositories
{
    public class RecipeLimitRepository : IRecipeLimitRepository
    {
        private readonly BatchContext _context;

        public RecipeLimitRepository(BatchContext context)
        {
            _context = context;
        }

        public RecipeLimits GetLimitInfo(RecipeTypes recipeType, LimitType limitType)
        {
            return _context.RecipeLimits.Where(x => x.RecipeType == recipeType && x.LimitTypes == limitType).FirstOrDefault();
        }
                
    }
}
