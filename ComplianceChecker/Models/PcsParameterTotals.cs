using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using ComplianceChecker;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BatchReports.ComplianceChecker.Models
{
    public class PcsParameterTotals
    {
        [DebuggerDisplay("Name : { Name } - Score : { Score }")]
        public string Name { get; set; }
        public int TotalChecked { get; set; }
        public int TotalInRangeCount { get; set; }
        public decimal Percentage { get; set; }
        public int Score { get; set; }
        public List<IPcsIndividualParameters> Weights { get; private set; } = new List<IPcsIndividualParameters>();

        bool foundOutOfRange = false;
        private readonly IPcsScoringRepository _pcsScoringRepository;

        public PcsParameterTotals(string name, List<IPcsIndividualParameters> weights, IPcsScoringRepository pcsScoringRepository)
        {
            Name = name;
            Weights = weights;
            _pcsScoringRepository = pcsScoringRepository;
            ProcessScores();
        }

        public void ProcessScores()
        {
            TotalChecked = Weights.Count;
            TotalInRangeCount = CountAmountOfWeightsinRange();
            foundOutOfRange = CheckForOutOfRange();
            Percentage = GetPercentage(TotalInRangeCount, Weights.Count);
            Score = GetScore(Percentage, foundOutOfRange);
        }
        private bool CheckForOutOfRange()
        {
            foreach (var weight in Weights)
            {
                if (weight.IsOutOfRange)
                {
                    return true;
                }
            }
            return false;
        }

        private int CountAmountOfWeightsinRange()
        {
            int output = 0;

            foreach (var weight in Weights)
            {
                if (!weight.IsOutOfTolerance)
                {
                    output++;
                }
            }
            return output;
        }

        private decimal GetPercentage(int inRangeCount, int totalChecked)
        {
            if (TrimName(Name).ToLower() == "fatty alc" && totalChecked < 5 && inRangeCount != totalChecked)
            {
                // This stops the score of zero for the fatty alc, sometimes 2 batches are made a day and one is
                // out of tolerance and returns 50% which is a score of 0
                return 84;                
            }

            decimal range = Convert.ToDecimal(inRangeCount);
            decimal total = Convert.ToDecimal(totalChecked);
            return range / total * 100;
        }

        private string TrimName(string Name)
        {
            if (Name.Contains('('))
            {
                return Name.Substring(0, Name.IndexOf('(') - 1);
            }
            return Name;
        }

        private int GetScore(decimal percentage, bool outOfRange)
        {
            if (outOfRange)
            {
                return 0;
            }

            PcsScoring scoreTargets = _pcsScoringRepository.GetScoringParameters();

            if (percentage >= scoreTargets.Score2Target)
            {
                return 2;
            }
            else if (percentage >= scoreTargets.Score1Lower && percentage < scoreTargets.Score2Target)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public string GetErrorHeading()
        {
            string shortName = Name;
            if (Name.Contains("("))
            {
                shortName = Name.Substring(0, Name.IndexOf('('));
            }
            return $"{ shortName } Scored { Score } for the following reasons";
        }

        public string GetSubheading()
        {
            //int outOfTolerance = TotalChecked - TotalInRangeCount;
            decimal tolerance = Weights.Select(x => x.Tolerance).FirstOrDefault();
            decimal range = Weights.Select(x => x.ToleranceOutOfRange).FirstOrDefault();
            int outOfRange = Weights.Where(x => x.IsOutOfRange).Count();
            int outOfTolerance = Weights.Where(x => x.IsOutOfTolerance).Count();

            if (outOfRange > 0)
            {
                return $"{ TotalChecked } batches were made and { outOfRange } were over the { range }% tolerance";
            }
            return $"{ TotalChecked } batches were made and { outOfTolerance } were over the { tolerance }% tolerance";
        }

        public List<KeyValuePair<string, string>> GetComplianceErrorsOutOfRange()
        {
            List<KeyValuePair<string, string>> output = new List<KeyValuePair<string, string>>();

            foreach (var weight in Weights)
            {
                if (weight.IsOutOfRange)
                {
                    output.Add(weight.GetErrorDisplayMessage());
                }
            }
            return output;
        }

        public List<KeyValuePair<string, string>> GetComplianceErrorsOutOfTolerance()
        {
            List<KeyValuePair<string, string>> output = new List<KeyValuePair<string, string>>();

            foreach (var weight in Weights)
            {
                if (weight.IsOutOfTolerance)
                {
                    output.Add(weight.GetErrorDisplayMessage());
                }
            }
            return output;
        }
    }
}
