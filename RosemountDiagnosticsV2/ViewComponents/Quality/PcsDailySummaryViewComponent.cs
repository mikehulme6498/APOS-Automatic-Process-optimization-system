using BatchReports.ComplianceChecker.Models;
using Microsoft.AspNetCore.Mvc;
using RosemountDiagnosticsV2.View_Models.Quality;
using System.Collections.Generic;

namespace RosemountDiagnosticsV2.ViewComponents.Quality
{
    public class PcsDailySummaryViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PcsDailyResults pcsWeightsDailyResults)
        {
            PcsSummary pcsSummary = new PcsSummary
            {
                Date = pcsWeightsDailyResults.Date,
                Title = pcsWeightsDailyResults.Date.DayOfWeek.ToString() + " " + pcsWeightsDailyResults.Date.ToShortDateString() + " - Compliance " + pcsWeightsDailyResults.PcsCompliancePercentage.ToString("0.##") + "%",
                Percentage = pcsWeightsDailyResults.PcsCompliancePercentage
            };
            GetClassColours(pcsSummary, pcsWeightsDailyResults.PcsCompliancePercentage);

            foreach (var parameter in pcsWeightsDailyResults.MaterialsChecked)
            {
                PcsSummaryParameter currentParam = new PcsSummaryParameter();

                switch (parameter.Score)
                {
                    case 0:
                        currentParam.Reasons = parameter.GetComplianceErrorsOutOfRange();
                        if (currentParam.Reasons.Count == 0)
                        {
                            currentParam.Reasons = parameter.GetComplianceErrorsOutOfTolerance();
                        }
                        currentParam.Heading = parameter.GetErrorHeading();
                        currentParam.Subheading = parameter.GetSubheading();
                        break;
                    case 1:
                        currentParam.Reasons = parameter.GetComplianceErrorsOutOfTolerance();
                        currentParam.Heading = parameter.GetErrorHeading();
                        currentParam.Subheading = parameter.GetSubheading();
                        break;
                    default:
                        currentParam.Reasons = new List<KeyValuePair<string, string>>();
                        break;
                }
                pcsSummary.ParameterSummary.Add(currentParam);
            }


            return View(pcsSummary);
        }

        private void GetClassColours(PcsSummary pcsSummary, decimal percentage)
        {
            if (percentage >= 85)
            {
                pcsSummary.IconColourClass = "pcs-daily-icon-good";
                pcsSummary.Icon = "fas fa-check-circle";
                pcsSummary.TitleColourClass = "pcs-daily-heading-good";
            }
            else if (percentage > 70 && percentage < 85)
            {
                pcsSummary.IconColourClass = "pcs-daily-icon-warning";
                pcsSummary.Icon = "fas fa-exclamation-circle";
                pcsSummary.TitleColourClass = "pcs-daily-heading-warning";
            }
            else
            {
                pcsSummary.IconColourClass = "pcs-daily-icon-error";
                pcsSummary.Icon = "fas fa-times-circle";
                pcsSummary.TitleColourClass = "pcs-daily-heading-error";
            }
        }
    }
}
