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
    public partial class InboxMessage : PageBaseCallBack
    {
        protected string _phanCach = "::::";
        protected string _phanCachArg = "_____";
        public string _ObjSearch
        {
            get
            {
                QlbInBox obj = new QlbInBox();

                var ser = new JavaScriptSerializer();
                ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
                return ser.Serialize(obj);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public override string GetCallbackResult()
        {
            if (_EventArgument == "") return "";
            string kq = "";
            string[] _arg = _EventArgument.Split(new string[] { _phanCachArg }, StringSplitOptions.RemoveEmptyEntries);
            string[] ThamSo = _arg[0].Split(new string[] { _phanCach }, StringSplitOptions.RemoveEmptyEntries);
            switch (_arg[1])
            {
                case "btnSend_Onclick":
                    kq = btnSend_Onclick(_arg[0]);
                    break;
            }
            return kq;
        }
        private string btnSend_Onclick(string thamso)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            dynamic obj = JsonConvert.DeserializeObject(thamso);
            bool ax = new QlbInBoxDAL().Insert((object)obj);
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[QlbInBoxDAL]", 0, $"[Insert][{ax.ToString()}]", 0);
            return ax ? "Sussess!" : "Error!";
        }
    }
}