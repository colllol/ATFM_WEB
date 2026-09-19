using prjComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjInfo;
using prjApplication;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Converters;

namespace prjApplication.Atkl
{
    public partial class EditAtklFull : PageBaseCallBack
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetInfoByFirstLoad();
            }
        }
        public override string GetCallbackResult()
        {
            if (_EventArgument == "") return "";
            string kq = "";
            string[] _arg = _EventArgument.Split(new string[] { "_____" }, StringSplitOptions.RemoveEmptyEntries);
            string[] ThamSo = _arg[0].Split(new string[] { "::::" }, StringSplitOptions.RemoveEmptyEntries);
            switch (_arg[1])
            {
                case "GetOneFlight":
                    kq = GetOneFlight(_arg[0]);
                    break;
                case "btnUpdateOnclick":
                    kq = btnUpdateOnclick(_arg[0]);
                    break;
            }
            return kq;
        }
        private string btnUpdateOnclick(string thamso)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<prjInfo.Atkl>(thamso, dateTimeConverter);
            //obj.LASTUSER = _user.UserName;
            bool kq = new AtklDAL().UpdateAtkl(obj);
            string ax = kq.ToString() == true.ToString() ? "Update sussess" : "Update error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermMasterScDAL]", 0, $"[UpdateObject] [{ax}]", 0.0);
            if (!kq)
                return "Update error";
            return "Update sussess";
        }
        protected void GetInfoByFirstLoad()
        {
            if (Request.QueryString["ID"] == null)
                return;
            string kq = GetOneFlight(Request.Params["ID"]);
            this.ExcuteJavascript($"ReadInfoPerm('{kq}'); listfile();");

        }
        private string GetOneFlight(string id)
        {
            prjInfo.Atkl obj = new AtklDAL().GetAtklById(id);
            var ser = new JavaScriptSerializer();
            ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
            return ser.Serialize(obj);
        }
    }
}