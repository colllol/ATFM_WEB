using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjInfo;
using prjBusinessLogic;
using System.Data;
namespace prjApplication.FinishFlights
{
    public partial class EditFinishFlight : PageCoreAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                                
                LoadDropdownList();
                if (Request.QueryString["ID"].ToString()!="0")
                {
                    LoadInfoById();
                    btnCreate.Visible = false;
                }
                else
                {
                    btnUpdate.Visible = false;
                }
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            var ax = new FinishedFlightsDAL().Insert(GetObecjt());
            if (ax) this.AlertMessage("Insert sussess!");
            else this.AlertMessage("Insert error!");
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            var ax = new FinishedFlightsDAL().Update(GetObecjt());
            if (ax) this.AlertMessage("Update sussess!");
            else this.AlertMessage("Update error!");
        }
        void LoadDropdownList()
        {
            this.FillDropdownList<CraftType>(ddlCRAFT_ID, new CraftTypeDAL().GetAllCraftType(), "MA", "CRAFT_ID");
            this.FillDropdownList<CraftType>(ddlREAL_CRAFT_TYPE, new CraftTypeDAL().GetAllCraftType(), "MA", "CRAFT_ID");
            this.FillDropdownList<FlyPurpose>(ddlPURPOSE, new FlyPurposeDAL().GetAllObject(), "PURPOSE_NAME", "PURPOSE_CODE");
            this.FillDropdownList<Aero>(ddlTO_AIRP, new AeroDAL().GetListAll(), "AE_NAME", "AE_ID");
            this.FillDropdownList<Aero>(ddlFROM_AIRP, new AeroDAL().GetListAll(), "AE_NAME", "AE_ID");
            
            this.FillDropdownList<Oper>(ddlOPER_ID, new OperDAL().GetAllObject(), "OPER_NAME", "ID");
            
        }
        private prjInfo.FinishedFlights GetObecjt()
        {
            var obj = new prjInfo.FinishedFlights();
            obj.FLIGHT_ID = Convert.ToInt32(Request.QueryString["ID"]);
            obj.ATA = txtATA.Value;
            obj.ATD = txtATD.Value;
            obj.ETA = txtETA.Value;
            obj.ETD = txtETD.Value;
            obj.FLIGHTDATE = DateTimeHelper.ConvertToDateTime(txtFLIGHTDATE.Value);            
            //obj.FLIGHTDATE = UltilFunc.ToDateddMMyyyhhmm(txtFLIGHTDATE.Value);           
            obj.FLIGHTNBR = txtFLIGHTNBR.Value;
            obj.FPL_VIA = txtFPL_VIA.Value;
            obj.PERMNBR = txtPERMNBR.Value;
            obj.REGISTRATION = txtREGISTRATION.Value;
            obj.REMARK = txtREMARK.Value;
            obj.VALIDHOURS = Convert.ToInt32( txtVALIDHOURS.Value);
            obj.VIA = txtVIA.Value;
            obj.CRAFT_ID = Convert.ToInt32(ddlCRAFT_ID.SelectedValue);
            obj.FLIGHT_TYPE = ddlFLIGHT_TYPE.Value;
            obj.FROM_AIRP = ddlFROM_AIRP.SelectedValue;
            obj.OPER_ID = ddlOPER_ID.SelectedValue;
            obj.PERMTYPE = ddlPERMTYPE.Value;
            obj.PURPOSE = ddlPURPOSE.SelectedValue;
            obj.REAL_CRAFT_TYPE = ddlREAL_CRAFT_TYPE.SelectedValue;
            obj.TO_AIRP = ddlTO_AIRP.SelectedValue;
            obj.LASTUSER = _user.UserName;      
            return obj;
        }
        void LoadInfoById()
        {
            DataTable dt = new FinishedFlightsDAL().GetOne(Request.QueryString["ID"].ToString());
            txtATA.Value = dt.Rows[0]["ATA"].ToString();
            txtATD.Value = dt.Rows[0]["ATD"].ToString();
            txtETA.Value = dt.Rows[0]["ETA"].ToString();
            txtETD.Value = dt.Rows[0]["ETD"].ToString();
            txtFLIGHTDATE.Value = DateTimeHelper.ToDate(dt.Rows[0]["FLIGHTDATE"].ToString(), "dd/MM/yyyy").ToString("dd/MM/yyyy");
            txtFLIGHTNBR.Value = dt.Rows[0]["FLIGHTNBR"].ToString();
            txtFPL_VIA.Value = dt.Rows[0]["FPL_VIA"].ToString();
            txtPERMNBR.Value = dt.Rows[0]["PERMNBR"].ToString();
            txtREGISTRATION.Value = dt.Rows[0]["REGISTRATION"].ToString();
            txtREMARK.Value = dt.Rows[0]["REMARK"].ToString();
            txtVALIDHOURS.Value = dt.Rows[0]["VALIDHOURS"].ToString();
            txtVIA.Value = dt.Rows[0]["VIA"].ToString();
            this.SetDisplayDropdownList(ddlCRAFT_ID, dt.Rows[0]["CRAFT_ID"].ToString());
            
            this.SetDisplayDropdownList(ddlFROM_AIRP, dt.Rows[0]["FROM_AIRP"].ToString());
            this.SetDisplayDropdownList(ddlOPER_ID, dt.Rows[0]["OPER_ID"].ToString());
            
            this.SetDisplayDropdownList(ddlPURPOSE, dt.Rows[0]["PURPOSE"].ToString());
            this.SetDisplayDropdownList(ddlREAL_CRAFT_TYPE, dt.Rows[0]["REAL_CRAFT_TYPE"].ToString());
            this.SetDisplayDropdownList(ddlTO_AIRP, dt.Rows[0]["TO_AIRP"].ToString());
        }
    }
}