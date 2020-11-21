using System;
using System.Linq;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using static BatchDataAccessLibrary.Helpers.BatchHelperMethods;
using static BatchDataAccessLibrary.Models.Vessel;

namespace BatchReports.IssueScanner
{
    public class AdjustmentsFirstIssueScanner : IssueScannerBase
    {
        public AdjustmentsFirstIssueScanner(IMaterialDetailsRepository materialDetailsRepository) : base(materialDetailsRepository)
        {
            ScanType = ScanTypes.Adjustment;
            IssueDescriptor = "Adjustment";
        }
        public override void ScanForIssues(BatchReport report)
        {
            //AdjustWaterStartTimes(report);
            //AdjustPerfumeStartTime(report);
            SortMaterialsByStartTimes(report);
        }

        private void AdjustWaterStartTimes(BatchReport report)
        {
            // COLD & HOT Water start times are wrong, the start time is actually the time it finished
            // This function alters the start times to make them correct
            
                Material hotWater = GetSingleMaterialFromVessel(report, VesselTypes.MainMixer, "HOT WTR");
                Material coldWater = GetSingleMaterialFromVessel(report, VesselTypes.MainMixer, "COLD WTR");
                hotWater.StartTime = hotWater.StartTime.AddMinutes(-hotWater.WeighTime);
                coldWater.StartTime = coldWater.StartTime.AddMinutes(-coldWater.WeighTime);         
        }

        private void AdjustPerfumeStartTime(BatchReport report)
        {
            // The Perfume start time is wrong, the start time is actually the time it finished
            // This function alters the start times to make them correct

            Material perfume = GetSingleMaterialFromVessel(report, VesselTypes.PerfumePreWeigher, 0);
            perfume.StartTime = perfume.StartTime.AddMinutes(-perfume.WeighTime);
        }

        private void SortMaterialsByStartTimes(BatchReport report)
        {
            foreach (var vessel in report.AllVessels)
            {                             
                vessel.Materials = vessel.Materials.OrderBy(m => m.StartTime).ToList();
            }
        }
    }
}
