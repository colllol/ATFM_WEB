//using System;
//using System.Data;
//using System.Collections;
//using System.Web.UI.WebControls;
//using System.Web.UI.HtmlControls;
//using System.Web.UI;
//using System.Text.RegularExpressions;
//using System.ComponentModel;
//using System.IO;
//using System.Text;

//namespace prjComponents.UI
//{
//    /// <summary>
//    /// Hien thi thong tin ngay thang
//    /// </summary>
//    [ToolboxData("<{0}:NetDatePicker runat=server></{0}:NetDatePicker>"), DefaultProperty("Text")]
//    public class NetDatePicker : TextBox
//    {
//        // Fields
//        //protected Style _TextBoxStyle = new Style();
//        protected System.Web.UI.WebControls.Style _TextBoxStyle = new System.Web.UI.WebControls.Style();


//        // Methods
//        protected override void OnInit(EventArgs e)
//        {
//            base.OnInit(e);

//            if ((this.Page != null) && !this.Page.ClientScript.IsClientScriptBlockRegistered("DatePickerScripts"))
//            {
//                StringBuilder builder = new StringBuilder();
//                builder.Append("\r\n<SCRIPT language=\"javascript\" type=\"text/javascript\">var dpImgFolder='" + Global.ApplicationPath + "/Scripts/DatePicker/Images/';</SCRIPT>");
//                builder.Append("\r\n<SCRIPT language=\"javascript\" type=\"text/javascript\" src='" + Global.ApplicationPath + "/Scripts/DatePicker/datepicker.js'></SCRIPT>\r\n");
//                Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "DatePickerScripts", builder.ToString());

//            }
//        }

//        protected override void Render(HtmlTextWriter output)
//        {
//            if (this.Width.Type == UnitType.Percentage)
//            {
//                output.Write("<TABLE border=\"0\" style=\"background-color:Red;\" bordercolor=\"red\" Cellspacing=\"0px\" CellPadding=\"0px\" Width=\"" + this.Width.ToString() + "\"><TR>");
//                output.Write("<TD>");
//            }
//            base.ApplyStyle(this._TextBoxStyle);
//            if (this.Width.Type == UnitType.Percentage)
//            {
//                output.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
//            }
//            output.AddAttribute(HtmlTextWriterAttribute.Value, this.ValueString);//this.Value.ToString(this.Format));
//            base.Render(output);
//            if (this.Width.Type == UnitType.Percentage)
//            {
//                output.Write("</TD>");
//                output.Write("<TD>");
//            }
//            output.AddAttribute(HtmlTextWriterAttribute.Id, "datepicker_" + this.ID);
//            output.AddAttribute(HtmlTextWriterAttribute.Name, "datepicker_" + this.ID);
//            output.AddAttribute(HtmlTextWriterAttribute.Onclick, "return popUpCalendar(" + this.ClientID + ", " + this.ClientID + ", '" + this.Format + "', '" + this.ImageFolder + "');return false;");
//            output.AddAttribute(HtmlTextWriterAttribute.Alt, this.ToolTip);
//            output.AddAttribute(HtmlTextWriterAttribute.Src, this.ImageUrl);
//            output.AddAttribute(HtmlTextWriterAttribute.Align, "absmiddle");
//            output.AddAttribute(HtmlTextWriterAttribute.Style, "cursor:pointer;");
//            output.RenderBeginTag(HtmlTextWriterTag.Img);
//            output.RenderEndTag();
//            if (this.Width.Type == UnitType.Percentage)
//            {
//                output.Write("</TD>&nbsp;</TR></TABLE>");
//            }
//        }

//        // Properties
//        [Bindable(true), Category("Content"), DefaultValue("")]
//        public string Format
//        {
//            get
//            {
//                if (this.ViewState["Format"] == null)
//                {
//                    return "dd/MM/yyyy";
//                }
//                return this.ViewState["Format"].ToString();
//            }
//            set
//            {
//                this.ViewState["Format"] = value;
//            }
//        }

//        [Category("Appearance"), DefaultValue(""), Bindable(true)]
//        public string ImageFolder
//        {
//            get
//            {
//                if (this.ViewState["ImageFolder"] == null)
//                {
//                    return "";
//                }
//                return this.ViewState["ImageFolder"].ToString();
//            }
//            set
//            {
//                this.ViewState["ImageFolder"] = value;
//            }
//        }

//        [DefaultValue(""), Bindable(true), Category("Appearance")]
//        public string ImageUrl
//        {
//            get
//            {
//                if (this.ViewState["ImageUrl"] == null)
//                {
//                    return "";
//                }
//                return this.ViewState["ImageUrl"].ToString();
//            }
//            set
//            {
//                this.ViewState["ImageUrl"] = value;
//            }
//        }

//        [Bindable(true), DefaultValue(""), Category("Content")]
//        public string ScriptSource
//        {
//            get
//            {
//                if (this.ViewState["ScriptSource"] == null)
//                {
//                    return "";
//                }
//                return this.ViewState["ScriptSource"].ToString();
//            }
//            set
//            {
//                this.ViewState["ScriptSource"] = value;
//            }
//        }

