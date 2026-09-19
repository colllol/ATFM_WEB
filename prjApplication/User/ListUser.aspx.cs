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

namespace prjApplication.User
{
    public partial class ListUser : System.Web.UI.Page
    {
        private bool _refreshState;
        private bool _isRefresh;
        protected override void LoadViewState(object savedState)
        {
            try
            {
                object[] AllStates = (object[])savedState;
                base.LoadViewState(AllStates[0]);
                _refreshState = bool.Parse(AllStates[1].ToString());
                _isRefresh = _refreshState ==
                bool.Parse(Session["__ISREFRESH"].ToString());
            }
            catch
            { }
        }
        protected override object SaveViewState()
        {
            Session["__ISREFRESH"] = _refreshState;
            object[] AllStates = new object[2];
            AllStates[0] = base.SaveViewState();
            AllStates[1] = !(_refreshState);
            return AllStates;
        }
        prjBusinessLogic.UserDAL _userDAL = new UserDAL();
        prjInfo.T_Users _user = null;
        protected prjInfo.T_RolePermission _Role = null;
        private int UserID
        {
            get { if (ViewState["UserID"] != null) return Convert.ToInt32(ViewState["UserID"]); else return 0; }

            set { ViewState["UserID"] = value; }
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
                    _Role = MenuCache.GetRoleOrLoad(
                        _user.UserID, Convert.ToInt32(Request["Menu_ID"]), _userDAL);
                    AvtiverPermission();
                    if (!IsPostBack)
                    {
                        try
                        {
                            CustomPaging1.ValueSearch = Session[_AliasSession].ToString();

                            Session.SetValueForControlSearch(this, _AliasSession);
                        }
                        catch { Session.SetValueSearch(_AliasSession, " 1=1"); }

                        LoadData();
                    }
                }
            }
            //string t = _userDAL.GetAll("/api/Users/GetAllUsers");
            //lit.Text = t;
        }
        protected void AvtiverPermission()
        {
            this.btnAddMenu.Visible = _Role.R_Add;
        }
        protected void pages_IndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
        public void Search_OnClick(object sender, EventArgs e)
        {
            //pages.PageIndex = 0;
            LoadData();
        }
        protected void btnAddMenu_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/User/EditUser.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
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
        public void gdListMenu_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemIndex >= 0)
            {
                e.Item.Attributes.Add("onmouseover", "currColor=this.style.backgroundColor;this.style.backgroundColor='" + CommonLib.HPCOnmouseoverGrid() + "'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currColor");
            }
            CheckBox chk_Select = (CheckBox)e.Item.FindControl("optSelect");
            if (chk_Select != null)
            {
                int temp = e.Item.ItemIndex + 2;
                chk_Select.InputAttributes.Add("value", "" + temp + "");
                chk_Select.Attributes.Add("onclick", "return ChkParrent(this,this.value);");
            }
        }
        public void grdListUser_EditCommand(object source, DataGridCommandEventArgs e)
        {
            switch (e.CommandArgument.ToString().ToLower())
            {
                case "role":
                    SetAddEdit(false, true, false, false, false, false);
                    UserID = Convert.ToInt32(grdListUser.DataKeys[e.Item.ItemIndex]);
                    this.roleChucNang.Text = _userDAL.GetUserByUserName_ID(UserID).UserFullName;//Global.RM.GetString("USER_TITLE_ROLEWITDHMENU") + _userDAL.GetUserByUserName_ID(UserID).UserName;
                    this.gdListMenu.DataSource = _userDAL.BindGridMenuByUser(UserID).DefaultView;
                    this.gdListMenu.DataBind();
                    break;
                case "editusers":
                    Response.Redirect("~/User/EditUser.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString() + "&ID=" + this.grdListUser.DataKeys[e.Item.ItemIndex].ToString());
                    break;
                case "group":
                    SetAddEdit(false, false, true, false, false, false);
                    UserID = Convert.ToInt32(grdListUser.DataKeys[e.Item.ItemIndex]);
                    this.lblThuocNhom.Text =  _userDAL.GetUserByUserName_ID(UserID).UserFullName;
                    DataBindGroup(UserID);
                    break;
                case "rolecategory":
                    SetAddEdit(false, false, false, true, false, false);
                    LoadComboBoxCate();
                    UserID = Convert.ToInt32(grdListUser.DataKeys[e.Item.ItemIndex]);
                    this.roleChuyenMuc.Text = _userDAL.GetUserByUserName_ID(UserID).UserFullName;
                    this.dgListCategorys.DataSource = _userDAL.BindGridT_UserCategoryByUser(UserID, UltilFunc.SPGet_ChuyenMucDefault()).DefaultView;
                    this.dgListCategorys.DataBind();
                    break;
                case "rolesubject":
                    SetAddEdit(false, false, false, false, false, false);
                    pnlRolesChuyenDe.Visible = true;
                    LoadComboBoxChuyenDe();
                    UserID = Convert.ToInt32(grdListUser.DataKeys[e.Item.ItemIndex]);
                    this.pnlRolesChuyenDe.GroupingText = "Phân quyền chuyên đề người dùng: " + _userDAL.GetUserByUserName_ID(UserID).UserName;
                    this.dgChuyenDe.DataSource = _userDAL.BindGridT_UserCategoryByUserSubject(UserID, UltilFunc.SPGet_ChuyenMucDefault()).DefaultView;
                    this.dgChuyenDe.DataBind();
                    break;
                case "rolelang":
                    SetAddEdit(false, false, false, false, true, false);
                    UserID = Convert.ToInt32(grdListUser.DataKeys[e.Item.ItemIndex]);
                    this.lblRoleNgonNgu.Text = "Quyền báo cáo của người dùng: " + _userDAL.GetUserByUserName_ID(UserID).UserFullName;
                    this.dgListLanguages.DataSource = _userDAL.BindGridT__UserLanguagesByUser(UserID).DefaultView;
                    this.dgListLanguages.DataBind();
                    break;
                case "rolepaper":
                    SetAddEdit(false, false, false, false, false, true);
                    UserID = Convert.ToInt32(grdListUser.DataKeys[e.Item.ItemIndex]);
                    this.lbLoaibao.Text = "Quyền loại báo cáo của người dùng: " + _userDAL.GetUserByUserName_ID(UserID).UserFullName;
                    this.drgListPaper.DataSource = _userDAL.BindGridT_UserPaperByUser(UserID).DefaultView;
                    this.drgListPaper.DataBind();
                    break;
                case "resetpass":
                    UserID = Convert.ToInt32(grdListUser.DataKeys[e.Item.ItemIndex]);
                    string pass = prjComponents.HPCSecurity.Encrypt("Pass1234");//Default Pass:  123456
                    //UltilFunc _until = new UltilFunc();
                    //_until.ExecSql("UPDATE T_Users SET UserPass = '" + pass + "' WHERE UserID =" + UserID);
                    _userDAL.CMS_ResetPass(UserID, pass);

                    WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Cập nhật]", Request["Menu_ID"].ToString(), "[Người dùng] [Thao tác khôi mục mật khẩu: " + _userDAL.GetOneFromT_UsersByID(UserID).UserFullName + "]", 0);
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('" + Global.RM.GetString("UpdateSuccessfully") + "');", true);
                    this.LoadData();
                    break;
                case "delete":
                    UserID = Convert.ToInt32(grdListUser.DataKeys[e.Item.ItemIndex]);
                    if (_userDAL.CheckDelete_users(UserID))
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('Bạn không được xóa User đã phat sinh.!');", true);
                        return;
                    }
                    else
                    {
                        WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Xóa]", Request["Menu_ID"].ToString(), "[Người dùng] [Thao tác xóa người dùng: " + _userDAL.GetOneFromT_UsersByID(UserID).UserFullName + "]", 0);
                        _userDAL.DeleteFromT_UsersByID(UserID);
                        SetAddEdit(true, false, false, false, false, false);
                        this.LoadData();
                    }
                    break;
                case "isreporter":
                    int _ID = Convert.ToInt32(this.grdListUser.DataKeys[e.Item.ItemIndex].ToString());
                    int _userCurrentID = _user.UserID;
                    if (_userCurrentID != _ID)
                    {
                        bool check = Convert.ToBoolean(_userDAL.GetOneFromT_UsersByID(_ID).UserActive);
                        if (check)
                        {
                            //_userDAL.UpdateFromT_UsersDynamic(" UserActive = 0 Where UserID = " + _ID);
                            _userDAL.Update_UserActiveDynamic(_ID);
                            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Cập nhật]", Request["Menu_ID"].ToString(), "[Người dùng] [Thao tác cập nhật trạng thái người dùng: " + _userDAL.GetOneFromT_UsersByID(_ID).UserFullName + "]", 0);
                        }
                        else
                        {
                            // _userDAL.UpdateFromT_UsersDynamic(" UserActive = 1 Where UserID = " + _ID);
                            _userDAL.Update_UserActiveDynamic(_ID);
                            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Cập nhật]", Request["Menu_ID"].ToString(), "[Người dùng] [Thao tác cập nhật trạng thái người dùng: " + _userDAL.GetOneFromT_UsersByID(_ID).UserFullName + "]", 0);
                        }
                        SetAddEdit(true, false, false, false, false,false);
                        this.LoadData();
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('Bạn không thể khóa chính mình khi bạn đang đăng nhập!');", true);
                        return;
                    }
                    break;
                case "ispublish":
                    int UID = Convert.ToInt32(this.grdListUser.DataKeys[e.Item.ItemIndex].ToString());
                    int _roleXb = _userDAL.GetOneFromT_UsersByID(UID).IsReporter;
                    if (_roleXb==1)
                    {
                        _userDAL.UpdateFromT_UsersDynamic(" IsReporter = 0 Where UserID = " + UID);
                        WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Cập nhật]", Request["Menu_ID"].ToString(), "[Người dùng] [Thao tác cập nhật quyền xuất bản người dùng: " + _userDAL.GetOneFromT_UsersByID(UID).UserFullName + "]", 0);
                    }
                    else
                    {
                        _userDAL.UpdateFromT_UsersDynamic(" IsReporter = 1 Where UserID = " + UID);
                        WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Cập nhật]", Request["Menu_ID"].ToString(), "[Người dùng] [Thao tác cập nhật quyền xuất bản người dùng: " + _userDAL.GetOneFromT_UsersByID(UID).UserFullName + "]", 0);
                    }
                    SetAddEdit(true, false, false, false, false, false);
                    this.LoadData();                    
                    break;
                default:
                    Response.Redirect("~/User/EditUser.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString() + "&ID=" + this.grdListUser.DataKeys[e.Item.ItemIndex].ToString());
                    break;
            }
        }
        protected string IsStatusGet(string str)
        {
            string strReturn = "";
            if (str == "1")
                strReturn = Global.ApplicationPath + "/Images/check.png";
            if (str == "0")
                strReturn = Global.ApplicationPath + "/Images/uncheck.png";
            return strReturn;
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
        protected void DataBindGroup(int UserID)
        {
            UserDAL _usermenuDAL = new UserDAL();
            this.leftPane.DataSource = _usermenuDAL.GetAllFrom_T_GroupNotInUser("LeftPane", UserID);
            this.leftPane.DataBind();
            this.rightPane.DataSource = _usermenuDAL.GetAllFrom_T_GroupInUser("RightPane", UserID);
            this.rightPane.DataBind();
        }
        private void SetAddEdit(bool isList, bool isRole, bool isgroup, bool isRoleCategorys, bool isRoleLanguages,bool isRoleLoaibao)
        {
            pnlList.Visible = isList;
            plRole.Visible = isRole;
            plGroup.Visible = isgroup;
            pnlRoleCategorys.Visible = isRoleCategorys;
            pnlLanguages.Visible = isRoleLanguages;
            panelLoaibao.Visible = isRoleLoaibao;
        }
        public string SetNameMenu(object _menuName, object _menuID)
        {
            if (CommonLib.isParrentMenu(Convert.ToInt32(_menuID)))
                return string.Format("<b>{0}", _menuName.ToString());
            else
                return _menuName.ToString();
        }
        private void LoadComboBoxCate()
        {          
            UltilFunc.BindCombox(ddlLang, "Languages_ID", "Languages_Name", "T_Languages", string.Format(" 1=1 AND Languages_ID IN ({0}) Order by Languages_Name ", UltilFunc.GetLanguagesByUser(_user.UserID)), "---Tất cả---");
            if (ddlLang.Items.Count >= 3)
                ddlLang.SelectedIndex = prjComponents.Global.DefaultLangID;
            else ddlLang.SelectedIndex = UltilFunc.GetIndexControl(ddlLang, prjComponents.Global.DefaultCombobox);
        }
        private void LoadComboBoxChuyenDe()
        {
            UltilFunc.BindCombox(ddlChuyenDe, "Languages_ID", "Languages_Name", "T_Languages", " 1=1  Order by Languages_Name", "");
            ddlChuyenDe.SelectedIndex = UltilFunc.GetIndexControl(ddlChuyenDe, UltilFunc.SPGet_ChuyenMucDefault().ToString());
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            //DataGrid ax = grdListUser;
            GridView ax = GridView1;
            ax.Visible = true;           
            ax.DataSource = new UserDAL().GetAllUser();           
            ax.DataBind();
            this.CreateExcel(this.RenderToHTML(ax), "LIST_USER_" + DateTime.Now.ToFileTime() + ".xls");
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
        public void LoadData()
        {
            //List<T_Users> t = _userDAL.GetAllUser();
            //grdListUser.DataSource = t;
            //grdListUser.DataBind();

            List<prjInfo.T_Users> t = new UserDAL().GetPageUsers_New(CustomPaging1.PageSize, CustomPaging1.PageIndex, CustomPaging1.ValueSearch);
            CustomPaging1.TotalsRecord = t.GetTotalRecord<prjInfo.T_Users>();
            grdListUser.DataSource = t;
            grdListUser.DataBind();
        }
        #region Tim KIEM
        private const string _AliasSession = "ListUsers";
        private void PutclsSearchDetail()
        {
            List<clsSearchDetail> lisSearch = new List<clsSearchDetail>();
            lisSearch.Add(new clsSearchDetail("USERNAME", "txtSearch_UserName", txtSearch_UserName.Text.Trim()));

            Session.SetValueSearch(_AliasSession, lisSearch);
        }
        protected void linkSearch_Click(object sender, EventArgs e)
        {
            string where = GetWhereCondition();

            PutclsSearchDetail();
            Session.SetValueSearch(_AliasSession, where);
            CustomPaging1.ValueSearch = where;
            LoadData();
        }
        public string GetWhereCondition()
        {
            string where = " 1=1";
            if (!String.IsNullOrEmpty(txtSearch_UserName.Text.Trim()))
                where += " AND " + (string.Format(" UPPER(USERNAME) like N'%{0}%'", UltilFunc.SqlFormatText(this.txtSearch_UserName.Text.Trim().ToUpper())));
            return HttpUtility.UrlEncode(where.ToUpper());
        }
        #endregion

        #region EventClick
        #region Phan Quyen Ngon Ngu
        protected void linkRoleExitLanguages_Click(object sender, EventArgs e)
        {
            SetAddEdit(true, false, false, false, false,false);            
        }
        protected void linkRoleSavesLanguages_Click(object sender, EventArgs e)
        {
            Save_UserLanguages(UserID);
            this.LoadData();
        }
        private void Save_UserLanguages(int UserID)
        {
            UserDAL _usermenuDAL = new UserDAL();
            _usermenuDAL.DeleteFromT_UserReportDynamic(UserID, 0);//Xoa thong tin
            foreach (DataGridItem m_Item in dgListLanguages.Items)
            {
                CheckBox chk_Select = (CheckBox)m_Item.FindControl("optSelect");
                if (chk_Select != null && chk_Select.Checked && chk_Select.Enabled == true)
                {
                    int Report_ID = Convert.ToInt32(this.dgListLanguages.DataKeys[m_Item.ItemIndex].ToString());
                    _usermenuDAL.InsertT_UserReport(UserID, Report_ID, 0);
                }
            }
        }
        #endregion
        #region Phan Quyen Loai bao
        protected void linkRoleExitPaper_Click(object sender, EventArgs e)
        {
            SetAddEdit(true, false, false, false, false, false);
           
        }
        protected void linkRoleSavesPaper_Click(object sender, EventArgs e)
        {
            Save_UserPapers(UserID);
            this.LoadData();
        }
        private void Save_UserPapers(int UserID)
        {
            UserDAL _usermenuDAL = new UserDAL();
            _usermenuDAL.DeleteFromT_UserPaperDynamic(UserID, "UserID=" + UserID + " AND Group_ID=0");//Xoa thong tin
            foreach (DataGridItem m_Item in drgListPaper.Items)
            {
                CheckBox chk_Select = (CheckBox)m_Item.FindControl("optSelect");
                if (chk_Select != null && chk_Select.Checked && chk_Select.Enabled == true)
                {
                    int Paper_ID = Convert.ToInt32(this.drgListPaper.DataKeys[m_Item.ItemIndex].ToString());
                    _usermenuDAL.InsertT_UserPapers(UserID,Paper_ID, 0);
                }
            }
        }
        #endregion
        #region Phan Quyen Chuyen De
        protected void ddlChuyenDe_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetAddEdit(false, false, false, false, false,false);
            pnlRolesChuyenDe.Visible = true;
            this.dgChuyenDe.DataSource = _userDAL.BindGridT_UserCategoryByUserSubject(UserID, Convert.ToInt32(ddlChuyenDe.SelectedValue)).DefaultView;
            this.dgChuyenDe.DataBind();
        }
        protected void linkRoleExitChuyenDe_Click(object sender, EventArgs e)
        {
            pnlRolesChuyenDe.Visible = false;
            SetAddEdit(true, false, false, false, false,false);
            this.LoadData();
        }
        protected void linkRoleSavesChuyenDe_Click(object sender, EventArgs e)
        {
            Save_UserCategorySubject(UserID);
            this.LoadData();
        }
        private void Save_UserCategorySubject(int UserID)
        {
            UserDAL _usermenuDAL = new UserDAL(); ;
            //Xoa thong tin
            //_usermenuDAL.DeleteFromT_UserCategoryDynamic(UserID, "UserID=" + UserID + " AND Group_ID=0");
            foreach (DataGridItem m_Item in dgChuyenDe.Items)
            {
                CheckBox chk_Select = (CheckBox)m_Item.FindControl("optSelect");
                if (chk_Select != null && chk_Select.Checked == false && chk_Select.Enabled == true)
                {
                    int Cate_ID = Convert.ToInt32(this.dgChuyenDe.DataKeys[m_Item.ItemIndex].ToString());
                    _usermenuDAL.DeleteFromT_UserCategoryDynamic(UserID, "UserID=" + UserID + " AND Group_ID=0 AND Categorys_ID IN(" + Cate_ID + ")");
                }
                if (chk_Select != null && chk_Select.Checked && chk_Select.Enabled == true)
                {
                    int Cate_ID = Convert.ToInt32(this.dgChuyenDe.DataKeys[m_Item.ItemIndex].ToString());
                    _usermenuDAL.InsertT_UserCategory(UserID, Cate_ID, 0);
                }
            }
        }
        #endregion

        #region Phan Quyen Chuyen Muc
        protected void ddlLoaibao_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetAddEdit(false, false, false, true, false, false);
            ddlLang.Items.Clear();
            UltilFunc.BindCombox(ddlLang, "Languages_ID", "Languages_Name", "T_Languages", string.Format(" 1=1 AND Languages_ID IN ({0}) Order by Languages_Name ", UltilFunc.GetLanguagesByUser(_user.UserID)), "---Tất cả---");
        }
        protected void ddlLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetAddEdit(false, false, false, true, false,false);
            if (ddlLang.SelectedIndex > 0)
            {
                this.dgListCategorys.DataSource = _userDAL.BindGridT_UserCategoryByUser(UserID, Convert.ToInt32(ddlLang.SelectedValue)).DefaultView;
                this.dgListCategorys.DataBind();
            }
            
        }
        protected void linkRoleCateExit_Click(object sender, EventArgs e)
        {
            //this.LoadData();
            SetAddEdit(true, false, false, false, false,false);            
        }
        protected void linkRoleCateSaves_Click(object sender, EventArgs e)
        {
            Save_UserCategory(UserID);
            this.LoadData();
            SetAddEdit(true, false, false, false, false,false);            
        }
        private void Save_UserCategory(int UserID)
        {
            UserDAL _usermenuDAL = new UserDAL(); ;
            //Xoa thong tin
            //_usermenuDAL.DeleteFromT_UserCategoryDynamic(UserID, "UserID=" + UserID + " AND Group_ID=0");
            foreach (DataGridItem m_Item in dgListCategorys.Items)
            {
                CheckBox chk_Select = (CheckBox)m_Item.FindControl("optSelect");
                if (chk_Select != null && chk_Select.Checked == false && chk_Select.Enabled == true)
                {
                    int Cate_ID = Convert.ToInt32(this.dgListCategorys.DataKeys[m_Item.ItemIndex].ToString());
                    _usermenuDAL.DeleteFromT_UserCategoryDynamic(UserID, "UserID=" + UserID + " AND Group_ID=0 AND Categorys_ID IN(" + Cate_ID + ")");
                }
                if (chk_Select != null && chk_Select.Checked && chk_Select.Enabled == true)
                {
                    int Cate_ID = Convert.ToInt32(this.dgListCategorys.DataKeys[m_Item.ItemIndex].ToString());
                    _usermenuDAL.InsertT_UserCategory(UserID, Cate_ID, 0);
                }
            }
        }
        #endregion

        #region Phan QUYEN CHUC NANG NGUOI DUNG
        protected void btnApplyRole_Click(object sender, EventArgs e)
        {
            //Lay theo nhom thuoc danh sach duoc phan
            Save_UserMenu(UserID);
            SetAddEdit(true, false, false, false, false,false);
           
        }
        private void Save_UserMenu(int UserID)
        {
            Int32 R_Edit = 0, R_Del =0, R_Add = 0, R_Pub =0;
            UserDAL _usermenuDAL = new UserDAL();
            //Xoa thong tin
            _usermenuDAL.DeleteFromT_UserMenuDynamic(UserID, "UserID=" + UserID + " AND Group_ID=0");
          
            foreach (DataGridItem m_Item in gdListMenu.Items)
            {
                CheckBox chk_Select = (CheckBox)m_Item.FindControl("optSelect");
                CheckBox chkR_Add = (CheckBox)m_Item.FindControl("chkR_Add");
                CheckBox chkR_Edit = (CheckBox)m_Item.FindControl("chkR_Edit");
                CheckBox chkR_Del = (CheckBox)m_Item.FindControl("chkR_Del");
                CheckBox chkR_Pub = (CheckBox)m_Item.FindControl("chkR_Pub");
                if (chk_Select != null && chk_Select.Checked && chk_Select.Enabled == true)
                {
                    int Menu_ID = Convert.ToInt32(this.gdListMenu.DataKeys[m_Item.ItemIndex].ToString());
                    if (chkR_Add != null && chkR_Add.Checked) R_Add = 1;
                    else R_Add = 0;
                    if (chkR_Edit != null && chkR_Edit.Checked) R_Edit = 1;
                    else R_Edit = 0;
                    if (chkR_Del != null && chkR_Del.Checked) R_Del = 1;
                    else R_Del = 0;
                    if (chkR_Pub != null && chkR_Pub.Checked) R_Pub = 1;
                    else R_Pub = 0;
                    _usermenuDAL.InsertT_UserMenu(UserID, Menu_ID,R_Edit,R_Del,R_Add,R_Pub, 0);
                }
            }
        }
        protected void btnCancelRole_Click(object sender, EventArgs e)
        {
            SetAddEdit(true, false, false, false, false,false);
            
        }
        #endregion
        #endregion

        #region Phan Quyen Nhom
        ArrayList lasset = new ArrayList();
        ArrayList lsubordinate = new ArrayList();
        protected void btAddAll_Click(object sender, EventArgs e)
        {
            if (UserID > 0)
            {
                while (leftPane.Items.Count != 0)
                {
                    for (int i = 0; i < leftPane.Items.Count; i++)
                    {

                        if (!lasset.Contains(leftPane.Items[i]))
                        {
                            lasset.Add(leftPane.Items[i]);
                        }
                    }

                    for (int i = 0; i < lasset.Count; i++)
                    {
                        if (!rightPane.Items.Contains(((ListItem)lasset[i])))
                        {
                            rightPane.Items.Add(((ListItem)lasset[i]));
                        }
                        leftPane.Items.Remove(((ListItem)lasset[i]));
                    }
                }
            }
        }
        protected void btRemoveAll_Click(object sender, EventArgs e)
        {
            UserDAL _usermenuDAL = new UserDAL();
            if (UserID > 0)
            {
                while (rightPane.Items.Count != 0)
                {
                    for (int i = 0; i < rightPane.Items.Count; i++)
                    {
                        if (!lsubordinate.Contains(rightPane.Items[i]))
                        {
                            lsubordinate.Add(rightPane.Items[i]);
                        }
                    }
                    for (int i = 0; i < lsubordinate.Count; i++)
                    {
                        if (!leftPane.Items.Contains(((ListItem)lsubordinate[i])))
                        {
                            leftPane.Items.Add(((ListItem)lsubordinate[i]));
                        }
                        rightPane.Items.Remove(((ListItem)lsubordinate[i]));
                    }
                }
                _usermenuDAL.DeleteFromT_UserMenuDynamic(UserID, " UserID=" + UserID + " AND Group_ID <> 0");
                //CATEGORY
                //_usermenuDAL.DeleteFromT_UserCategoryDynamic(UserID, " UserID=" + UserID + " AND Group_ID <> 0");
                //Languages
                //_usermenuDAL.DeleteFromT_UserLanguagesDynamic(UserID, " UserID=" + UserID + " AND Group_ID <> 0");
            }
        }
        protected void btAddOne_Click(object sender, EventArgs e)
        {
            if (UserID > 0)
            {
                if (leftPane.SelectedIndex != -1)
                {
                    ListItem selectedItem = leftPane.SelectedItem;
                    selectedItem.Selected = false;
                    leftPane.Items.Remove(selectedItem);
                    //// INSERT
                    //_usermenuDAL.InsertT_UserMenu(UserID, Convert.ToInt32(_dt.Rows[i]["Menu_ID"]), Group_ID);
                    //_usermenuDAL.InsertT_UserCategory(UserID, Convert.ToInt32(_dtCate.Rows[i]["Categorys_ID"]), Group_ID);
                    //_usermenuDAL.InsertT_UserLanguages(UserID, Convert.ToInt32(_dtLang.Rows[i]["Languages_ID"]), Group_ID);
                    ////END
                    rightPane.Items.Add(selectedItem);
                }
            }
            leftPane.UpdateAfterCallBack = true;
            rightPane.UpdateAfterCallBack = true;
        }
        protected void btRemoveOne_Click(object sender, EventArgs e)
        {
            UserDAL _usermenuDAL = new UserDAL(); ;
            if (UserID > 0)
            {
                ArrayList modules = _usermenuDAL.GetAllFrom_T_GroupInUser("RightPane", UserID);
                if (rightPane.SelectedIndex != -1)
                {
                    ListItem selectedItem = rightPane.SelectedItem;
                    //DELETE
                    _usermenuDAL.DeleteFromT_UserMenuDynamic(UserID, " UserID=" + UserID + " AND Group_ID=" + int.Parse(rightPane.SelectedValue));
                    //CATEGORYS
                    //_usermenuDAL.DeleteFromT_UserCategoryDynamic(UserID, " UserID=" + UserID + " AND Group_ID=" + int.Parse(rightPane.SelectedValue));
                    // Languages
                    //_usermenuDAL.DeleteFromT_UserLanguagesDynamic(UserID, " UserID=" + UserID + " AND Group_ID=" + int.Parse(rightPane.SelectedValue));
                    //END
                    selectedItem.Selected = false;
                    rightPane.Items.Remove(selectedItem);
                    leftPane.Items.Add(selectedItem);
                }
            }
            leftPane.UpdateAfterCallBack = true;
            rightPane.UpdateAfterCallBack = true;
        }
        protected void linkGroupExit_Click(object sender, EventArgs e)
        {
            SetAddEdit(true, false, false, false, false,false);
            
        }
        #endregion

        #region Phan Quyen Nhom SAVES
        protected void linkGroupApplly_Click(object sender, EventArgs e)
        {
            if (UserID > 0)
            {
                for (int i = 0; i < rightPane.Items.Count; i++)
                {
                    if (!lsubordinate.Contains(rightPane.Items[i]))
                    {
                        lsubordinate.Add(rightPane.Items[i]);
                    }
                }
                UpdateUser_Group(lsubordinate);
            }
            SetAddEdit(true, false, false, false, false,false);
            
        }
        protected void SaveGroupByMenuID(Int32 UserID, Int32 Group_ID)
        {
            UserDAL _usermenuDAL = new UserDAL(); ;
            DataTable _dt;
            DataTable _dtCate;
            DataTable _dtLang;
            DataTable _dtPaper;
            try
            {
                _dt = _usermenuDAL.GetT_GroupMenuDynamic(Group_ID);
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        Int32 _R_Edit = 0;
                        Int32 _R_Del = 0;
                        Int32 _R_Add = 0;
                        Int32 _R_Pub = 0;
                        if (_dt.Rows[i]["R_Edit"] != DBNull.Value)
                            _R_Edit = Convert.ToInt32(_dt.Rows[i]["R_Edit"]);
                        if (_dt.Rows[i]["R_Del"] != DBNull.Value)
                            _R_Del = Convert.ToInt32(_dt.Rows[i]["R_Del"]);
                        if (_dt.Rows[i]["R_Add"] != DBNull.Value)
                            _R_Add = Convert.ToInt32(_dt.Rows[i]["R_Add"]);
                        if (_dt.Rows[i]["R_Pub"] != DBNull.Value)
                            _R_Pub = Convert.ToInt32(_dt.Rows[i]["R_Pub"]);
                        //_usermenuDAL.DeleteFromT_UserMenuDynamic(UserID, " UserID = " + UserID + " AND Menu_ID=" + Convert.ToInt32(_dt.Rows[i]["Menu_ID"]));
                        _usermenuDAL.DeleteT_UserMenuByMenu(UserID, Convert.ToInt32(_dt.Rows[i]["Menu_ID"]));
                        _usermenuDAL.InsertT_UserMenu(UserID, Convert.ToInt32(_dt.Rows[i]["Menu_ID"]), _R_Edit, _R_Del, _R_Add, _R_Pub, Group_ID);
                    }
                }

                /*
                 
                //INSERT CATEGORYS
                _dtCate = _usermenuDAL.GetT_GroupCategoryDynamic(Group_ID);
                if (_dtCate.Rows.Count > 0)
                {
                    for (int i = 0; i < _dtCate.Rows.Count; i++)
                    {
                        //_usermenuDAL.DeleteFromT_UserCategoryDynamic(UserID, " UserID = " + UserID + " AND Categorys_ID=" + Convert.ToInt32(_dtCate.Rows[i]["Categorys_ID"]) + " AND Group_ID = " + Group_ID + "");
                        //_usermenuDAL.DeleteFromT_UserCategoryDynamic(UserID, " UserID = " + UserID + " AND Categorys_ID=" + Convert.ToInt32(_dtCate.Rows[i]["Categorys_ID"]) + " AND Group_ID <>0 ");
                        _usermenuDAL.InsertT_UserCategory(UserID, Convert.ToInt32(_dtCate.Rows[i]["Categorys_ID"]), Group_ID);
                    }
                }
                //END
                //INSERT LANGUAGES_ID
                _dtLang = _usermenuDAL.GetT_GroupLanguagesDynamic(Group_ID);
                if (_dtLang.Rows.Count > 0)
                {
                    for (int i = 0; i < _dtLang.Rows.Count; i++)
                    {
                        //_usermenuDAL.DeleteFromT_UserLanguagesDynamic(UserID, " UserID = " + UserID + " AND Languages_ID=" + Convert.ToInt32(_dtLang.Rows[i]["Languages_ID"]) + " AND Group_ID = " + Group_ID + "");
                        //_usermenuDAL.DeleteFromT_UserLanguagesDynamic(UserID, " UserID = " + UserID + " AND Languages_ID=" + Convert.ToInt32(_dtLang.Rows[i]["Languages_ID"]) + " AND Group_ID <> 0 ");
                        _usermenuDAL.InsertT_UserLanguages(UserID, Convert.ToInt32(_dtLang.Rows[i]["Languages_ID"]), Group_ID);
                    }
                }
                //END
                //INSERT PAPERS
                _dtPaper = _usermenuDAL.GetT_GroupPapersDynamic(Group_ID);
                if (_dtLang.Rows.Count > 0)
                {
                    for (int i = 0; i < _dtLang.Rows.Count; i++)
                    {
                        _usermenuDAL.InsertT_UserPapers(UserID, Convert.ToInt32(_dtLang.Rows[i]["Paper_ID"]), Group_ID);
                    }
                }
                */
                //END
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void UpdateUser_Group(ArrayList _item)
        {
            UserDAL _usermenuDAL = new UserDAL();
            if (lsubordinate.Count > 0)
            {
                //_usermenuDAL.DeleteFromT_UserMenuDynamic(UserID, " UserID=" + UserID + " AND Group_ID=0");
                for (int i = 0; i < _item.Count; i++)
                {
                    ListItem _it = (ListItem)_item[i];
                    //_usermenuDAL.DeleteFromT_UserMenuDynamic(UserID, " UserID=" + UserID + " AND Group_ID=" + int.Parse(_it.Value));
                    _usermenuDAL.DeleteT_UserMenuByGroup(UserID,int.Parse(_it.Value));

                    //CATEGORYS
                    //_usermenuDAL.DeleteFromT_UserCategoryDynamic(UserID, " UserID=" + UserID + " AND Group_ID=" + int.Parse(_it.Value));
                    //_usermenuDAL.DeleteFromT_UserCategoryDynamic(UserID, " UserID=" + UserID + " AND Group_ID <> 0 ");

                    // Languages
                    //_usermenuDAL.DeleteFromT_UserLanguagesDynamic(UserID, " UserID=" + UserID + " AND Group_ID=" + int.Parse(_it.Value));
                    //_usermenuDAL.DeleteFromT_UserLanguagesDynamic(UserID, " UserID=" + UserID + " AND Group_ID <> 0");

                    // Papers
                    //_usermenuDAL.DeleteFromT_UserPaperDynamic(UserID, " UserID=" + UserID + " AND Group_ID=" + int.Parse(_it.Value));

                    SaveGroupByMenuID(UserID, Convert.ToInt32(_it.Value));
                }
            }
            else
            {   //UserMenu
                //_usermenuDAL.DeleteFromT_UserMenuDynamic(UserID, " UserID=" + UserID + " AND Group_ID <> 0");

                _usermenuDAL.DeleteT_UserMenuByAll(UserID);

                //CATEGORY
                ///_usermenuDAL.DeleteFromT_UserCategoryDynamic(UserID, " UserID=" + UserID + " AND Group_ID <> 0");
                //Languages
                /// _usermenuDAL.DeleteFromT_UserLanguagesDynamic(UserID, " UserID=" + UserID + " AND Group_ID <> 0");
                // Papers
                /// _usermenuDAL.DeleteFromT_UserPaperDynamic(UserID, " UserID=" + UserID + " AND Group_ID <> 0");
            }
        }

        #endregion

    }
}
