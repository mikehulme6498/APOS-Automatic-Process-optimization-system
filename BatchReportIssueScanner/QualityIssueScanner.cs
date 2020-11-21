using BatchDataAccessLibrary.Helpers;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using System.ComponentModel;
using System.Linq;

namespace BatchReports.IssueScanner
{
    public class QualityIssueScanner : IssueScannerBase
    {
        private readonly IMaterialDetailsRepository _materialDetailsRepository;

        public QualityIssueScanner(IMaterialDetailsRepository materialDetailsRepository) : base(materialDetailsRepository)
        {
            IssueDescriptor = "Quality Issues";
            ScanType = ScanTypes.Quality;
            _materialDetailsRepository = materialDetailsRepository;
        }
        public override void ScanForIssues(BatchReport report)
        {
            var tempSensitiveMaterials = _materialDetailsRepository.GetAllMaterialDetails()
                .Where(x => x.MinRawTemp > 0 || x.MaxRawTemp > 0)
                .ToList();

            foreach (var material in tempSensitiveMaterials)
            {
                var materialFromBatch = BatchHelperMethods.GetSingleMaterialFromVessel(report, material.Name);
                if (materialFromBatch != null)
                {
                    bool overTemp = materialFromBatch.RawMatTemp > material.MaxRawTemp;
                    bool underTemp = materialFromBatch.RawMatTemp < material.MinRawTemp;

                    if (overTemp || underTemp)
                    {
                        report.BatchIssues.Add(new BatchIssue
                        {
                            FaultType = overTemp ? BatchIssue.FaultTypes.TemperatureHigh : BatchIssue.FaultTypes.TemperatureLow,
                            Message = $"{material.Name} temperature in storage was {UnderOverText(overTemp)} of {GetMaxMinTemp(overTemp, material)}C",
                            MaterialName = material.Name,
                            MaterialShortName = material.ShortName,
                            TimeLost = 0,
                            ActualReading = materialFromBatch.RawMatTemp,
                            IssueCreatedBy = IssueDescriptor
                        });
                    }
                }

            }
            
            SetIssueScannedFor(report);
        }

        private string UnderOverText(bool overMax)
        {
            return overMax ? "over the maximum" : "under the minimum";
        }

        private double GetMaxMinTemp(bool overMax, MaterialDetails material)
        {
            return overMax ? material.MaxRawTemp : material.MinRawTemp;
        }
        
    }
}
