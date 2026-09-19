using prjComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjInfo;
using prjApplication;

namespace prjApplication.Static.ReportStatic
{
    public partial class EditReportStatic : PageCoreAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CommonLib.IsNumeric(Request["Menu_ID"]) == true)
                    ddlGroupReport();
                    LoadObjectById();
                btnSave.Enabled = _Role.R_Add;
            }
        }


        #region control - event
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckValidate()) return;
            prjInfo.ReportStatic obj;
            if (CommonLib.IsNumeric(Request["ID"]) == true)
            {
                // update
                obj = GetInfoObject();
                int _id = Convert.ToInt32(Request["ID"].ToString());
                obj.ID = _id;
                var kq = new ReportStaticDAL().UpdateReportStatic(obj);
                if (kq) lblResuft.InnerText = "Update sussess!";
                else lblResuft.InnerText = "Update error!";

                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit ReportStatic]", 0, "[UpdateReportStatic]" + lblResuft.InnerText, 0.0);
            }
            else
            {
                // insert
                obj = GetInfoObject();
                var kq = new ReportStaticDAL().InsertReportStatic(obj);
                if (kq) lblResuft.InnerText = "Insert sussess!";
                else lblResuft.InnerText = "Insert error!";

                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit ReportStatic]", 0, "[InsertReportStatic]" + lblResuft.InnerText, 0.0);
            }

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Static/ReportStatic/ListReportStatic.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }
        #endregion

        #region bind data to control
        private void ddlGroupReport()
        {
            List<prjInfo.ReportNameStatic> lis = new ReportStaticDAL().GetAllReportNameStatic();
            this.FillDropdownList<prjInfo.ReportNameStatic>(ddlReport, lis, "NAME_REPORT", "NUMBER_REPORT");
        }

        #endregion

        #region funciton

        private prjInfo.ReportStatic GetInfoObject()
        {
            int id = CommonLib.IsNumeric(Request["ID"]) ? Convert.ToInt32(Request["ID"].ToString()) : 0;
            return new prjInfo.ReportStatic()
            {
                NUMBER_REPORT = Convert.ToInt32(ddlReport.SelectedValue),
                ADDRESS_FILE = txtADDRESS_FILE.Text,
                NAME_REPORT = txtNAME_REPORT.Text,
                REMARK = txtREMARK.Text,
                ID = id
            };
        }
        private void LoadObjectById()
        {
            if (Request["ID"] != null && Request["ID"].ToString() != "" && Request["ID"].ToString() != String.Empty)
            {
                if (CommonLib.IsNumeric(Request["ID"]) == true)
                {
                    int _id = Convert.ToInt32(Request["ID"].ToString());
                    PopulateItem(_id);
                }
            }
        }
        private void PopulateItem(int _id)
        {
            prjInfo.ReportStatic obj = new ReportStaticDAL().GetReportStaticById(_id.ToString());
            this.SetDisplayDropdownList(ddlReport, obj.NUMBER_REPORT.ToString());
            txtADDRESS_FILE.Text = obj.ADDRESS_FILE;
            txtNAME_REPORT.Text = obj.NAME_REPORT;
            txtREMARK.Text = obj.REMARK;
        }


        private bool CheckValidate()
        {
            return true;
        }
        #endregion

    }
}