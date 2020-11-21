using RosemountDiagnosticsV2.Interfaces;

namespace RosemountDiagnosticsV2.Models.ApplicationData
{
    public class ApplicationData : IApplicationData
    {
        public string PageTitle => "No 4 Making Plant - Diagnositcs";

        public string PageLogoFilePath => "/Images/UnileverLogoWhiteSmall.png";

        public string ApplicationMode => "full";
    }
}
