using BatchDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RosemountDiagnosticsV2.View_Models.Quality
{
    public class PcsScoringSettingsViewModel
    {
        public PcsScoring ScoringParams { get; set; }
        public PcsToleranceParameters ToleranceParams { get; set; }
    }
}
