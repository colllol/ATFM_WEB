using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic.Cant;
using prjInfo;
using prjComponents;
using System.IO;
using System.Data;
using TuesPechkin;
using prjBusinessLogic;

namespace prjApplication.Errors
{
    public partial class ListErrors : PageBaseCallBack
    {
        private const string _AliasSession = "Errors";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        #region function
        public void LoadData()
        {
            List<prjInfo.Errors> t = new ErrorsDAL().GetPage(PhanTrang1.PageSize, PhanTrang1.PageIndex, GetWhereCondition());
            if (t.Count != 0)
            PhanTrang1.TotalRecord = t.GetTotalRecord<prjInfo.Errors>();
            grdSource.DataSource = t;
            grdSource.DataBind();
        }
        private void PutclsSearchDetail()
        {
            List<clsSearchDetail> lisSearch = new List<clsSearchDetail>();
            lisSearch.Add(new clsSearchDetail("NBR", "txtSearch_UserName", txtSearch_UserName.Text.Trim()));

            Session.SetValueSearch(_AliasSession, lisSearch);
        }
        #endregion

        protected void linkSearch_Click(object sender, EventArgs e)
        {
            //string where = GetWhereCondition();

            PutclsSearchDetail();
            //Session.SetValueSearch(_AliasSession, where);
            //PhanTrang1.ValueSearch = where;
            LoadData();
        }

        public string GetWhereCondition()
        {
            string where = " 1=1";
            if (!String.IsNullOrEmpty(txtSearch_UserName.Text.Trim()))
                where += " AND " + string.Format(" NBR like N'%{0}%'", UltilFunc.SqlFormatText(this.txtSearch_UserName.Text.Trim()));
            return where;
        }
        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        protected void PhanTrang1_Paging_IndexChange(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}