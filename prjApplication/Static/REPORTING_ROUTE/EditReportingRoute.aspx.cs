using prjComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjBusinessLogic.DAL;
using prjInfo;

namespace prjApplication.Static.REPORTING_ROUTE
{
    public partial class EditReportingRoute : PageCoreAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CommonLib.IsNumeric(Request["Menu_ID"]) == true)
                    LoadObjectById();
                btnSave.Enabled = _Role.R_Add;
            }
        }
        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckValidate()) return;
            prjInfo.ReportingRoute obj;
            if (CommonLib.IsNumeric(Request["ROUTE_ID"]) == true)
            {
                obj = GetInfoObject();
                var kq = new ReportingRouteDAL().UpdateReportingRoute(obj);
                if (kq) lblResuft.InnerText = "Update sussess!";
                else lblResuft.InnerText = "Update error!";

                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit Route]", 0, "[UpdateRoute]" + lblResuft.InnerText, 0.0);
            }
        }
        
        private prjInfo.ReportingRoute GetInfoObject()
        {
            int id = CommonLib.IsNumeric(Request["ROUTE_ID"]) ? Convert.ToInt32(Request["ROUTE_ID"].ToString()) : 0;
            return new prjInfo.ReportingRoute()
            {
                SOKM = txtSokm.Value,
                ROUTE_ID = id
            };
        }
        private void LoadObjectById()
        {
            if (Request["ROUTE_ID"] != null && Request["ROUTE_ID"].ToString() != "" && Request["ROUTE_ID"].ToString() != String.Empty)
            {
                if (CommonLib.IsNumeric(Request["ROUTE_ID"]) == true)
                {
                    int _id = Convert.ToInt32(Request["ROUTE_ID"].ToString());
                    PopulateItem(_id);
                }
            }
        }
        private void PopulateItem(int _id)
        {
            prjInfo.ReportingRoute obj = new ReportingRouteDAL().GetOneRoute(_id);
            txtSokm.Value = obj.SOKM;

        }


        private bool CheckValidate()
        {
            return true;
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Static/REPORTING_ROUTE/ListReportingRoute.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }
    }
}