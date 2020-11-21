using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BatchDataAccessLibrary.Repositories
{
    public class MockPcsScoringRepository : IPcsScoringRepository
    {
        public PcsScoring GetScoringParameters()
        {
            return new PcsScoring { PcsScoringId = 1, Score1Lower = 70, Score2Target = 85 };
        }
    }
}
