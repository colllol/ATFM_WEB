using prjInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using System.Text;
using System.IO;

namespace prjApplication.FlightManagement
{
    public partial class FlightDeleted : PageBaseCallBack
    {
        public string _phanCach = "::::";
        public string _IdSelect = "0";
        private string _AliasSession = "FlightDetail";
        public string _ObjRender
        {
            get
            {
                FlightPermissionDetail obj = new FlightPermissionDetail();
                var ser = new JavaScriptSerializer();
                ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
                return ser.Serialize(obj);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region call back
        public override string GetCallbackResult()
        {
            if (_EventArgument == "") return "";
            string kq = "";
            string[] _arg = _EventArgument.Split(new string[] { "_____" }, StringSplitOptions.RemoveEmptyEntries);
            string[] ThamSo = _arg[0].Split(new string[] { _phanCach }, StringSplitOptions.RemoveEmptyEntries);
            switch (_arg[1])
            {
                case "btnRestoreOnclick":
                    kq = btnRestoreOnclick(_arg[0]);
                    break;
                case "LoadDataGrid":
                    kq = LoadDataGrid(_arg[0]);
                    break;
            }

            return kq;
        }
        #endregion
        #region function
        private string btnRestoreOnclick(string thamso)
        {
            return "";
        }
        private string LoadDataGrid(string ThamSo)
        {
            List<FlightPermissionDetail> t = new FlightPermissionDetailDAL().GetPageObject(CustomPaging1.PageSize, CustomPaging1.PageIndex, CustomPaging1.ValueSearch);
            CustomPaging1.TotalsRecord = t.GetTotalRecord<FlightPermissionDetail>();
            grdSource.DataSource = t;
            grdSource.DataBind();
            StringBuilder sb = new StringBuilder();
            StringWriter tw = new StringWriter(sb);
            HtmlTextWriter hw = new HtmlTextWriter(tw);
            grdSource.RenderControl(hw);
            return sb.ToString();
        }
        #endregion

    }
}