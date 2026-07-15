using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjInfo;
using prjComponents;
using System.IO;
using TuesPechkin;

namespace prjApplication.ATS
{
    public partial class ListAtsWay : PageCoreAdmin
    {
        private const string _AliasSession = "ListAtsFir";
        protected void Page_Load(object sender, EventArgs e)
        {
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

        #region FUNCTION
        public void LoadData()
        {
            List<ATSWAYPOINT> t = new AtsDAL().GetListPageWay(CustomPaging1.PageSize, CustomPaging1.PageIndex, CustomPaging1.ValueSearch);
            CustomPaging1.TotalsRecord = t.GetTotalRecord<ATSWAYPOINT>();
            grdSource.DataSource = t;
            grdSource.DataBind();

        }
        private void PutclsSearchDetail()
        {
            List<clsSearchDetail> lisSearch = new List<clsSearchDetail>();
            lisSearch.Add(new clsSearchDetail("IDENTIFIER", "txtSearch_UserName", txtSearch_UserName.Text.Trim()));
            Session.SetValueSearch(_AliasSession, lisSearch);
        }

        #endregion

        #region grd event

        protected void grdSource_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            switch (e.CommandName)
            {
                case "DeleteByID":
                    var kq = new AtsDAL().DeleteAtsWay(id);
                    if (kq) this.AlertMessage("Delete sussess!");
                    else this.AlertMessage("Delete error!");
                    LoadData();
                    break;
            }
        }
        protected void grdSource_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.TableSection = TableRowSection.TableHeader;
        }
        #endregion

        protected void linkSearch_Click(object sender, EventArgs e)
        {
            string where = GetWhereCondition();
            PutclsSearchDetail();
            Session[_AliasSession] = where;
            CustomPaging1.ValueSearch = where;
            LoadData();
        }
        public string GetWhereCondition()
        {
            string where = " 1=1";
            if (!String.IsNullOrEmpty(txtSearch_UserName.Text.Trim()))
            {
                where += " AND " + string.Format(" UPPER(IDENTIFIER) like N'%{0}%'", UltilFunc.SqlFormatText(this.txtSearch_UserName.Text.Trim().ToUpper()));
            }

            return HttpUtility.UrlEncode(where.ToUpper());
        }
    }
}