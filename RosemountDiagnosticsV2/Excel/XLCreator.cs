using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RosemountDiagnosticsV2.Excel
{
    public class XLCreator
    {
        public XLWorkbook book;

        public XLCreator()
        {
            book = new XLWorkbook();
        }

        public void AddToWorkBook<T>(string parameterName, List<T> values, decimal lowerLimit, decimal UpperLimit)
        {
            using (book)
            {
                var worksheet = book.Worksheets.Add(parameterName);
                worksheet.Cell("A2").Value = "Std Dev";
                worksheet.Cell("A3").Value = "CPK Value";
                worksheet.Cell("B1").Value = parameterName;
                int count = 6;
                foreach (var value in values)
                {
                    worksheet.Cell("B" + count).Value = value;
                    count++;
                }
                decimal difference = UpperLimit - lowerLimit;
                worksheet.Cell("B2").FormulaA1 = $"=STDEV.P(B6:B{ count })";
                worksheet.Cell("B3").FormulaA1 = $"={difference}/(6*B2)";
                //book.SaveAs($"{parameterName}-cpkValues.xlsx");
            }
        }

        
    }
}
