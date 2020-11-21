using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using System;
using System.Linq;
using static BatchDataAccessLibrary.Models.Vessel;

namespace BatchDataAccessLibrary.Helpers
{
    public class DemoBatchHelperMethods : IHelperMethods
    {
        public Material FindReworkInBatch(BatchReport report)
        {
            Vessel vessel = report.AllVessels.Where(x => x.VesselType == VesselTypes.MainMixer).First();
            Material output = vessel.Materials.Where(x => x.Name == "Material 94" || x.Name == "Material 3").FirstOrDefault();
            return output;
        }

        public decimal GetTemperatureOfActiveDrop(Vessel vessel)
        {
            // Temperarture is taken from the material before Empty Vxx3 as temperature on report is
            // registered once the material has finished dropping.

            vessel.Materials = vessel.Materials.OrderBy(x => x.StartTime.Date).ThenBy(x => x.StartTime.TimeOfDay).ToList();

            for (int i = 0; i < vessel.Materials.Count; i++)
            {
                string material = vessel.Materials[i].Name.ToLower();
                if (material == "empty preweigher1" || material == "empty preweigher2" || material == "empty preweigher3")
                {
                    //if (i == 0)
                    //{
                    //    return Convert.ToDecimal(vessel.Materials[i].VesselTemp);
                    //}

                    return Convert.ToDecimal(vessel.Materials[i - 1].VesselTemp);
                }
            }
            return 0;
        }
    }
}
