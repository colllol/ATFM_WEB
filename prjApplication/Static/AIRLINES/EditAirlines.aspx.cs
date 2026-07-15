using prjComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic.DAL;
using prjBusinessLogic;
using prjInfo;

namespace prjApplication.Static.AIRLINES
{
    public partial class EditAirlines : PageCoreAdmin
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
                        LoadAirlinesById();
                        btnSave.Enabled = _Role.R_Add;
                    }
                }
            }
        }

        private void LoadAirlinesById()
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
            Airlines obj = new AirlinesDAL().GetOneAirlines(_id.ToString());
            txtAIRLINES_NAME.Text = obj.AIRLINES_NAME;
            txtAIRLINES_CODE.Text = obj.AIRLINES_CODE;
            txtDESCRIPTION.Text = obj.DESCRIPTION;
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

        private Airlines GetInfoAirlines()
        {
            int id = CommonLib.IsNumeric(Request["ID"]) ? Convert.ToInt32(Request["ID"].ToString()) : 0;
            return new Airlines()
            {

                AIRLINES_NAME = txtAIRLINES_NAME.Text,
                AIRLINES_CODE = txtAIRLINES_CODE.Text,
                DESCRIPTION = txtDESCRIPTION.Text,
                ID = id
            };
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckValidate()) return;
            Airlines obj;
            if (CommonLib.IsNumeric(Request["ID"]) == true)
            {
                // update
                obj = GetInfoAirlines();
                int _id = Convert.ToInt32(Request["ID"].ToString());
                obj.ID = _id;
                var kq = new AirlinesDAL().UpdateAirlines(obj);
                if (kq) lblResuft.InnerText = "Update sussess!";
                else lblResuft.InnerText = "Update error!";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit Airlines]", 0, "[UpdateAirlines]" + lblResuft.InnerText, 0.0);
            }
            else
            {
                // insert
                obj = GetInfoAirlines();
                var kq = new AirlinesDAL().InsertAirlines(obj);
                if (kq) lblResuft.InnerText = "Insert sussess!";
                else lblResuft.InnerText = "Insert error!";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit Airlines]", 0, "[InsertAirlines]" + lblResuft.InnerText, 0.0);
            }

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Static/AIRLINES/ListAirlines.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }
    }
}