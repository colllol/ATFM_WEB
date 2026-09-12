using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjInfo;
using prjBusinessLogic;
namespace prjApplication.CalendarFlight
{
    public partial class CalendarDayFlightDeleted : PageBaseCallBack
    {
        public string _phanCach = "::::";
        public string _phanCachArg = "_____";
        public string _IdSelect = "0";
        private string _AliasSession = "CalendarDayFlightDeleted";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }
        protected void grdSource_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex != -1 && e.Row.RowType != DataControlRowType.Header)
            {
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                GridView grd = ((GridView)sender);
                gvRow.CssClass = "detail-row";
                TableCell tCell = new TableCell();
                tCell.CssClass = "text-left";
                tCell.ColumnSpan = ((GridView)sender).Columns.Count;
                string id = grd.DataKeys[e.Row.RowIndex].Value.ToString();
                tCell.Text = rListHistoryFlightDetails(new DayFlightsDAL().GetHistoryById(id), id);
                if (tCell.Text == "")
                    return;
                gvRow.Cells.Add(tCell);
                Table tbl = e.Row.Parent as Table;
                tbl.Rows.Add(gvRow);
            }
        }
        public override string GetCallbackResult()
        {
            if (_EventArgument == "") return "";
            string kq = "";
            string[] _arg = _EventArgument.Split(new string[] { _phanCachArg }, StringSplitOptions.RemoveEmptyEntries);
            string[] ThamSo = _arg[0].Split(new string[] { _phanCach }, StringSplitOptions.RemoveEmptyEntries);
            switch (_arg[1])
            {
                case "RestoreHistory":
                    kq = RestoreHistory(ThamSo);
                    break;
                case "LoadDataGrid":
                    kq = LoadDataGrid();
                    break;
            }
            return kq;
        }
        private string RestoreHistory(string[] thamso)
        {
            bool kq = new DayFlightsDAL().RestoreRecode(thamso[0], thamso[1], thamso[2]);
            string ax = kq.ToString() == true.ToString() ? "Restore sussess" : "Restore error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[DayFlightsDAL]", 0, $"[RestoreRecode] [{ax}]", 0.0);
            return kq ? "OK" : "NOK";
        }
        private void LoadData()
        {
            grdHis.DataSource = new DayFlightsDAL().GetTableDelete(" 1=1");
            grdHis.DataBind();
        }
        private string LoadDataGrid()
        {
            grdHis.DataSource = new DayFlightsDAL().GetTableDelete(" 1=1");
            grdHis.DataBind();
            return this.RenderToHTML(grdHis);
        }
    }
}