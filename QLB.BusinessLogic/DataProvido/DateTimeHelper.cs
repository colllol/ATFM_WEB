using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.BusinessLogic
{
    public static class DateTimeHelper
    {
        public static DateTime ConvertToDateTime(string value)
        {
            if (string.IsNullOrEmpty(value)) return new DateTime();
            if (value.IndexOf("-") > 0)
            {
                value = value.Replace("-", "");
            }
            else if (value.IndexOf("/") > 0) value = value.Replace("/", "");
            return DateTime.ParseExact(value, "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
        }

    }    
}
