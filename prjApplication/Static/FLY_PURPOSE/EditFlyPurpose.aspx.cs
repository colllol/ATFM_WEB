using prjComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjInfo;


namespace prjApplication.Static.FLY_PURPOSE
{
    public partial class EditFlyPurpose : PageCoreAdmin
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


        #region control - event
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckValidate()) return;
            FlyPurpose obj;
            if (CommonLib.IsNumeric(Request["ID"]) == true)
            {
                // update
                obj = GetInfoObject();
                int _id = Convert.ToInt32(Request["ID"].ToString());
                obj.ID = _id;
                var kq = new FlyPurposeDAL().UpdateObject(obj);
                if (kq) lblResuft.InnerText = "Update sussess!";
                else lblResuft.InnerText = "Update error!";

                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit FlyPurpose]", 0, "[UpdateFlyPurpose]" + lblResuft.InnerText, 0.0);
            }
            else
            {
                // insert
                obj = GetInfoObject();
                var kq = new FlyPurposeDAL().InsertObject(obj);
                if (kq) lblResuft.InnerText = "Insert sussess!";
                else lblResuft.InnerText = "Insert error!";

                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit FlyPurpose]", 0, "[InsertFlyPurpose]" + lblResuft.InnerText, 0.0);
            }

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Static/FLY_PURPOSE/ListFlyPurpose.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }
        #endregion

        #region bind data to control
        
        #endregion

        #region funciton

        private FlyPurpose GetInfoObject()
        {
            int id = CommonLib.IsNumeric(Request["ID"]) ? Convert.ToInt32(Request["ID"].ToString()) : 0;
            return new FlyPurpose()
            {
               ID = id,
               PURPOSE_CODE = txtPURPOSE_CODE.Text,
               PURPOSE_NAME = txtPURPOSE_NAME.Text
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
            FlyPurpose obj = new FlyPurposeDAL().GetOneFlyPurpose(_id.ToString());
            txtPURPOSE_CODE.Text = obj.PURPOSE_CODE;
            txtPURPOSE_NAME.Text = obj.PURPOSE_NAME;       
        }


        private bool CheckValidate()
        {            
            return true;
        }
        #endregion

    }
}