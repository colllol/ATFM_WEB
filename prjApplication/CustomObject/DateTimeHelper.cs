using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace prjApplication
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

        public static DateTime ToDate(string x, string kieu)
        {
            try
            {
                int sp1 = x.IndexOf('/'), sp2 = x.LastIndexOf('/');
                int day, month, year;
                if (kieu.Equals("MM/dd/yyyy")) // MM/dd/yyyy hoac M/d/yyyy
                {
                    day = int.Parse(x.Substring(sp1 + 1, sp2 - sp1 - 1));
                    month = int.Parse(x.Substring(0, sp1));
                }
                else //'dd/MM/yyyy' hoac d/M/yyyy
                {
                    day = int.Parse(x.Substring(0, sp1));
                    month = int.Parse(x.Substring(sp1 + 1, sp2 - sp1 - 1));
                }
                year = int.Parse(x.Substring(sp2 + 1, 4));
                return new DateTime(year, month, day);
            }
            catch
            {
                throw new Exception("Sai kieu ngay thang");
            }
        }
    }
}