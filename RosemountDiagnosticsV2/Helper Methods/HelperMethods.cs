using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using RosemountDiagnosticsV2.Models;
using System.Collections.Generic;

namespace RosemountDiagnosticsV2.Helper_Methods
{
    public class GeneralHelperMethods
    {
        private readonly IBatchRepository _BatchRepository;

        public GeneralHelperMethods(IBatchRepository batchRepository)
        {
            _BatchRepository = batchRepository;
        }
        public List<BatchReport> GetBatchReportsForDateSelector(DateSelectorModal dateSelectorModal)
        {
            List<BatchReport> reports = new List<BatchReport>();

            switch (dateSelectorModal.TimeFrame)
            {
                case "year":
                    reports = _BatchRepository.GetBatchesByYear(dateSelectorModal.Year);
                    dateSelectorModal.TimeFrameTitle = dateSelectorModal.Year.ToString();
                    break;
                case "week":
                    reports = _BatchRepository.GetBatchesByWeek(dateSelectorModal.Week, dateSelectorModal.YearForWeek);
                    dateSelectorModal.TimeFrameTitle = dateSelectorModal.YearForWeek.ToString() + " Week : " + dateSelectorModal.Week;
                    break;
                case "dates":
                    reports = _BatchRepository.GetBatchesByDates(dateSelectorModal.DateFrom, dateSelectorModal.DateTo);
                    dateSelectorModal.TimeFrameTitle = dateSelectorModal.DateFrom.ToShortDateString() + " To " + dateSelectorModal.DateTo.ToShortDateString();
                    break;
                default:
                    break;
            }

            return reports;
        }

        public double CalculateDyeAmountInSolution(string dyeName, double value)
        {
            double amountOfDye = 0;

            switch (dyeName)
            {
                case "YELL DYE":
                    amountOfDye = (double)18 / 324 * value;
                    break;
                case "BLUE DYE":
                    amountOfDye = (double)4.5 / 486 * value;
                    break;
                case "VIOLET DYE":
                    amountOfDye = (double)18 / 724 * value;
                    break;
                case "PINK DYE":
                    amountOfDye = (double)9 / 500 * value;
                    break;
                default:
                    break;
            }
            return amountOfDye;
        }
    }
}
