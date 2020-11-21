using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosemountDiagnosticsV2.Extension_Methods;
using System;
using System.Collections.Generic;
using System.Text;

namespace RosemountDiagnosticsV2_Tests
{
    [TestClass]
    public class Extension_Method_Tests
    {
        [TestMethod]
        public void StandardDeviationShouldMatch()
        {
            List<decimal> values = new List<decimal>()
            {
                11,11,12,12,11,15,13,18,12,12,12,12,13,13
            };

            decimal result = values.StandardDeviation();

            Assert.AreEqual(1.797106518M, result);
        }

        [TestMethod]
        public void StandardDeviationEmptyListShouldReturnNegative1()
        {
            List<decimal> values = new List<decimal>();

            decimal result = values.StandardDeviation();

            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void StandardDeviationListAllSameValuesShouldReturn0()
        {
            List<decimal> values = new List<decimal>() { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11 };

            decimal result = values.StandardDeviation();

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void StandardDeviationWithTwoValuesWithOneIncrementShouldReturnHalf()
        {
            List<decimal> values = new List<decimal>() { 11, 12 };

            decimal result = values.StandardDeviation();

            Assert.AreEqual(0.5M, result);
        }


        [DataTestMethod]
        [DataRow("joe.bloggs@Unilever.com", "J.Bloggs")]
        [DataRow("jOe.BLOGGS@Unilever.com", "J.Bloggs")]
        [DataRow("joe.bloggs.unilever@unilever.com", "J.Bloggs")]
        public void EmailToShortNameShouldHaveCorrectResult(string email, string expectedResult)
        {
            Assert.AreEqual(expectedResult, email.EmailToShortName());
        }

        [DataTestMethod]
        [DataRow("joe.bloggs@gmail.com", "joe.bloggs@gmail.com")]
        [DataRow("joebloggs@unilever.com", "joebloggs@unilever.com")]
        [DataRow("joebloggs.com", "joebloggs.com")]
        public void EmailToShortNameShouldReturnOriginalEmail(string email, string expectedResult)
        {
            Assert.AreEqual(expectedResult, email.EmailToShortName());
        }
    }
}
