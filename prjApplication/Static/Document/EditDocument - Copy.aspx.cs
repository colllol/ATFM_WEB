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

namespace prjApplication.Static.Document
{
    public partial class EditDocument : System.Web.UI.Page
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
                      
                        LoadDocumentById();
                    }
                }
            }
        }

        private void LoadDocumentById()
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
            prjInfo.Document obj = new DocumentDAL().GetDocumentById(_id.ToString());
            txt_NAME.Text = obj.NAME;
            txtValue.Value = obj.PATH;
            txtNote.Value = obj.NOTE;
            

        }

        private prjInfo.Document GetInfoDocument()
        {

            prjInfo.Document _obj = new prjInfo.Document();
            int id = CommonLib.IsNumeric(Request["ID"]) ? Convert.ToInt32(Request["ID"].ToString()) : 0;            
            _obj.ID = id;
            _obj.PATH = txtValue.Value;
            _obj.NAME = txt_NAME.Text;
            _obj.NOTE = txtNote.Value;
                      
            return _obj;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            prjInfo.Document obj;
            if (CommonLib.IsNumeric(Request["ID"]) == true)
            {
                // update
                obj = GetInfoDocument();
                int _id = Convert.ToInt32(Request["ID"].ToString());
                obj.ID = _id;
                var kq = new DocumentDAL().UpdateDocument(obj);
                if (kq) lblResuft.InnerText = "Update sussess!";
                else lblResuft.InnerText = "Update error!";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit Document]", 0, "[UpdateDocument]" + lblResuft.InnerText, 0.0);
            }
            else
            {
                // insert
                obj = GetInfoDocument();
                var kq = new DocumentDAL().InsertDocument(obj);
                if (kq) lblResuft.InnerText = "Insert sussess!";
                else lblResuft.InnerText = "Insert error!";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit Alarm]", 0, "[InsertDocument]" + lblResuft.InnerText, 0.0);
            }

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Static/Document/ListDocument.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }
    }
}