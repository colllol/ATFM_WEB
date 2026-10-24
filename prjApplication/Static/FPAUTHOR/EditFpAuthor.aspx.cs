using prjComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjInfo;


namespace prjApplication.Static.FPAUTHOR
{
    public partial class EditFpAuthor : PageCoreAdmin
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
            FpAuthor obj;
            if (CommonLib.IsNumeric(Request["ID"]) == true)
            {
                // update
                obj = GetInfoObject();
                int _id = Convert.ToInt32(Request["ID"].ToString());
                obj.ID = _id;
                var kq = new FpAuthorDAL().UpdateObject(obj);
                if (kq) lblResuft.InnerText = "Update sussess!";
                else lblResuft.InnerText = "Update error!";

                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit FpAuthor]", 0, "[UpdateFpAuthor]" + lblResuft.InnerText, 0.0);
            }
            else
            {
                // insert
                obj = GetInfoObject();
                var kq = new FpAuthorDAL().InsertObject(obj);
                if (kq) lblResuft.InnerText = "Insert sussess!";
                else lblResuft.InnerText = "Insert error!";

                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit FpAuthor]", 0, "[InsertFpAuthor]" + lblResuft.InnerText, 0.0);
            }

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Static/FPAUTHOR/ListFpAuthor.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }
        #endregion

        #region bind data to control
        
        #endregion

        #region funciton

        private FpAuthor GetInfoObject()
        {
            int id = CommonLib.IsNumeric(Request["ID"]) ? Convert.ToInt32(Request["ID"].ToString()) : 0;
            return new FpAuthor()
            {
               AUTHOR_CODE = txtAUTHOR_CODE.Text,
               AUTHOR_NAME = txtAUTHOR_NAME.Text
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
            FpAuthor obj = new FpAuthorDAL().GetOneFpauthor(_id.ToString());
            txtAUTHOR_CODE.Text = obj.AUTHOR_CODE;
            txtAUTHOR_NAME.Text = obj.AUTHOR_NAME;                
        }


        private bool CheckValidate()
        {            
            return true;
        }
        #endregion

    }
}