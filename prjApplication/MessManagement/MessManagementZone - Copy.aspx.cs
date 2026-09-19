using prjInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using System.Data;
using System.IO;
namespace prjApplication.MessManagement
{
    public partial class MessManagementZone : PageBaseCallBack
    {
        protected string _phanCach = "::::";
        protected string _phanCachArg = "_____";
        public string _IdSelect = "0";
        private string _AliasSession = "CreateMess";
        private bool IsSearch = false;
        //public string _ObjSearch
        //{
        //    get
        //    {
        //        QlbInBox obj = new QlbInBox();

        //        var ser = new JavaScriptSerializer();
        //        ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
        //        return ser.Serialize(obj);
        //    }
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                txtBEGINDATE.Value = DateTime.Now.ToString("dd/MM/yyyy");
                LoadData(txtBEGINDATE.Value);
            }
        }
        protected void btnSelect_Click(object sender, EventArgs e)
        {
            LoadData(txtBEGINDATE.Value);
        }
        void LoadData(string _date)
        {            
            DataTable dt = new DayFlightsDAL().GetTableWithPartNoZone(_date);
            
            if (dt.Rows.Count > 0)
            {

                grdListPartMessage.DataSource = dt;
                grdListPartMessage.DataBind();
            }
            else
            {
                grdListPartMessage.DataSource = null;
                grdListPartMessage.DataBind();
            }
        }
        protected string LoadDataDetail(string _partNo,string messType)
        {
            DataTable dt = new DayFlightsDAL().GetTableWithPartNoIDZone(txtBEGINDATE.Value, _partNo, messType);
            string kq="";
            if (dt.Rows.Count > 0)
            {
                kq = dt.Rows[0]["CONTENT"].ToString();
            }
            return kq;
        }
        protected string LoadAddreeeDetail(string _No)
        {
            DataTable dt = new DayFlightsDAL().GetTableGroupAddressWithPartNoID(_No);
            string kq = "";
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (kq == "")
                    {
                        kq = row["Address"].ToString();
                    }
                    else
                    {
                        kq += "," + row["Address"].ToString();
                    }
                }

            }
            return kq;
        }
        public override string GetCallbackResult()
        {
            if (_EventArgument == "") return "";
            string kq = "";
            string[] _arg = _EventArgument.Split(new string[] { _phanCachArg }, StringSplitOptions.RemoveEmptyEntries);
            string[] ThamSo = _arg[0].Split(new string[] { _phanCach }, StringSplitOptions.RemoveEmptyEntries);
            switch (_arg[1])
            {
                case "ViewContent":
                    kq = LoadDataDetail(ThamSo[0], ThamSo[1]);
                    break;
                case "btnGetAddredd":
                    kq = LoadAddreeeDetail(ThamSo[0]);
                    break;
                case "btnSend_Onclick":
                    kq = btnSend_Onclick(ThamSo[0], ThamSo[1], ThamSo[2]);
                    break;
            }
            return kq;
        }

        private string btnSend_Onclick(string partNo, string messType, string thamso)
        {
            if (partNo == " ")
            {
                return "Not Content!";
            }
            else
            {
                bool a1x = new QlbOutBoxDAL().UpdateStatusNew(txtBEGINDATE.Value, partNo, messType);

                var dateTimeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject(thamso);
                bool ax = new QlbOutBoxDAL().Insert((object)obj);
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[QlbInBoxDAL]", 0, $"[Insert][{ax.ToString()}]", 0);
                return ax ? "Sussess!" : "Error!";
            }
        }
    }
}