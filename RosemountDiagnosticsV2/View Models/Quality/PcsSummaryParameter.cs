using System.Collections.Generic;

namespace RosemountDiagnosticsV2.View_Models.Quality
{
    public class PcsSummaryParameter
    {        
        
        public string Heading { get; set; }
        public string Subheading { get; set; }
        public List<KeyValuePair<string, string>> Reasons { get; set; }
        
    }
}