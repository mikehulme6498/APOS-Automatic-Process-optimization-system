using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BatchDataAccessLibrary.Repositories
{
    public class MockToleranceParameterRepository : IPcsToleranceParameterRepository
    {
        public PcsToleranceParameters GetTolerances()
        {
            return new PcsToleranceParameters { PcsToleranceParametersId = 1, TolerancePercent = 5, OutOfRangePercent = 20 };
        }
    }
}
