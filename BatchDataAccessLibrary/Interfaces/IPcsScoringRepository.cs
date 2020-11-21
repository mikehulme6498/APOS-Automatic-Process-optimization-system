using BatchDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BatchDataAccessLibrary.Interfaces
{
    public interface IPcsScoringRepository
    {
        PcsScoring GetScoringParameters();
    }
}
