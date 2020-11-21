using BatchDataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RosemountDiagnosticsV2.ViewComponents.ShiftLog
{
    public class ListOperatorsViewComponent : ViewComponent
    {
        private readonly IShiftLogRepository _shiftLogRepository;

        public ListOperatorsViewComponent(IShiftLogRepository shiftLogRepository)
        {
            _shiftLogRepository = shiftLogRepository;
        }
        public IViewComponentResult Invoke(int shiftId)
        {
            var OperatorsAsString = _shiftLogRepository.GetOperators(shiftId);
            List<string> operators = OperatorsAsString.Split(",").ToList();
            return View(operators);
        }

       
    }
}
