using DocumentFormat.OpenXml.Presentation;
using Microsoft.CodeAnalysis.Editing;
using RosemountDiagnosticsV2.Extension_Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RosemountDiagnosticsV2.View_Models.Quality
{
    public class ControlChartData
    {
        public List<decimal> Values { get; set; }
        public List<string> XAxisLabels { get; set; }
        public decimal Target { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public string Title { get; set; }
        public string SeriesName { get; set; }
        public string YAxisSuffix { get; set; }
        public string ChartId { get; set; }
        public decimal CpkValue { get; private set; }
        public string CpkJudgement { get; private set; }
        public string CpkAction { get; private set; }
        public string CpkBgColour { get; set; }


        private void SetCpkValue()
        {
            decimal standardDeviation = Values.StandardDeviation();
            decimal upperLowerDifference = Max - Min;
            if (standardDeviation != 0)
            {
                CpkValue = decimal.Round(upperLowerDifference / (6 * standardDeviation), 2);
            }
            else
            {
                CpkValue = 0M;
            }
        }
        private void SetCpkInformation()
        {
            if(CpkValue == 0M)
            {
                CpkJudgement = "Unable to obtain CPK Value";
                CpkAction = "Only 1 value availble for this parameter.";
                CpkBgColour = "bg-green";
                return;
            }

            if(CpkValue > 1.67M)
            {
                CpkJudgement = "Process capability is more than enough";
                CpkAction = "Simplification of process control and cost reduction can be considered in certain cases.";
                CpkBgColour = "bg-green";
                return;
            }
            if (CpkValue > 1.33M)
            {
                CpkJudgement = "Process capability is sufficiently high";
                CpkAction = "Ideal condition. Maintain it.";
                CpkBgColour = "bg-green";
                return;
            }
            if (CpkValue > 1M)
            {
                CpkJudgement = "Process capability is not sufficiently high but it is adequate";
                CpkAction = "Control process properly and maintain it in a control state. Defects may result if Cp approaches 1. Take action if needed.";
                CpkBgColour = "bg-orange";
                return;
            }
            if (CpkValue > 0.67M)
            {
                CpkJudgement = "Process capability is not sufficient.";
                CpkAction = "Defects have been generated. Screening inspection and process control and Kaizen will be required.";
                CpkBgColour = "bg-red";
                return;
            }
            CpkJudgement = "Process capability is vary low.";
            CpkAction = "Cannot satisfy quality. Quality must be improved, cause must be pursued and emergency actions must be taken. Re-examine standards.";
            CpkBgColour = "bg-red";

        }

        public void ProcessCpkValues()
        {
            SetCpkValue();
            SetCpkInformation();
        }
    }
}
