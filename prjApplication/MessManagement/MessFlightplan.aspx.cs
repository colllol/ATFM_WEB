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
    public partial class MessFlightplan : PageBaseCallBack
    {
        protected string _phanCach = "::::";
        protected string _phanCachArg = "_____";
        public string _IdSelect = "0";
        private string _AliasSession = "CreateMessFPL";
        private bool IsSearch = false;    

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
            if (thamso == " ")
            {
                return "Not Content!";
            }
            else
            {
                
                var dateTimeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject(thamso);
                bool ax = new QlbOutBoxDAL().Insert((object)obj);
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[QlbInBoxDAL]", 0, $"[Insert][{ax.ToString()}]", 0);
                return ax ? "Sussess!" : "Error!";
            }
        }
    }
}