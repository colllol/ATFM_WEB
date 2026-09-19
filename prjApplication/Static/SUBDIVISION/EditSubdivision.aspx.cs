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

namespace prjApplication.Static.SUBDIVISION
{
    public partial class EditSubdivision : PageCoreAdmin
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
                        LoadSubdivisionById();
                        btnSave.Enabled = _Role.R_Add;
                    }
                }
            }
        }

        private void LoadSubdivisionById()
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
            Subdivision obj = new SubdivisionDAL().GetOneSubdivision(_id.ToString());
            txtSUBDIVISION_NAME.Text = obj.SUBDIVISION_NAME.ToString();
            txtDESCRIPTION.Text = obj.DESCRIPTION.ToString();
            if(!String.IsNullOrEmpty(obj.FIR_HN.ToString()))
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
            //if (this.txtCountryCode.Text.Length <= 0)
            //{
            //    this.AlertMessage("Please enter a value Country code!");                
            //    return false;
            //}
            //if (this.txtCountryName.Text.Length <= 0)
            //{
            //    this.AlertMessage("Please enter a value Country name!");                
            //    return false;            
            //}
            return true;
        }

        private Subdivision GetInfoSubdivision()
        {
            int id = CommonLib.IsNumeric(Request["ID"]) ? Convert.ToInt32(Request["ID"].ToString()) : 0;
            string fir_HN = "0";
            string fir_HCM = "0";
            if(ckFIR_HN.Checked)
                fir_HN = "1";
            if (ckFIR_HCM.Checked)
                fir_HCM = "1";
            return new Subdivision()
            {
                SUBDIVISION_NAME = txtSUBDIVISION_NAME.Text,
                DESCRIPTION = txtDESCRIPTION.Text,
                FIR_HN = fir_HN,
                FIR_HCM = fir_HCM,
                ID = id
            };
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckValidate()) return;
            Subdivision obj;
            if (CommonLib.IsNumeric(Request["ID"]) == true)
            {
                // update
                obj = GetInfoSubdivision();
                int _id = Convert.ToInt32(Request["ID"].ToString());
                obj.ID = _id;
                var kq = new SubdivisionDAL().UpdateSubdivision(obj);
                if (kq) lblResuft.InnerText = "Update sussess!";
                else lblResuft.InnerText = "Update error!";

                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit Subdivision]", 0, "[UpdateSubdivision]" + lblResuft.InnerText, 0.0);
            }
            else
            {
                // insert
                obj = GetInfoSubdivision();
                var kq = new SubdivisionDAL().InsertSubdivision(obj);
                if (kq) lblResuft.InnerText = "Insert sussess!";
                else lblResuft.InnerText = "Insert error!";

                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit Subdivision]", 0, "[InsertSubdivision]" + lblResuft.InnerText, 0.0);
            }

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Static/SUBDIVISION/ListSubdivision.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }
    }
}