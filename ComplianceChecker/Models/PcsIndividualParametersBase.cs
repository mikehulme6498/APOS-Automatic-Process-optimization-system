using BatchDataAccessLibrary.Enums;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using ComplianceChecker;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BatchReports.ComplianceChecker.Models
{
    [DebuggerDisplay("{ BatchNumber } - Weight : { ActualWeight } - Out Of 3%  & 12% : { IsOutOfTolerance } { IsOutOfRange }")]
    abstract public class PcsIndividualParametersBase : IPcsIndividualParameters
    {
        private readonly IPcsToleranceParameterRepository _pcsToleranceRepository;

        public PcsIndividualParametersBase(string parameterName, string batchNum, string recipeName, RecipeTypes recipeType, IPcsToleranceParameterRepository _PcsToleranceRepository)
        {
            _pcsToleranceRepository = _PcsToleranceRepository;
            ParameterName = parameterName;
            BatchNumber = batchNum;
            RecipeType = recipeType;
            RecipeName = recipeName;
        }
        public string BatchNumber { get; set; }
        public decimal TargetWeight { get; set; }
        public decimal ActualWeight { get; set; }
        public RecipeTypes RecipeType { get; set; }
        public string RecipeName { get; set; }
        public bool IsOutOfTolerance { get; set; } = false;
        public bool IsOutOfRange { get; set; } = false;
        public decimal Tolerance { get; protected set; }
        public decimal ToleranceOutOfRange { get; protected set; }

        public string ParameterName { get; protected set; }
        public decimal UpperLimit { get; protected set; }
        public decimal LowerLimit { get; protected set; }
        public decimal OutOfRangeUpperLimit { get; protected set; }
        public decimal OutOfRangeLowerLimit { get; protected set; }

        protected decimal GetPercentage(decimal actual, decimal target)
        {
            return Decimal.Round(100 - (actual / target * 100), 2);
        }

        protected abstract void SetLimits();
        protected virtual bool CalculateOutOfSpec(decimal upperLimit, decimal lowerLimit)
        {
            if (ActualWeight > upperLimit || ActualWeight < lowerLimit)
            {
                return true;
            }
            return false;
        }
        protected virtual decimal CalculateUpperLimit(decimal target, decimal toleranceInPercent)
        {
            decimal percentage = toleranceInPercent / 100;
            return target + (target * percentage);
        }

        protected virtual decimal CalculateLowerLimit(decimal target, decimal toleranceInPercent)
        {
            decimal percentage = toleranceInPercent / 100;
            return target - (target * percentage);
        }

        protected virtual void SetTolerances()
        {
            PcsToleranceParameters pcsToleranceParameters = _pcsToleranceRepository.GetTolerances();
            Tolerance = pcsToleranceParameters.TolerancePercent;
            ToleranceOutOfRange = pcsToleranceParameters.OutOfRangePercent;
        }

        protected virtual internal string GetUnderOverString()
        {
            if (ActualWeight < TargetWeight)
            {
                return "was below";
            }
            return "was over";
        }

        public abstract KeyValuePair<string, string> GetErrorDisplayMessage();

    }
}
