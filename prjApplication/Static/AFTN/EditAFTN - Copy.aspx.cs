using prjComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjInfo;


namespace prjApplication.Static.AFTN
{
    public partial class EditAFTN : PageCoreAdmin
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
            Aftn obj;
            if (CommonLib.IsNumeric(Request["ID"]) == true)
            {
                // update
                obj = GetInfoObject();
                int _id = Convert.ToInt32(Request["ID"].ToString());
                int _gamID = Convert.ToInt32(Request["G_A_M_ID"].ToString());
                obj.ID = _id;
                obj.G_A_M_ID = _gamID;
                var kq = new AftnDAL().UpdateObject(obj);
                if (kq) lblResuft.InnerText = "Update sussess!";
                else lblResuft.InnerText = "Update error!";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit Aftn]", 0, "[UpdateAftn]" + lblResuft.InnerText, 0.0);
            }
            else
            {
                // insert
                obj = GetInfoObject();
                var kq = new AftnDAL().InsertObject(obj);
                if (kq) lblResuft.InnerText = "Insert sussess!";
                else lblResuft.InnerText = "Insert error!";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit Aftn]", 0, "[InsertAftn]" + lblResuft.InnerText, 0.0);
            }

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Static/AFTN/ListAFTN.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }
        #endregion

        #region bind data to control
        
        #endregion

        #region funciton

        private Aftn GetInfoObject()
        {
            int id = CommonLib.IsNumeric(Request["ID"]) ? Convert.ToInt32(Request["ID"].ToString()) : 0;
            int gamId = CommonLib.IsNumeric(Request["G_A_M_ID"]) ? Convert.ToInt32(Request["G_A_M_ID"].ToString()) : 0;
            return new Aftn()
            {
               DECRIPTION = txtDECRIPTION.Text,
               GROUP_NAME = txtGROUP_NAME.Text,
                //G_A_M_ID = Convert.ToInt32(txtG_A_M_ID.Text),
               G_A_M_ID = gamId,
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
            Aftn obj = new AftnDAL().GetOneGam(_id.ToString());
            txtDECRIPTION.Text = obj.DECRIPTION;
            txtGROUP_NAME.Text = obj.GROUP_NAME;
            //txtG_A_M_ID.Text = obj.G_A_M_ID.ToString();            
        }


        private bool CheckValidate()
        {            
            return true;
        }
        #endregion

    }
}