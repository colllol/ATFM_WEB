using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
namespace prjApplication
{
    public static class clsSearch
    {
        public static void SetValueSearch(this HttpSessionState session, string sName, string sValue)
        {
            session[sName] = sValue;
        }
        public static void SetValueSearch(this HttpSessionState session, string sName, List<clsSearchDetail> obj)
        {
            session["obj" + sName] = obj;
        }
        public static string GetValueSearch(this HttpSessionState session, string sName)
        {

            try
            {
                if (HttpContext.Current.Request["s"].ToString() != "")
                {
                    if (session[sName] == null)
                        return " 1=1";
                    return session[sName].ToString();
                }
                else return " 1=1 ";
            }
            catch
            {
                return " 1=1 ";
            }

        }

        public static void SetValueForControlSearch(this HttpSessionState session, System.Web.UI.Page page, string sName)
        {
            try
            {
                List<clsSearchDetail> lisObj = (List<clsSearchDetail>)session["obj" + sName];
                foreach (var item in lisObj)
                {
                    var ax = FindControlRecursive(page, item.IdControl);
                    try { ((TextBox)ax).Text = item.Value; break; } catch { }
                    try { ((HtmlInputText)ax).Value = item.Value; break; } catch { }

                }
            }
            catch { }


        }
        private static System.Web.UI.Control FindControlRecursive(System.Web.UI.Control Root, string idControl)
        {
            if (Root.ID == idControl)
                return Root;
            foreach (System.Web.UI.Control ctrl in Root.Controls)
            {
                System.Web.UI.Control Cont = FindControlRecursive(ctrl, idControl);
                if (Cont != null)
                    return Cont;
            }
            return null;
        }
    }
    public class clsSearchDetail
    {
        public clsSearchDetail(string namecolumn, string idcontrol, string value)
        {
            _NameColumn = namecolumn;
            _IdControl = idcontrol;
            _Value = value;
        }
        #region properties
        private string _NameColumn;
        private string _IdControl;
        private string _Value;

        public string NameColumn
        {
            get
            {
                return _NameColumn;
            }

            set
            {
                _NameColumn = value;
            }
        }

        public string IdControl
        {
            get
            {
                return _IdControl;
            }

            set
            {
                _IdControl = value;
            }
        }

        public string Value
        {
            get
            {
                return _Value;
            }

            set
            {
                _Value = value;
            }
        }

        #endregion

        #region function

        #endregion
    }    
}