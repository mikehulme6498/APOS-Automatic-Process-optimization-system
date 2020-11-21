using BatchDataAccessLibrary.DataAccess;
using BatchDataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BatchDataAccessLibrary.Repositories
{
    public class GapInTimeReasonsRepository : IGapInTimeReasons
    {
        private readonly BatchContext _context;

        public GapInTimeReasonsRepository(BatchContext context)
        {
            _context = context;
        }
        public List<string> GetReasonForGap(string material1, string material2)
        {
            List<string> reasons = _context.GapInTimeReasons.Where(x => x.Material1 == material1 && x.Material2 == material2).Select(x => x.Reason).ToList();

            if(reasons.Count == 0)
            {
                reasons.Add($"There is a gap between {material2} and {material1}. I am unsure what this issue is Please feel free to add an explanation");
            }

            return reasons;
        }
    }
}
