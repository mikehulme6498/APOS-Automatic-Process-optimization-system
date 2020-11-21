using BatchReports.ComplianceChecker.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace RosemountDiagnosticsV2.View_Models.Quality
{
    public class PcsComplianceViewModel
    {
        public List<PcsDailyResults> DailyResults { get; set; } = new List<PcsDailyResults>();
        public int CurrentWeek { get; set; }
        public bool ShouldShowNextWeekLink { get; set; } = false;
        public bool ShouldShowNextPrviousLink { get; set; } = true;
        public List<string> RecipesWithRework { get; set; }
        public DataSet DisplayTables { get; set; } = new DataSet();

        public bool NoReportsThisWeek { get; set; } = false;

        public void GenerateDataSet()
        {
            DisplayTables.Tables.Add(GeneratePcsResultsTable());
            //DisplayTables.Tables.Add(GeneratePcsReworkTable());
        }

        private DataTable GeneratePcsReworkTable()
        {
            DataTable output = new DataTable();
            output.Columns.Add("Rework");
            AddDaysToColumns(output);
            foreach (var recipe in DailyResults.First().DailyRework.Select(x => x.RecipeName).ToList())
            {
                output.Rows.Add(GetRowDailyReworkForRecipe().ToArray<string>());
            }

            return null;
        }

        private List<string> GetRowDailyReworkForRecipe()
        {
            List<string> output = new List<string>();
            foreach (var day in DailyResults)
            {
                foreach (var reworkTotal in day.DailyRework)
                {
                    output.Add(reworkTotal.BatchesMade.ToString());
                    output.Add(reworkTotal.ActualReworkAmount.ToString() + " Kg");
                }
            }

            return output;
        }

        private DataTable GeneratePcsResultsTable()
        {
            DataTable pcsResults = new DataTable();

            pcsResults.Columns.Add("PCS");

            AddDaysToColumns(pcsResults);

            foreach (var param in GetParametersForWeek())
            {
                List<string> rowScores = GetRowOfCombinedScores(param);
                pcsResults.Rows.Add(rowScores.ToArray<string>());
            }

            pcsResults.Rows.Add(GetDailyActualScores().ToArray<string>());
            pcsResults.Rows.Add(GetDailyPossibleScoresReduced().ToArray<string>());
            pcsResults.Rows.Add(GetDailyCombinedCompliance().ToArray<string>());

            return pcsResults;
        }

        private void AddDaysToColumns(DataTable table)
        {
            foreach (var day in DailyResults)
            {
                table.Columns.Add(day.Date.ToShortDateString());
            }
        }
        private List<string> GetParametersForWeek()
        {
            List<string> output = new List<string>();

            foreach (var day in DailyResults)
            {
                foreach (var param in day.GetParameterNamesForToday())
                {
                    if (!output.Contains(param))
                    {
                        output.Add(param);
                    }
                }
            }
            return output.OrderBy(x => x).ToList();
        }
        private List<string> GetDailyActualScores()
        {
            List<string> ActualScores = new List<string>
            {
                "Total Score"
            };
            foreach (var day in DailyResults)
            {
                ActualScores.Add(day.TotalActualScore.ToString());
            }
            return ActualScores;
        }
        private List<string> GetDailyPossibleScoresReduced()
        {
            List<string> PossibleDailyScore = new List<string>
            {
                "Maximum Score"
            };
            foreach (var day in DailyResults)
            {
                PossibleDailyScore.Add(day.TotalPossibleScore.ToString());
            }
            return PossibleDailyScore;
        }
        private List<string> GetDailyCombinedCompliance()
        {
            List<string> percentages = new List<string>
            {
                "PCS Compliance"
            };
            foreach (var day in DailyResults)
            {
                percentages.Add(decimal.Round(day.PcsCompliancePercentage, 2).ToString());
            }
            return percentages;
        }
        private List<string> GetRowOfCombinedScores(string parameter)
        {
            List<string> rowScores = new List<string>
            {
                parameter
            };
            foreach (var day in DailyResults)
            {
                if (day.MaterialsChecked.Any(x => x.Name == parameter))
                {
                    rowScores.Add(day.MaterialsChecked.Find(x => x.Name == parameter).Score.ToString());
                }
                else
                {
                    rowScores.Add("non-used");
                }
            }
            return rowScores;
        }
    }
}
