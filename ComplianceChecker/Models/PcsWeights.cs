using BatchDataAccessLibrary.Enums;
using BatchDataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;

namespace BatchReports.ComplianceChecker.Models
{
    public class PcsWeights : PcsIndividualParametersBase
    {
        public PcsWeights(string parameterName, string batchNum, string recipeName, decimal targetWeight, decimal actualWeight, RecipeTypes recipeType, IPcsToleranceParameterRepository pcsToleranceParameterRepository) :
            base(parameterName, batchNum, recipeName, recipeType, pcsToleranceParameterRepository)
        {
            if(parameterName.ToLower() == "fatty alc")
            {
                Console.WriteLine();
            }
            TargetWeight = targetWeight;
            ActualWeight = actualWeight;
            SetTolerances();
            SetLimits();
            IsOutOfTolerance = CalculateOutOfSpec(UpperLimit, LowerLimit);
            IsOutOfRange = CalculateOutOfSpec(OutOfRangeUpperLimit, OutOfRangeLowerLimit);
        }

        protected override void SetLimits()
        {
            UpperLimit = CalculateUpperLimit(TargetWeight, Tolerance);
            LowerLimit = CalculateLowerLimit(TargetWeight, Tolerance);
            OutOfRangeUpperLimit = CalculateUpperLimit(TargetWeight, ToleranceOutOfRange);
            OutOfRangeLowerLimit = CalculateLowerLimit(TargetWeight, ToleranceOutOfRange);
        }
        public override KeyValuePair<string, string> GetErrorDisplayMessage()
        {
            string underOver = GetUnderOverString();
            decimal percentageOut = GetPercentage(ActualWeight, TargetWeight);
            return new KeyValuePair<string, string>(BatchNumber, $"({RecipeName}) { underOver } { ParameterName.ToLower() }  by { Math.Abs(percentageOut) }%");
        }
        protected internal override string GetUnderOverString()
        {
            if (ActualWeight < TargetWeight)
            {
                return "under weighed";
            }
            return "over weighed";
        }




    }
}
