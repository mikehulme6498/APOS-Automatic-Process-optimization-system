using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RosemountDiagnosticsV2.Extension_Methods
{
    public static class StringExtensions
    {
        public static string EmailToShortName(this string email)
        {
            if (!email.Contains("@") || !email.Contains('.')){ return email; }

            var emailstart = email.Substring(0, email.IndexOf('@'));
            var domain = email.Substring(email.IndexOf('@') + 1, email.LastIndexOf('.') - email.IndexOf('@')-1);

            if (!domain.ToLower().Contains("unilever") || !emailstart.Contains(".")){ return email; }

            var initial = email.Substring(0, 1);
            var surname = email.Substring(email.IndexOf('.')+1, email.IndexOf('@')- email.IndexOf('.') -1);

            if (surname.Contains('.'))
            {
                surname = surname.Substring(0, surname.IndexOf('.'));
            }
            var surnameFormatted = "";
            for (int i = 0; i < surname.Length; i++)
            {
                if (i == 0)
                {
                    surnameFormatted += surname[i].ToString().ToUpper();
                }
                else
                {
                    surnameFormatted += surname[i].ToString().ToLower();
                }

            }
            return $"{initial.ToUpper()}.{surnameFormatted}";
        }
    }
}
