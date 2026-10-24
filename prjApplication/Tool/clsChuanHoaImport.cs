using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using prjBusinessLogic;

namespace prjApplication.Tool
{
    public static class clsChuanHoaImport
    {
        public static string ChuanHoaCraft(string value)
        {
            return value.Replace("-", "/").Replace(",", "/").Replace(" ", "/").Replace("(", "").Replace(")", "");
        }
        public static string CraftType(string value)
        {
            string[] ax = value.Split(new string[] { "/", "-" }, StringSplitOptions.RemoveEmptyEntries);
            if (ax.Length > 1) { return ax[0]; }
            return value;

        }
        public static string Remark_Craft(string value)
        {
            string[] ax = value.Split(new string[] { "/", "-" }, StringSplitOptions.RemoveEmptyEntries);
            string _remark = "";
            if (ax.Length > 1)
            {
                for (int i = 1; i < ax.Length; i++)
                {
                    _remark += ax[i] + "/";
                }
                _remark = _remark.Substring(0, _remark.Length - 1);
            }
            else _remark = "";
            return _remark;
        }
        #region ChuanHoaThu
        //public static string ChuanHoaDaylyNew(string value, string month, string year)
        //{
        //    string kq = "";
        //    string thu = "";
        //    DateTime dt = new DateTime(Convert.ToInt32(year), Convert.ToInt32(sMonthly(month)), Convert.ToInt32(value));
        //    thu = dt.DayOfWeek.ToString().Substring(0, 3).ToUpper();
        //    kq = ChuanHoaDayly(thu);
        //    return kq;
        //}
        #endregion

        public static string ChuanHoaDaylySVR(string value, string month, string year)
        {
            string kq = "";
            string thu = "";

            string[] sArrProdID = null;
            char[] sep = { ',' };
            sArrProdID = value.ToString().Trim().Split(sep);

            for (int i = 0; i < sArrProdID.Length; i++)
            {
                DateTime dt = new DateTime(Convert.ToInt32(year), Convert.ToInt32(sMonthly(month)), Convert.ToInt32(sArrProdID[i]));
                if (thu == "")
                    thu = dt.DayOfWeek.ToString().Substring(0, 3).ToUpper();
                else
                    thu += "," + dt.DayOfWeek.ToString().Substring(0, 3).ToUpper();
                if (sArrProdID.Length > 2)
                {
                    if (i == 2)
                        break;
                }

            }
            kq = ChuanHoaDayly(thu);
            return kq;
        }

        public static string ChuanHoaDaylyLKE(string value, string month, string year)
        {
            string kq = "";
            string thu = "";

            string[] sArrProdID = null;
            char[] sep = { ',' };
            sArrProdID = value.ToString().Trim().Split(sep);

            for (int i = 0; i < sArrProdID.Length; i++)
            {
                DateTime dt = new DateTime(Convert.ToInt32(year), Convert.ToInt32(sMonthly(month)), Convert.ToInt32(sArrProdID[i]));
                if (thu == "")
                    thu = dt.DayOfWeek.ToString().Substring(0, 3).ToUpper();
                else
                    thu += "," + dt.DayOfWeek.ToString().Substring(0, 3).ToUpper();
            }
            kq = ChuanHoaDayly(thu);
            return kq;
        }

