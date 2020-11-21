using Microsoft.AspNetCore.Mvc;
using RosemountDiagnosticsV2.Interfaces;

namespace RosemountDiagnosticsV2.ViewComponents
{
    public class ApplicationLogoViewComponent : ViewComponent
    {
        private readonly IApplicationData _applicationData;

        public ApplicationLogoViewComponent(IApplicationData applicationData)
        {
            _applicationData = applicationData;
        }

        public IViewComponentResult Invoke()
        {
            object logoPath = _applicationData.PageLogoFilePath;
            return View(logoPath);
        }
    }
}
