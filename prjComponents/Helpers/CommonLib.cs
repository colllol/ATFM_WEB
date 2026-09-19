using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
namespace prjComponents
{
    public class CommonLib
    {
        #region Conversation functions
        public static bool CheckNullBool(object Value)
        {
            if ((Value == DBNull.Value) || (Value == null) || (Value.ToString().Trim() == "")) return false;
            else return Convert.ToBoolean(Value);
        }

        public static byte CheckNullByte(object Value)
        {
            if (Value == DBNull.Value) return 0;
            else return Convert.ToByte(Value);
        }

        public static int CheckNullInt(object Value)
        {
            if (Value == DBNull.Value) return 0;
            else return Convert.ToInt32(Value);
        }

        public static int CheckNullInt(string Value)
        {
            if (Value == "") return 0;
            else return Convert.ToInt32(Value);
        }

        public static double CheckNullDbl(object Value)
        {
            if (Value == DBNull.Value) return 0;
            else return Convert.ToDouble(Value);
        }

        public static float CheckNullFloat(object Value)
        {
            if (Value == DBNull.Value) return 0;
            else return Convert.ToSingle(Value);
        }

        public static long CheckNullLong(object Value)
        {
            if (Value == DBNull.Value) return 0;
            else return Convert.ToInt64(Value);
        }

        public static string CheckNullStr(object Value)
        {
            if (Value == DBNull.Value) return "";
            else return Value.ToString();
        }

        public static DateTime CheckNullDate(object Value)
        {
            if (Value == DBNull.Value) return DateTime.MinValue;
            else return Convert.ToDateTime(Value);
        }

        public static object CheckDBNullDate(object Value)
        {
            if (Convert.ToDateTime(Value) == DateTime.MinValue) return DBNull.Value;
            else return Convert.ToDateTime(Value);
        }

        public static string ShowNullDate(DateTime Value)
        {
            if (Value == DateTime.MinValue) return "";
            else return Value.ToString("dd/MM/yyyy");
        }

        public static DateTime GetNullDate(string Value)
        {
            if (Value.Trim() == "") return DateTime.MinValue;
            else return ToDate(Value, "dd/MM/yyyy");
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

        public static bool IsInt(string Value)
        {
            try
            {
                Convert.ToInt32(Value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region ReadXML
        public static string ReadXML(string ItemName)
        {
            return Global.RM.GetString(ItemName);
        }
        #endregion

        #region Other
        public static bool IsNumeric(string str)
        {
            bool temp = true;
            try
            {
                str = str.Trim();
                int foo = int.Parse(str);
            }
            catch
            {
                temp = false;
            }
            return temp;
        }
        public static int GetIndexControl(System.Web.UI.WebControls.DropDownList sControl, string iValue)
        {
            int iCount;
            int retVal = 0;
            iCount = sControl.Items.Count;
            for (int i = 0; i <= iCount - 1; i++)
            {
                if (sControl.Items[i].Value == iValue)
                {
                    return i;
                }
            }
            return retVal;
        }
        public static int GetLatestID(string TableName, string IDFieldName)
        {
            prjBusinessLogic.UltilFunc _untilFuncDAL = new prjBusinessLogic.UltilFunc();
            try
            {
                DataSet ds = _untilFuncDAL.GetStoreDataSet("[CMS_GetLatestID]", new string[] { "@TableName", "@IDFieldName" }, new object[] { TableName, IDFieldName });
                return Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray.GetValue(0));
            }
            catch
            {
                return 0;
            }
        }
        public static int GetLatestID(string TableName, string IDFieldName, string Condition)
        {

            prjBusinessLogic.UltilFunc _untilFuncDAL = new prjBusinessLogic.UltilFunc();
            try
            {
                DataSet ds = _untilFuncDAL.GetStoreDataSet("[CMS_GetLatestID]", new string[] { "@TableName", "@IDFieldName", "@Condition" }, new object[] { TableName, IDFieldName, Condition });
                return Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray.GetValue(0));
            }
            catch
            {
                return 0;
            }
        }
        #endregion
        public static string CheckImgView(Object imgPath)
        {
            string _isDisplay = "";
            if ((Convert.ToString(imgPath) != "") && (imgPath != DBNull.Value) && (Convert.ToString(imgPath).Length > 0))
            {
                _isDisplay = "Display:yes;";
            }
            else
            {
                _isDisplay = "Display:none;";
            }
            return _isDisplay;
        }
        public static string tinpath(Object strFileName)
        {

            if (Convert.ToString(strFileName) != "")
                return System.Configuration.ConfigurationManager.AppSettings["tinpath"] + "/" + CommonLib.CleanHTML(Convert.ToString(strFileName));
            else
                return "";
        }
        public static string HPCOnmouseoverGrid()
        {
            return System.Configuration.ConfigurationManager.AppSettings["HPCOnmouseoverGrid"].ToString();
        }
        public static string HPCOnmouseoverOut()
        {
            return System.Configuration.ConfigurationManager.AppSettings["HPCOnmouseoverOut"].ToString();
        }
        public static string CleanWordHtml(string html)
        {
            StringCollection sc = new StringCollection();
            // get rid of unnecessary tag spans (comments and title)
            sc.Add(@"<!--(\w|\W)+?-->");
            sc.Add(@"<title>(\w|\W)+?</title>");
            // Get rid of classes and styles
            //			sc.Add(@"\s?class=\w+");
            sc.Add(@"\s+style='[^']+'");
            // Get rid of unnecessary tags
            sc.Add(@"<(meta|link|/?o:|/?style|/?div|/?st\d|/?head|/?html|body|/?body|/?span|!\[)[^>]*?>");
            // Get rid of empty paragraph tags
            sc.Add(@"(<[^>]+>)+&nbsp;(</\w+>)+");
            // remove bizarre v: element attached to <img> tag
            sc.Add(@"\s+v:\w+=""[^""]+""");
            // remove extra lines
            sc.Add(@"(\n\r){2,}");
            foreach (string s in sc)
            {
                html = Regex.Replace(html, s, "", RegexOptions.IgnoreCase);
            }
            return html;
        }
        public static string CleanHTML(string Contents)
        {
            Contents = Regex.Replace(Contents, "<(select|option|script|style|title)(.*?)>((.|\n)*?)</(select|option|script|style|title)>", " ", RegexOptions.IgnoreCase);
            Contents = Regex.Replace(Contents, "&(nbsp|quot|copy);", "");
            Contents = Regex.Replace(Contents, "'", "''");
            Contents = Regex.Replace(Contents, "(;|--|create|drop|select|insert|delete|update|union|sp_|xp_)", "");
            Contents = Regex.Replace(Contents, "<([\\s\\S])+?>", " ", RegexOptions.IgnoreCase).Replace("  ", " ");
            return (Contents);
        }
        public static bool isParrentMenu(int Menu_ID)
        {
            bool _return = true;
            //prjBusinessLogic.UltilFunc _lib = new prjBusinessLogic.UltilFunc();

            // DataSet ds = _lib.GetStoreDataSet("[CMS_isParrentMenu]", new string[] { "@Menu_ID" }, new object[] { Menu_ID });
            DataTable dt  = new clsResuftAPI().GetTableParamaterMenu("api/Users/isParrentMenu/", Menu_ID);
            if (Convert.ToInt32(dt.Rows[0].ItemArray.GetValue(0)) > 0)
                _return = false;
            return _return;
        }
    }
}
