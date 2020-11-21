using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RosemountDiagnosticsV2.TagHelpers
{
    public class StreamInfoBoxTagHelper : TagHelper
    {
        public string StreamName { get; set; }
        public string LossType { get; set; }
        public double LossInfo { get; set; }
        public int Occurances { get; set; }
        public double Percentage { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string className = LossInfo > 0 ? "positive" : "negative";

            StringBuilder html = new StringBuilder();
            //html.Append("<div class='col-md-2'>");
            html.Append("<div class='stream-outline'>");
            html.Append($"<div class='stream-name'>{StreamName}</div>");
            html.Append($"<div class='loss-type {className}'>{LossType}</div>");
            html.Append($"<div class='loss-info {className}'>{LossInfo} Kg</div>");
            html.Append($"<div class='occurances'>{Occurances} Occurances</div>");
            html.Append($"<div class='percentage {className}'>{Percentage} %</div>");
            html.Append("</div>");
                        
            output.Content.SetHtmlContent(html.ToString());
        }
    }
}
