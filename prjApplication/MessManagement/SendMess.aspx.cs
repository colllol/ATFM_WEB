using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using prjComponents;
using prjInfo;
using prjBusinessLogic;
using prjBusinessLogic.DAL;
using HPCServerDataAccess;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace prjApplication.Common
{
    public partial class SendMess : PageBaseCallBack
    {      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtd.Text = DateTime.Now.ToString("dd/MM/yyyy");
                LoadData();
            }
        }

        public void grdListUser_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            ImageButton btnDelete = (ImageButton)e.Item.FindControl("btnDelete");
            if (btnDelete != null)
                btnDelete.Attributes.Add("onclick", "return confirm(\"Bạn có chắc chắn muốn xóa không?\");");
            if (e.Item.ItemIndex >= 0)
            {
                e.Item.Attributes.Add("onmouseover", "currColor=this.style.backgroundColor;this.style.backgroundColor='" + CommonLib.HPCOnmouseoverGrid() + "'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currColor");
            }
        }
        protected void btnAddMenu_Click(object sender, EventArgs e)
        {
            UserDAL _obj = new UserDAL();
            _obj.DeleteExportLog(DateTime.Now.ToString("dd/mm/yyyy"));
            LoadData();
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string _date = txtd.Text;
            UserDAL _obj = new UserDAL();
            DataTable t = _obj.ViewMess(_date,txtSearch_UserName.Text);
            grdListUser.DataSource = t;
            grdListUser.DataBind();
        }
        public void LoadData()
        {
            string _date = txtd.Text;
            UserDAL _obj = new UserDAL();
            DataTable t = _obj.ViewMess(_date, txtSearch_UserName.Text);
            grdListUser.DataSource = t;
            grdListUser.DataBind();
        }
        protected string IsStatusRolePublish(string str)
        {
            string strReturn = "";
            if (str == "1")
                strReturn = Global.ApplicationPath + "/Images/Icons/Display.gif";
            if (str == "0")
                strReturn = Global.ApplicationPath + "/Images/Icons/uncheck.gif";
            return strReturn;
        }
        protected string IsStatusGet(string str)
        {
            string strReturn = "";
            if (str == "1")
                strReturn = Global.ApplicationPath + "/Images/check.png";
            if (str == "0")
                strReturn = Global.ApplicationPath + "/Images/cross-32.png";
            return strReturn;
        }
        public void grdListUser_EditCommand(object source, DataGridCommandEventArgs e)
        {
            switch (e.CommandArgument.ToString().ToLower())
            {
                case "lock":
                    
                    break;
                
                default:
                    Response.Redirect("~/User/EditUser.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString() + "&ID=" + this.grdListUser.DataKeys[e.Item.ItemIndex].ToString());
                    break;
            }
        }
        
       
    }
}
