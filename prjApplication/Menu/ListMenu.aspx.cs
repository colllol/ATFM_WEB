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
using prjBusinessLogic;
using prjInfo;
using prjComponents;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using TuesPechkin;
namespace prjApplication.Menu
{
    public partial class ListMenu : PageCoreAdmin
    {
        
        private const string _AliasSession = "ListMenu";
        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnAddMenu.Visible = _Role.R_Add;
            if (!IsPostBack)
            {
                try
                {
                    
                    this.btnAddMenu.Visible = _Role.R_Add;
                    CustomPaging1.ValueSearch = Session[_AliasSession].ToString();
                    Session.SetValueForControlSearch(this, _AliasSession);
                }
                catch
                {
                    Session.SetValueSearch(_AliasSession, " 1=1");
                }
                LoadData();
            }
        }
        #region Method
        protected string IsActiveSync(string str)
        {
            string strReturn = "";
            if (str.ToLower() == "true")
                strReturn = Global.ApplicationPath + "/Images/check.png";
            if (str.ToLower() == "false")
                strReturn = Global.ApplicationPath + "/Images/uncheck.png";
            return strReturn;
        }
        protected string IsActiveSyncTooltip(string str)
        {
            string strReturn = "";
            if (str.ToLower() == "true")
                strReturn = "Đồng bộ";
            if (str.ToLower() == "false")
                strReturn = "Không đồng bộ";
            return strReturn;
        }
        protected void pages_IndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            List<T_Menus> t = new MenuDAL().GetPageMenus(CustomPaging1.PageSize, CustomPaging1.PageIndex, CustomPaging1.ValueSearch);
            CustomPaging1.TotalsRecord = t.GetTotalRecord<T_Menus>();
            gdListMenu.DataSource = t;
            gdListMenu.DataBind();
        }
        #endregion

        #region Method Click
        protected void linkSearch_Click(object sender, EventArgs e)
        {
            string where = GetWhereCondition();
            Session.SetValueSearch(_AliasSession, where);
            CustomPaging1.ValueSearch = where;
            LoadData();
        }
        public string GetWhereCondition()
        {
            string where = " 1=1";
            if (!String.IsNullOrEmpty(txtSearch_Menu.Text.Trim()))
                where += " AND " + string.Format(" UPPER(MenuName) like N'%{0}%'", UltilFunc.SqlFormatText(this.txtSearch_Menu.Text.Trim().ToUpper()));
            return HttpUtility.UrlEncode(where.ToUpper());
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            GridView ax = GridView;
            ax.Visible = true;
            ax.DataSource = new MenuDAL().GetAllMenus();
            ax.AutoGenerateColumns = false;
            ax.DataBind();
            this.CreateExcel(this.RenderToHTML(ax), "List_Menu_" + DateTime.Now.ToFileTime() + ".xls");
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
        protected void btnAddMenu_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Menu/EditMenu.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }
        public void gdListMenu_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            ImageButton btnDeleteAll = (ImageButton)e.Item.FindControl("btnDelete");
            if (btnDeleteAll != null)
                btnDeleteAll.Attributes.Add("onclick", "return confirm(\"Bạn có chắc chắn muốn xóa không?\");");
            if (e.Item.ItemIndex >= 0)
            {
                e.Item.Attributes.Add("onmouseover", "currColor=this.style.backgroundColor;this.style.backgroundColor='" + CommonLib.HPCOnmouseoverGrid() + "'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currColor");
            }
        }
        public void gdListMenu_EditCommand(object source, DataGridCommandEventArgs e)
        {
            MenuDAL _menuDAL = new MenuDAL();
            switch (e.CommandArgument.ToString().ToLower())
            {
                case "edit":
                    Response.Redirect("~/Menu/EditMenu.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString() + "&ID=" + this.gdListMenu.DataKeys[e.Item.ItemIndex].ToString());
                    break;
                case "issync":
                    {
                        int _ID = Convert.ToInt32(this.gdListMenu.DataKeys[e.Item.ItemIndex].ToString());
                        bool check = Convert.ToBoolean(_menuDAL.GetOneFromT_MenusByID(_ID).ActiveSync);
                        string _strLog = "";
                        if (check)
                        {
                            _menuDAL.UpdateFromT_MenusDynamic(" ActiveSync = 0 Where ID = " + _ID);
                            _strLog = "[Danh sách chức năng Hệ thống] [Cập nhật đồng bộ text: " + _menuDAL.GetOneFromT_MenusByID(_ID).MenuName + "]";
                            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Cập nhật]", Request["Menu_ID"], _strLog, 0);
                        }
                        else
                        {
                            _menuDAL.UpdateFromT_MenusDynamic(" ActiveSync = 1 Where ID = " + _ID);
                            _strLog = "[Danh sách chức năng Hệ thống] [Cập nhật đồng bộ text: " + _menuDAL.GetOneFromT_MenusByID(_ID).MenuName + "]";
                            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Cập nhật]", Request["Menu_ID"], _strLog, 0);
                        }
                        break;
                    }
                case "issyncimage":
                    {
                        int _ID = Convert.ToInt32(this.gdListMenu.DataKeys[e.Item.ItemIndex].ToString());
                        bool check = Convert.ToBoolean(_menuDAL.GetOneFromT_MenusByID(_ID).ActiveSyncImages);
                        string _strLog = "";
                        if (check)
                        {
                            _menuDAL.UpdateFromT_MenusDynamic(" ActiveSyncImages = 0 Where ID = " + _ID);
                            _strLog = "[Danh sách chức năng Hệ thống] [Cập nhật đồng bộ ảnh: " + _menuDAL.GetOneFromT_MenusByID(_ID).MenuName + "]";
                            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Cập nhật]", Request["Menu_ID"], _strLog, 0);
                        }
                        else
                        {
                            _menuDAL.UpdateFromT_MenusDynamic(" ActiveSyncImages = 1 Where ID = " + _ID);
                            _strLog = "[Danh sách chức năng Hệ thống] [Cập nhật đồng bộ ảnh: " + _menuDAL.GetOneFromT_MenusByID(_ID).MenuName + "]";
                            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Cập nhật]", Request["Menu_ID"], _strLog, 0);
                        }
                        break;
                    }
                case "delete":
                    {
                        int _menuID = Convert.ToInt32(this.gdListMenu.DataKeys[e.Item.ItemIndex].ToString());
                        string _strLog = "[Danh sách chức năng Hệ thống] [Xóa chức năng: " + _menuDAL.GetOneFromT_MenusByID(_menuID).MenuName + "]";
                        _menuDAL.DeleteFromT_MenusByID(_menuID);
                        WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Cập nhật]", Request["Menu_ID"], _strLog, 0);
                        break;
                    }
                   
            }
            this.LoadData();
        }
        #endregion
        protected void GridView_PreRender(object sender, EventArgs e)
        {
            GridView gv = (GridView)sender;

            if ((gv.ShowHeader == true && gv.Rows.Count > 0)
                || (gv.ShowHeaderWhenEmpty == true))
            {
                //Force GridView to use <thead> instead of <tbody> - 11/03/2013 - MCR.
                gv.HeaderRow.TableSection = TableRowSection.TableHeader;

            }
            if (gv.ShowFooter == true && gv.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody> - 11/03/2013 - MCR.
                gv.FooterRow.TableSection = TableRowSection.TableFooter;
            }

        }
    }
}
