using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RosemountDiagnosticsV2.TagHelpers
{
    public class ViscoGuageTagHelper : TagHelper
    {
        public string Title { get; set; }
        public double ActualVisco { get; set; }
        public double Max { get; set; }
        public double Min { get; set; }
        public int Target { get; set; }
        public double GuageMax { get; set; }
        public double Tolerance { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            StringBuilder html = new StringBuilder();

           
            html.Append("var opts = {");
            html.Append("angle: 0,");
            html.Append("radiusScale: 1");
            html.Append("pointer: {length: 0.6,strokeWidth: 0.035,color: '#000000'},");
            html.Append("limitMax: false,");
            html.Append("limitMin: false,");
            html.Append("colorStart: '#6FADCF',");
            html.Append("colorStop: '#8FC0DA',");
            html.Append("strokeColor: '#E0E0E0',");
            html.Append("generateGradient: true,");
            html.Append("highDpiSupport: true,");
            html.Append("staticZones: [");
            html.Append("{ strokeStyle: '#F03E3E', min: 0, max: " + Min + " },");
            html.Append("{ strokeStyle: '#FFDD00', min: " + Min + ", max: " + (Target - Tolerance) +" },");
            html.Append("{ strokeStyle: '#30B32D', min: " + (Target - Tolerance) + ", max: " + Target + Tolerance + "},");
            html.Append("{ strokeStyle: '#FFDD00', min: " + (Target + Tolerance) + ", max: " + Max +"},");
            html.Append("{ strokeStyle: '#F03E3E', min: " + Max + ", max: " + GuageMax + " }],},");
            html.Append($"var currentGauge = document.getElementById('{Title}');");
            html.Append("var Gauge = new Gauge(currentGauge).setOptions(opts);");
            html.Append($"Gauge.maxValue = {GuageMax};");
            html.Append("Gauge.setMinValue(0);");
            html.Append("Gauge.animationSpeed = 32;");
            html.Append($"Gauge.set({ActualVisco});");
            html.Append($"Gauge.setTextField(document.getElementById('{Title} - Value'));");
            

            output.Content.SetHtmlContent(html.ToString());
        }
    }
}



           


      