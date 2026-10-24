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
using System.Text;

namespace prjApplication.Until
{
    public partial class InsertNewsRelations : System.Web.UI.Page
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
            if (cboNgonNgu.Items.Count >= 3)
                cboNgonNgu.SelectedIndex = prjComponents.Global.DefaultLangID;
            else
                cboNgonNgu.SelectedIndex = UltilFunc.GetIndexControl(cboNgonNgu, prjComponents.Global.DefaultCombobox);
            if (cboNgonNgu.SelectedIndex != 0)
                UltilFunc.BindCombox(cbo_chuyenmuc, "Categorys_ID", "Category_Name", "T_Categorys", string.Format(" IsCateOrSubject=1  AND Languages_ID= " + this.cboNgonNgu.SelectedValue + " AND Categorys_ID IN ({0})", UltilFunc.GetCategory4User(_user.UserID)), "---Tất cả---", "Category_ParrentID", " Order by Category_Order ASC");
        }
        private string GetOrderString()
        {
            if ((ViewState["OrderString"] != null) && (ViewState["OrderString"].ToString() != ""))
            {
                return ViewState["OrderString"].ToString();
            }
            else return " News_DateEdit DESC";
        }
        string BuildSQL(int status, string sOrder)
        {
            string sql = "";
            string sClause = " 1=1 and News_Status=" + status + "";
            string sWhere = "";

            if (txtTenbai.Text.Length > 0)
            {
                if (sWhere.Trim() != "") sWhere += " AND ";
                sWhere += " News_Tittle LIKE " + string.Format("N'%{0}%'", UltilFunc.SqlFormatText(txtTenbai.Text.Trim()));               
            }
            if (cbo_chuyenmuc.SelectedIndex > 0)
            {
                if (sWhere.Trim() != "") sWhere += " AND ";
                sWhere += string.Format(" CAT_ID IN (SELECT * FROM [fn_Return_Category_Tree] ({0}))", this.cbo_chuyenmuc.SelectedValue);
            }
            if (cboNgonNgu.SelectedIndex > 0)
            {
                if (sWhere.Trim() != "") sWhere += " AND ";
                sWhere += "  Lang_ID=" + cboNgonNgu.SelectedValue.ToString();
            }
            if (txt_tungay.Text.Length > 0)
            {
                if (sWhere.Trim() != "") sWhere += " AND ";
                sWhere += "  News_DatePublished >= convert(datetime,'" + txt_tungay.Text.Trim() + " 00:00:00',103) ";
            }
            if (txt_denngay.Text.Length > 0)
            {
                if (sWhere.Trim() != "") sWhere += " AND ";
                sWhere += "  News_DatePublished <= convert(datetime,'" + txt_denngay.Text.Trim() + " 23:59:59',103) ";
            }
            if (chkNewsIsBaidinh.Checked)
            {
                if (sWhere.Trim() != "") sWhere += " AND ";
                sWhere += "  News_IsHomePages=1 ";
            }
            if (chkNewsIsFocus.Checked)
            {
                if (sWhere.Trim() != "") sWhere += " AND ";
                sWhere += "  News_IsHot=1 ";
            }

            if (chkNewTieudiem.Checked)
            {
                if (sWhere.Trim() != "") sWhere += " AND ";
                sWhere += "  News_IsFocus=1 ";
            }
            if (chkNewFocusParent.Checked)
            {
                if (sWhere.Trim() != "") sWhere += " AND ";
                sWhere += "  News_IsCategoryParrent=1 ";
            }
            if (chkNewFocusChild.Checked)
            {
                if (sWhere.Trim() != "") sWhere += " AND ";
                sWhere += "  News_IsCategorys=1 ";
            }

            if (chkImageIsFocus.Checked)
            {
                if (sWhere.Trim() != "") sWhere += " AND ";
                sWhere += "  News_IsImages=1 ";
            }
            if (chkVideoIsFocus.Checked)
            {
                if (sWhere.Trim() != "") sWhere += " AND ";
                sWhere += "  News_IsVideo=1 ";
            }
            if (chkHosoIsFocus.Checked)
            {
                if (sWhere.Trim() != "") sWhere += " AND ";
                sWhere += "  News_IsHistory=1 ";

            }
            if (chDisplayMobi.Checked)
            {
                if (sWhere.Trim() != "") sWhere += " AND ";
                sWhere += "  News_DisplayMobile=1 ";

            }
            sql += sClause;
            if (sWhere.Trim().Length > 0)
                sql += " AND" + sWhere;
            return sql + sOrder;
        }
        private void LoadData_DangXuly()
        {
            //string sOrder = GetOrderString() == "" ? "" : " ORDER BY " + GetOrderString();
            //pages.PageSize = Global.MPerPageLienQuan;
            //prjBusinessLogic.DAL.T_NewsDAL _T_newsDAL = new prjBusinessLogic.DAL.T_NewsDAL();
            //string tieude = txt_tieude.Text.Trim();
            //Session["searchvalueroot"] = null;
            //if (tieude != "")
            //{
            //    Session["searchvalueroot"] = UltilFunc.SplitString(tieude);
            //}
            //DataSet _ds;
            //_ds = _T_newsDAL.BindGridT_NewsDynamic(pages.PageIndex, pages.PageSize, BuildSQL(6, sOrder), UltilFunc.ReplaceAll(UltilFunc.SplitString(tieude), "'", "’"));
            
            //int TotalRecords = Convert.ToInt32(_ds.Tables[1].Rows[0].ItemArray[0].ToString());
            //int TotalRecord = Convert.ToInt32(_ds.Tables[0].Rows.Count);
            //if (TotalRecord == 0)
            //    _ds = _T_newsDAL.BindGridT_NewsDynamic(pages.PageIndex, pages.PageSize, BuildSQL(6, sOrder), UltilFunc.ReplaceAll(UltilFunc.SplitString(tieude), "'", "’"));
            
            //dgr_tintuc1.DataSource = _ds;
            //dgr_tintuc1.DataBind();
            //pages.TotalRecords = CurrentPage2.TotalRecords = TotalRecords;
            //CurrentPage2.TotalPages = pages.CalculateTotalPages();
            //CurrentPage2.PageIndex = pages.PageIndex;
        }
        private void HuybaiDaDXB(DataGrid dgr)
        {

        }

        #region Event Click
        protected void dgr_tintuc1_EditCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandArgument.ToString().ToLower() == "downloadalias")
            {
                int _ID = Convert.ToInt32(this.dgr_tintuc1.DataKeys[e.Item.ItemIndex].ToString());
            }
        }
        protected void dgr_tintuc1_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            e.Item.Attributes.Add("onmouseover", "currColor=this.style.backgroundColor;this.style.backgroundColor='" + CommonLib.HPCOnmouseoverGrid() + "'");
            e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currColor");
        }
        protected void cmdSeek_Click(object sender, EventArgs e)
        {
            pages.PageIndex = 0;
            this.LoadData_DangXuly();
        }
        public void pages_IndexChanged_baidangxuly(object sender, EventArgs e)
        {
            LoadData_DangXuly();
        }
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            HuybaiDaDXB(dgr_tintuc1);
        }
        protected void HuyDXB_Click(object sender, EventArgs e)
        {
            HuybaiDaDXB(dgr_tintuc1);
        }
        protected void cbo_lanquage_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbo_chuyenmuc.Items.Clear();
            if (cboNgonNgu.SelectedIndex > 0)
            {
                UltilFunc.BindCombox(cbo_chuyenmuc, "Categorys_ID", "Category_Name", "T_Categorys", string.Format(" IsCateOrSubject=1  AND Languages_ID= " + this.cboNgonNgu.SelectedValue + " AND Categorys_ID IN ({0})", UltilFunc.GetCategory4User(_user.UserID)), "---Tất cả---", "Category_ParrentID", " Order by Category_Order ASC");
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
        #region Bôi màu
        public string paintColorSearch(Object strSearchword)
        {
            string txtFindSearchWord = Server.HtmlDecode(Convert.ToString(strSearchword).Trim());
            Regex v_reg_regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string v_str_FormD = txtFindSearchWord.Normalize(NormalizationForm.FormD);
            v_str_FormD = v_reg_regex.Replace(v_str_FormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
            string[] arrayWordSearch = arrSearchWord(Convert.ToString(Session["searchvalueroot"]));
            for (int x = 0; x < arrayWordSearch.Length; x++)
            {
                if (arrayWordSearch[x].ToString().Trim() != ""
                    && arrayWordSearch[x].ToString().Trim().ToLower() != "or"
                    && arrayWordSearch[x].ToString().Trim().ToLower() != "and"
                    && arrayWordSearch[x].ToString().Trim().ToLower() != " or"
                    && arrayWordSearch[x].ToString().Trim().ToLower() != " and")
                {
                    txtFindSearchWord = boimau(txtFindSearchWord, arrayWordSearch[x].ToString().Trim());
                }
            }
            return FunctionReplace(txtFindSearchWord);
        }

        public string boimau(string _root, string _strsearch)
        {
            string _strreturn = _root, _search = _strsearch;
            Regex v_reg_regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            _search = _search.Normalize(NormalizationForm.FormD);
            _search = v_reg_regex.Replace(_search, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
            int check = 0, subindex = 0;
            if (_root.Length >= _strsearch.Length)
            {
                for (int i = 0; i < _root.Length; i++)
                {
                    string _strcheck = "";
                    if (_root.Length - i >= 1)
                    {
                        if (_root.Length - i >= 1)
                            _strcheck = _root.Substring(i, 1);
                        if (_strcheck == "<")
                            check = 1;
                        else
                        {
                            if (check == 1)
                            {
                                string _strcheck2 = "";
                                if (_root.Length - i >= 1)
                                    _strcheck2 = _root.Substring(i, 1);
                                if (_strcheck2 == ">")
                                    check = 0;
                            }
                        }
                    }
                    if ((_root.Length - i) >= _search.Length && check == 0)
                    {
                        string _strcomp = "";

                        if (i > 0)
                        {
                            _strcomp = _root.Substring(i, _search.Length);
                        }
                        else if (i == 0)
                        {
                            if ((_root.Length - (i + _search.Length)) >= 1)
                                _strcomp = _root.Substring(i, _search.Length + 1);
                            else if ((_root.Length - (i + _search.Length)) == 0)
                                _strcomp = _root.Substring(i, _search.Length);
                        }
                        _strcomp = _strcomp.Normalize(NormalizationForm.FormD);
                        _strcomp = v_reg_regex.Replace(_strcomp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
                        if (_strcomp.ToLower().Trim() == _search.ToLower())
                        {
                            int tes = _strreturn.Length;
                            _strreturn = _strreturn.Substring(0, i + subindex)
                                + "<span style=\"background-color: #FFFF00\">" + _strreturn.Substring(i + subindex, _search.Length) + "</span>"
                                + _strreturn.Substring(i + subindex + _search.Length, _strreturn.Length - (i + subindex + _search.Length));//47
                            subindex = subindex + 47;
                        }
                    }
                }
            }
            return _strreturn;

        }

        public string FunctionReplace(string wordSearch)
        {
            string word_search = Regex.Replace(wordSearch, "<sp<span style=\"background-color: #FFFF00\">an</span>", "<span>", System.Text.RegularExpressions.RegexOptions.None);
            return Regex.Replace(word_search, "</sp<span style=\"background-color: #FFFF00\">an</span>", "<span>", System.Text.RegularExpressions.RegexOptions.None); ;
        }

        public string[] arrSearchWord(string strString)
        {
            string[] arInfo;
            char[] splitter = { '"' };
            arInfo = strString.Split(splitter);
            return arInfo;
        }
        #endregion
    }
}
