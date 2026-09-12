using prjBusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjInfo;
using System.Data;

namespace prjApplication.ReportMenu
{
    public partial class ReportSS : System.Web.UI.Page
    {
        public string _content = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();                
            }
        }


        public void LoadData()
        {
            //MenuReportDAL _rpDAL = new MenuReportDAL();
            //DataTable _dtR = null;
            //_dtR = _rpDAL.ReportDHB_Get_All();            
            //grdSource.DataSource = _dtR;
            //grdSource.DataBind();
            List<ReportStatic> t = new MenuReportDAL().ReportSS_Get_All();
            grdSource.DataSource = t;
            grdSource.DataBind();

        }
        protected void grdSource_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    LinkButton btn = (LinkButton)e.Row.FindControl("btnEdit");
            //    btn.Attributes.Add("OnClientClick", "f_CallReports(1);");
            //}
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.TableSection = TableRowSection.TableHeader;
        }
      
    }
}