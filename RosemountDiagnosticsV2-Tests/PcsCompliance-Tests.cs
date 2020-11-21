using BatchDataAccessLibrary.Enums;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Repositories;
using BatchReports.ComplianceChecker.Models;
using ComplianceChecker;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace RosemountDiagnosticsV2_Tests
{
    [TestClass]

    public class PcsCompliance_Tests
    {
        private readonly IPcsScoringRepository _pcsScoringRepository;
        private readonly IPcsToleranceParameterRepository _pcsToleranceParameter;
        private readonly IPcsActiveTempParameters _pcsActiveTempParameters;

        public PcsCompliance_Tests()
        {
            _pcsScoringRepository = new MockPcsScoringRepository();
            _pcsToleranceParameter = new MockToleranceParameterRepository();
            _pcsActiveTempParameters = new MockPcsActiveTempParametersRepository();
        }

        [TestMethod]
        public void PcsWeightCreationShouldSetAllParametersCorrectly()
        {
            PcsWeights testWeight = new PcsWeights("SOFTQUAT", "480-1", "BLUCON", 1333, 1333, RecipeTypes.Conc, _pcsToleranceParameter);

            Assert.AreEqual(false, testWeight.IsOutOfRange);
            Assert.AreEqual(false, testWeight.IsOutOfTolerance);
            Assert.AreEqual(RecipeTypes.Conc, testWeight.RecipeType);
            Assert.AreEqual("BLUCON", testWeight.RecipeName);
            Assert.AreEqual("480-1", testWeight.BatchNumber, "480-1");
            Assert.AreEqual(1333M, testWeight.ActualWeight);
            Assert.AreEqual(1333M, testWeight.TargetWeight);
            Assert.AreEqual(1399.65M, testWeight.UpperLimit);
            Assert.AreEqual(1266.35M, testWeight.LowerLimit);
            Assert.AreEqual(1599.6M, testWeight.OutOfRangeUpperLimit);
            Assert.AreEqual(1066.4M, testWeight.OutOfRangeLowerLimit);
        }

        [TestMethod]
        public void PcsWeightCreationShouldBeOutOfToleranceLower()
        {
            PcsWeights testWeight = new PcsWeights("SOFTQUAT", "480-1", "BLUCON", 1333M, 1265M, RecipeTypes.Conc, _pcsToleranceParameter);
            Assert.AreEqual(true, testWeight.IsOutOfTolerance);
            Assert.AreEqual(false, testWeight.IsOutOfRange);
        }
        [TestMethod]

        public void PcsWeightCreationShouldBeOutOfToleranceUpper()
        {
            PcsWeights testWeight = new PcsWeights("SOFTQUAT", "480-1", "BLUCON", 1333M, 1400M, RecipeTypes.Conc, _pcsToleranceParameter);
            Assert.AreEqual(true, testWeight.IsOutOfTolerance);
            Assert.AreEqual(false, testWeight.IsOutOfRange);
        }
        [TestMethod]

        public void PcsWeightCreationShouldBeOutOfRangeUpper()
        {
            PcsWeights testWeight = new PcsWeights("SOFTQUAT", "480-1", "BLUCON", 1333M, 1600, RecipeTypes.Conc, _pcsToleranceParameter);
            Assert.AreEqual(true, testWeight.IsOutOfTolerance);
            Assert.AreEqual(true, testWeight.IsOutOfRange);
        }

        [TestMethod]
        public void PcsWeightCreationShouldBeOutOfRangeLower()
        {
            PcsWeights testWeight = new PcsWeights("SOFTQUAT", "480-1", "BLUCON", 1333M, 1065M, RecipeTypes.Conc, _pcsToleranceParameter);
            Assert.AreEqual(true, testWeight.IsOutOfTolerance);
            Assert.AreEqual(true, testWeight.IsOutOfRange);
        }


        [TestMethod]
        public void PcsParameterCreationShouldPassWithTopScore()
        {
            List<IPcsIndividualParameters> weights = CreateListOfPcsWeightsAllInSpec("FATTY ALC", 500M, 40);
            PcsParameterTotals pcsParameter = new PcsParameterTotals("FATTY ALC", weights, _pcsScoringRepository);
            pcsParameter.ProcessScores();

            Assert.AreEqual(2, pcsParameter.Score);
            Assert.AreEqual(100, pcsParameter.Percentage);
            Assert.AreEqual(40, pcsParameter.TotalChecked);
            Assert.AreEqual(40, pcsParameter.TotalInRangeCount);
            Assert.AreEqual("FATTY ALC", pcsParameter.Name);
            Assert.AreEqual(40, pcsParameter.Weights.Count);
        }

        [TestMethod]
        public void PcsParameterCreationShouldPassWithScoreOf1()
        {
            List<IPcsIndividualParameters> weights = CreateListOfPcsWeightsAllInSpec("FATTY ALC", 500M, 40);
            weights.AddRange(CreateListOfPcsWeightsOutOfSpec("FATTY ALC", 500M, 11, 6));

            PcsParameterTotals pcsParameter = new PcsParameterTotals("FATTY ALC", weights, _pcsScoringRepository);
            pcsParameter.ProcessScores();

            Assert.AreEqual(1, pcsParameter.Score);
            Assert.AreEqual(78.43M, Decimal.Round(pcsParameter.Percentage, 2));
            Assert.AreEqual(51, pcsParameter.TotalChecked);
            Assert.AreEqual(40, pcsParameter.TotalInRangeCount);
            Assert.AreEqual("FATTY ALC", pcsParameter.Name);
            Assert.AreEqual(51, pcsParameter.Weights.Count);
        }
        [TestMethod]
        public void PcsParameterCreationShouldPassWithScoreOf0()
        {
            List<IPcsIndividualParameters> weights = CreateListOfPcsWeightsAllInSpec("FATTY ALC", 500M, 40);
            weights.AddRange(CreateListOfPcsWeightsOutOfSpec("FATTY ALC", 500M, 1, 21));

            PcsParameterTotals pcsParameter = new PcsParameterTotals("FATTY ALC", weights, _pcsScoringRepository);
            pcsParameter.ProcessScores();

            Assert.AreEqual(0, pcsParameter.Score);
            Assert.AreEqual(97.56M, Decimal.Round(pcsParameter.Percentage, 2));
            Assert.AreEqual(41, pcsParameter.TotalChecked);
            Assert.AreEqual(40, pcsParameter.TotalInRangeCount);
            Assert.AreEqual("FATTY ALC", pcsParameter.Name);
            Assert.AreEqual(41, pcsParameter.Weights.Count);
        }

        [TestMethod]
        public void PcsParameterCreationShouldPassWithScoreOf0DueToTooMany3PercentOut()
        {
            List<IPcsIndividualParameters> weights = CreateListOfPcsWeightsAllInSpec("FATTY ALC", 500M, 20);
            weights.AddRange(CreateListOfPcsWeightsOutOfSpec("FATTY ALC", 500M, 20, 6));

            PcsParameterTotals pcsParameter = new PcsParameterTotals("FATTY ALC", weights, _pcsScoringRepository);
            pcsParameter.ProcessScores();

            Assert.AreEqual(0, pcsParameter.Score);
            Assert.AreEqual(50m, Decimal.Round(pcsParameter.Percentage, 2));
            Assert.AreEqual(40, pcsParameter.TotalChecked);
            Assert.AreEqual(20, pcsParameter.TotalInRangeCount);
            Assert.AreEqual("FATTY ALC", pcsParameter.Name);
            Assert.AreEqual(40, pcsParameter.Weights.Count);
        }
        [TestMethod]
        public void PcsActiveTempsParametersShouldAllMatch()
        {
            PcsActiveTemps temps = new PcsActiveTemps("SOFTQUAT", "480-1", "BLUCON", 38, RecipeTypes.Conc, _pcsActiveTempParameters, _pcsToleranceParameter);
            Assert.AreEqual("SOFTQUAT", temps.ParameterName);
            Assert.AreEqual("480-1", temps.BatchNumber);
            Assert.AreEqual("BLUCON", temps.RecipeName);
            Assert.AreEqual(38M, temps.ActualWeight);
            Assert.AreEqual(RecipeTypes.Conc, temps.RecipeType);
            Assert.AreEqual(false, temps.IsOutOfRange);
            Assert.AreEqual(false, temps.IsOutOfTolerance);
            Assert.AreEqual(38.5M, temps.UpperLimit);
            Assert.AreEqual(37.5M, temps.LowerLimit);
        }

        [TestMethod]
        public void PcsActiveTempsParametersShouldbeOutOfToleranceUpper()
        {
            PcsActiveTemps temps = new PcsActiveTemps("SOFTQUAT", "480-1", "BLUCON", 38.7M, RecipeTypes.Conc, _pcsActiveTempParameters, _pcsToleranceParameter);
            Assert.AreEqual(true, temps.IsOutOfTolerance);
            Assert.AreEqual(false, temps.IsOutOfRange);
        }

        [TestMethod]
        public void PcsActiveTempsParametersShouldGetAdjustedAndBeInToleranceUpper()
        {
            // This is 0.1 degree above upper tolerance, it should get adjusted by +/- 0.1 and return in tolerance
            PcsActiveTemps temps = new PcsActiveTemps("SOFTQUAT", "480-1", "BLUCON", 38.6M, RecipeTypes.Conc, _pcsActiveTempParameters, _pcsToleranceParameter);
            Assert.AreEqual(false, temps.IsOutOfTolerance);
            Assert.AreEqual(false, temps.IsOutOfRange);
        }

        [TestMethod]
        public void PcsActiveTempsParametersShouldGetAdjustedAndBeInToleranceLower()
        {
            // This is 0.1 degree below lower tolerance, it should get adjusted by +/- 0.1 and return in tolerance
            PcsActiveTemps temps = new PcsActiveTemps("SOFTQUAT", "480-1", "BLUCON", 37.4M, RecipeTypes.Conc, _pcsActiveTempParameters, _pcsToleranceParameter);
            Assert.AreEqual(false, temps.IsOutOfTolerance);
            Assert.AreEqual(false, temps.IsOutOfRange);
        }

        [TestMethod]
        public void PcsActiveTempsParametersShouldbeOutOfToleranceLower()
        {
            PcsActiveTemps temps = new PcsActiveTemps("SOFTQUAT", "480-1", "BLUCON", 37.3M, RecipeTypes.Conc, _pcsActiveTempParameters, _pcsToleranceParameter);
            Assert.AreEqual(true, temps.IsOutOfTolerance);
            Assert.AreEqual(false, temps.IsOutOfRange);
        }

        private List<IPcsIndividualParameters> CreateListOfPcsWeightsAllInSpec(string name, decimal target, int howManyToCreate)
        {
            List<IPcsIndividualParameters> weights = new List<IPcsIndividualParameters>();
            for (int i = 0; i < howManyToCreate; i++)
            {
                weights.Add(new PcsWeights(name, "480-" + i.ToString(), "BLUCON", target, target, RecipeTypes.Conc, _pcsToleranceParameter));
            }
            return weights;
        }

        private List<IPcsIndividualParameters> CreateListOfPcsWeightsOutOfSpec(string name, decimal target, int howManyToCreate, int percentageOut)
        {
            List<IPcsIndividualParameters> weights = new List<IPcsIndividualParameters>();
            decimal percent = Convert.ToDecimal(percentageOut) / 100;
            decimal actualWeight = new decimal((double)target);
            actualWeight += (target * percent);

            for (int i = 0; i < howManyToCreate; i++)
            {
                weights.Add(new PcsWeights(name, "480-" + i.ToString(), "BLUCON", target, actualWeight, RecipeTypes.Conc, _pcsToleranceParameter));
            }
            return weights;
        }
    }
}
