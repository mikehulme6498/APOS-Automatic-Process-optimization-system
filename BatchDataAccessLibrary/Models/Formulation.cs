using BatchDataAccessLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BatchDataAccessLibrary.Models
{
    public class Formulation
    {
        public int FormulationId { get; set; } 
        public string Name { get; set; }
        public int Code { get; set; }
        public RecipeTypes RecipeTypes { get; set; }
        public string RecipeTypeDisplayName { get; set; }
        public List<FormulationMaterials> Materials { get; set; } = new List<FormulationMaterials>();
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}

