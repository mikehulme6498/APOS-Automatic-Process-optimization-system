using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RosemountDiagnosticsV2.TagHelpers
{
    public class InfoBoxTagHelper : TagHelper
    {
        public string Heading { get; set; }
        public string Value { get; set; }
        public string Subheading { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            StringBuilder html = new StringBuilder();

            html.Append("<div class='info-box'>");
            html.Append($"<div class='info-box-heading'>{Heading}</div>");
            html.Append($"<div class='info-box-value-big'>{Value}</div>");
            html.Append($"<div class='info-box-subheading'>{Subheading}</div>");
            html.Append("</div>");


            output.Content.SetHtmlContent(html.ToString());
        }
    }
}
