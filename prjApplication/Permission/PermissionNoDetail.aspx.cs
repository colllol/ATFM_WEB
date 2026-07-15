using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjInfo;
using Newtonsoft.Json.Converters;
using System.Web.Script.Serialization;
using System.Text;
using System.IO;
using System.Data;
using TuesPechkin;

namespace prjApplication.Permission
{
    public partial class PermissionNoDetail : PageBaseCallBack
    {
        public string _phanCach = "::::";
        public string _IdSelect = "0";
        private string _AliasSession = "PermissionNoDetail";
        public string IDCHA
        {
            get
            {
                try
                {
                    return Convert.ToInt64(Request["ID"]).ToString();
                }
                catch
                {
                    return "";
                }
            }
        }
        public string _ObjRender
        {
            get
            {
                PermDetailNo obj = new PermDetailNo();

                var ser = new JavaScriptSerializer();
                ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
                return ser.Serialize(obj);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CustomPaging1.ValueSearch = Session[_AliasSession].ToString();
                Session.SetValueForControlSearch(this, _AliasSession);
                try
                {
                    var ax = Convert.ToInt64(Request["ID"]);
                    Session.SetValueSearch(_AliasSession, " 1=1 AND a.PERM_ID=" + Request["ID"].ToString());
                    CustomPaging1.ValueSearch = Session[_AliasSession].ToString();
                }
                catch
                {
                    return;
                }

            }
            catch
            {
                Session.SetValueSearch(_AliasSession, " 1=1 AND a.PERM_ID=" + Request["ID"].ToString());
                CustomPaging1.ValueSearch = Session[_AliasSession].ToString();
            }
            if (!IsPostBack)
            {
                
                LoadData();
                LoadMultiDropdownList();

            }
        }
        #region call back
        public override string GetCallbackResult()
        {
            if (_EventArgument == "") return "";
            string kq = "";
            string[] _arg = _EventArgument.Split(new string[] { "_____" }, StringSplitOptions.RemoveEmptyEntries);
            string[] ThamSo = _arg[0].Split(new string[] { _phanCach }, StringSplitOptions.RemoveEmptyEntries);
            switch (_arg[1])
            {
                case "btnUpdateOnclick":
                    kq = btnUpdateOnclick(_arg[0]);
                    break;
                case "btnCreateOnclick":
                    kq = btnCreateOnclick(_arg[0]);
                    break;
                case "GetOneObject":
                    kq = GetOneObject(_arg[0]);
                    break;
                case "LoadDataGrid":
                    kq = LoadDataGrid(ThamSo);
                    break;
                case "btnDeleteOnclick":
                    kq = btnDeleteOnclick(_arg[0]);
                    break;
                case "mbtnAddNewFlightDetailOnclick":
                    kq = mbtnAddNewFlightDetailOnclick(_arg[0]);
                    break;
                case "GetOneObjectNoUpdate":
                    kq = GetOneObjectNoUpdate(_arg[0]);
                    break;
                case "mDeleteRowOnclick":
                    kq = mDeleteRowOnclick(_arg[0]);
                    break;
                case "mShowDetail":
                    kq = mShowDetail(_arg[0]);
                    break;
                case "mbtnUpdateFlightDetailOnclick":
                    kq = mbtnUpdateFlightDetailOnclick(_arg[0]);
                    break;
                case "RestoreHistory":
                    kq = RestoreHistory(ThamSo);
                    break;
            }

            return kq;
        }
        #endregion
        #region function
        void LoadData()
        {
            List<PermDetailNo> t = new PermDetailNoDAL().GetPageObject(CustomPaging1.PageSize, CustomPaging1.PageIndex, CustomPaging1.ValueSearch);
            CustomPaging1.TotalsRecord = t.GetTotalRecord<PermDetailNo>();
            grdSource.DataSource = t;
            grdSource.DataBind();

        }
        private void LoadMultiDropdownList()
        {
            //this.FillDropdownList<CraftType>(ddlCRAFT_ID, new CraftTypeDAL().GetAllCraftType(), "MA", "CRAFT_ID");
            this.FillDropdownList<FlyPurpose>(ddlPURPOSE_ID, new FlyPurposeDAL().GetAllObject(), "PURPOSE_NAME", "PURPOSE_CODE");
            this.FillDropdownList<Aero>(ddlTO_AIRP, new AeroDAL().GetListAll(), "AE_NAME", "AE_ID");
            this.FillDropdownList<Aero>(ddlFROM_AIRP, new AeroDAL().GetListAll(), "AE_NAME", "AE_ID");
            //this.FillDropdownList<CraftType>(mCRAFT_ID, new CraftTypeDAL().GetAllCraftType(), "MA", "CRAFT_ID");
            this.FillDropdownList<FlyPurpose>(mPURPOSE_ID, new FlyPurposeDAL().GetAllObject(), "PURPOSE_NAME", "PURPOSE_CODE");
            this.FillDropdownList<Aero>(mFROM_AIRP, new AeroDAL().GetListAll(), "AE_NAME", "AE_ID");
            this.FillDropdownList<Aero>(mTO_AIRP, new AeroDAL().GetListAll(), "AE_NAME", "AE_ID");
        }
        private string LoadDataGrid(string[] ThamSo)
        {
            List<PermDetailNo> t = new PermDetailNoDAL().GetPageObject(CustomPaging1.PageSize, CustomPaging1.PageIndex, CustomPaging1.ValueSearch);
            CustomPaging1.TotalsRecord = t.GetTotalRecord<PermDetailNo>();
            grdSource.DataSource = t;
            grdSource.DataBind();
            StringBuilder sb = new StringBuilder();
            StringWriter tw = new StringWriter(sb);
            HtmlTextWriter hw = new HtmlTextWriter(tw);
            grdSource.RenderControl(hw);
            return sb.ToString();
        }
        private string GetOneObject(string id)
        {
            PermDetailNo obj = new PermDetailNoDAL().GetOneObject(id);
            var ser = new JavaScriptSerializer();
            ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
            return ser.Serialize(obj);
        }
        private string mShowDetail(string id)
        {
            return GetOneObject(id);
        }
        private string GetOneObjectNoUpdate(string id)
        {
            PermDetailNo obj = new PermDetailNoDAL().GetOneObject(id);
            var ser = new JavaScriptSerializer();
            ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
            return ser.Serialize(obj);
        }
        private string RestoreHistory(string[] thamso)
        {
            bool kq = new PermDetailNoDAL().RestoreRecode(thamso[0], thamso[1], thamso[2]);
            string ax = kq.ToString() == true.ToString() ? "Restore sussess" : "Restore error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermDetailNoDAL]", 0, $"[RestoreRecode] [{ax}]", 0.0);
            return kq ? "OK" : "NOK";
        }
        private string btnCreateOnclick(string ThamSo)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<PermDetailNo>(ThamSo, dateTimeConverter);
            //obj.LAST_USER = _user.UserID.ToString();
            //obj.USER_NAME = _user.UserID.ToString();
            obj.ID = 0;
            bool kq = new PermDetailNoDAL().InsertObject(obj);
            string ax = kq.ToString() == true.ToString() ? "Insert sussess" : "Insert error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermDetailNoDAL]", 0, $"[InsertObject] [{ax}]", 0.0);
            if (!kq)
                return "Insert error";
            return "Insert sussess";
        }
        private string btnUpdateOnclick(string ThamSo)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<PermDetailNo>(ThamSo, dateTimeConverter);
            obj.LASTUSER = _user.UserID.ToString();
            bool kq = new PermDetailNoDAL().UpdateObject(obj);
            string ax = kq.ToString() == true.ToString() ? "Update sussess" : "Update error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermDetailNoDAL]", 0, $"[UpdateObject] [{ax}]", 0.0);
            if (!kq)
                return "Update error";
            return "Update sussess";
        }
        private string btnDeleteOnclick(string thamso)
        {
            bool kq = new PermDetailNoDAL().DeleteObject(thamso);
            string ax = kq.ToString() == true.ToString() ? "Delete sussess" : "Delete error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermDetailNoDAL]", 0, $"[DeleteObject] [{ax}]", 0.0);
            if (!kq)
                return "Delete error";
            return "Delete sussess";
        }
        private string mDeleteRowOnclick(string thamso)
        {
            bool kq = new PermDetailNoDAL().DeleteObject(thamso);
            string ax = kq.ToString() == true.ToString() ? "Delete sussess" : "Delete error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermDetailNoDAL]", 0, $"[DeleteObject] [{ax}]", 0.0);
            if (!kq)
                return "NOK";
            return "OK";
        }
        private string mbtnAddNewFlightDetailOnclick(string ThamSo)
        {
            try
            {
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<PermDetailNo>(ThamSo, dateTimeConverter);
                obj.ID = 0;
                var kq = new PermDetailNoDAL().InsertReturnId(obj);
                string ax = kq.ToString() == "-1" ? "Insert sussess" : "Insert error";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermDetailNoDAL]", 0, $"[InsertReturnId] [{ax}]", 0.0);
                return kq.ToString();
            }
            catch(Exception ex) { return "-1"; }
        }
        
        private string mbtnUpdateFlightDetailOnclick(string thamso)
        {
            var kq = btnUpdateOnclick(thamso);
            return kq == "Update sussess" ? "OK" : "NOK";
        }
        #endregion        
        #region control event
        protected void linkSearch_Click(object sender, EventArgs e)
        {

        }
        protected void grdSource_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex != -1 && e.Row.RowType != DataControlRowType.Header)
            {
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                GridView grd = ((GridView)sender);
                gvRow.CssClass = "detail-row";
                TableCell tCell = new TableCell();
                tCell.CssClass = "text-left";
                tCell.ColumnSpan = ((GridView)sender).Columns.Count;
                string id = grd.DataKeys[e.Row.RowIndex].Value.ToString();
                tCell.Text = rListHistoryFlightDetails(new PermDetailNoDAL().GetHistoryById(id), id);
                if (tCell.Text == "")
                    return;
                gvRow.Cells.Add(tCell);
                Table tbl = e.Row.Parent as Table;
                tbl.Rows.Add(gvRow);
            }
        }
        public string GetWhereCondition()
        {
            string where = " 1=1";
            var ax = Convert.ToInt64(Request["ID"]);
            if (!String.IsNullOrEmpty(txtSearch_UserName.Text.Trim()))
                where += " AND " + string.Format(" PERMNBR_ID like N'%{0}%'", UltilFunc.SqlFormatText(this.txtSearch_UserName.Text.Trim()));
            return where;
        }

        #endregion
        #region pdf excel 

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            GridView ax = grdSource;
            ax.ID = "grdPDF";
            ax.PreRender += GridView_PreRender;
            ax.Columns[0].Visible = false;
            ax.Columns[ax.Columns.Count - 1].Visible = false;
            ax.DataSource = new PermDetailNoDAL().GetPageObject(CustomPaging1.PageSize, CustomPaging1.PageIndex, Session[_AliasSession].ToString());
            ax.DataBind();

            ax.RenderControl(htw);
            string html = sw.ToString();
            //divContent.InnerHtml += html;
            html = html.Insert(0, "<style>thead {display: table-header-group;}tfoot {display: table-row-group;}tr {page-break-inside: avoid;}</style> ");
            this.CreatePDF(html, DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/assets/css/bootstrap1.min.css"), System.Drawing.Printing.PaperKind.A4);

        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter htw = new HtmlTextWriter(sw);            
            GridView ax = grdSource;
            ax.DataSource = new PermDetailNoDAL().GetPageObject(CustomPaging1.PageSize, CustomPaging1.PageIndex, Session[_AliasSession].ToString());
            ax.DataBind();
            //ax.DataBind();
            //ax.RenderControl(htw);
            //string html = sw.ToString();
            grdSource.DataSource = new PermDetailNoDAL().GetPageObject(CustomPaging1.PageSize, CustomPaging1.PageIndex, Session[_AliasSession].ToString());
            grdSource.DataBind();
            this.CreateExcel(this.RenderToHTML(ax), DateTime.Now.ToFileTime() + ".xls");
        }
        

        protected void GridView_PreRender(object sender, EventArgs e)
        {
            GridView gv = (GridView)sender;

            if ((gv.ShowHeader == true && gv.Rows.Count > 0)
                || (gv.ShowHeaderWhenEmpty == true))
            {
                //Force GridView to use <thead> instead of <tbody> - 11/03/2013 - MCR.
                gv.HeaderRow.TableSection = TableRowSection.TableHeader;

            }
            if (gv.ShowFooter == true && gv.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody> - 11/03/2013 - MCR.
                gv.FooterRow.TableSection = TableRowSection.TableFooter;
            }

        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
        #endregion

    }
}