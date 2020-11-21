using BatchDataAccessLibrary.Enums;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using System.Collections.Generic;

namespace BatchReports.ComplianceChecker.Models
{
    public class PcsActiveTemps : PcsIndividualParametersBase
    {
        private readonly IPcsActiveTempParameters _pcsActiveTempParameters;


        public PcsActiveTemps(string parameterName, string batchNum, string recipeName, decimal vesselTemp, RecipeTypes recipeType, IPcsActiveTempParameters pcsActiveTempParameters, IPcsToleranceParameterRepository pcsToleranceParameterRepository) :
            base(parameterName, batchNum, recipeName, recipeType, pcsToleranceParameterRepository)
        {
            _pcsActiveTempParameters = pcsActiveTempParameters;
            ActualWeight = vesselTemp;
            SetLimits();
            IsOutOfTolerance = CalculateOutOfSpec(UpperLimit, LowerLimit);
            IsOutOfRange = CalculateOutOfSpec(OutOfRangeUpperLimit, OutOfRangeLowerLimit);
        }
        protected override void SetLimits()
        {
            PcsTempTargets temps = _pcsActiveTempParameters.GetTargetsFor(RecipeName);
            TargetWeight = temps.Target;
            UpperLimit = temps.Target + 0.5M;
            LowerLimit = temps.Target - 0.5M;
            OutOfRangeLowerLimit = 1; //temps.Target - 0.6M;
            OutOfRangeUpperLimit = 99; //temps.Target + 0.6M;
        }
        public override KeyValuePair<string, string> GetErrorDisplayMessage()
        {
            string underOver = GetUnderOverString();
            return new KeyValuePair<string, string>(BatchNumber, $"({RecipeName}) { ParameterName } { underOver } by { ActualWeight - TargetWeight }  C");
        }
        protected internal override string GetUnderOverString()
        {
            if (ActualWeight < TargetWeight)
            {
                return "was below";
            }
            return "was over";
        }

        protected override bool CalculateOutOfSpec(decimal upperLimit, decimal lowerLimit)
        {
            bool output = false;

            if (ActualWeight > upperLimit || ActualWeight < lowerLimit)
            {
                output = true;
            }
            if (ActualWeight + 0.1M <= upperLimit && ActualWeight + 0.1M >= lowerLimit)
            {
                output = false;
            }
            if(ActualWeight - 0.1M <= upperLimit && ActualWeight + 0.1M >= lowerLimit)
            {
                output = false;
            }
            if(ActualWeight < upperLimit && ActualWeight > lowerLimit)
            {
                output = false;
            }

            return output;
        }

        

    }



}
