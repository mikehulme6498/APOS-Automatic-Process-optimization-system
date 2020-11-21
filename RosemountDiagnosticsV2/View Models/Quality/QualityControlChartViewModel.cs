using Microsoft.AspNetCore.Mvc.Rendering;
using RosemountDiagnosticsV2.Interfaces;
using RosemountDiagnosticsV2.Models;
using RosemountDiagnosticsV2.View_Models.Quality;
using System.Collections.Generic;

namespace RosemountDiagnosticsV2.View_Models
{
    public class QualityControlChartViewModel
    {
        public DateSelectorModal DateSelectorModal { get; set; }
        public string ProductType { get; set; }
        public string Parameter { get; set; }
        public string ParameterId { get; set; }
        public List<SelectListItem> ProductTypeForDropdown { get; set; }
        public List<SelectListItem> ParametersForDropdown { get; set; }
        public List<ControlChartData> ChartData { get; set; } = new List<ControlChartData>();


        public QualityControlChartViewModel()
        {
            ProductTypeForDropdown = new List<SelectListItem>();
            ParametersForDropdown = new List<SelectListItem>();
        }

        public void SetDemoMode()
        {
            SetProductTypes();
            SetDemoParameters();
        }

        public void SetFullAppMode()
        {
            SetProductTypes();
            SetParameters();
        }


        private void SetProductTypes()
        {
            ProductTypeForDropdown.Clear();
            ProductTypeForDropdown.Add(new SelectListItem() { Text = "Concentrate", Value = "1" });
            ProductTypeForDropdown.Add(new SelectListItem() { Text = "Ultra Dilute", Value = "2" });
            ProductTypeForDropdown.Add(new SelectListItem() { Text = "Big Bang", Value = "3" });
        }

        private void SetParameters()
        {
            ParametersForDropdown.Clear();
            ParametersForDropdown.Add(new SelectListItem() { Text = "Visco", Value = "1" });
            ParametersForDropdown.Add(new SelectListItem() { Text = "pH", Value = "2" });
            ParametersForDropdown.Add(new SelectListItem() { Text = "Softquat Quantity", Value = "3" });
            ParametersForDropdown.Add(new SelectListItem() { Text = "Stenol Quantity", Value = "4" });
            ParametersForDropdown.Add(new SelectListItem() { Text = "Active Drop Temperature", Value = "5" });
            ParametersForDropdown.Add(new SelectListItem() { Text = "HCL Quantity", Value = "6" });
        }

        private void SetDemoParameters()
        {
            ParametersForDropdown.Clear();
            ParametersForDropdown.Add(new SelectListItem() { Text = "Visco", Value = "1" });
            ParametersForDropdown.Add(new SelectListItem() { Text = "pH", Value = "2" });
            ParametersForDropdown.Add(new SelectListItem() { Text = "Material 51 Quantity", Value = "3" });
            ParametersForDropdown.Add(new SelectListItem() { Text = "Material 21 Quantity", Value = "4" });
            ParametersForDropdown.Add(new SelectListItem() { Text = "Active Drop Temperature", Value = "5" });
            ParametersForDropdown.Add(new SelectListItem() { Text = "Material 59 Quantity", Value = "6" });
        }
    }

}

