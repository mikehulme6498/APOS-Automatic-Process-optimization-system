using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using RosemountDiagnosticsV2.View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RosemountDiagnosticsV2.ViewComponents
{
    public class QualityControlChartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(QualityControlChartViewModel qualityControlChartViewModel)
        {
            return View(qualityControlChartViewModel);
        }
    }
}
