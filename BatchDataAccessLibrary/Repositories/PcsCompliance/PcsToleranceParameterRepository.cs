using BatchDataAccessLibrary.DataAccess;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BatchDataAccessLibrary.Repositories
{
    public class PcsToleranceParameterRepository : IPcsToleranceParameterRepository
    {
        private readonly BatchContext _batchContext;

        public PcsToleranceParameterRepository(BatchContext batchContext)
        {
            _batchContext = batchContext;
        }
        public PcsToleranceParameters GetTolerances()
        {
            return _batchContext.PcsToleranceParameters.First();
        }
    }
}
