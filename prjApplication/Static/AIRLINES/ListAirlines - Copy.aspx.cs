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

namespace prjApplication.Static.AIRLINES
{
    public partial class ListAirlines : PageCoreAdmin
    {
        private const string _AliasSession = "ListAirlines";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
                btnAddAirlines.Enabled = _Role.R_Add;
            }
        }

        #region function
        public void LoadData()
        {
            List<Airlines> t = new AirlinesDAL().GetPageAirlines(PhanTrang1.PageSize, PhanTrang1.PageIndex, GetWhereCondition());
            PhanTrang1.TotalRecord = t.GetTotalRecord<Airlines>();
            grdSource.DataSource = t;
            grdSource.DataBind();

        }
        private void PutclsSearchDetail()
        {
            List<clsSearchDetail> lisSearch = new List<clsSearchDetail>();
            lisSearch.Add(new clsSearchDetail("AIRLINES_CODE", "txtSearch_UserName", txtSearch_UserName.Text.Trim()));

            Session.SetValueSearch(_AliasSession, lisSearch);
        }
        #endregion

        protected void linkSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        public string GetWhereCondition()
        {
            
            string where = " 1=1";
            if (!String.IsNullOrEmpty(txtSearch_UserName.Text.Trim()))
                where += " AND " + string.Format(" UPPER(AIRLINES_CODE) like N'%{0}%'", UltilFunc.SqlFormatText(this.txtSearch_UserName.Text.Trim().ToUpper()));
            return HttpUtility.UrlEncode(where.ToUpper());
        }
        #region grd event

        protected void grdSource_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            switch (e.CommandName)
            {
                case "DeleteByID":
                    var kq = new AirlinesDAL().DeleteAirlines(id.ToString());
                    if (kq) this.AlertMessage("Delete sussess!");
                    else this.AlertMessage("Delete error!");
                    LoadData();
                    break;
                case "Edit":
                    Response.Redirect("~/Static/AIRLINES/EditAirlines.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString() + "&ID=" + id.ToString());
                    break;
            }
        }
        protected void PhanTrang1_Paging_IndexChange(object sender, EventArgs e)
        {
            LoadData();
        }
        protected void grdSource_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.TableSection = TableRowSection.TableHeader;
        }

        #endregion
        protected void btnAddAirlines_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Static/AIRLINES/EditAirlines.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }

        #region pdf excel 

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            GridView ax = grdSource;
            ax.ID = "grdPDF";
            ax.PreRender += GridView_PreRender;
            //ax.Columns[0].Visible = false;
            ax.Columns[ax.Columns.Count - 1].Visible = false;
            if (!String.IsNullOrEmpty(txtSearch_UserName.Text.Trim()))
                ax.DataSource = new AirlinesDAL().GetPageAirlinesExport(GetWhereCondition());
            else
                ax.DataSource = new AirlinesDAL().GetAllAirlines();

            ax.DataBind();

            ax.RenderControl(htw);
            string html = sw.ToString();
            //divContent.InnerHtml += html;
            html = html.Insert(0, "<style>thead {display: table-header-group;}tfoot {display: table-row-group;}tr {page-break-inside: avoid;}</style> ");
            this.CreatePDF(html, "LISTAIRLINES_" + DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/assets/css/bootstrap.min.css"), System.Drawing.Printing.PaperKind.A4);

        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            GridView ax = grdSource;
            ax.Columns[ax.Columns.Count - 1].Visible = false;
            if (!String.IsNullOrEmpty(txtSearch_UserName.Text.Trim()))
                ax.DataSource = new AirlinesDAL().GetPageAirlinesExport(GetWhereCondition());
            else
                ax.DataSource = new AirlinesDAL().GetAllAirlines();

            ax.DataBind();
            ax.RenderControl(htw);
            string html = sw.ToString();
            this.CreateExcel(html, "LISTAIRLINES_" + DateTime.Now.ToFileTime() + ".xls");
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