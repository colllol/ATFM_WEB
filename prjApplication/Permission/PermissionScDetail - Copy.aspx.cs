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
    public partial class PermissionScDetail : PageBaseCallBack
    {
        public string _phanCach = "::::";
        public string _IdSelect = "0";
        private string _AliasSession = "PermissionScDetail";
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
        public string NAMECHA { get { if (string.IsNullOrEmpty(IDCHA)) return ""; return new PermMasterScDAL().GetOneObject(IDCHA).PERMNBR_ID; } }
        public string _ObjRender
        {
            get
            {
                PermDetailSc obj = new PermDetailSc();

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
                    var ax = Convert.ToInt32(Request["ID"]);
                    Session.SetValueSearch(_AliasSession, " 1=1 AND a.PERM_ID=" + Request["ID"].ToString());
                    CustomPaging1.ValueSearch = Session[_AliasSession].ToString();
                }
                catch { return; }
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
            List<PermDetailSc> t = new PermDetailScDAL().GetPageObject(CustomPaging1.PageSize, CustomPaging1.PageIndex, CustomPaging1.ValueSearch);
            CustomPaging1.TotalsRecord = t.GetTotalRecord<PermDetailSc>();
            grdSource.DataSource = t;
            grdSource.DataBind();

        }
        private void LoadMultiDropdownList()
        {            
            this.FillDropdownList<CraftType>(ddlCRAFT_ID, new CraftTypeDAL().GetAllCraftType(), "MA", "CRAFT_ID");
            this.FillDropdownList<FlyPurpose>(ddlPURPOSE_ID, new FlyPurposeDAL().GetAllObject(), "PURPOSE_NAME", "PURPOSE_CODE");
            this.FillDropdownList<Aero>(ddlTO_AIRP, new AeroDAL().GetListAll(), "AE_NAME", "AE_ID");
            this.FillDropdownList<Aero>(ddlFROM_AIRP, new AeroDAL().GetListAll(), "AE_NAME", "AE_ID");            
            this.FillDropdownList<CraftType>(mCRAFT_ID, new CraftTypeDAL().GetAllCraftType(), "MA", "CRAFT_ID");
            this.FillDropdownList<FlyPurpose>(mPURPOSE_ID, new FlyPurposeDAL().GetAllObject(), "PURPOSE_NAME", "PURPOSE_CODE");
            this.FillDropdownList<Aero>(mFROM_AIRP, new AeroDAL().GetListAll(), "AE_NAME", "AE_ID");
            this.FillDropdownList<Aero>(mTO_AIRP, new AeroDAL().GetListAll(), "AE_NAME", "AE_ID");            
        }
        
        private string LoadDataGrid(string[] ThamSo)
        {
            List<PermDetailSc> t = new PermDetailScDAL().GetPageObject(CustomPaging1.PageSize, CustomPaging1.PageIndex, CustomPaging1.ValueSearch);
            CustomPaging1.TotalsRecord = t.GetTotalRecord<PermDetailSc>();
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
            PermDetailSc obj = new PermDetailScDAL().GetOneObject(id);
            var ser = new JavaScriptSerializer();
            ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
            return ser.Serialize(obj);
        }
        private string mShowDetail(string id)
        {
            return GetOneObject(id);
        }
        private string RestoreHistore(string id, string version)
        {
            return "";
        }
        private string GetOneObjectNoUpdate(string id)
        {
            PermDetailSc obj = new PermDetailScDAL().GetOneObject(id);
            var ser = new JavaScriptSerializer();
            ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
            return ser.Serialize(obj);
        }
        private string btnCreateOnclick(string ThamSo)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<PermDetailSc>(ThamSo, dateTimeConverter);
            //obj.LAST_USER = _user.UserID.ToString();
            //obj.USER_NAME = _user.UserID.ToString();
            obj.ID = 0;
            bool kq = new PermDetailScDAL().InsertObject(obj);
            string ax = kq.ToString() == true.ToString() ? "Insert sussess" : "Insert error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermDetailScDAL]", 0, $"[InsertObject] [{ax}]", 0.0);
            if (!kq)
                return "Insert error";
            return "Insert sussess";
        }
        private string btnUpdateOnclick(string ThamSo)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<PermDetailSc>(ThamSo, dateTimeConverter);
            //obj.LAST_USER = _user.UserID.ToString();
            bool kq = new PermDetailScDAL().UpdateObject(obj);
            string ax = kq.ToString() == true.ToString() ? "Update sussess" : "Update error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermDetailScDAL]", 0, $"[UpdateObject] [{ax}]", 0.0);
            if (!kq)
                return "Update error";
            return "Update sussess";
        }
        private string btnDeleteOnclick(string thamso)
        {
            bool kq = new PermDetailScDAL().DeleteObject(thamso);
            string ax = kq.ToString() == true.ToString() ? "Delete sussess" : "Delete error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermDetailScDAL]", 0, $"[DeleteObject] [{ax}]", 0.0);
            if (!kq)
                return "Delete error";
            return "Delete sussess";
        }
        private string mDeleteRowOnclick(string thamso)
        {
            bool kq = new PermDetailScDAL().DeleteObject(thamso);
            string ax = kq.ToString() == true.ToString() ? "Delete sussess" : "Delete error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermDetailScDAL]", 0, $"[DeleteObject] [{ax}]", 0.0);
            if (!kq)
                return "NOK";
            return "OK";
        }
        private string RestoreHistory(string[] thamso)
        {
            bool kq = new PermDetailScDAL().RestoreRecode(thamso[0], thamso[1], thamso[2]);
            string ax = kq.ToString() == true.ToString() ? "Restore sussess" : "Restore error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermDetailScDAL]", 0, $"[RestoreRecode] [{ax}]", 0.0);
            return kq ? "OK" : "NOK";
        }
        private string mbtnAddNewFlightDetailOnclick(string ThamSo)
        {
            try
            {
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<PermDetailSc>(ThamSo, dateTimeConverter);
                obj.ID = 0;
                var kq = new PermDetailScDAL().InsertReturnId(obj);
                string ax = kq.ToString() == "-1" ? "Delete sussess" : "Delete error";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermDetailScDAL]", 0, $"[DeleteObject] [{ax}]", 0.0);
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
                tCell.ColumnSpan = ((GridView)sender).Columns.Count - 1;
                string id = grd.DataKeys[e.Row.RowIndex].Value.ToString();
                tCell.Text = rListHistoryFlightDetails(new PermDetailScDAL().GetHistoryById(id), id);
                if (tCell.Text == "")
                    return;
                gvRow.Cells.Add(tCell);
                Table tbl = e.Row.Parent as Table;
                tbl.Rows.Add(gvRow);
            }
        }

        #endregion
        public string GetWhereCondition()
        {
            string where = " 1=1";
            if (!String.IsNullOrEmpty(txtSearch_UserName.Text.Trim()))
                where += " AND " + string.Format(" PERMNBR_ID like N'%{0}%'", UltilFunc.SqlFormatText(this.txtSearch_UserName.Text.Trim()));
            return where;
        }

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
            if (!String.IsNullOrEmpty(txtSearch_UserName.Text.Trim()))
            {
                ax.DataSource = new PermDetailScDAL().GetPagePermDetailScExport(GetWhereCondition());
            }
            else
                ax.DataSource = new PermDetailScDAL().GetAllObject();
            ax.DataBind();

            ax.RenderControl(htw);
            string html = sw.ToString();
            //divContent.InnerHtml += html;
            html = html.Insert(0, "<style>thead {display: table-header-group;}tfoot {display: table-row-group;}tr {page-break-inside: avoid;}</style> ");
            CreatePDF(html, DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/assets/css/bootstrap1.min.css"), System.Drawing.Printing.PaperKind.A4);

        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            GridView ax = grdSource;
            if (!String.IsNullOrEmpty(txtSearch_UserName.Text.Trim()))
                ax.DataSource = new PermDetailScDAL().GetPagePermDetailScExport(GetWhereCondition());
            else
                ax.DataSource = new PermDetailScDAL().GetAllObject();
            ax.DataBind();
            ax.RenderControl(htw);
            string html = sw.ToString();
            CreateExcel(html, DateTime.Now.ToFileTime() + ".xls");
        }
        protected void CreatePDF(string html, string fileName, string pathCSS, System.Drawing.Printing.PaperKind pageSize)
        {
            if (File.Exists(pathCSS))
            {
                html = html.Insert(0, string.Format("<style type=\"text/css\">{0}</style>", File.ReadAllText(pathCSS)));
            }
            Response.ClearContent();
            Response.Clear();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition",
             "attachment;filename=" + fileName);
            var tempFolderDeployment = new TempFolderDeployment();
            var win32EmbeddedDeployment = new WinAnyCPUEmbeddedDeployment(new TempFolderDeployment());
            var remotingToolset = new RemotingToolset<PdfToolset>(win32EmbeddedDeployment);

            var converter =
                 new ThreadSafeConverter(remotingToolset);
            HtmlToPdfDocument Document = new HtmlToPdfDocument
            {
                Objects =
            {

                new ObjectSettings
                {
                    HtmlText = html,
                    WebSettings = new WebSettings {
                        DefaultEncoding = "UTF-8",
                        MinimumFontSize = 8,
                        EnableIntelligentShrinking = true,
                        PrintBackground = true
                        ,LoadImages=true
                        //,PrintMediaType=true

                    }
                }
            },
                GlobalSettings =
            {
                    OutputFormat = GlobalSettings.DocumentOutputFormat.PDF,
                    ColorMode = GlobalSettings.DocumentColorMode.Color,
                    ProduceOutline = true,
                    DocumentTitle = "BAO CAO",
                    PaperSize = pageSize,
                    Margins =
                    {
                        All = 0.375,
                        Unit = TuesPechkin.Unit.Centimeters
                    }
            }
            };
            byte[] result = converter.Convert(Document);
            remotingToolset.Unload();
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.OutputStream.Write(result, 0, result.Length);
            Response.End();
        }
        protected void CreateExcel(string html, string fileName)
        {
            try
            {
                Response.ContentType = "application/force-download";
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                Response.AddHeader("Content-Length", html.Length.ToString());
                Response.Write("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
                Response.Write("<head>");
                Response.Write("<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                Response.Write("<!--[if gte mso 9]><xml>");
                Response.Write("<x:ExcelWorkbook>");
                Response.Write("<x:ExcelWorksheets>");
                Response.Write("<x:ExcelWorksheet>");
                Response.Write("<x:Name>Report Data</x:Name>");
                Response.Write("<x:WorksheetOptions>");
                Response.Write("<x:Print>");
                Response.Write("<x:ValidPrinterInfo/>");
                Response.Write("</x:Print>");
                Response.Write("</x:WorksheetOptions>");
                Response.Write("</x:ExcelWorksheet>");
                Response.Write("</x:ExcelWorksheets>");
                Response.Write("</x:ExcelWorkbook>");
                Response.Write("</xml>");
                Response.Write("<![endif]--> ");
                Response.Write("</head>");
                Response.Write(string.Format("<body>{0}</body></html>", html));
                Response.Flush();
                Response.Buffer = true;
                Response.Close();

            }
            catch
            {
                Response.Close();
            }

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