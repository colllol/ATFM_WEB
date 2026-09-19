using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace prjApplication
{
    public static class clsSuportDropdownList
    {        
        public static void FillDropdownList<T>(this System.Web.UI.Page page, DropDownList ddl, List<T> source, string textField, string valueField)
        {            
            ddl.DataTextField = textField;
            ddl.DataValueField = valueField;
            ddl.DataSource = source;
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("-- [Select] --", "-1"));
            ddl.SelectedIndex = 0;
            ddl.DataBind();
        }
        public static void FillDropdownList<T>(this System.Web.UI.Page page, DropDownList ddl, List<T> source, string textField, string valueField, string textItem0Default)
        {
            ddl.DataTextField = textField;
            ddl.DataValueField = valueField;
            ddl.DataSource = source;
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem(textItem0Default, ""));
            ddl.SelectedIndex = 0;
        }
        public static void FillDropdownListAllVV<T>(this System.Web.UI.Page page, DropDownList ddl, List<T> source, string textField, string valueField, string textItem0Default)
        {
            ddl.DataTextField = textField;
            ddl.DataValueField = valueField;
            ddl.DataSource = source;
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem(textItem0Default, "VV"));
            ddl.SelectedIndex = 0;
        }
        public static void FillDropdownList(this System.Web.UI.Page page, DropDownList ddl, System.Data.DataTable dtSource, string textField, string valueField)
        {
            ddl.DataTextField = textField;
            ddl.DataValueField = valueField;
            ddl.DataSource = dtSource;
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("-- [Select] --", "-1"));
            ddl.SelectedIndex = 0;
            ddl.DataBind();
        }
        public static void SetDisplayDropdownList(this System.Web.UI.Page page,DropDownList ddl, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                ddl.SelectedIndex = 0;
                return;
            }
            int iValue = ddl.Items.IndexOf(ddl.Items.FindByValue(value));
            int iText = ddl.Items.IndexOf(ddl.Items.FindByText(value));
            if (iValue == iText)
                ddl.SelectedIndex = iValue;
            else if (iValue != iText && iValue != 0)
                ddl.SelectedIndex = iValue;
            else ddl.SelectedIndex = iText;                    
        }
    }
}