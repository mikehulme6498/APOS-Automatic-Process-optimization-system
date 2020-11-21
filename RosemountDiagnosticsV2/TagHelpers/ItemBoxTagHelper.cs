using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RosemountDiagnosticsV2.TagHelpers
{
    public class ItemBoxTagHelper : TagHelper
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int ProductCode { get; set; }
        public double CostPerTon { get; set; }
        public double GainLossKg { get; set; }
        public double GainLossEuro { get; set; }
        public bool Reverse { get; set; } = false;

        

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            StringBuilder html = new StringBuilder();

            html.Append("<tr>");
            html.Append("<td class='search-icon' id='"+ Name +"'><div class='fas fa-search'></div></td>");
            
            html.Append("<td class='material-info'>");
            html.Append($"<div class='material-heading'>{ShortName}</div>");
            html.Append($"<div class='material-info'>Product Code : {ProductCode}<br />Cost Per Ton : {CostPerTon} €</div>");
            html.Append("</td>");

            html.Append("<td class='figures'>");
            html.Append($"<div class='total-kg'> {GainLossKg} Kg</div>");
            html.Append($"<div class='total-euro {GetClassName(GainLossEuro)}'> {MakeAbsoluteIfNegative(GainLossEuro)} €</div>");
            html.Append("</td>");

            html.Append("<td class='up-down'>");
            html.Append($"<div class='{GetUpDownIcon(GainLossEuro)} pad-left-15'></div>");
            html.Append("</td>");
            html.Append("</tr>");

            html.Append("<tr><td colspan = '4' class='seperation-line'><hr /></td></tr>");
            output.Content.SetHtmlContent(html.ToString());
        }

        private string GetClassName(double value)
        {
            if (value < 0)
            {
                if (Reverse)
                {
                    return "positive";
                }
               return "negative";
            }

            if (Reverse)
            {
                return "negative";
            }
            return "positive";
        }

        private string GetUpDownIcon(double value)
        {
            if (value < 0)
            {
                if (Reverse)
                {
                    return "fas fa-arrow-circle-up positive";
                }
                else
                {
                    return "fas fa-arrow-circle-down negative";
                }
            }
            if (Reverse) 
            {
                return "fas fa-arrow-circle-down negative";
            }
            return "fas fa-arrow-circle-up positive";
        }

        private double MakeAbsoluteIfNegative(double value)
        {
            if (value < 0)
            {
                return Math.Abs(value);
            }
            return value;
        }





    }
}
