using RosemountDiagnosticsV2.Interfaces;

namespace RosemountDiagnosticsV2.Models.ApplicationData
{
    public class ApplicationDataMock : IApplicationData
    {
        public string PageTitle => "No 4 Making Plant - Diagnositcs (In-Memory Version)";

        public string PageLogoFilePath => "/Images/UnileverLogoWhiteSmall.png";

        public string ApplicationMode => "mock";
    }
}
