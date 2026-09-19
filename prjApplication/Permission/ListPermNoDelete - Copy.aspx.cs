using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjInfo;
using prjBusinessLogic;
using System.Data;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;

namespace prjApplication.Permission
{
    public partial class ListPermNoDelete : PageBaseCallBack
    {
        public string _phanCach = "::::";
        public string _phanCachArg = "_____";
        public string _IdSelect = "0";
        private string _AliasSession = "ListPermissionNo";
        public string _ObjRender
        {
            get
            {
                PermMasterNo obj = new PermMasterNo();

                var ser = new JavaScriptSerializer();
                ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
                return ser.Serialize(obj);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
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

            }
            return kq;
        }
        void LoadData()
        {
            grdHis.DataSource = new PermMasterNoDAL().GetTableDelete("");
            grdHis.DataBind();
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
                tCell.Text = rListHistoryFlightDetails(new PermMasterNoDAL().GetHistoryById(id), id);
                if (tCell.Text == "")
                    return;
                gvRow.Cells.Add(tCell);
                Table tbl = e.Row.Parent as Table;
                tbl.Rows.Add(gvRow);
            }
        }
        private string RestoreHistory(string[] thamso)
        {
            return new PermMasterNoDAL().RestoreRecode(thamso[0], thamso[1], thamso[2]) ? "OK" : "NOK";
        }
    }
}