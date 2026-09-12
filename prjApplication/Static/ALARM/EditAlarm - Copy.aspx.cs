using prjComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjBusinessLogic.DAL;
using prjInfo;

namespace prjApplication.Static.ALARM
{
    public partial class EditAlarm : PageCoreAdmin
    {
        prjBusinessLogic.UserDAL _userDAL = new UserDAL();
        prjInfo.T_Users _user = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["Menu_ID"] != null && Request["Menu_ID"].ToString() != "" && Request["Menu_ID"].ToString() != string.Empty)
            {
                if (CommonLib.IsNumeric(Request["Menu_ID"]) == true)
                {
                    if (!HPCSecurity.IsAccept(Convert.ToInt32(Request["Menu_ID"])))
                        Response.Redirect("~/Errors/AccessDenied.aspx");
                    _user = _userDAL.GetUserByUserName(HPCSecurity.CurrentUser.Identity.Name);
                    if (!IsPostBack)
                    {
                        LoadAlarmById();
                        btnSave.Enabled = _Role.R_Add;
                    }
                }
            }
        }

        private void LoadAlarmById()
        {
            if (Request["ID"] != null && Request["ID"].ToString() != "" && Request["ID"].ToString() != String.Empty)
            {
                if (CommonLib.IsNumeric(Request["ID"]) == true)
                {
                    int _id = Convert.ToInt32(Request["ID"].ToString());
                    PopulateItem(_id);
                }
            }
        }

        private void PopulateItem(int _id)
        {
            Alarm obj = new AlarmDAL().GetOneAlarm(_id);
            txtEVENT_NAME.Text = obj.EVENT_NAME.ToString();
            if (obj.EVENT_DATE != null && obj.EVENT_DATE != DateTime.MaxValue && obj.EVENT_DATE != DateTime.MinValue)
                this.txtEVENT_DATE.Value = obj.EVENT_DATE.ToString("dd/MM/yyyy");
            if (obj.BEGIN_VALID != null && obj.BEGIN_VALID != DateTime.MaxValue && obj.BEGIN_VALID != DateTime.MinValue)
                this.txtBEGIN_VALID.Value = obj.BEGIN_VALID.ToString("dd/MM/yyyy");
            if (obj.END_VALID != null && obj.END_VALID != DateTime.MaxValue && obj.END_VALID != DateTime.MinValue)
                this.txtEND_VALID.Value = obj.END_VALID.ToString("dd/MM/yyyy");           
            if (!String.IsNullOrEmpty(obj.STATUS.ToString()))
            {
                if (obj.STATUS.ToString() == "1")
                    ckSTATUS.Checked = true;
            }
            
        }
        
        private Alarm GetInfoAlarm()
        {
            
            Alarm _obj = new Alarm();
            int id = CommonLib.IsNumeric(Request["ID"]) ? Convert.ToInt32(Request["ID"].ToString()) : 0;
            int _status = 0;
            if (ckSTATUS.Checked)
                _status = 1;
            _obj.ID = id;
            _obj.EVENT_NAME = txtEVENT_NAME.Text;
            if (this.txtEVENT_DATE.Value.Length > 0)
            {
                if (this.txtEVENT_DATE.Value.ToString() != "")
                    _obj.EVENT_DATE = UltilFunc.ToDate(this.txtEVENT_DATE.Value, "dd/MM/yyyy");
            }

            if (this.txtBEGIN_VALID.Value.Length > 0)
            {
                if (this.txtBEGIN_VALID.Value.ToString() != "")
                    _obj.BEGIN_VALID = UltilFunc.ToDate(this.txtBEGIN_VALID.Value, "dd/MM/yyyy");
            }

            if (this.txtEND_VALID.Value.Length > 0)
            {
                if (this.txtEND_VALID.Value.ToString() != "")
                    _obj.END_VALID = UltilFunc.ToDate(this.txtEND_VALID.Value, "dd/MM/yyyy");
            }
            _obj.STATUS = _status;

            return _obj;


        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
           
            Alarm obj;
            if (CommonLib.IsNumeric(Request["ID"]) == true)
            {
                // update
                obj = GetInfoAlarm();
                int _id = Convert.ToInt32(Request["ID"].ToString());
                obj.ID = _id;
                var kq = new AlarmDAL().UpdateAlarmAsync(obj);
                if (kq) lblResuft.InnerText = "Update sussess!";
                else lblResuft.InnerText = "Update error!";

                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit Alarm]", 0, "[UpdateAlarm]" + lblResuft.InnerText, 0.0);
            }
            else
            {
                // insert
                obj = GetInfoAlarm();
                var kq = new AlarmDAL().InsertAlarm(obj);
                if (kq) lblResuft.InnerText = "Insert sussess!";
                else lblResuft.InnerText = "Insert error!";

                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit Alarm]", 0, "[InsertAlarm]" + lblResuft.InnerText, 0.0);
            }

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Static/ALARM/ListAlarm.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }
    }
}