        public static string sMonthly(string value)
        {
            string kq = "";


            switch (value.ToString())
            {
                case "JAN":
                    kq = "01";
                    break;
                case "FER":
                    kq = "02";
                    break;
                case "MAR":
                    kq = "03";
                    break;
                case "APR":
                    kq = "04";
                    break;
                case "MAY":
                    kq = "05";
                    break;
                case "JUN":
                    kq = "06";
                    break;
                case "JUL":
                    kq = "07";
                    break;
                case "AUG":
                    kq = "08";
                    break;
                case "SEP":
                    kq = "09";
                    break;
                case "OCT":
                    kq = "10";
                    break;
                case "NOV":
                    kq = "11";
                    break;
                case "DEC":
                    kq = "12";
                    break;
            }

            return kq;
        }
        public static string ChuanHoaDayly(string value)
        {
            string kq = "";
            kq = value.Replace("0", ".").Replace("_", ".").Replace("-", ".").Replace(",", ".").Replace("SUN", "7").Replace("MON", "1").Replace("TUE", "2").Replace("WED", "3").Replace("THU", "4").Replace("FRI", "5").Replace("SAT", "6").Replace("/", "").Replace("DAILY", "1234567").Replace("SU", "7").Replace("MO", "1").Replace("TU", "2").Replace("WE", "3").Replace("TH", "4").Replace("FR", "5").Replace("SA", "6");

            kq = sDayly(kq);
            return kq;
        }
        public static string sDayly(string value)
        {
            string kq = "";

            string[] ar = new string[7];
            for (int i = 0; i < value.Length; i++)
            {
                switch (value[i].ToString())
                {
                    case "1":
                        ar[0] = "1";
                        break;
                    case "2":
                        ar[1] = "2";
                        break;
                    case "3":
                        ar[2] = "3";
                        break;
                    case "4":
                        ar[3] = "4";
                        break;
                    case "5":
                        ar[4] = "5";
                        break;
                    case "6":
                        ar[5] = "6";
                        break;
                    case "7":
                        ar[6] = "7";
                        break;
                }
            }
            foreach (var item in ar)
            {
                kq += string.IsNullOrEmpty(item) ? "." : item;
            }

            return kq;
        }
        public static string ChuanHoaGioBay(string value)
        {
            if (value.Contains(":"))
            {
                return ChuanHoaChuSoHangChuc(value.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0]) + ChuanHoaChuSoHangChuc(value.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[1]);
            }
            return value;
        }
        public static string ChuanHoaDateTime(string v)
        {
            if (string.IsNullOrEmpty(v))
                return v;
            if (!v.Contains("-"))
            {
                v = v.ToLower();
                v = v.Replace("jan", "-01-").Replace("feb", "-02-").Replace("mar", "-03-").Replace("apr", "-04-").Replace("may", "-05-").Replace("jun", "-06-").Replace("jul", "-07-").Replace("aug", "-08-").Replace("sep", "-09-").Replace("oct", "-10-").Replace("0ct", "-10-").Replace("nov", "-11-").Replace("dec", "-12-");
            }
            string[] c = v.Split(new string[] { "-", "/" }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                v = ChuanHoaChuSoHangChuc(c[0]) + "-" + ChuanHoaChuSoHangChuc(c[1]) + "-" + c[2];
            }
            catch
            {
            }

            if (c[2].Length == 2)
                c[2] = c[2].Insert(0, "20");
            for (int i = 0; i < dateObj.GetLength(0); i++)
            {
                if (c[1].ToLower().Equals(dateObj[i, 1].ToLower()))
                {
                    c[1] = dateObj[i, 0];
                    v = ChuanHoaChuSoHangChuc(c[0]) + "-" + c[1] + "-" + c[2];
                    break;
                }
            }
            try
            {
                v = ChuanHoaChuSoHangChuc(c[0]) + "-" + ChuanHoaChuSoHangChuc(c[1]) + "-" + c[2];
            }
            catch
            {
            }
            return v.ToUpper();
        }
        #region Chuyennt
        public static string ChuanHoaDateTimeFull(string v)
        {
            if(!v.Contains("-"))
            {
                v = v.ToUpper();
                v = v.Replace("JANUARY", "-01-").Replace("FERBURY", "-02-").Replace("MARCH", "-03-").Replace("APRIL", "-04-").Replace("MAY", "-05-").Replace("JUNE", "-06-").Replace("JULY", "-07-").Replace("AUGUST", "-08-").Replace("SEPTEMPER", "-09-").Replace("OCTOBER", "-10-").Replace("NOVEMBER", "-11-").Replace("DECEMBER", "-12-");
            }
            return v;
        }
        #endregion
        public static string ChuanHoaDateTime1(string v)
        {
            if (!v.Contains("-"))
            {
                v = v.ToLower();
                v = v.Replace("jan", "-01-").Replace("feb", "-02-").Replace("mar", "-03-").Replace("apr", "-04-").Replace("may", "-05-").Replace("jun", "-06-").Replace("jul", "-07-").Replace("aug", "-08-").Replace("sep", "-09-").Replace("oct", "-10-").Replace("0ct", "-10-").Replace("nov", "-11-").Replace("dec", "-12-");
            }
            string[] c = v.Split(new string[] { "-", "/" }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                v = ChuanHoaChuSoHangChuc(c[0]) + "-" + ChuanHoaChuSoHangChuc(c[1]) + "-" + c[2];
            }
            catch
            {
            }
            if (c[2].Length == 2)
                c[2] = c[2].Insert(0, "20");
            for (int i = 0; i < dateObj2.GetLength(0); i++)
            {
                if (c[1].ToLower().Equals(dateObj2[i, 1].ToLower()))
                {
                    c[1] = dateObj2[i, 0];
                    v = ChuanHoaChuSoHangChuc(c[0]) + "-" + c[1] + "-" + c[2];
                    break;
                }
            }
            try
            {
                v = ChuanHoaChuSoHangChuc(c[0]) + "-" + ChuanHoaChuSoHangChuc(c[1]) + "-" + c[2];
            }
            catch
            {
            }
            return v.ToUpper();
        }



        public static string ChuanHoaDateTime2(string v)
        {
            DateTime _dt;
            string _date = "";
            try
            {
                if (!String.IsNullOrEmpty(v))
                {
                    _dt = Convert.ToDateTime(v.ToString());
                    _date = _dt.ToString("dd/MM/yyyy").Replace("/", "-");
                }

            }
            catch
            {
            }
            return _date.ToUpper();
        }
        public static string ChuanHoaChuSoHangChuc(string v)
        {
            if (v.Length == 1)
                return "0" + v;
            return v;
        }

        static string[,] dateObj = new string[,] {
            {"01","Jan" },
            {"02","Feb" },
            {"03","Mar" },
            {"04","Apr" },
            {"05","May" },
            {"06","Jun" },
            {"07","Jul" },
            {"08","Aug" },
            {"09","Sep" },
            {"10","Oct" },
            {"11","Nov" },
            {"12","Dec" }
        };

        static string[,] dateObj2 = new string[,] {
            {"01","JANUARY" },
            {"02","FERBURY" },
            {"03","MARCH" },
            {"04","APRIL" },
            {"05","MAY" },
            {"06","JUNE" },
            {"07","JULY" },
            {"08","AUGUST" },
            {"09","SEPTEMPER" },
            {"10","OCTOBER" },
            {"11","NOVEMBER" },
            {"12","DECEMBER" }
        };
    }
}