using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using ComplianceChecker;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BatchReports.ComplianceChecker.Models
{
    public class PcsDailyResults
    {
        private readonly IPcsScoringRepository _pcsScoringRepository;

        [DebuggerDisplay("Date : { Date } - { PcsCompliancePercentage }%")]
        public List<PcsParameterTotals> MaterialsChecked { get; set; }
        public List<PcsReworkTotals> DailyRework { get; set; }
        public double TotalReworkUsedForToday { get; set; }
        public int TotalBatchesThatCouldRework { get; set; }
        public DateTime Date { get; set; }
        public decimal PcsCompliancePercentage { get; private set; }
        public decimal TotalPossibleScore { get; private set; }
        public decimal TotalActualScore { get; private set; }

        public PcsDailyResults(DateTime date, IPcsScoringRepository pcsScoringRepository)
        {
            Date = date;
            _pcsScoringRepository = pcsScoringRepository;
            MaterialsChecked = new List<PcsParameterTotals>();
            DailyRework = new List<PcsReworkTotals>();
        }

        public void ProcessCompliance()
        {
            MaterialsChecked = CombineMatchingParameters();
            SetPossibleScore();
            GetTotalScoreFromEachMaterial();
            CalculatePcsCompliancePercentage();
            GetScore(PcsCompliancePercentage);
            CalculateTotalReworkUsedToday();
        }

        private void CalculateTotalReworkUsedToday()
        {
            foreach (var reworkUsed in DailyRework)
            {
                TotalReworkUsedForToday += Convert.ToDouble(reworkUsed.ActualReworkAmount);
                TotalBatchesThatCouldRework += reworkUsed.BatchesMade;
            }
        }

        public List<string> GetParameterNamesForToday()
        {
            return MaterialsChecked.Select(x => x.Name).ToList();
        }

        private List<string> GetTodaysParameters()
        {
            return MaterialsChecked.Select(x => x.Name).Distinct().ToList();
        }

        private List<PcsParameterTotals> CombineMatchingParameters()
        {
            List<PcsParameterTotals> output = new List<PcsParameterTotals>();

            foreach (var parameter in GetTodaysParameters())
            {
                List<PcsParameterTotals> SeperateParameters = MaterialsChecked.Where(x => x.Name == parameter).ToList();
                List<IPcsIndividualParameters> newList = new List<IPcsIndividualParameters>();
                foreach (var param in SeperateParameters)
                {
                    foreach (var weight in param.Weights)
                    {
                        newList.Add(weight);
                    }
                }
                output.Add(new PcsParameterTotals(parameter, newList, _pcsScoringRepository));
            }
            foreach (var param in output)
            {
                param.ProcessScores();
            }
            return output;
        }

        private int GetScore(decimal percentage)
        {
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

        private void SetPossibleScore()
        {
            TotalPossibleScore = MaterialsChecked.Count * 2;
        }

        private void GetTotalScoreFromEachMaterial()
        {
            foreach (var material in MaterialsChecked)
            {
                TotalActualScore += material.Score;
            }
        }
        private void CalculatePcsCompliancePercentage()
        {
            PcsCompliancePercentage = TotalActualScore / TotalPossibleScore * 100;
        }
    }
}
