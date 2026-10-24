using prjInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;

namespace prjApplication.FinishFlights
{
    public partial class NotCompletedFlights : PageBaseCallBack
    {
        public string _phanCach = "::::";
        public string _IdSelect = "0";
        private string _AliasSession = "NotCompletedFlights";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //var ax = new CantRealizeDAL().GetOne("3").Rows[0]["CONTENT"].ToString();
                LoadDataGrid(new string[] { txtStartDate.Value, txtFinishDate.Value });
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadDataGrid(new string[] { txtStartDate.Value, txtFinishDate.Value });
        }
        public override string GetCallbackResult()
        {
            if (_EventArgument == "") return "";
            string kq = "";
            string[] _arg = _EventArgument.Split(new string[] { "_____" }, StringSplitOptions.RemoveEmptyEntries);
            string[] ThamSo = _arg[0].Split(new string[] { _phanCach }, StringSplitOptions.RemoveEmptyEntries);
            switch (_arg[1])
            {
                case "LoadDataGrid":
                    kq = LoadDataGrid(ThamSo);
                    break;
                    //case "GetContent":
                    //    kq = GetContent(_arg[0]);
                    //    break;
            }
            return kq;
        }
        private string LoadDataGrid(string[] thamso)
        {
            List<prjInfo.NotCompletedFlights> t = new NotCompletedFlightsDAL().GetPage(CustomPaging1.PageSize, CustomPaging1.PageIndex, CustomPaging1.ValueSearch, DateTimeHelper.ConvertToDateTime(thamso[0]), DateTimeHelper.ConvertToDateTime(thamso[1]));
            CustomPaging1.TotalsRecord = t.GetTotalRecord<prjInfo.NotCompletedFlights>();
            grdSource.DataSource = t;
            grdSource.DataBind();
            return this.RenderToHTML(grdSource);
        }
        //private string GetContent(string thamso)
        //{
        //    return new NotCompletedFlightsDAL().GetOne(thamso).Rows[0]["CONTENT"].ToString();
        //}
        public override void VerifyRenderingInServerForm(Control control)
        {

        }
    }
}