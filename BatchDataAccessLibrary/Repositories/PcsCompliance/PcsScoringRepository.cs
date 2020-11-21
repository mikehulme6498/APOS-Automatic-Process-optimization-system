using BatchDataAccessLibrary.DataAccess;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BatchDataAccessLibrary.Repositories
{
    public class PcsScoringRepository : IPcsScoringRepository
    {
        private readonly BatchContext _batchContext;

        public PcsScoringRepository(BatchContext batchContext)
        {
            _batchContext = batchContext;
        }
        public PcsScoring GetScoringParameters()
        {
            return _batchContext.PcsScoringsTargets.First();
        }
    }
}
