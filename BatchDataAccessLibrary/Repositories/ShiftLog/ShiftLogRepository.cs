using BatchDataAccessLibrary.DataAccess;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using BatchDataAccessLibrary.Models.ShiftLog;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace BatchDataAccessLibrary.Repositories.ShiftLog
{
    public class ShiftLogRepository : IShiftLogRepository
    {
        private readonly BatchContext _batchContext;

        public ShiftLogRepository(BatchContext batchContext)
        {
            _batchContext = batchContext;
        }

        public void AddBatchesToShiftLog(List<BatchReport> reports)
        {

            foreach (var report in reports.Where(x => x.IsValidBatch == true))
            {
                var shiftLog = GetOrCreateShiftLog(report.StartTime);
                if (!DoesBatchExistInShift(shiftLog.OperatorShiftLogId, report.BatchReportId))
                {
                    _batchContext.BatchesForShift.Add(new BatchesForShift
                    {
                        ShiftId = shiftLog.OperatorShiftLogId,
                        BatchId = report.BatchReportId,
                    });
                }
            }

            _batchContext.SaveChanges();
        }

        public void AddCipWashToShiftLog(int shiftId, string vesselWashedName)
        {
            throw new NotImplementedException();
        }

        public void AddCommentToShiftlog(int shiftId, string commeent)
        {
            throw new NotImplementedException();
        }

        public void AddEffluentToShiftLog(int shiftId, double effluentLevel)
        {
            var shiftLog = _batchContext.ShiftLog.Where(x => x.OperatorShiftLogId == shiftId).FirstOrDefault();
            shiftLog.EffluentAtStartOfShift = effluentLevel;
            _batchContext.Update(shiftLog);
            _batchContext.SaveChanges();
        }

        public void AddOperatorToShiftLog(int shiftId, string operatorName)
        {
            var shiftLog = _batchContext.ShiftLog.Where(x => x.OperatorShiftLogId == shiftId).FirstOrDefault();
            if (shiftLog.Operators != null)
            {
                shiftLog.Operators = shiftLog.Operators + ", " + operatorName;
            }
            else
            {
                shiftLog.Operators = operatorName;
            }
            _batchContext.Update(shiftLog);
            _batchContext.SaveChanges();
        }

        public void AddToteChange(int shiftId, string toteName)
        {
            ToteChange toteChange = new ToteChange
            {
                ShiftId = shiftId,
                ToteName = toteName
            };

            _batchContext.ToteChanges.Add(toteChange);
            _batchContext.SaveChanges();
        }

        private OperatorShiftLog CreateNewShift(DateTime dateTime)
        {
            //dateTime = AdjustDateIfAfter12OClock(dateTime);
            OperatorShiftLog newLog = new OperatorShiftLog()
            {
                Date = dateTime,
                DaysNights = GetShiftDayNight(dateTime),
            };

            _batchContext.Add(newLog);
            _batchContext.SaveChanges();
            return newLog;
        }

        public List<BatchesForShift> GetBatchesForShift(int shiftId)
        {
            return _batchContext.BatchesForShift.Where(x => x.ShiftId == shiftId).ToList();
        }

        public int GetBBReworkToteCount(int shiftId)
        {
            return _batchContext.ShiftLog.Where(x => x.OperatorShiftLogId == shiftId).Select(x => x.BigBangReworkTotes).First();
        }

        public ShiftTeam GetOperators(string shiftColour)
        {
            return _batchContext.ShiftTeams.Where(x => x.ShiftColour.ToLower() == shiftColour.ToLower()).FirstOrDefault();
        }

        public string GetOperators(int shiftId)
        {
            return _batchContext.ShiftLog.Where(x => x.OperatorShiftLogId == shiftId).Select(x => x.Operators).First();
        }

        public int GetReworkToteCount(int shiftId)
        {
            return _batchContext.ShiftLog.Where(x => x.OperatorShiftLogId == shiftId).Select(x => x.ReworkTotes).FirstOrDefault();
        }

        public int GetTotalReworkCount(int shiftId)
        {
            int normalRework = GetReworkToteCount(shiftId);
            int bbRework = GetBBReworkToteCount(shiftId);
            return normalRework + bbRework;
        }

        public List<string> GetToteChanges(int shiftId)
        {
            return _batchContext.ToteChanges.Where(x => x.ShiftId == shiftId).Select(x => x.ToteName).ToList();
        }

        public int GetToteChangesCount(int shiftId)
        {
            return _batchContext.ToteChanges.Where(x => x.ShiftId == shiftId).Count();
        }

        public void RemoveOperatorFromShiftLog(int shiftId, string operatorName)
        {
            var shiftLog = _batchContext.ShiftLog.Where(x => x.OperatorShiftLogId == shiftId).FirstOrDefault();
            string[] ops = shiftLog.Operators.Split(',').Where(n => n != operatorName).ToArray();
            string opsNew = string.Join(",", ops);
            shiftLog.Operators = opsNew;
            _batchContext.Update(shiftLog);
            _batchContext.SaveChanges();

        }

        public void RemoveToteFromShift(int shiftId, string toteName)
        {
            ToteChange toteChange = _batchContext.ToteChanges.Where(x => x.ToteName == toteName).Last();
            _batchContext.ToteChanges.Remove(toteChange);
            _batchContext.SaveChanges();
        }

        public void UpdateBigBangReworkTotes(int shiftId, int toteCount)
        {
            OperatorShiftLog shiftLog = _batchContext.ShiftLog.Where(x => x.OperatorShiftLogId == shiftId).First();
            shiftLog.BigBangReworkTotes = toteCount;
            _batchContext.Update(shiftLog);
            _batchContext.SaveChanges();
        }

        public void UpdateReworkTotes(int shiftId, int toteCount)
        {
            OperatorShiftLog shiftLog = _batchContext.ShiftLog.Where(x => x.OperatorShiftLogId == shiftId).First();
            shiftLog.ReworkTotes = toteCount;
            _batchContext.Update(shiftLog);
            _batchContext.SaveChanges();
        }

        public void UpdateBatchesForShift(int shiftId, int batchId, bool washed, string vesselType, string vesselName)
        {
            var batch = _batchContext.BatchesForShift.Where(x => x.ShiftId == shiftId && x.BatchId == batchId).First();
            switch (vesselType)
            {
                case "stream":
                    batch.StreamWash = washed;
                    batch.StreamWashVesselName = washed ? vesselName : "";
                    break;
                case "stocktank":
                    batch.StockTankWash = washed;
                    batch.StockTankWashVesselName = washed ? vesselName : "";
                    break;
            }
            _batchContext.Update(batch);
            _batchContext.SaveChanges();
        }

        public void AddGoodStockToWaste(GoodStockToWaste goodStockToWaste)
        {
            _batchContext.GoodStockToWaste.Add(goodStockToWaste);
            _batchContext.SaveChanges();
        }

        public bool DoesShiftExist(DateTime dateTime)
        {
            return _batchContext.ShiftLog.Any(x => x.Date.Day == dateTime.Day && x.Date.Month == dateTime.Month && x.Date.Year == dateTime.Year && x.DaysNights == GetShiftDayNight(dateTime));
        }

        private string GetShiftDayNight(DateTime time)
        {
            if (time.Hour >= 06 && time.Hour < 18)
            {
                return "Days";
            }
            return "Nights";
        }

        public OperatorShiftLog GetShiftLogById(int shiftId)
        {
            return _batchContext.ShiftLog.Where(x => x.OperatorShiftLogId == shiftId).FirstOrDefault();
        }

        public OperatorShiftLog GetOrCreateShiftLog(DateTime dateTime)
        {
            
            dateTime = AdjustDateIfAfter12OClock(dateTime);

            if (DoesShiftExist(dateTime))
            {
                return _batchContext.ShiftLog.Where(x => x.Date.Day == dateTime.Day && x.Date.Month == dateTime.Month && x.Date.Year == dateTime.Year && x.DaysNights == GetShiftDayNight(dateTime)).FirstOrDefault();
            }
            else
            {
                return CreateNewShift(dateTime);
            }
        }

        private DateTime AdjustDateIfAfter12OClock(DateTime dateTime)
        {
            if (dateTime.Hour >= 0 && dateTime.Hour < 6)
            {
                return dateTime.AddDays(-1);
            }
            return dateTime;
        }

        public List<SelectListItem> GetShiftColoursForDropDown()
        {
            List<SelectListItem> output = new List<SelectListItem>();

            foreach (var colour in _batchContext.ShiftTeams.OrderBy(x => x.ShiftColour).Select(x => x.ShiftColour).ToList())
            {
                output.Add(new SelectListItem { Text = colour, Value = colour });
            }

            return output;
        }

        public void AddShiftColourOperatorsToShiftLog(int shiftId, string shiftColour)
        {
            ShiftTeam shiftTeam = GetOperators(shiftColour);
            OperatorShiftLog shiftLog = GetShiftLogById(shiftId);

            shiftLog.ShiftColour = shiftTeam.ShiftColour;
            shiftLog.Operators = $"{shiftTeam.Operator1}, {shiftTeam.Operator2}";
            _batchContext.SaveChanges();
        }

        public List<OperatorShiftLog> GetAllShifts()
        {
            return _batchContext.ShiftLog
                .OrderByDescending(x => x.Date).
                ThenByDescending(x => x.DaysNights).
                ToList();
        }

        public bool DoesBatchExistInShift(int shiftId, int batchId)
        {
            return _batchContext.BatchesForShift.Any(x => x.ShiftId == shiftId && x.BatchId == batchId);
        }
    }
}
