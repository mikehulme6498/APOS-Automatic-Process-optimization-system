using System.Collections.Generic;

namespace BatchDataAccessLibrary.Interfaces
{
    public interface IGapInTimeReasons
    {
        List<string> GetReasonForGap(string material1, string material2);
    }
}
