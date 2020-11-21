using System.Text;

namespace BatchDataAccessLibrary.Helpers
{
    public static class StringExtentionMethods
    {
        public static string StringArrayToString(this string[] array)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var text in array)
            {
                sb.AppendLine(text);
            }

            return sb.ToString();
        }
    }

}
