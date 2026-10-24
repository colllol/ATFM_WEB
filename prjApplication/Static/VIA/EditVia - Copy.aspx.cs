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

namespace prjApplication.Static.VIA
{
    public partial class EditVia : PageCoreAdmin
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
                        ddl_Load(); ddl_Oper();
                        LoadViaById();
                        btnSave.Enabled = _Role.R_Add;
                    }
                }
            }
        }

        private void LoadViaById()
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
        public void ddl_Load()
        {
            var s_Aero = new AeroDAL().GetListAll();          
            foreach (var item in s_Aero)
            {
                ListItem li = new ListItem();
                li.Value = item.AE_CODE;
                li.Text = item.AE_CODE + " - " + item.AE_IATA;
                ddlFROM_AIRP.Items.Add(li);
                ddlTO_AIRP.Items.Add(li);
            }
            ddlFROM_AIRP.Items.Insert(0, new ListItem("--", ""));
            ddlTO_AIRP.Items.Insert(0, new ListItem("--", ""));

        }
        public void ddl_Oper()
        {
            var s_Oper = new OperDAL().GetAllObject();
            foreach (var item in s_Oper)
            {
                ListItem li = new ListItem();
                li.Value = item.OPER_ICAO.ToString();
                li.Text = item.OPER_ICAO;
                ddlOper.Items.Add(li);                
            }
            ddlOper.Items.Insert(0, new ListItem("--", ""));
            

        }
        private void PopulateItem(int _id)
        {
            Via obj = new ViaDAL().GetViaById(_id.ToString());
            txtEVENT_NAME.Text = obj.SOHIEU;
            txtVIA.Value = obj.VIA;
            txtPOINT_IN.Value = obj.POINT_IN;
            txtPOINT_OUT.Value= obj.POINT_OUT;
            txtQUYTAC.Value= obj.QUYTAC;
            txtCraft.Value= obj.CRAFT_TYPE;
            txtFLIGHT.Value = obj.MUCBAY;
            ddlFROM_AIRP.SelectedIndex = UltilFunc.GetIndexControl(ddlFROM_AIRP, obj.FROM_AIRP);
            ddlTO_AIRP.SelectedIndex = UltilFunc.GetIndexControl(ddlTO_AIRP, obj.TO_AIRP);
            ddlOper.SelectedIndex = UltilFunc.GetIndexControl(ddlOper, obj.OPER.ToString());

        }

        private Via GetInfoVia()
        {

            Via _obj = new Via();
            int id = CommonLib.IsNumeric(Request["ID"]) ? Convert.ToInt32(Request["ID"].ToString()) : 0;            
            _obj.ID = id;
            _obj.SOHIEU = txtEVENT_NAME.Text;
            _obj.VIA = txtVIA.Value;
            _obj.POINT_IN = txtPOINT_IN.Value;
            _obj.POINT_OUT = txtPOINT_OUT.Value;
            _obj.QUYTAC = txtQUYTAC.Value;
            _obj.CRAFT_TYPE = txtCraft.Value;
            _obj.MUCBAY = txtFLIGHT.Value;
            _obj.TO_AIRP = ddlTO_AIRP.SelectedValue;
            _obj.FROM_AIRP = ddlFROM_AIRP.SelectedValue; 
            _obj.OPER            = ddlOper.SelectedValue; 
            return _obj;


        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            Via obj;
            if (CommonLib.IsNumeric(Request["ID"]) == true)
            {
                // update
                obj = GetInfoVia();
                int _id = Convert.ToInt32(Request["ID"].ToString());
                obj.ID = _id;
                var kq = new ViaDAL().UpdateVia(obj);
                if (kq) lblResuft.InnerText = "Update sussess!";
                else lblResuft.InnerText = "Update error!";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit Via]", 0, "[UpdateVia]" + lblResuft.InnerText, 0.0);
            }
            else
            {
                // insert
                obj = GetInfoVia();
                var kq = new ViaDAL().InsertVia(obj);
                if (kq) lblResuft.InnerText = "Insert sussess!";
                else lblResuft.InnerText = "Insert error!";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit Alarm]", 0, "[InsertVia]" + lblResuft.InnerText, 0.0);
            }

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Static/VIA/ListVia.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }
    }
}