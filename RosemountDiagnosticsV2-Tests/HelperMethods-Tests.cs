using BatchDataAccessLibrary.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace RosemountDiagnosticsV2_Tests
{
    [TestClass]
    public class HelperMethods_Tests
    {
                
        [TestMethod]
        public void GetWeekNumberShouldPass()
        {
            int expected = 17;
            int actual = HelperMethods.GetWeekNumber(new DateTime(2020,4,24));
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetWeekNumberShouldPass1()
        {
            int expected = 1;
            int actual = HelperMethods.GetWeekNumber(new DateTime(2020, 1, 1));
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetWeekNumberShouldPass2()
        {
            int expected = 53;
            int actual = HelperMethods.GetWeekNumber(new DateTime(2020, 12, 31));
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetWeekNumberShouldPass3()
        {
            int expected = 53;
            int actual = HelperMethods.GetWeekNumber(new DateTime(2021, 1, 1));
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetWeekNumberShouldPass4()
        {
            int expected = 1;
            int actual = HelperMethods.GetWeekNumber(new DateTime(2021, 1, 4));
            Assert.AreEqual(expected, actual);
        }

        
    }
}
