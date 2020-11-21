using BatchDataAccessLibrary.Enums;
using System.Collections.Generic;

namespace ComplianceChecker
{
     public interface IPcsIndividualParameters
    {
        string BatchNumber { get; set; }
        decimal TargetWeight { get; set; }
        decimal ActualWeight { get; set; }
        RecipeTypes RecipeType { get; set; }
        string RecipeName { get; set; }
         bool IsOutOfTolerance { get; set; }
         bool IsOutOfRange { get; set; }
         decimal Tolerance { get; }
         decimal ToleranceOutOfRange { get;  }
         string ParameterName { get;  }
         decimal UpperLimit { get; }
         decimal LowerLimit { get;}
         decimal OutOfRangeUpperLimit { get;  }
         decimal OutOfRangeLowerLimit { get; }
        KeyValuePair<string, string> GetErrorDisplayMessage();
    }
}