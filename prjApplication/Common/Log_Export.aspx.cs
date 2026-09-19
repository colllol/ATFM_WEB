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
    public partial class Log_Export : PageBaseCallBack
    {      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }
       
        public void LoadData()
        {
            DataTable t = GetFilteredLogData();
            grdListUser.DataSource = t;
            grdListUser.DataBind();
        }

        private DataTable GetFilteredLogData()
        {
            DataTable source = new UserDAL().getLogExp("1");
            string name = txtName.Text.Trim();
            string sqlCode = txtSqlCode.Text.Trim();
            string err = txtErr.Text.Trim();

            if (name.Length == 0 && sqlCode.Length == 0 && err.Length == 0)
                return source;

            DataTable result = source.Clone();
            foreach (DataRow row in source.Rows)
            {
                if (!ContainsIgnoreCase(GetLogValue(row, "NAME"), name)
                    || !ContainsIgnoreCase(GetLogValue(row, "SQLCODE"), sqlCode)
                    || !ContainsIgnoreCase(GetLogValue(row, "ERR"), err))
                    continue;

                result.ImportRow(row);
            }
            return result;
        }

        private static string GetLogValue(DataRow row, string columnName)
        {
            if (!row.Table.Columns.Contains(columnName) || row.IsNull(columnName))
                return string.Empty;
            return Convert.ToString(row[columnName]);
        }

        private static bool ContainsIgnoreCase(string value, string search)
        {
            return search.Length == 0
                || value.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;
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
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            DataGrid ax = grdListUser;
            ax.DataSource = GetFilteredLogData();
            ax.DataBind();
            this.CreateExcel(this.RenderToHTML(ax), "LOG_" + DateTime.Now.ToFileTime() + ".xls");
        }

    }
}
