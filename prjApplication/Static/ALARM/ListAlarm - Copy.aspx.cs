using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjInfo;
using prjComponents;
using System.IO;
using TuesPechkin;

namespace prjApplication.Static.ALARM
{
    public partial class ListAlarm : PageCoreAdmin
    {
        private const string _AliasSession = "ListAlarm";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    CustomPaging1.ValueSearch = Session[_AliasSession].ToString();
                    Session.SetValueForControlSearch(this, _AliasSession);
                }
                catch
                {
                    Session.SetValueSearch(_AliasSession, " 1=1");
                }
                LoadData();
                btnAddAlarm.Enabled = _Role.R_Add;
            }
        }

        #region function
        public void LoadData()
        {
            List<Alarm> t = new AlarmDAL().GetPageAlarm(CustomPaging1.PageSize, CustomPaging1.PageIndex, CustomPaging1.ValueSearch);
            CustomPaging1.TotalsRecord = t.GetTotalRecord<Alarm>();
            grdSource.DataSource = t;
            grdSource.DataBind();
        }
        private void PutclsSearchDetail()
        {
            List<clsSearchDetail> lisSearch = new List<clsSearchDetail>();
            lisSearch.Add(new clsSearchDetail("EVENT_NAME", "txtSearch_UserName", txtSearch_UserName.Text.Trim()));
            Session.SetValueSearch(_AliasSession, lisSearch);
        }


        #endregion

        #region control-event
        protected void pages_IndexChanged(object sender, EventArgs e)
        {

            LoadData();
        }

        protected void linkSearch_Click(object sender, EventArgs e)
        {
            string where = GetWhereCondition();

            PutclsSearchDetail();
            Session.SetValueSearch(_AliasSession, where);
            CustomPaging1.ValueSearch = where;
            LoadData();
        }

        public string GetWhereCondition()
        {
            string where = " 1=1";
            if (!String.IsNullOrEmpty(txtSearch_UserName.Text.Trim()))
                where += " AND " + string.Format(" UPPER(EVENT_NAME) like N'%{0}%'", UltilFunc.SqlFormatText(this.txtSearch_UserName.Text.Trim().ToUpper()));
            return HttpUtility.UrlEncode(where.ToUpper());
        }
       
        protected void btnAddCountry_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Static/ALARM/EditAlarm.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }

        #endregion

        #region grd event

        protected void grdSource_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            switch (e.CommandName)
            {
                case "DeleteByID":
                    var kq = new AlarmDAL().DeleteAlarm(id.ToString());
                    if (kq) this.AlertMessage("Delete sussess!");
                    else this.AlertMessage("Delete error!");
                    LoadData();
                    break;
                case "Edit":
                    Response.Redirect("~/Static/ALARM/EditAlarm.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString() + "&ID=" + id.ToString());
                    break;
            }
        }
        protected void grdSource_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.TableSection = TableRowSection.TableHeader;
        }
        protected string IsStatusGet(string str)
        {
            string strReturn = "";
            if (str == "1")
                strReturn = Global.ApplicationPath + "/Images/check.png";
            if (str == "0")
                strReturn = Global.ApplicationPath + "/Images/cross-32.png";
            return strReturn;
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
            if (!String.IsNullOrEmpty(txtSearch_UserName.Text.Trim()))
                ax.DataSource = new AlarmDAL().GetPageAlarmExport(GetWhereCondition());
            else
                ax.DataSource = new AlarmDAL().GetAllAlarm();
            ax.DataBind();

            ax.RenderControl(htw);
            string html = sw.ToString();
            //divContent.InnerHtml += html;
            html = html.Insert(0, "<style>thead {display: table-header-group;}tfoot {display: table-row-group;}tr {page-break-inside: avoid;}</style> ");
            this.CreatePDF(html, "LISTALARM_" + DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/assets/css/bootstrap1.min.css"), System.Drawing.Printing.PaperKind.A4);

        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            GridView ax = GridView1;
            ax.Visible = true;
            if (!String.IsNullOrEmpty(txtSearch_UserName.Text.Trim()))
                ax.DataSource = new AlarmDAL().GetPageAlarmExport(GetWhereCondition());
            else
                ax.DataSource = new AlarmDAL().GetAllAlarm();
            ax.AutoGenerateColumns = false;

            ax.DataBind();
            this.CreateExcel(this.RenderToHTML(ax), "LISTALARM_" + DateTime.Now.ToFileTime() + ".xls");
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