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

namespace prjApplication.Flight_Fixes
{
    public partial class ListFlight_Fixes : System.Web.UI.Page
    {
        private const string _AliasSession = "FLIGHTFIXES";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    CustomPaging1.ValueSearch = Session[_AliasSession].ToString();

                    Session.SetValueForControlSearch(this, _AliasSession);
                }
                catch { Session.SetValueSearch(_AliasSession, " 1=1 "); }
                LoadData();

            }
        }

        #region FUNCTION
        public void LoadData()
        {
            List<M_FLIGHT_FIXES> t = new FLight_FixesDAL().GetListPage(CustomPaging1.PageSize, CustomPaging1.PageIndex, CustomPaging1.ValueSearch);
            CustomPaging1.TotalsRecord = t.GetTotalRecord<M_FLIGHT_FIXES>();
            grdSource.DataSource = t;
            grdSource.DataBind();

            lit.Text="Tổng số : " + t.GetTotalRecord<M_FLIGHT_FIXES>();

        }
        private void PutclsSearchDetail()
        {
            List<clsSearchDetail> lisSearch = new List<clsSearchDetail>();
            lisSearch.Add(new clsSearchDetail("FIXNAME", "txtSearch_UserName", txtSearch_UserName.Text.Trim()));
            lisSearch.Add(new clsSearchDetail("CURRENTSECTOR", "txtSearch_Sector", txtSearch_Sector.Text.Trim()));
            Session.SetValueSearch(_AliasSession, lisSearch);
        }

        #endregion

        #region grd event

        protected void grdSource_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //int id = Convert.ToInt32(e.CommandArgument);

            //switch (e.CommandName)
            //{
            //    case "DeleteByID":
            //        var kq = new AtsDAL().DeleteAtsRoute(id);
            //        if (kq) this.AlertMessage("Delete sussess!");
            //        else this.AlertMessage("Delete error!");
            //        LoadData();
            //        break;
            //}
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
            string where = " 1=1 ";
            if (!String.IsNullOrEmpty(txtSearch_UserName.Text.Trim()))
            {
                where += " AND " + string.Format(" UPPER(FIXNAME) like N'%{0}%'", UltilFunc.SqlFormatText(this.txtSearch_UserName.Text.Trim().ToUpper()));
            }
            if (!String.IsNullOrEmpty(txtFixkind.Text.Trim()))
            {
                where += " AND " + string.Format(" UPPER(FIXKIND) like N'%{0}%'", UltilFunc.SqlFormatText(this.txtFixkind.Text.Trim().ToUpper()));
            }
            if (!String.IsNullOrEmpty(txtSearch_Sector.Text.Trim()))
            {
                where += " AND " + string.Format(" UPPER(CURRENTSECTOR) like N'%{0}%'", UltilFunc.SqlFormatText(this.txtSearch_Sector.Text.Trim().ToUpper()));
            }
            if (txtFromDate.Value.Trim()!= "__/__/____")
            {
                where += " AND  to_date(substr(TIMESTAMP,0,10) || ' ' || substr(TIMESTAMP,11,6) || ':00','mm/dd/yyyy hh24:mi:ss')  BETWEEN to_date('" + txtFromDate.Value + " " + txtFromTime.Value + ":00', 'mm/dd/yyyy hh24:mi:ss') and to_date('" + txtFromDate.Value + " " + txtToTime.Value + ":00', 'mm/dd/yyyy hh24:mi:ss')";
            }

                

            return HttpUtility.UrlEncode(where.ToUpper());
        }
    }
}