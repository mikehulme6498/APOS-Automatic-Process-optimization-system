using BatchDataAccessLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BatchDataAccessLibrary.Models
{
    public class PcsWeightParameters
    {
        public int PcsWeightParametersId { get; set; }
        public RecipeTypes RecipeType { get; set; }
        public string Parameter { get; set; }

       
    }
}
