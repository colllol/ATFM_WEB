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

namespace prjApplication.Static.Config
{
    public partial class EditConfig : System.Web.UI.Page
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
                      
                        LoadConfigById();
                    }
                }
            }
        }

        private void LoadConfigById()
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
            prjInfo.Config obj = new ConfigDAL().GetConfigById(_id.ToString());
            txt_NAME.Text = obj.NAME;
            txtValue.Value = obj.VALUE;
            txtNote.Value = obj.NOTE;
            

        }

        private prjInfo.Config GetInfoConfig()
        {

            prjInfo.Config _obj = new prjInfo.Config();
            int id = CommonLib.IsNumeric(Request["ID"]) ? Convert.ToInt32(Request["ID"].ToString()) : 0;            
            _obj.ID = id;
            _obj.VALUE = txtValue.Value;
            _obj.NAME = txt_NAME.Text;
            _obj.NOTE = txtNote.Value;
                      
            return _obj;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            prjInfo.Config obj;
            if (CommonLib.IsNumeric(Request["ID"]) == true)
            {
                // update
                obj = GetInfoConfig();
                int _id = Convert.ToInt32(Request["ID"].ToString());
                obj.ID = _id;
                var kq = new ConfigDAL().UpdateConfig(obj);
                if (kq) lblResuft.InnerText = "Update sussess!";
                else lblResuft.InnerText = "Update error!";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit Config]", 0, "[UpdateConfig]" + lblResuft.InnerText, 0.0);
            }
            else
            {
                // insert
                obj = GetInfoConfig();
                var kq = new ConfigDAL().InsertConfig(obj);
                if (kq) lblResuft.InnerText = "Insert sussess!";
                else lblResuft.InnerText = "Insert error!";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit Alarm]", 0, "[InsertConfig]" + lblResuft.InnerText, 0.0);
            }

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Static/Config/ListConfig.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }
    }
}