using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using prjInfo;
using prjBusinessLogic;
using System.Web.UI.WebControls;

namespace prjApplication.ReportStatistic
{
    public partial class FlightsStatistic : System.Web.UI.Page
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
            List<ReportStatic> t = new MenuReportDAL().FlightsStatistic_Get_All();
            grdSource.DataSource = t;
            grdSource.DataBind();

        }
        protected void grdSource_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.TableSection = TableRowSection.TableHeader;
        }
    }
}