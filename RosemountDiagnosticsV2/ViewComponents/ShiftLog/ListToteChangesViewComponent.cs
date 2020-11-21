using BatchDataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace RosemountDiagnosticsV2.ViewComponents.ShiftLog
{
    public class ListToteChangesViewComponent : ViewComponent
    {
        private readonly IShiftLogRepository _shiftLogRepository;

        public ListToteChangesViewComponent(IShiftLogRepository shiftLogRepository)
        {
            _shiftLogRepository = shiftLogRepository;
        }
        public IViewComponentResult Invoke(int shiftId)
        {
            var toteChanges = _shiftLogRepository.GetToteChanges(shiftId);
            return View(toteChanges);
        }
    }
}
