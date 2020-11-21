using BatchDataAccessLibrary.Helpers;
using BatchDataAccessLibrary.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace RosemountDiagnosticsV2_Tests
{
    [TestClass]
    public class BatchHelperMethod_Tests
    {
        [TestMethod]
        public void StoppageTimeShouldPass()
        {
            List<BatchIssue> issues = new List<BatchIssue>
            {
                new BatchIssue { BatchIssueId=1, FaultType= BatchIssue.FaultTypes.WaitTime, TimeLost=15, MaterialName="ANTIFOAM"},
                new BatchIssue { BatchIssueId=2, FaultType= BatchIssue.FaultTypes.WeighTime, TimeLost=15, MaterialName="ANTIFOAM"},
                new BatchIssue { BatchIssueId=3, FaultType= BatchIssue.FaultTypes.AcquireTime, TimeLost=15, MaterialName="ANTIFOAM"},
                new BatchIssue { BatchIssueId=4, FaultType= BatchIssue.FaultTypes.Overweigh, TimeLost=15, MaterialName="ANTIFOAM"},
                new BatchIssue { BatchIssueId=5, FaultType= BatchIssue.FaultTypes.Quality, TimeLost=15, MaterialName="ANTIFOAM"},
            };

            double timeLost = BatchHelperMethods.GetTotalStoppageTime(issues);

            Assert.AreEqual(45, timeLost);
        }
        [TestMethod]
        public void StoppageTimeShouldReturn0WhenNullPassedIn()
        {
            double timeLost = BatchHelperMethods.GetTotalStoppageTime(null);
            Assert.AreEqual(0, timeLost);
        }

        [TestMethod]
        public void StoppageTimeShouldReturnWhenEmptyListPassedIn()
        {
            List<BatchIssue> issues = new List<BatchIssue>();
            double timeLost = BatchHelperMethods.GetTotalStoppageTime(issues);
            Assert.AreEqual(0, timeLost);
        }

        [TestMethod]
        public void StoppageTimeShouldBe4()
        {
            List<BatchIssue> issues = new List<BatchIssue>
            {
                new BatchIssue { BatchIssueId=1, FaultType= BatchIssue.FaultTypes.Quality, TimeLost=15, MaterialName="ANTIFOAM"},
                new BatchIssue { BatchIssueId=2, FaultType= BatchIssue.FaultTypes.TemperatureHigh, TimeLost=15, MaterialName="ANTIFOAM"},
                new BatchIssue { BatchIssueId=3, FaultType= BatchIssue.FaultTypes.AcquireTime, TimeLost=15, MaterialName="ANTIFOAM"},
                new BatchIssue { BatchIssueId=4, FaultType= BatchIssue.FaultTypes.TemperatureLow, TimeLost=15, MaterialName="ANTIFOAM"},
                new BatchIssue { BatchIssueId=5, FaultType= BatchIssue.FaultTypes.Quality, TimeLost=15, MaterialName="ANTIFOAM"},
            };

            double issueCount = BatchHelperMethods.GetTotalQualityIssues(issues);

            Assert.AreEqual(4, issueCount);
        }

        [TestMethod]
        public void CountShouldReturn0WhenNullPassedIn()
        {
            double issueCount = BatchHelperMethods.GetTotalQualityIssues(null);
            Assert.AreEqual(0, issueCount);
        }

        [TestMethod]
        public void ShouldReturnCorrectWeightOfAllMaterialsInVessel()
        {
            Vessel vessel = new Vessel();
            List<Material> materials = new List<Material>()
            {
                new Material { ActualWeight = 11 },
                new Material { ActualWeight = 22 },
                new Material { ActualWeight = 33 },
                new Material { ActualWeight = 44 },
                new Material { ActualWeight = 55 },
                new Material { ActualWeight = 66 }
            };
            vessel.Materials.AddRange(materials);

            double totalWeights = BatchHelperMethods.CountActualWeightOfAllMaterialsInVessel(vessel);
            Assert.AreEqual(231, totalWeights);
        }
    }
}
