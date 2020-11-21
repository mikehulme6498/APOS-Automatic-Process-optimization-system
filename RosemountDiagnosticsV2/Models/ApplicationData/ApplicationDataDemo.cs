using RosemountDiagnosticsV2.Interfaces;

namespace RosemountDiagnosticsV2.Models.ApplicationData
{
    public class ApplicationDataDemo : IApplicationData
    {
        public string PageTitle => "Process Plant Diagnositcs";

        public string PageLogoFilePath => "/Images/FactoryIcon.png";

        public string ApplicationMode => "demo";
    }
}
