using BatchDataAccessLibrary.Models;
using BatchDataAccessLibrary.Models.ShiftLog;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BatchDataAccessLibrary.Interfaces
{
    public interface IShiftLogRepository
    {
        //OperatorShiftLog CreateNewShift(DateTime dateTime);
        void AddBatchesToShiftLog(List<BatchReport> reports);
        void AddOperatorToShiftLog(int shiftId, string operatorName);
        void AddShiftColourOperatorsToShiftLog(int shiftId, string shiftColour);
        void AddCipWashToShiftLog(int shiftId, string vesselWashedName);
        void AddEffluentToShiftLog(int shiftId, double effluentLevel);
        void AddGoodStockToWaste(GoodStockToWaste goodStockToWaste);
        void AddCommentToShiftlog(int shiftId, string commeent);
        void AddToteChange(int shiftId, string toteName);
        void UpdateReworkTotes(int shiftId, int toteCount);
        void UpdateBigBangReworkTotes(int shiftId, int toteCount);
        void RemoveOperatorFromShiftLog(int shiftId, string operatorName);
        void RemoveToteFromShift(int shiftId, string toteName);

        List<string> GetToteChanges(int shiftId);
        int GetReworkToteCount(int shiftId);
        int GetBBReworkToteCount(int shiftId);
        int GetTotalReworkCount(int shiftId);
        int GetToteChangesCount(int shiftId);
        ShiftTeam GetOperators(string shiftColour);
        string GetOperators(int shiftId);
        List<OperatorShiftLog> GetAllShifts();
        OperatorShiftLog GetShiftLogById(int shiftId);
        OperatorShiftLog GetOrCreateShiftLog(DateTime dateTime);
        List<BatchesForShift> GetBatchesForShift(int shiftId);
        List<SelectListItem> GetShiftColoursForDropDown();
        bool DoesShiftExist(DateTime dateTime);
        bool DoesBatchExistInShift(int shiftId, int batchId);

        void UpdateBatchesForShift(int shiftId, int batchId, bool washed, string vesselType, string vesselName );
        

    }
}