//        [Category("Content"), Bindable(true), DefaultValue("")]
//        public DateTime Value
//        {
//            get
//            {
//                try
//                {
//                    return CommonLib.ToDate(this.Text, this.Format);
//                }
//                catch
//                {
//                    return DateTime.Now;
//                }
//            }
//            set
//            {
//                this.Text = value.ToString(this.Format);
//            }
//        }
//        public string ValueString
//        {
//            get
//            {
//                try
//                {
//                    return this.Text;
//                }
//                catch
//                {
//                    return DateTime.Now.ToString();
//                }
//            }
//            set
//            {
//                this.Text = value.ToString();
//            }
//        }
//    } 	
//}
using System;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace prjComponents.UI
{
    /// <summary>
    /// Hien thi thong tin ngay thang
    /// </summary>
    [ToolboxData("<{0}:NetDatePicker runat=server></{0}:NetDatePicker>"), DefaultProperty("Text")]
    public class NetDatePicker : TextBox
    {
        // Fields
        //protected Style _TextBoxStyle = new Style();
        protected System.Web.UI.WebControls.Style _TextBoxStyle = new System.Web.UI.WebControls.Style();


        // Methods
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if ((this.Page != null) && !this.Page.ClientScript.IsClientScriptBlockRegistered("DatePickerScripts"))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("\r\n<SCRIPT language='javascript' type='text/javascript'>var dpImgFolder='" + this.ImageFolder + "/';</SCRIPT>");
                builder.Append("\r\n<SCRIPT language='javascript' type='text/javascript' src='" + this.ScriptSource + "'></SCRIPT>\r\n");
                Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "DatePickerScripts", builder.ToString());

            }
        }

        protected override void Render(HtmlTextWriter output)
        {
            if (this.Width.Type == UnitType.Percentage)
            {
                output.Write("<TABLE Border='0px' Cellspacing='0px' CellPadding='0px' Width='" + this.Width.ToString() + "'><TR>");
                output.Write("<TD>");
            }
            base.ApplyStyle(this._TextBoxStyle);
            if (this.Width.Type == UnitType.Percentage)
            {
                output.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
            }
            output.AddAttribute(HtmlTextWriterAttribute.Value, this.ValueString);//this.Value.ToString(this.Format));
            base.Render(output);
            if (this.Width.Type == UnitType.Percentage)
            {
                output.Write("</TD>");
                output.Write("<TD>");
            }
            output.AddAttribute(HtmlTextWriterAttribute.Id, "datepicker_" + this.ID);
            output.AddAttribute(HtmlTextWriterAttribute.Name, "datepicker_" + this.ID);
            output.AddAttribute(HtmlTextWriterAttribute.Alt, this.ToolTip);
            output.AddAttribute(HtmlTextWriterAttribute.Src, this.ImageUrl);
            output.AddAttribute(HtmlTextWriterAttribute.Align, "left");
            output.AddAttribute(HtmlTextWriterAttribute.Onclick, "popUpCalendar('" + this.ClientID + "', '" + this.ClientID + "', '" + this.Format + "', '" + this.ImageFolder + "'); return false;");
            output.AddAttribute(HtmlTextWriterAttribute.Style, "cursor:pointer;");
            output.RenderBeginTag(HtmlTextWriterTag.Img);
            output.RenderEndTag();
            if (this.Width.Type == UnitType.Percentage)
            {
                output.Write("</TD>&nbsp;</TR></TABLE>");
            }
        }

        // Properties
        [Bindable(true), Category("Content"), DefaultValue("")]
        public string Format
        {
            get
            {
                if (this.ViewState["Format"] == null)
                {
                    return "dd/MM/yyyy";
                }
                return this.ViewState["Format"].ToString();
            }
            set
            {
                this.ViewState["Format"] = value;
            }
        }

        [Category("Appearance"), DefaultValue(""), Bindable(true)]
        public string ImageFolder
        {
            get
            {
                if (this.ViewState["ImageFolder"] == null)
                {
                    return "";
                }
                return this.ViewState["ImageFolder"].ToString();
            }
            set
            {
                this.ViewState["ImageFolder"] = value;
            }
        }

        [DefaultValue(""), Bindable(true), Category("Appearance")]
        public string ImageUrl
        {
            get
            {
                if (this.ViewState["ImageUrl"] == null)
                {
                    return "";
                }
                return this.ViewState["ImageUrl"].ToString();
            }
            set
            {
                this.ViewState["ImageUrl"] = value;
            }
        }

        [Bindable(true), DefaultValue(""), Category("Content")]
        public string ScriptSource
        {
            get
            {
                if (this.ViewState["ScriptSource"] == null)
                {
                    return "";
                }
                return this.ViewState["ScriptSource"].ToString();
            }
            set
            {
                this.ViewState["ScriptSource"] = value;
            }
        }

        [Category("Content"), Bindable(true), DefaultValue("")]
        public DateTime Value
        {
            get
            {
                try
                {
                    return CommonLib.ToDate(this.Text, this.Format);
                }
                catch
                {
                    return DateTime.Now;
                }
            }
            set
            {
                this.Text = value.ToString(this.Format);
            }
        }
        public string ValueString
        {
            get
            {
                try
                {
                    return this.Text;
                }
                catch
                {
                    return DateTime.Now.ToString();
                }
            }
            set
            {
                this.Text = value.ToString();
            }
        }
    }
}
