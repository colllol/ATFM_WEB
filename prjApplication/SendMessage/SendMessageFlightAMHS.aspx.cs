using Newtonsoft.Json.Converters;
using prjBusinessLogic;
using prjInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace prjApplication.SendMessage
{
    public partial class SendMessageFlightAMHS : PageBaseCallBack
    {
        protected string _phanCach = "::::";
        protected string _phanCachArg = "_____";
        public string _ObjSearch
        {
            get
            {
                QlbOutBox obj = new QlbOutBox();

                var ser = new JavaScriptSerializer();
                ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
                return ser.Serialize(obj);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["ID"] != null)
                {
                    string content = GetContentByKey(Request.QueryString["ID"].ToString());
                    string[] _arg = content.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    content = content.Replace(_arg[0], " ");
                    content = content.Replace(_arg[1], " ");
                    divcontent.InnerHtml = $"{content}";
                    //this.ExcuteJavascript($"<script>$('#txtContent').val('{content}');</script>");
                }
                else
                    divcontent.InnerHtml = $"NNNN";
            }
        }
        private string GetContentByKey(string valeu)
        {
            // DateTime date = DateTime.Parse(valeu);
            DataTable _dt;
            string _return = "";
            _dt = new clsResuftAPI().GetTableApiExtension("MESSAGE_PKG", "GetInboxDetailBy", new { P_ID = valeu });
            if (_dt.Rows.Count > 0)
            {
                _return = _dt.Rows[0]["CONTENT"].ToString();//.Replace("\n", "\\n");
            }
            return _return;
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

                case "btnGetAddredd":
                    kq = LoadAddreeeDetail(ThamSo[0]);
                    break;
                case "btnSend_Onclick":
                    kq = btnSend_Onclick(ThamSo[0], ThamSo[1], ThamSo[2]);
                    break;
                case "checkMessageICAO_Click":
                    kq = checkMessageICAO_Click(ThamSo[0]);
                    break;
            }
            return kq;
        }
        private string btnSend_Onclick(string toOrigin, string toAdd, string content)
        {
            //var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            //dynamic obj = JsonConvert.DeserializeObject(thamso);
            //bool ax = new QlbOutBoxDAL().Insert((object)obj);
            //WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[QlbOutBoxDAL]", 0, $"[Insert][{ax.ToString()}]", 0);
            //return ax?"Sussess!":"Error!";

            var dateTimeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            //dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject(thamso);
            //bool ax = new QlbOutBoxDAL().Insert((object)obj);
            bool ax = new clsResuftAPI().GetValueApiExtension("MESSAGE_PKG", "message_send_one_amhs", new { P_ORIGIN = toOrigin, P_TOADD = toAdd, P_CONTENT = content }).ToString() == "1" ? true : false;
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[QlbInBoxDAL]", 0, $"[InsertOrigin][{ax.ToString()}]", 0);
            return ax ? "Sussess!" : "Error!";

        }
        private string checkMessageICAO_Click(string thamso)
        {
            return new clsResuftAPI().GetValueApiExtension("MESSAGE_PKG", "CheckMessageICAO4444", new { P_CONTENT = thamso }).ToString();
        }
    }
}