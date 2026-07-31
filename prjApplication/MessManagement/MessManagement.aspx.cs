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
    public partial class MessManagement : PageBaseCallBack
    {
        protected string _phanCach = "::::";
        protected string _phanCachArg = "_____";
        public string _IdSelect = "0";
        private string _AliasSession = "CreateMess";
        private bool IsSearch = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtBEGINDATE.Value = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                LoadData(txtBEGINDATE.Value);
            }
        }
        protected void btnSelect_Click(object sender, EventArgs e)
        {
            if(ddlType.Value=="0")
            {
                LoadData(txtBEGINDATE.Value);
            }
            else
            {
                LoadData_Cancel(txtBEGINDATE.Value);
            }
            
        }
        void LoadData(string _date)
        {
            DataTable dt = new DayFlightsDAL().GetTableWithPartNo(_date);

            if (dt.Rows.Count > 0)
            {
                grdListPartMessage.DataSource = dt;
                grdListPartMessage.DataBind();
                rptCacels.DataSource = null;
                rptCacels.DataBind();
            }
            else
            {
                grdListPartMessage.DataSource = null;
                grdListPartMessage.DataBind();
                rptCacels.DataSource = null;
                rptCacels.DataBind();
            }
        }
        void LoadData_Cancel(string _date)
        {
            DataTable dt = new DayFlightsDAL().GetTableWithPartNo_Cancel(_date);

            if (dt.Rows.Count > 0)
            {
                grdListPartMessage.DataSource = null;
                grdListPartMessage.DataBind();
                rptCacels.DataSource = dt;
                rptCacels.DataBind();
            }
            else
            {
                grdListPartMessage.DataSource = null;
                grdListPartMessage.DataBind();
                rptCacels.DataSource = null;
                rptCacels.DataBind();
            }
        }
        protected string LoadDataDetail(string _partNo, string messType)
        {
            DataTable dt = new DayFlightsDAL().GetTableWithPartNoID(txtBEGINDATE.Value, _partNo, messType);
            string kq = "";
            if (dt.Rows.Count > 0)
            {
                kq = dt.Rows[0]["CONTENT"].ToString();
            }
            return kq;
        }

        protected string LoadDataDetail_Cancel(string _partNo, string messType)
        {
            DataTable dt = new DayFlightsDAL().GetTableWithPartNoID_Cancel(txtBEGINDATE.Value, _partNo, messType);
            string kq = "";
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
                case "ViewContentCL":
                    kq = LoadDataDetail_Cancel(ThamSo[0], ThamSo[1]);
                    break;
                case "btnGetAddredd":
                    kq = LoadAddreeeDetail(ThamSo[0]);
                    break;
                case "btnSend_Onclick":
                    kq = btnSend_Onclick(ThamSo[4],ThamSo[0], ThamSo[1], ThamSo[2], ThamSo[3]);
                    break;
                case "btnSendAMHS_Onclick":
                    kq = btnSendAMHS_Onclick(ThamSo[4], ThamSo[0], ThamSo[1], ThamSo[2], ThamSo[3]);
                    break;
                case "btnOnclickAll":
                    kq = btnSendOnclickAll(ThamSo[0], ThamSo[1], ThamSo[2],ThamSo[4], ThamSo[3], ThamSo[5]);
                    break;
            }
            return kq;
        }

        private string btnSendOnclickAll(string vn, string ld, string of,string toOrigin, string toAdd, string _date)
        {
            bool ax = Convert.ToBoolean(new clsResuftAPI().GetValueApiExtension("MESSAGE_PKG", "Khb_SendAll_MessageAll", new { P_VN=vn, P_LD=ld, P_OF=of, P_ORIGIN = toOrigin, P_TOADD = toAdd, P_DATE = DateTime.Parse(_date) }).ToString() == "1" ? true : false);
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[QlbInBoxDAL]", 0, $"[Insert][{ax.ToString()}]", 0);
             return ax ? "Sussess!" : "Error!";
           
        } 
        private string btnOnclickAll(string[] thamso)
        {
            bool ax = Convert.ToBoolean(new clsResuftAPI().GetValueApiExtension("MESSAGE_PKG", "Khb_SendAll_Message", new { P_HEADER= thamso[0], P_DATE= DateTime.Parse(thamso[1])}).ToString() == "1" ? true : false);
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[QlbInBoxDAL]", 0, $"[Insert][{ax.ToString()}]", 0);
            return ax ? "Sussess!" : "Error!";
        }
        private string btnSend_Onclick(string partNo, string messType, string toAdd, string content)
        {
            if (partNo == " ")
            {
                return "Not Content!";
            }
            else
            {
                bool a1x = new QlbOutBoxDAL().UpdateStatus(txtBEGINDATE.Value, partNo, messType);

                var dateTimeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                //dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject(thamso);
                //bool ax = new QlbOutBoxDAL().Insert((object)obj);
                bool ax = new clsResuftAPI().GetValueApiExtension("MESSAGE_PKG", "message_send_one", new { P_TOADD = toAdd, P_CONTENT = content }).ToString() == "1" ? true : false;
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[QlbInBoxDAL]", 0, $"[Insert][{ax.ToString()}]", 0);
                return ax ? "Sussess!" : "Error!";
            }
        }



        private string btnSend_Onclick1(string toOrigin, string toAdd, string content)
        {
            //var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            //dynamic obj = JsonConvert.DeserializeObject(thamso);
            //bool ax = new QlbOutBoxDAL().Insert((object)obj);
            //WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[QlbOutBoxDAL]", 0, $"[Insert][{ax.ToString()}]", 0);
            //return ax?"Sussess!":"Error!";

            var dateTimeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            //dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject(thamso);
            //bool ax = new QlbOutBoxDAL().Insert((object)obj);
            bool ax = new clsResuftAPI().GetValueApiExtension("MESSAGE_PKG", "message_send_one_origin", new { P_ORIGIN = toOrigin, P_TOADD = toAdd, P_CONTENT = content }).ToString() == "1" ? true : false;
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[QlbInBoxDAL]", 0, $"[InsertOrigin][{ax.ToString()}]", 0);
            return ax ? "Sussess!" : "Error!";

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
            bool ax = new clsResuftAPI().GetValueApiExtension("MESSAGE_PKG", "message_send_one_origin", new { P_ORIGIN = toOrigin, P_TOADD = toAdd, P_CONTENT = content }).ToString() == "1" ? true : false;
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[QlbInBoxDAL]", 0, $"[InsertOrigin][{ax.ToString()}]", 0);
            return ax ? "Sussess!" : "Error!";

        }
        private string btnSend_Onclick(string toOrigin, string partNo, string messType, string toAdd, string content)
        {
            if (partNo == " ")
            {
                return "Not Content!";
            }
            else
            {
                bool a1x = new QlbOutBoxDAL().UpdateStatus(txtBEGINDATE.Value, partNo, messType);

                var dateTimeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                //dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject(thamso);
                //bool ax = new QlbOutBoxDAL().Insert((object)obj);
                //bool ax = new clsResuftAPI().GetValueApiExtension("MESSAGE_PKG", "message_send_one", new { P_TOADD = toAdd, P_CONTENT = content }).ToString() == "1" ? true : false;
                bool ax = new clsResuftAPI().GetValueApiExtension("MESSAGE_PKG", "message_send_one_origin", new { P_ORIGIN = toOrigin, P_TOADD = toAdd, P_CONTENT = content }).ToString() == "1" ? true : false;
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[QlbInBoxDAL]", 0, $"[Insert][{ax.ToString()}]", 0);
                return ax ? "Sussess!" : "Error!";
            }
        }
        private string btnSendAMHS_Onclick(string toOrigin, string partNo, string messType, string toAdd, string content)
        {
            if (string.IsNullOrWhiteSpace(partNo) || string.IsNullOrWhiteSpace(content))
            {
                return "Không có nội dung điện văn để gửi AMHS.";
            }

            if (string.IsNullOrWhiteSpace(toAdd))
            {
                return "Chưa nhập địa chỉ nhận AMHS.";
            }

            try
            {
                // Luồng AMHS phải gọi message_send_one_amhs. Store này ghi dữ liệu vào
                // OUTBOX_ORACLE và OUTBOX_ADDRESS_ORACLE trong cùng transaction.
                object apiResult = new clsResuftAPI().GetValueApiExtension(
                    "MESSAGE_PKG",
                    "message_send_one_amhs",
                    new
                    {
                        P_ORIGIN = toOrigin,
                        P_TOADD = toAdd,
                        P_CONTENT = content
                    });

                string resultCode = Convert.ToString(apiResult);
                bool isSuccess = string.Equals(resultCode, "1", StringComparison.Ordinal);

                WriteLogHistory2Database.WriteHistory2Database(
                    _user.UserID,
                    _user.UserFullName,
                    "[MESSAGE_PKG.message_send_one_amhs]",
                    0,
                    string.Format("[SendAMHS][Result={0}]", string.IsNullOrEmpty(resultCode) ? "NULL" : resultCode),
                    0);

                if (!isSuccess)
                {
                    return string.IsNullOrEmpty(resultCode)
                        ? "Gửi AMHS không thành công: API không trả về kết quả. Kiểm tra kết nối API và log MESSAGE_PKG."
                        : "Gửi AMHS không thành công. Mã kết quả: " + resultCode + ".";
                }

                // Chỉ đánh dấu điện văn đã gửi sau khi MESSAGE_PKG xác nhận ghi Outbox thành công.
                bool statusUpdated = new QlbOutBoxDAL().UpdateStatus(txtBEGINDATE.Value, partNo, messType);
                if (!statusUpdated)
                {
                    return "Gửi AMHS thành công, nhưng chưa cập nhật được trạng thái điện văn nguồn.";
                }

                return "Gửi AMHS thành công.";
            }
            catch (Exception ex)
            {
                WriteLogHistory2Database.WriteHistory2Database(
                    _user.UserID,
                    _user.UserFullName,
                    "[MESSAGE_PKG.message_send_one_amhs]",
                    0,
                    "[SendAMHS][Exception] " + ex.Message,
                    0);

                return "Gửi AMHS không thành công: " + ex.Message;
            }
        }

    }
}
