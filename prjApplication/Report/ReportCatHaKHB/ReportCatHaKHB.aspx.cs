using prjBusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjInfo;
using System.Data;

namespace prjApplication.Report.ReportCatHaKHB
{
    public partial class ReportCatHaKHB : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDropdownList();
                LoadData();
            }
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string html = this.RenderToHTML(grdBCTHKHB);
            this.CreateExcel(html, "THSLB_DAY_N_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleReports.css"));
        }
        public void LoadData()
        {

        }
        protected void btnSelect_Click(object sender, EventArgs e)
        {
            ReportStatisticDAL _objCC = new ReportStatisticDAL();
            DataTable t = _objCC.THSLB_GET_SCHEDULE_SLB(txtBEGINDATE.Value, txtENDDATE.Value, ddlAERO.SelectedValue, txtHOURBEGIN.Value, txtHOUREND.Value, lblET.Value);
            grdBCTHKHB.DataSource = t;
            grdBCTHKHB.DataBind();
        }
        protected void LoadDropdownList()
        {
            this.FillDropdownListAllVV<Aero>(ddlAERO, new AeroDAL().GetListAeroVV(), "AE_CODE", "AE_CODE", "--");
        }
    }
}