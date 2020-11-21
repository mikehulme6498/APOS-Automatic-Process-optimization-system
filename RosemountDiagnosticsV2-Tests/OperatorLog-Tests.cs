using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosemountDiagnosticsV2.Models.ShiftLog;
using System;
using System.Data;

namespace RosemountDiagnosticsV2_Tests
{
    [TestClass]
    public class OperatorLog_Tests
    {
        [DataTestMethod]
        [DataRow("2020/1/2 07:30:00", "Days")]
        [DataRow("2020/1/2 17:59:59", "Days")]
        [DataRow("2020/1/2 06:01:01", "Days")]
        [DataRow("2020/1/2 18:00:00", "Nights")]
        [DataRow("2020/1/2 18:00:01", "Nights")]
        [DataRow("2020/1/2 05:59:59", "Nights")]
        public void GetShiftDayNight_ShouldReturnCorrectResult(string dateTime, string expected)
        {
            DateTime convertedDateTime = DateTime.Parse(dateTime);
            ShiftSelector shiftSelector = new ShiftSelector();
            Assert.AreEqual(expected, shiftSelector.GetShiftDayNight(convertedDateTime));
        }
    }
}
