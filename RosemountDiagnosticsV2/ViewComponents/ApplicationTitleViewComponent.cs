using DocumentFormat.OpenXml.Presentation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.DotNet;
using RosemountDiagnosticsV2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RosemountDiagnosticsV2.ViewComponents
{
    public class ApplicationTitleViewComponent : ViewComponent
    {
        private readonly IApplicationData _applicationData;

        public ApplicationTitleViewComponent(IApplicationData applicationInfo)
        {
            _applicationData = applicationInfo;
        }
        public IViewComponentResult Invoke()
        {
            object title = _applicationData.PageTitle;
            return View(title);
        }
    }
}
