using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjBusinessLogic.DAL;
using prjInfo;
using prjComponents;
using System.IO;
using TuesPechkin;
using System.Web.Script.Serialization;

namespace prjApplication.Static.REPORTING_SECTOR
{
    public partial class ReportingSector : PageBaseCallBack
    {
        private const string _AliasSession = "ListReportingSector";
        public string _phanCach = "::::";
        public string _IdSelect = "0";
        private bool IsSearch = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }
        #region callback
        public override string GetCallbackResult()
        {
            if (_EventArgument == "") return "";
            string kq = "";
            string[] _arg = _EventArgument.Split(new string[] { "_____" }, StringSplitOptions.RemoveEmptyEntries);
            string[] ThamSo = _arg[0].Split(new string[] { "::::" }, StringSplitOptions.RemoveEmptyEntries);
            switch (_arg[1])
            {               
                
                case "GetAllSectorPoint":
                    kq = GetAllSectorPoint(_arg[0]);
                    break;
                case "chkCheck_OnchaneSector":
                    kq = chkCheck_OnchaneSector(ThamSo);
                    break;
                case "LoadDataGird":
                    kq = LoadDataGird();
                    break;

            }
            return kq;
        }

        #endregion
        
        private string GetAllSectorPoint(string ThamSo)
        {
            List<Sectors> obj = new ReportingPointDAL().GetAddSectorPoint(ThamSo);
            GridView2.DataSource = obj;
            GridView2.DataBind();
            var ax = this.RenderToHTML(GridView2);
            return ax;
        }

        #region function

        
        private string chkCheck_OnchaneSector(string[] thamso)
        {
            return new ReportingRouteDAL().SetSectorIdIntoRouteID(thamso[0], thamso[1], thamso[2]) ? "true" : "false";
        }
        protected string IsStatusGet(string str)
        {
            string strReturn = "";
            if (str.Length > 0)
            {
                if (str == "1")
                    strReturn = Global.ApplicationPath + "/Images/check.png";
                if (str == "0")
                    strReturn = Global.ApplicationPath + "/Images/cross-32.png";
            }
            return strReturn;
        }
        #endregion




        #region grd event

        protected void grdSource_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            switch (e.CommandName)
            {
                case "DeleteReportingSector":
                    var kq = new ReportingRouteDAL().DeleteReportingSector(id.ToString());
                    if (kq) this.AlertMessage("Delete sussess!");
                    else this.AlertMessage("Delete error!");
                    LoadData();
                    break;                
            }
        }
        protected void grdSource_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.TableSection = TableRowSection.TableHeader;
        }

        #endregion

        protected void btnAddReportingRoute_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Static/REPORTING_ROUTE/EditReportingRoute.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }

        #region pdf excel 

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            GridView ax = grdSource;
            ax.ID = "grdPDF";
            //ax.PreRender += GridView_PreRender;
            //ax.Columns[0].Visible = false;
            ax.Columns[ax.Columns.Count - 1].Visible = false;
            ax.Columns[ax.Columns.Count - 2].Visible = false;
            if (!String.IsNullOrEmpty(txtSearch_UserName.Text.Trim()))
                ax.DataSource = new ReportingRouteDAL().GetPageReportingRouteExport(txtSearch_UserName.Text.ToUpper().Trim());
            else
                ax.DataSource = new ReportingRouteDAL().GetAllReportingRoute();

            ax.DataBind();

            ax.RenderControl(htw);
            string html = sw.ToString();
            //divContent.InnerHtml += html;
            html = html.Insert(0, "<style>thead {display: table-header-group;}tfoot {display: table-row-group;}tr {page-break-inside: avoid;}</style> ");
            this.CreatePDF(html, "ListReportingRoute_" + DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/assets/css/bootstrap.min.css"), System.Drawing.Printing.PaperKind.A4);

        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            GridView ax = grdSource;
            ax.Visible = true;
            ax.Columns[ax.Columns.Count - 1].Visible = false;
            ax.Columns[ax.Columns.Count - 2].Visible = false;
            if (!String.IsNullOrEmpty(txtSearch_UserName.Text.Trim()))
                ax.DataSource = new ReportingRouteDAL().GetPageReportingRouteExport(txtSearch_UserName.Text.Trim().ToUpper());
            else
                ax.DataSource = new ReportingRouteDAL().GetAllReportingRoute();
            ax.AutoGenerateColumns = false;

            ax.DataBind();
            this.CreateExcel(this.RenderToHTML(ax), "ListReportingSector_" + DateTime.Now.ToFileTime() + ".xls");
        }


        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
        #endregion

        protected void PhanTrang1_Paging_IndexChange(object sender, EventArgs e)
        {
            LoadData();
        }
        protected void linkSearch_Click(object sender, EventArgs e)
        {
            IsSearch = true;
            LoadData();
            IsSearch = false;
        }

        private string LoadDataGird()
        {
            LoadData();
            return this.RenderToHTML(grdSource);
        }
        public void LoadData()
        {
            List<SectorRoute> t = new ReportingRouteDAL().GetPageReportingSector(PhanTrang1.PageSize, IsSearch ? 0 : PhanTrang1.PageIndex, txtSearch_UserName.Text.Trim());
            PhanTrang1.TotalRecord = t.GetTotalRecord<SectorRoute>();
            grdSource.DataSource = t;
            grdSource.DataBind();

        }
    }
}