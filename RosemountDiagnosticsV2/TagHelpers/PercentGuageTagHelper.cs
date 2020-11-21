using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Text;

namespace RosemountDiagnosticsV2.TagHelpers
{
    public class PercentGuageTagHelper : TagHelper
    {
        public decimal Percent { get; set; }
        public int Id { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            StringBuilder html = new StringBuilder();

            html.Append($"<div id='container-speed-{Id}' class='chart-container'></div>");
            html.Append("<script>");
            html.Append("var chartSpeed = Highcharts.chart('container-speed-" + Id + "', Highcharts.merge(gaugeOptions, {");
            html.Append("yAxis: { min: 0, max: 100 },");
            html.Append("credits: { enabled: false },");
            html.Append("series:[{ name: 'Percent', data:[" + Decimal.Round(Percent, 2) + "], ");
            //html.Append("dataLabels: {format:'<div style=\"text-align:center; margin-top: -50%; margin-left: -20%;\"><span style=\"font-size:20px;\">{y}%</span></div>\'},");
            html.Append("dataLabels: { verticalAlign: 'middle', format:'<div style=\"text-align:center;\"><span style=\"font-size:14px;\">{y}%</span></div>\'},");
            html.Append("tooltip: { valueSuffix: ' %' } ");
            html.Append("}] }));");
            html.Append("</script>");
            output.Content.SetHtmlContent(html.ToString());
        }
    }
}









