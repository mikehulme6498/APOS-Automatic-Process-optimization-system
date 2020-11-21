using BatchReports.ComplianceChecker.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RosemountDiagnosticsV2.TagHelpers
{
    public class ReworkTotalsTagHelper : TagHelper
    {
        public List<PcsDailyResults> DailyReworkResult { get; set; }
        public string RecipeName { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            StringBuilder html = new StringBuilder();
            html.Append($"<td class='rework-recipe'>{RecipeName}</td>");
            foreach (var day in DailyReworkResult.Select(x => x.DailyRework).ToList())
            {
                foreach (var recipe in day)
                {
                    if (recipe.RecipeName == RecipeName)
                    {
                        html.Append($"<td class='text-center'>{recipe.BatchesMade}</td>");
                        html.Append($"<td class='text-center'>{recipe.ActualReworkAmount}</td>");
                    }
                }
            }


            output.Content.SetHtmlContent(html.ToString());
        }
    }
}
