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

namespace prjApplication.Groups
{
    public partial class ListGroups : System.Web.UI.Page
    {
        prjBusinessLogic.UserDAL _userDAL = new UserDAL();
        prjInfo.T_Users _user = null;
        protected prjInfo.T_RolePermission _Role = null;
        private int manhom
        {
            get { if (ViewState["manhom"] != null) return Convert.ToInt32(ViewState["manhom"]); else return 0; }

            set { ViewState["manhom"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["Menu_ID"] != null && Request["Menu_ID"].ToString() != "" && Request["Menu_ID"].ToString() != String.Empty)
            {
                if (CommonLib.IsNumeric(Request["Menu_ID"]) == true)
                {
                    if (!HPCSecurity.IsAccept(Convert.ToInt32(Request["Menu_ID"])))
                        Response.Redirect("~/Errors/AccessDenied.aspx");
                    _user = _userDAL.GetUserByUserName(HPCSecurity.CurrentUser.Identity.Name);
                    _Role = _userDAL.GetRole4UserMenu(_user.UserID, Convert.ToInt32(Request["Menu_ID"]));
                    ActivePermission();
                    if (!IsPostBack)
                        BindData();
                }
            }
        }
        protected void ActivePermission()
        {
            this.linkAddNews.Visible = _Role.R_Add;
        }
        private void LoadComboBoxCate()
        {
            UltilFunc.BindCombox(ddlLang, "Languages_ID", "Languages_Name", "T_Languages", " 1=1  Order by Languages_Name", "");
            ddlLang.SelectedIndex = UltilFunc.GetIndexControl(ddlLang, UltilFunc.SPGet_ChuyenMucDefault().ToString());
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            DataGrid ax = grdListNhom;
            ax.Visible = true;
            ax.DataSource = new GroupDAL().GetAllGroup();
            ax.DataBind();
            this.CreateExcel(this.RenderToHTML(ax), "GROUP_" + DateTime.Now.ToFileTime() + ".xls");
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
        protected void BindData()
        {
            List<prjInfo.T_Groups> t = new GroupDAL().GetPageGroups(CustomPaging1.PageSize, CustomPaging1.PageIndex, CustomPaging1.ValueSearch);
            CustomPaging1.TotalsRecord = t.GetTotalRecord<prjInfo.T_Groups>();
            grdListNhom.DataSource = t;
            grdListNhom.DataBind();
        }
        #region Tim KIEM
        protected void linkSearch_OnClick(object sender, EventArgs e)
        {
            string where = GetWhereCondition();
            CustomPaging1.ValueSearch = where;
            BindData();
        }
        public string GetWhereCondition()
        {
            string where = " 1=1";
            if (!String.IsNullOrEmpty(txtTenNhom.Text.Trim()))
                where += " AND " + (string.Format(" UPPER(Group_Name) like N'%{0}%'", UltilFunc.SqlFormatText(this.txtTenNhom.Text.Trim().ToUpper())));
            return HttpUtility.UrlEncode(where.ToUpper());
        }
        #endregion
        protected void pages_IndexChanged(object sender, EventArgs e)
        {
            BindData();
        }
        protected void ddlLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CategoryDAL _cateDAL = new CategoryDAL();
            //SetAddEdit(false, false, true, false,false);
            //this.dgCategory.DataSource = _cateDAL.BindGridCategoryByGroup(manhom, Convert.ToInt32(ddlLang.SelectedValue)).DefaultView;
            //this.dgCategory.DataBind();
        }
        protected string IpAddress()
        {
            string strIp;
            strIp = Page.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (strIp == null)
            {
                strIp = Page.Request.ServerVariables["REMOTE_ADDR"];
            }
            return strIp;
        }
        public void grdListNhom_EditCommand(object source, DataGridCommandEventArgs e)
        {
            #region GhiLog
            ActionHistoryDAL actionDAL = new ActionHistoryDAL();
            T_ActionHistory action = new T_ActionHistory();
            action.UserID = _user.UserID;
            action.FullName = _user.UserName;
            action.HostIP = IpAddress();
            action.DateModify = DateTime.Now;
            #endregion
            GroupDAL _nhomnguoidungDAL = new GroupDAL();
            if (e.CommandArgument.ToString().ToLower() == "role")
            {
                SetAddEdit(false, true,false,false,false);
                manhom = Convert.ToInt32(this.grdListNhom.DataKeys[e.Item.ItemIndex]);
                this.roleChucNang.Text = _nhomnguoidungDAL.GetOneFromT_GroupsByID(manhom).Group_Name;
                gdListMenu.DataSource = _nhomnguoidungDAL.BindGridMenuByGroup(manhom).DefaultView;
                gdListMenu.DataBind();
            }
            if (e.CommandArgument.ToString().ToLower() == "edit")
            {
                manhom = Convert.ToInt32(this.grdListNhom.DataKeys[e.Item.ItemIndex]);
                Response.Redirect("~/Groups/EditGroup.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString() + "&ID=" + manhom.ToString());
            }
            if (e.CommandArgument.ToString().ToLower() == "btnrolecate")
            {
                //CategoryDAL _cateDAL = new CategoryDAL();
                //SetAddEdit(false, false, true,false,false);
                //LoadComboBoxCate();
                //manhom = Convert.ToInt32(this.grdListNhom.DataKeys[e.Item.ItemIndex]);
                //lblRoleChuyenMuc.Text = _nhomnguoidungDAL.GetByGroupName_ID(manhom).Group_Name;
                //this.dgCategory.DataSource = _cateDAL.BindGridCategoryByGroup(manhom, Convert.ToInt32(ddlLang.SelectedValue)).DefaultView;
                //dgCategory.DataBind();
            }
            if (e.CommandArgument.ToString().ToLower() == "rolepaper")
            {
                // CategoryDAL _cateDAL = new CategoryDAL();
                //SetAddEdit(false, false, false, false,true);
                //manhom = Convert.ToInt32(this.grdListNhom.DataKeys[e.Item.ItemIndex]);
                //lbLoaibao.Text = _nhomnguoidungDAL.GetByGroupName_ID(manhom).Group_Name;
                //this.drgListPaper.DataSource = _cateDAL.BindGridPapersByGroup(manhom).DefaultView;
                //drgListPaper.DataBind();
            }
            if (e.CommandArgument.ToString().ToLower() == "btnrolelang")
            {
                //CategoryDAL _cateDAL = new CategoryDAL();
                SetAddEdit(false, false, false, true, false);
                manhom = Convert.ToInt32(this.grdListNhom.DataKeys[e.Item.ItemIndex]);
                lblRoleNgonNgu.Text = _nhomnguoidungDAL.GetOneFromT_GroupsByID(manhom).Group_Name;
                dgListLanguages_ID.DataSource = _nhomnguoidungDAL.BindGridReportByGroup(manhom).DefaultView;
                dgListLanguages_ID.DataBind();
            }
            if (e.CommandArgument.ToString().ToLower() == "delete")
            {
                manhom = Convert.ToInt32(this.grdListNhom.DataKeys[e.Item.ItemIndex]);
                _nhomnguoidungDAL.DeleteFromT_GroupsByID(manhom);
                action.ActionsCode = "[Xóa GroupID]-->[Thao tác Xóa Trong Bảng T_GROUP][GroupID:" + manhom.ToString() + " ]";
                SetAddEdit(true, false,false,false,false);
                BindData();
            }
            actionDAL.InserT_ActionOrc(action);
            
        }
        public string SetNameMenu(object _menuName, object _menuID)
        {
            if (CommonLib.isParrentMenu(Convert.ToInt32(_menuID)))
            {
                return string.Format("<b>{0}", _menuName.ToString());
            }
            else
                return _menuName.ToString();
        }
        public void grdListNhom_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
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
        //gdListMenu_ItemDataBound
        public void gdListMenu_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            e.Item.Attributes.Add("onmouseover", "currColor=this.style.backgroundColor;this.style.backgroundColor='" + CommonLib.HPCOnmouseoverGrid() + "'");
            e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currColor");
        }
        // PHAN QUYEN CHUYEN MUC GROUP
        protected void linkRoleCateExit_Click(object sender, EventArgs e)
        {
            SetAddEdit(true, false, false,false,false);
            Response.Redirect("~/Groups/ListGroups.aspx?Menu_ID=" + Page.Request["Menu_ID"]);
        }
        protected void linkRoleCateSaves_Click(object sender, EventArgs e)
        {
            SetAddEdit(false, false, true,false,false);
            Save_GroupCategorys(manhom);
            SetAddEdit(true, false, false, false,false);
            Response.Redirect("~/Groups/ListGroups.aspx?Menu_ID=" + Page.Request["Menu_ID"]);
        }
        private void Save_GroupCategorys(int Group_ID)
        {
            //Xoa thong tin trong bang T_Groupcategory
            GroupDAL _groupDAL = new GroupDAL();
            _groupDAL.DeleteFromT_GroupCategoryDynamic(" Group_ID=" + Group_ID);
            //Cap nhat thong tin bang T_UserMenu - Kiem tra xem da co quyen chua
            foreach (DataGridItem m_Item in dgCategory.Items)
            {
                System.Web.UI.WebControls.CheckBox chk_Select = (CheckBox)m_Item.FindControl("optSelect");
                if (chk_Select != null && chk_Select.Checked == false)
                {
                    int Categorys_ID = Convert.ToInt32(this.dgCategory.DataKeys[m_Item.ItemIndex].ToString());
                    _groupDAL.DeleteFromT_GroupCategoryDynamic(" Group_ID=" + Group_ID + " AND Categorys_ID IN(" + Categorys_ID + ") ");
                }
                if (chk_Select != null && chk_Select.Checked)
                {
                    int Categorys_ID = Convert.ToInt32(this.dgCategory.DataKeys[m_Item.ItemIndex].ToString());
                    _groupDAL.InsertT_GroupCategory(Categorys_ID, Group_ID);
                    _groupDAL.Sp_AutoInsertCategoryFromGroup(Categorys_ID, Group_ID, DateTime.Now);
                }
            }
        }
        //END
        
        #region SAVE NHOM THEO NGU
        public void linkExitLang_Click(object sender, EventArgs e)
        {
            SetAddEdit(true, false, false,false,false);
            Page.Response.Redirect(Page.Request.RawUrl);
        }
        public void linkSaveLang_Click(object sender, EventArgs e)
        {
            Save_GroupLanguages(manhom);
            Page.Response.Redirect(Page.Request.RawUrl);
        }
        private void Save_GroupLanguages(int Group_ID)
        {
            GroupDAL _nhomnguoidungDAL = new GroupDAL();
            //Xoa thong tin trong bang T_GroupMenu
            _nhomnguoidungDAL.XoaChucnangNhomNguoidungLanguages(Group_ID);
            foreach (DataGridItem m_Item in dgListLanguages_ID.Items)
            {
                CheckBox chk_Select = (CheckBox)m_Item.FindControl("optSelect");
                if (chk_Select != null && chk_Select.Checked)
                {
                    int LangID = Convert.ToInt32(this.dgListLanguages_ID.DataKeys[m_Item.ItemIndex].ToString());
                    _nhomnguoidungDAL.InsertT_GroupReport(LangID, Group_ID);
                }
            }
        }
        #endregion

        private void SetAddEdit(bool isList, bool isRole,bool isRoleCate,bool isRoleLang,bool isLoaibao)
        {
            pnlList.Visible = isList;
            plRole.Visible = isRole;
            pnlRoleCategorys.Visible = isRoleCate;
            pnlLang.Visible = isRoleLang;
            panelLoaibao.Visible = isLoaibao;
        }
        public void LinkGroupExit_Click(object sender, EventArgs e)
        {
            SetAddEdit(true, false,false,false,false);
            Page.Response.Redirect(Page.Request.RawUrl);
        }

        public void linkGroupSave_Click(object sender, EventArgs e)
        {
            Save_GroupMenu(manhom);
            Page.Response.Redirect(Page.Request.RawUrl);
        }
        public void linkAddNews_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Groups/EditGroup.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }
        private void Save_GroupMenu(int Group_ID)
        {
            Int32 R_Edit = 0, R_Del = 0, R_Add = 0, R_Pub = 0;
            GroupDAL _nhomnguoidungDAL = new GroupDAL();
            //Xoa thong tin trong bang T_GroupMenu
            _nhomnguoidungDAL.XoaChucnangNhomNguoidung(Group_ID);
            foreach (DataGridItem m_Item in gdListMenu.Items)
            {
                System.Web.UI.HtmlControls.HtmlInputCheckBox chk_Select = (HtmlInputCheckBox)m_Item.FindControl("optSelect");
                HtmlInputCheckBox chkR_Add = (HtmlInputCheckBox)m_Item.FindControl("chkR_Add");
                HtmlInputCheckBox chkR_Edit = (HtmlInputCheckBox)m_Item.FindControl("chkR_Edit");
                HtmlInputCheckBox chkR_Del = (HtmlInputCheckBox)m_Item.FindControl("chkR_Del");
                HtmlInputCheckBox chkR_Pub = (HtmlInputCheckBox)m_Item.FindControl("chkR_Pub");
                if (chk_Select != null && chk_Select.Checked)
                {
                    int Menu_ID = Convert.ToInt32(this.gdListMenu.DataKeys[m_Item.ItemIndex].ToString());
                    if (chkR_Add != null && chkR_Add.Checked) R_Add = 1;
                    else R_Add = 0;
                    if (chkR_Edit != null && chkR_Edit.Checked)  R_Edit = 1;
                    else R_Edit = 0;
                    if (chkR_Del != null && chkR_Del.Checked)  R_Del = 1;
                    else R_Del = 0;
                    if (chkR_Pub != null && chkR_Pub.Checked)  R_Pub = 1;
                    else R_Pub = 0;
                    _nhomnguoidungDAL.InsertT_GroupMenu(Menu_ID, Group_ID,R_Edit,R_Del,R_Add,R_Pub);
                }
            }
        }
        #region Phan Quyen Loai bao
        protected void linkRoleExitPaper_Click(object sender, EventArgs e)
        {
            SetAddEdit(true, false, false, false, false);
            Page.Response.Redirect(Page.Request.RawUrl);
        }
        protected void linkRoleSavesPaper_Click(object sender, EventArgs e)
        {
            Save_GroupPapers(manhom);
            Page.Response.Redirect(Page.Request.RawUrl);
        }
        private void Save_GroupPapers(int Group_ID)
        {
            GroupDAL _nhomnguoidungDAL = new GroupDAL();
            //Xoa thong tin trong bang T_GroupMenu
            _nhomnguoidungDAL.XoaChucnangNhomNguoidungPaper(Group_ID);
            foreach (DataGridItem m_Item in drgListPaper.Items)
            {
                CheckBox chk_Select = (CheckBox)m_Item.FindControl("optSelect");
                if (chk_Select != null && chk_Select.Checked)
                {
                    int PID = Convert.ToInt32(this.drgListPaper.DataKeys[m_Item.ItemIndex].ToString());
                    _nhomnguoidungDAL.InsertT_GroupPapers(PID, Group_ID);
                }
            }
        }
        #endregion
    }
}
