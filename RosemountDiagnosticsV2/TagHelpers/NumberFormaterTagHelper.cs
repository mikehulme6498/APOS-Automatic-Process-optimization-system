using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace RosemountDiagnosticsV2.TagHelpers
{
    public class NumberFormaterTagHelper : TagHelper
    {
        public double Value { get; set; }
        public string Suffix { get; set; }
        public string TagName { get; set; } = null;
        public bool Reverse { get; set; }
        public bool RemoveNegativeSymbol { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            if (TagName != null)
            {
                output.TagName = TagName;
            }

            if (Value < 0)
            {
                if (Reverse)
                {
                    output.Attributes.SetAttribute("class", "positive");
                }
                else
                {
                    output.Attributes.SetAttribute("class", "negative");
                    Value = Math.Abs(Value);
                }
            }
            else
            {
                if (Reverse)
                {
                    output.Attributes.SetAttribute("class", "negative");
                    Value = Math.Abs(Value);
                }
                else
                {
                    output.Attributes.SetAttribute("class", "positive");
                }
            }

            if (RemoveNegativeSymbol)
            {
                Value = Math.Abs(Value);
            }

            output.Content.SetHtmlContent(Value.ToString("#,##0.00") + " " + Suffix);
        }
    }
}
