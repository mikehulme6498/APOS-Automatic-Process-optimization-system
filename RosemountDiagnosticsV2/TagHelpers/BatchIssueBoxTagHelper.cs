using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BatchDataAccessLibrary.Models.BatchIssue;

namespace RosemountDiagnosticsV2.TagHelpers
{
    public class BatchIssueBoxTagHelper : TagHelper
    {
        public string MaterialName { get; set; }
        public string Message { get; set; }
        public FaultTypes FaultType { get; set; }
        public double TimeLost { get; set; }
        public double PercentOut { get; set; }
        public double ActualReading { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            StringBuilder html = new StringBuilder();
            html.Append("<table class='issue-table'>");
            html.Append("<tr>");
            html.Append($"<td class='icon'><img src='../Images/Icons/{GetIconForFault()}' /></td >");
            html.Append($"<td class='material-title'>{MaterialName}</td>");
            html.Append("</tr>");
            if (Message != null)
            {
                html.Append("<tr>");
                html.Append("<td></td>");
                html.Append($"<td class='reason'>{Message}</td>");
                html.Append("</tr>");
            }
            if (TimeLost != 0)
            {
                html.Append("<tr>");
                html.Append($"<td colspan='2' class='lossTime'>Total Loss {TimeLost} Minutes</td>");
                html.Append("</tr>");
            }

            if(PercentOut != 0)
            {
                html.Append("<tr>");
                html.Append($"<td colspan='2' class='lossTime'>{PercentOut}% Out</td>");
                html.Append("</tr>");
            }

            if (ActualReading != 0)
            {
                html.Append("<tr>");
                html.Append($"<td colspan='2' class='lossTime'>Actual Reading {ActualReading}C</td>");
                html.Append("</tr>");
            }
            html.Append("</table>");
            output.Content.SetHtmlContent(html.ToString());
        }
        
       private string GetIconForFault()
        {
            switch (FaultType)
            {
                case FaultTypes.WeighTime:
                    return "WaitingHourGlass30x30.png";
                case FaultTypes.WaitTime:
                    return "Waiting30x30.png";
                case FaultTypes.Overweigh:
                    return "RedArrowUp30x30.png";
                case FaultTypes.AcquireTime:
                    return "WaitingHourGlass30x30.png";
                case FaultTypes.Underweigh:
                    return "OrangeArrowDown30x30.png";
                case FaultTypes.TemperatureHigh:
                    return "TempHot.png";
                case FaultTypes.TemperatureLow:
                    return "TempCold.png";
                case FaultTypes.Quality:
                    return "QualityIssue30x30.png";
                case FaultTypes.NoIssue:
                    return "GreenTick30x30.png";
                default:
                    return "Information30x30.png";

            }
        }


    }
}
