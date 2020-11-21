using BatchDataAccessLibrary.Enums;
using BatchDataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;

namespace BatchReports.ComplianceChecker.Models
{
    public class PcsDyes : PcsIndividualParametersBase
    {
        public PcsDyes(string parameterName, string batchNum, string recipeName, decimal targetWeight, decimal dyeWeight, RecipeTypes recipeType, IPcsToleranceParameterRepository pcsToleranceParameterRepository)
            : base(parameterName, batchNum, recipeName, recipeType, pcsToleranceParameterRepository)
        {
            ActualWeight = dyeWeight;
            TargetWeight = targetWeight;
            SetTolerances();
            SetLimits();
            IsOutOfTolerance = CalculateOutOfSpec(UpperLimit, LowerLimit);
            IsOutOfRange = CalculateOutOfSpec(OutOfRangeUpperLimit, OutOfRangeLowerLimit);
        }

        protected override bool CalculateOutOfSpec(decimal upperLimit, decimal lowerLimit)
        {
            if (TargetWeight < 5)
            {
                var tempUpperLimit = TargetWeight + (TargetWeight * 0.25M);
                var templowerLimit = TargetWeight - (TargetWeight * 0.25M);

                if (ActualWeight > tempUpperLimit || ActualWeight < templowerLimit)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (ActualWeight > upperLimit || ActualWeight < lowerLimit)
            {
                return true;
            }
            return false;
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
