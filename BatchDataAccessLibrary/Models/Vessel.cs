using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BatchDataAccessLibrary.Models
{
    [DebuggerDisplay("{ VesselName } - { VesselType }")]
    
    public class Vessel
    { 
        public enum VesselTypes { PerfumePreWeigher, ActivePreWeigher, CalciumPreWeigher, MainMixer, None };
    
        //public int BatchId { get; set; }
        public int VesselId { get; set; }
        public VesselTypes VesselType { get; set; }
        
        [MaxLength(100)]
        public string VesselName { get; set; }
        public double Decrease { get; set; }
        public double DumpTime { get; set; }
        public DateTime TimeCompleted { get; set; }
        
        public List<Material> Materials { get; set; }

        public Vessel()
        {
            Materials = new List<Material>();
            VesselType = VesselTypes.None;
        }

        public Vessel(VesselTypes vesselType)
        {
            VesselType = vesselType;
        }

        public Material getSingleMaterialFromList(int index)
        {
            return Materials[index];
        }

        public Material getSingleMaterialFromList(string name)
        {
            foreach (Material material in Materials)
            {
                if (material.Name == name)
                {
                    return material;
                }
            }
            return null;
        }

        public void SetVesselType(string name)
        {
            if (name == "V102" || name == "V112" || name == "V202" || name == "V212" || name == "V302")
            {
                VesselType = VesselTypes.PerfumePreWeigher;
            }
            else if (name == "V103" || name == "V203" || name == "V303")
            {
                VesselType = VesselTypes.ActivePreWeigher;
            }
            else if (name == "V906" || name == "V906 Part 1")
            {
                VesselType = VesselTypes.CalciumPreWeigher;
            }
            else if (name == "V104" || name == "V204" || name == "V304" || name == "V114" || name == "V214")
            {
                VesselType = VesselTypes.MainMixer;
            }
            else
            {
                VesselType = VesselTypes.None;
            }

        }
    }
}
