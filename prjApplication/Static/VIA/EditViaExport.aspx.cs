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
    public partial class EditViaExport : PageCoreAdmin
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
                        ddl_Load(); 
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
            //var s_Aero = new AeroDAL().GetListAll();
            //foreach (var item in s_Aero)
            //{
            //    ListItem li = new ListItem();
            //    li.Value = item.AE_CODE;
            //    li.Text = item.AE_CODE;
            //    ddlFROM_AIRP.Items.Add(li);
            //    ddlTO_AIRP.Items.Add(li);
            //}
            //ddlFROM_AIRP.Items.Insert(0, new ListItem("--", ""));
            //ddlTO_AIRP.Items.Insert(0, new ListItem("--", ""));

        }
        
        private void PopulateItem(int _id)
        {
            M_VIA_ARIPORT obj = new VIA_ARIPORT_DAL().GetViaById(_id.ToString());
           
            txtVIA.Value = obj.VIA;           
            txtCraft.Value = obj.PERMTYPE;
            txtFromAirp.Value = obj.FROM_AIRP;
            txtToAir.Value = obj.TO_AIRP;
            //ddlFROM_AIRP.SelectedIndex = UltilFunc.GetIndexControl(ddlFROM_AIRP, obj.FROM_AIRP);
            //ddlTO_AIRP.SelectedIndex = UltilFunc.GetIndexControl(ddlTO_AIRP, obj.TO_AIRP);


        }

        private M_VIA_ARIPORT GetInfoVia()
        {

            M_VIA_ARIPORT _obj = new M_VIA_ARIPORT();
            int id = CommonLib.IsNumeric(Request["ID"]) ? Convert.ToInt32(Request["ID"].ToString()) : 0;
            _obj.ID = id;            
            _obj.VIA = txtVIA.Value;            
            _obj.PERMTYPE = txtCraft.Value;            
            _obj.TO_AIRP = txtToAir.Value;
            _obj.FROM_AIRP = txtFromAirp.Value;            
            return _obj;


        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            M_VIA_ARIPORT obj;
            if (CommonLib.IsNumeric(Request["ID"]) == true)
            {
                // update
                obj = GetInfoVia();
                int _id = Convert.ToInt32(Request["ID"].ToString());
                obj.ID = _id;
                var kq = new VIA_ARIPORT_DAL().UpdateM_VIA_ARIPORT(obj);
                if (kq) lblResuft.InnerText = "Update sussess!";
                else lblResuft.InnerText = "Update error!";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit VIA_ARIPORT]", 0, "[UpdateVIA_ARIPORT]" + lblResuft.InnerText, 0.0);
            }
            else
            {
                // insert
                obj = GetInfoVia();
                var kq = new VIA_ARIPORT_DAL().InsertM_VIA_ARIPORT(obj);
                if (kq) lblResuft.InnerText = "Insert sussess!";
                else lblResuft.InnerText = "Insert error!";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit VIA_ARIPORT]", 0, "[InsertVIA_ARIPORT]" + lblResuft.InnerText, 0.0);
            }

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Static/VIA/ListViaReports.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }
    }
}