using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BatchDataAccessLibrary.Repositories
{
    public class MockGapInTimeReasons : IGapInTimeReasons
    {
        List<GapInTimeReasons> messages = new List<GapInTimeReasons>()
        {
            new GapInTimeReasons(){ Material1="WASHINGS RWK", Material2="HCL", Reason="One of the HCL valves going into faultputting the batch in a proc hold. "},
            new GapInTimeReasons(){ Material1="WASHINGS RWK", Material2="HCL FLUSH", Reason="The vessel temperature being incorrect holding the batch until operator corrects it. "},
            new GapInTimeReasons(){ Material1="HCL", Material2="HCL FLUSH", Reason="One of the HCL valves going into faultputting the batch in a proc hold. "},
            new GapInTimeReasons(){ Material1="", Material2="", Reason=""},
            new GapInTimeReasons(){ Material1="", Material2="", Reason=""},
            new GapInTimeReasons(){ Material1="", Material2="", Reason=""},
        };

        List<string> reasons = new List<string>();
        public List<string> GetReasonForGap(string material1, string material2)
        {
            reasons.Clear();
            foreach(var gapIssue in messages)
            {
                if(gapIssue.Material1 == material1 && gapIssue.Material2 == material2)
                {
                    reasons.Add(gapIssue.Reason);
                }
            }

            if (reasons.Count == 0)
            {
                reasons.Add($"There is a gap between {material2} and {material1}. I am unsure what this issue is Please feel free to add an explanation");
            }

            return reasons;
        }
    }
}
