using prjInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using System.Data;

namespace prjApplication.FinishFlights
{
    public partial class FinishedFlightsV2 : PageBaseCallBack
    {
        public string _phanCach = "::::";
        public string _IdSelect = "0";
        private string _AliasSession = "FinishedFlights";
        private bool IsSearch = false;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                LoadDropdownList();                
                LoadData();
            }
        }

        private clsDaylyFlightSearch GetObjectSearch()
        {
            clsDaylyFlightSearch obj = new clsDaylyFlightSearch();
            if(txtStartDate.Value!="") obj.StartDate = UltilFunc.ToDate(txtStartDate.Value,"dd/MM/yyyy");
            if(txtFinishDate.Value!="") obj.FinishDate = UltilFunc.ToDate(txtFinishDate.Value, "dd/MM/yyyy");
            obj.PageIndex = IsSearch ? 0 : PhanTrang1.PageIndex;
            obj.PageSize = PhanTrang1.PageSize;
            obj.ATA = txtATA.Value;
            obj.ATD = txtATD.Value;
            obj.ETA = txtETA.Value;
            obj.ETD = txtETD.Value;
            obj.FLIGHTNBR = txtFLIGHTNBR.Value;
            obj.FPL_VIA = txtFPL_VIA.Value;
            obj.PERMNBR = txtPERMNBR.Value;
            obj.REGISTRATION = txtREGISTRATION.Value;
            obj.REMARK = txtREMARK.Value;
            obj.VIA = txtVIA.Value;
            obj.TO_AIRP = ddlTO_AIRP.SelectedValue;
            obj.REAL_CRAFT_TYPE = ddlREAL_CRAFT_TYPE.SelectedValue;
            obj.PURPOSE = ddlPURPOSE.SelectedValue;
            obj.PERMTYPE = ddlPERMTYPE.Value;
            obj.OPER_ID = ddlOPER.SelectedValue;
            obj.FROM_AIRP = ddlFROM_AIRP.SelectedValue;
            obj.CRAFT_TYPE = ddlCRAFT_TYPE.SelectedValue;
            obj.FLIGHT_TYPE = ddlFLIGHT_TYPE.Value;
            obj.TypeNumber =Convert.ToInt32(ddlSelect.Value);
            if (txtValidateHour.Value != "")
                obj.VALIDHOURS = Convert.ToInt32(txtValidateHour.Value);
            obj.FLIGHTDATE = DateTimeHelper.ConvertToDateTime(txtFlightDate.Value);
            return obj;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            IsSearch = true;
            LoadDataGrid(GetObjectSearch());
            IsSearch = false;
        }

        private void LoadDataGrid(clsDaylyFlightSearch objSearch)
        {
            DataTable dt = null;
            if (ddlSelect.Value == "0")
                dt = new FinishedFlightsDAL().GetTableBySearchMN(objSearch);
            else if (ddlSelect.Value == "1")
                dt = new FinishedFlightsDAL().GetTableBySearchNC(objSearch);
            else if (ddlSelect.Value == "2")
                dt = new FinishedFlightsDAL().GetTableBySearchATA(objSearch);
            else if (ddlSelect.Value == "3")
                dt = new FinishedFlightsDAL().GetTableBySearchBCS(objSearch);
            else if (ddlSelect.Value == "4")
                dt = new FinishedFlightsDAL().GetTableBySearchVIA(objSearch);
            else if (ddlSelect.Value == "5")
                dt = new FinishedFlightsDAL().GetTableBySearchSAMECAL(objSearch);
            else
                dt = new FinishedFlightsDAL().GetTableBySearchTRUNGCAL(objSearch);

            PhanTrang1.TotalRecord = dt != null ? Convert.ToInt32(dt.Rows[0]["SumRecord"].ToString()) : 0;
            if (PhanTrang1.TotalRecord > 0)
                lit.Text = "<b>Totals Record :</b>  " + PhanTrang1.TotalRecord;
            else
                lit.Text = "<b>Totals Record :</b>  0";
            grdSource1.DataSource = dt;
            grdSource1.DataBind();
        }
        private string btnDelete_Onclick(string thamso)
        {
            return new FinishedFlightsDAL().Delete(thamso) ? "Delete sussess!" : "Delete error";
        }
        protected void PhanTrang1_Paging_IndexChange(object sender, EventArgs e)
        {
            LoadData();
            //LoadDataGrid(GetObjectSearch());
        }
        public override string GetCallbackResult()
        {
            if (_EventArgument == "") return "";
            string kq = "";
            string[] _arg = _EventArgument.Split(new string[] { "_____" }, StringSplitOptions.RemoveEmptyEntries);
            string[] ThamSo = _arg[0].Split(new string[] { "::::" }, StringSplitOptions.RemoveEmptyEntries);
            switch (_arg[1])
            {
                case "btnDelete_Onclick":
                    kq = btnDelete_Onclick(_arg[0]);
                    break;
                case "RestoreHistory":
                    kq = RestoreHistory(ThamSo);
                    break;
                case "LoadDataGrid":
                    kq = LoadDataGrid(ThamSo);
                    break;
            }
            return kq;
        }

        private string LoadDataGrid(string[] thamSo)
        {
            LoadData();
            return this.RenderToHTML(grdSource1);
        }

        void LoadDropdownList()
        {
            this.FillDropdownList<CraftType>(ddlCRAFT_TYPE, new CraftTypeDAL().GetAllCraftType(), "MA", "CRAFT_ID", "--");
            this.FillDropdownList<CraftType>(ddlREAL_CRAFT_TYPE, new CraftTypeDAL().GetAllCraftType(), "MA", "CRAFT_ID", "--");
            this.FillDropdownList<FlyPurpose>(ddlPURPOSE, new FlyPurposeDAL().GetAllObject(), "PURPOSE_NAME", "PURPOSE_CODE", "--");
            this.FillDropdownList<Aero>(ddlTO_AIRP, new AeroDAL().GetListAll(), "AE_NAME", "AE_CODE", "--");
            this.FillDropdownList<Aero>(ddlFROM_AIRP, new AeroDAL().GetListAll(), "AE_NAME", "AE_CODE", "--");

            this.FillDropdownList<Oper>(ddlOPER, new OperDAL().GetAllObject(), "OPER_NAME", "OPER_ICAO", "--");

        }
        

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            LinkButton lik = (LinkButton)sender;
            var kq = new FinishedFlightsDAL().Delete(lik.CommandArgument) ? "Delete sussess!" : "Delete error";
            this.AlertMessage(kq);
            PhanTrang1_Paging_IndexChange(sender, e);
        }
        private string RestoreHistory(string[] thamso)
        {
            bool kq = new FinishedFlightsDAL().RestoreRecode(thamso[0], thamso[1], _user.UserID.ToString());
            string ax = kq.ToString() == true.ToString() ? "Restore sussess" : "Restore error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[FinishedFlightsDAL]", 0, $"[RestoreRecode] [{ax}]", 0.0);
            return kq ? "OK" : "NOK";
        }
        protected void grdSource1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string id = ((Label)e.Item.FindControl("lblId")).Text;
                Label lblHis = (Label)e.Item.FindControl("lblHis");
                lblHis.Visible = true;
                lblHis.Text = $"<tr class=\"detail-row\"><td colspan=\"22\" class=\"text-left\">{rListHistoryFlightDetails(new FinishedFlightsDAL().GetHisById(id), id)}</td></tr>";
            }
        }
        protected void lnkShowDelete_Click(object sender, EventArgs e)
        {
            LinkHideDelete.Visible = !LinkHideDelete.Visible;
            lnkDelete.Visible = !lnkDelete.Visible;
            LoadData();
        }
        void LoadData()
        {
            clsDaylyFlightSearch objSearch = GetObjectSearch();
            if (IsSearch)
                objSearch.PageIndex = 0;
            else
                objSearch.PageIndex = PhanTrang1.PageIndex;
            objSearch.PageSize = PhanTrang1.PageSize;
            IsSearch = false;
            DataTable dt = new DataTable();
            if (!lnkDelete.Visible)
            {
                if(ddlSelect.Value=="0")
                    dt = new FinishedFlightsDAL().GetTableBySearchMN(objSearch);
                else if(ddlSelect.Value=="1")
                    dt = new FinishedFlightsDAL().GetTableBySearchNC(objSearch);
                else if (ddlSelect.Value == "2")
                    dt = new FinishedFlightsDAL().GetTableBySearchATA(objSearch);
                else if (ddlSelect.Value == "3")
                    dt = new FinishedFlightsDAL().GetTableBySearchBCS(objSearch);
                else if (ddlSelect.Value == "4")
                    dt = new FinishedFlightsDAL().GetTableBySearchVIA(objSearch);
                else if (ddlSelect.Value == "5")
                    dt = new FinishedFlightsDAL().GetTableBySearchSAMECAL(objSearch);
                else
                    dt = new FinishedFlightsDAL().GetTableBySearchTRUNGCAL(objSearch);
            }               
            else
                dt = new FinishedFlightsDAL().GetTableDelete(objSearch);
            if (dt == null)
            {
                grdSource1.DataSource = dt;
                grdSource1.DataBind();
                return;
            }
            if (dt.Rows.Count > 0)
                PhanTrang1.TotalRecord = Convert.ToInt32(dt.Rows[0]["SumRecord"].ToString());
            if (PhanTrang1.TotalRecord > 0)
                lit.Text = "<b>Totals Record :</b>  " + PhanTrang1.TotalRecord;
            else
                lit.Text = "<b>Totals Record :</b>  0";
            grdSource1.DataSource = dt;
            grdSource1.DataBind();
        }
    }
}