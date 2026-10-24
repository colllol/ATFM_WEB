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
using HPCServerDataAccess;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

namespace prjApplication.ckeditor.plugins.InsertContents
{
    public partial class InsertContents : System.Web.UI.Page
    {
        prjBusinessLogic.UserDAL _userDAL = new UserDAL();
        prjInfo.T_Users _user = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            _user = _userDAL.GetUserByUserName(HPCSecurity.CurrentUser.Identity.Name);
            if (_user != null)
                if (!IsPostBack)
                {
                    LoadCombox();
                    this.LoadData_DangXuly();
                }
        }
        public void LoadCombox()
        {
            UltilFunc.BindCombox(cboNgonNgu, "Languages_ID", "Languages_Name", "T_Languages", " 1=1 Order by Languages_Name ", "---Tất cả---");
            if (cboNgonNgu.Items.Count == 2)
                cboNgonNgu.SelectedIndex = 1;
            else
                cboNgonNgu.SelectedIndex = UltilFunc.GetIndexControl(cboNgonNgu, prjComponents.Global.DefaultCombobox);
            if (cboNgonNgu.SelectedIndex != 0)
                UltilFunc.BindCombox(cbo_chuyenmuc, "Categorys_ID", "Category_Name", "T_Categorys", " 1=1 AND KeywordSubject =0 and IsNew =1 AND Languages_ID = " + cboNgonNgu.SelectedValue.ToString() + " ", "---Tất cả---", "Category_ParrentID", " Order by Category_Order ASC");
        }
        private string GetOrderString()
        {
            if ((ViewState["OrderString"] != null) && (ViewState["OrderString"].ToString() != ""))
            {
                return ViewState["OrderString"].ToString();
            }
            else return " News_DateEdit DESC";
        }
        private string BuildSQL(int status, string sOrder)
        {
            string sql = "";
            string sClause = " 1=1 and News_Status=" + status + "";//"AND Lang_ID IN (SELECT T_UserLanguages.Languages_ID FROM T_UserLanguages WHERE T_UserLanguages.[UserID] = " + _user.UserID + ") and News_Status=" + status + " and CAT_ID in (select Categorys_ID from T_UserCategory where UserID = " + _userDAL.GetUserByUserName(HPCSecurity.CurrentUser.Identity.Name).UserID + ") ";
            string sWhere = "";
            if (this.txt_tieude.Text.Length > 0)
            {
                if (sWhere.Trim() != "") sWhere += " AND ";
                sWhere += " News_Tittle LIKE " + string.Format("N'%{0}%'", UltilFunc.SqlFormatText(txt_tieude.Text.Trim()));
            }
            if (this.cboNgonNgu.SelectedIndex > 0)
            {
                if (sWhere.Trim() != "") sWhere += " AND ";
                sWhere += "  Lang_ID=" + cboNgonNgu.SelectedValue.ToString();
            }
            if (this.cbo_chuyenmuc.SelectedIndex > 0)
            {
                if (sWhere.Trim() != "") sWhere += " AND ";
                sWhere += "" + string.Format(" CAT_ID IN (SELECT * FROM [fn_Return_Category_Tree] ({0}))", this.cbo_chuyenmuc.SelectedValue);
            }
            sql += sClause;
            if (sWhere.Trim().Length > 0)
                sql += " AND" + sWhere;
            return sql + sOrder;
        }
        private void LoadData_DangXuly()
        {
            //string sOrder = GetOrderString() == "" ? "" : " ORDER BY " + GetOrderString();
            //this.pages.PageSize = Global.MembersPerPage;
            //prjBusinessLogic.DAL.T_NewsDAL _T_newsDAL = new prjBusinessLogic.DAL.T_NewsDAL();
            //DataSet _ds;
            //_ds = _T_newsDAL.BindGridT_NewsEditor(pages.PageIndex, pages.PageSize, BuildSQL(6, sOrder));
            //int TotalRecords = Convert.ToInt32(_ds.Tables[1].Rows[0].ItemArray[0].ToString());
            //int TotalRecord = Convert.ToInt32(_ds.Tables[0].Rows.Count);
            //if (TotalRecord == 0)
            //    _ds = _T_newsDAL.BindGridT_NewsEditor(pages.PageIndex - 1, pages.PageSize, BuildSQL(6, sOrder));
            //this.dgr_tintuc1.DataSource = _ds;
            //this.dgr_tintuc1.DataBind(); _ds.Clear();
            //this.pages.TotalRecords = CurrentPage2.TotalRecords = TotalRecords;
            //this.CurrentPage2.TotalPages = pages.CalculateTotalPages();
            //this.CurrentPage2.PageIndex = pages.PageIndex;

        }
        

        #region Event Click
        //protected void dgr_tintuc1_ItemDataBound(object sender, DataGridItemEventArgs e)
        //{
        //    e.Item.Attributes.Add("onmouseover", "currColor=this.style.backgroundColor;this.style.backgroundColor='" + CommonLib.HPCOnmouseoverGrid() + "'");
        //    e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currColor");
        //}
        protected void cmdSeek_Click(object sender, EventArgs e)
        {
            pages.PageIndex = 0;
            this.LoadData_DangXuly();
        }
        public void pages_IndexChanged(object sender, EventArgs e)
        {
            LoadData_DangXuly();
        }
        protected void cbo_lanquage_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbo_chuyenmuc.Items.Clear();
            if (cboNgonNgu.SelectedIndex > 0)
            {
                UltilFunc.BindCombox(cbo_chuyenmuc, "Categorys_ID", "Category_Name", "T_Categorys", "1=1 AND KeywordSubject =0 and IsNew =1 AND Languages_ID = " + cboNgonNgu.SelectedValue + " ", "---Tất cả---", "Category_ParrentID", " Order by Category_Order ASC");
                cbo_chuyenmuc.UpdateAfterCallBack = true;
            }
            else
            {
                this.cbo_chuyenmuc.DataSource = null;
                this.cbo_chuyenmuc.DataBind();
                cbo_chuyenmuc.UpdateAfterCallBack = true;
            }
        }
        #endregion
    }
}
