using prjComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjInfo;


namespace prjApplication.Static.CraftType
{
    public partial class EditCraftType : PageCoreAdmin
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
            prjInfo.CraftType obj;
            if (CommonLib.IsNumeric(Request["ID"]) == true)
            {
                obj = GetInfoObject();
                //int _id = Convert.ToInt32(Request["ID"].ToString());
                //obj.ID = _id;
                var kq = new CraftTypeDAL().UpdateObject(obj);
                if (kq) lblResuft.InnerText = "Update sussess!";
                else lblResuft.InnerText = "Update error!";

                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit CraftType]", 0, "[UpdateCraftType]" + lblResuft.InnerText, 0.0);
            }
            else
            {
                // insert
                obj = GetInfoObject();
                var kq = new CraftTypeDAL().InsertObject(obj);
                if (kq) lblResuft.InnerText = "Insert sussess!";
                else lblResuft.InnerText = "Insert error!";

                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit CraftType]", 0, "[InsertCraftType]" + lblResuft.InnerText, 0.0);
            }

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Static/CraftType/ListCraftType.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }
        #endregion

        #region bind data to control
        
        #endregion

        #region funciton

        private prjInfo.CraftType GetInfoObject()
        {
            int id = CommonLib.IsNumeric(Request["ID"]) ? Convert.ToInt32(Request["ID"].ToString()) : 0;
            int craftID = CommonLib.IsNumeric(Request["CRAFT_ID"]) ? Convert.ToInt32(Request["CRAFT_ID"].ToString()) : 0;
            return new prjInfo.CraftType()
            {
               //NUMBER_CHAIRS = Convert.ToInt32(txtNUMBER_CHAIRS.Text),
               //YEAR_MANUFACTURE = txtYEAR_MANUFACTURE.Text,
               CRAFT_ID = craftID,               
               MA = txtMA.Text,
               SOHIEU = txtSOHIEU.Text,
               TAITRONG = decimal.Parse(txtTAITRONG.Text),
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
            prjInfo.CraftType obj = new CraftTypeDAL().GetOneCraftType(_id);
            //txtCRAFT_ID.Text = obj.CRAFT_ID.ToString();
            txtMA.Text = obj.MA;
            txtSOHIEU.Text = obj.SOHIEU;
            txtTAITRONG.Text = obj.TAITRONG.ToString();
            //txtNUMBER_CHAIRS.Text = obj.NUMBER_CHAIRS.ToString();
            //txtYEAR_MANUFACTURE.Text = obj.YEAR_MANUFACTURE;
                        
        }


        private bool CheckValidate()
        {            
            return true;
        }
        #endregion

    }
}