using prjComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjInfo;

namespace prjApplication.Static.REPORTING_POINT
{
    public partial class EditReportingPoint : PageCoreAdmin
    {
        prjBusinessLogic.UserDAL _userDAL = new UserDAL();
        prjInfo.T_Users _user = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["Menu_ID"] != null && Request["Menu_ID"].ToString() != "" && Request["Menu_ID"].ToString() != string.Empty)
            {
                if (CommonLib.IsNumeric(Request["Menu_ID"]) == true)
                {
                    if (!HPCSecurity.IsAccept(Convert.ToInt32(Request["Menu_ID"])))
                        Response.Redirect("~/Errors/AccessDenied.aspx");
                    _user = _userDAL.GetUserByUserName(HPCSecurity.CurrentUser.Identity.Name);
                    if (!IsPostBack)
                    {
                        LoadReportingPointById();
                        btnSave.Enabled = _Role.R_Add;
                    }
                }
            }
        }

        private void LoadReportingPointById()
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
            ReportingPoint obj = new ReportingPointDAL().GetOneReportingPoint(_id.ToString());
            txtPOINT_NAME.Text = obj.POINT_NAME;
            txtDESCRIPTION.Text = obj.DESCRIPTION;
            if (!String.IsNullOrEmpty(obj.COMPULSORY_ON_REQUEST.ToString()))
            {
                if (obj.COMPULSORY_ON_REQUEST.ToString() == "1")
                    ckCOMPULSORY_ON_REQUEST.Checked = true;
            }
            if (!String.IsNullOrEmpty(obj.IN_FIR_VN.ToString()))
            {
                if (obj.IN_FIR_VN.ToString() == "1")
                    ckIN_FIR_VN.Checked = true;
            }
            if (!String.IsNullOrEmpty(obj.FIR_HN.ToString()))
            {
                if (obj.FIR_HN.ToString() == "1")
                    ckFIR_HN.Checked = true;
            }
            if (!String.IsNullOrEmpty(obj.FIR_HCM.ToString()))
            {
                if (obj.FIR_HCM.ToString() == "1")
                    ckFIR_HCM.Checked = true;
            }           
        }
        private bool CheckValidate()
        {
            return true;
        }

        private ReportingPoint GetInfoReportingPoint()
        {
            int id = CommonLib.IsNumeric(Request["ID"]) ? Convert.ToInt32(Request["ID"].ToString()) : 0;
            string fir_VN = "0";
            string fir_HN = "0";
            string fir_HCM = "0";
            string compulsory = "0";
            if (ckCOMPULSORY_ON_REQUEST.Checked)
                compulsory = "1";
            if (ckIN_FIR_VN.Checked)
                fir_VN = "1";
            if (ckFIR_HN.Checked)
                fir_HN = "1";
            if (ckFIR_HCM.Checked)
                fir_HCM = "1";

            return new ReportingPoint()
            {
                POINT_NAME = txtPOINT_NAME.Text,
                COMPULSORY_ON_REQUEST = compulsory,
                DESCRIPTION = txtDESCRIPTION.Text,
                IN_FIR_VN = fir_VN,
                FIR_HN = fir_HN,
                FIR_HCM = fir_HCM,
                ID = id
            };
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckValidate()) return;
            ReportingPoint obj;
            if (CommonLib.IsNumeric(Request["ID"]) == true)
            {
                // update
                obj = GetInfoReportingPoint();
                int _id = Convert.ToInt32(Request["ID"].ToString());
                obj.ID = _id;
                var kq = new ReportingPointDAL().UpdateReportingPoint(obj);
                if (kq) lblResuft.InnerText = "Update sussess!";
                else lblResuft.InnerText = "Update error!";

                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit ReportingPoint]", 0, "[UpdateReportingPoint]" + lblResuft.InnerText, 0.0);
            }
            else
            {
                // insert
                obj = GetInfoReportingPoint();
                var kq = new ReportingPointDAL().InsertReportingPoint(obj);
                if (kq) lblResuft.InnerText = "Insert sussess!";
                else lblResuft.InnerText = "Insert error!";

                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit ReportingPoint]", 0, "[InsertReportingPoint]" + lblResuft.InnerText, 0.0);
            }

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Static/REPORTING_POINT/ListReportingPoint.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString()); 
        }
    }
}