using BatchDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RosemountDiagnosticsV2.View_Models
{
    public class CreateGuageViewModel
    {
        public RecipeLimits RecipeLimit { get; set; }
        public double ActualValue { get; set; }
        public string Title { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }
}
