using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BatchReports.IssueScanner
{
    public abstract class IssueScannerBase : IIssueScanner
    {
        internal enum ScanTypes
        {
            Quality,
            Issue,
            Adjustment,
            MatVar
        }

        private readonly IMaterialDetailsRepository _materialDetailsRepository;
        internal readonly List<MaterialDetails> materialDetails;
        internal string IssueDescriptor;
        internal const int WeighTimeLossThreshold = 1; // minutes
        internal const int GapInMaterialTimeThreshold = 4; // minutes
        internal ScanTypes ScanType;
        

        public IssueScannerBase(IMaterialDetailsRepository materialDetailsRepository)
        {
            _materialDetailsRepository = materialDetailsRepository;
            materialDetails = _materialDetailsRepository.GetAllMaterialDetails();
        }
        public abstract void ScanForIssues(BatchReport report);
        
        internal void SetIssueScannedFor(BatchReport report)
        {
            report.IssuesScannedFor.Add(new IssuesScannedFor()
            {
                IssueName = IssueDescriptor,
                IssueClassName = this.GetType().Name
            });
        }

        internal int GetCurrentMaterialWeighGroup(string materialName)
        {
            return materialDetails
                        .Where(x => x.Name == materialName)
                        .Select(x => x.ParallelWeighGroup)
                        .FirstOrDefault();
        }
        internal string GetLastMaterialNameFromGroup(int groupNumber)
        {
           return materialDetails
                        .Where(x => x.ParallelWeighGroup == groupNumber)
                        .OrderBy(x => x.ParallelGroupOrder)
                        .Select(x => x.Name)
                        .Last();
        }

        internal List<string> GetListOfMaterialsInGroup(int groupNumber)
        {
            return materialDetails.Where(x => x.ParallelWeighGroup == groupNumber)
                .OrderBy(x => x.ParallelGroupOrder)
                .Select(x => x.Name)
                .ToList();
        }

        ScanTypes IIssueScanner.GetScanType()
        {
            return ScanType;
        }

        public string Descriptor()
        {
            return IssueDescriptor;
        }
    }
}
