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
using Newtonsoft.Json.Converters;

namespace prjApplication.FinishedFlights
{
    public partial class FinishedFlights : PageBaseCallBack
    {
        public string _phanCach = "::::";
        public string _IdSelect = "0";
        private string _AliasSession = "FinishedFlights";

        #region newList
        public string _ListAero
        {
            get
            {
                string kq = "";
                foreach (var item in new AeroDAL().GetListAll())
                {
                    kq += item.AE_CODE + ",";
                }
                return kq.Substring(0, kq.Length - 1);
            }
        }
        public string _ListPurpose
        {
            get
            {
                string kq = "";
                foreach (var item in new FlyPurposeDAL().GetAllObject())
                {
                    kq += item.PURPOSE_CODE + ",";
                }
                return kq.Substring(0, kq.Length - 1);
            }
        }
        public string _ListCraft
        {
            get
            {
                string kq = "[";
                foreach (var item in new CraftTypeDAL().GetAllCraftType())
                {
                    kq += "{\"craftid\":\"" + item.ID + "\", \"name\":\"" + item.MA + "\"},";
                }
                kq= kq.Substring(0, kq.Length - 1);               
                return kq + "]";
            }
        }
        public string _ListOper
        {
            get
            {
                string kq = "";
                foreach (var item in new OperDAL().GetAllObject())
                {
                    kq += item.OPER_ICAO + ",";
                }
                return kq.Substring(0, kq.Length - 1);
            }
        }
        #endregion



        private bool IsSearch = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                LoadDropdownList();
                //var t = new FinishedFlightsDAL().GetTableBySearch(GetObjectSearch());                
                //PhanTrang1.TotalRecord = t != null ? Convert.ToInt32(t.Rows[0]["SumRecord"].ToString()) : 0;
                //PhanTrang1.Paging_IndexChange += btnSearch_Click;
                //grdSource1.DataSource = t;
                //grdSource1.DataBind();
                //var ax = this.RenderToHTML(PhanTrang1);
                txtStartDate.Value = DateTime.Now.AddDays(-2).ToString("dd'/'MM'/'yyyy");
                txtFinishDate.Value = DateTime.Now.ToString("dd'/'MM'/'yyyy");
                LoadData();
                btnAddNew.Visible = _Role.R_Add;
                btnUpdateCustum.Visible = _Role.R_Pub;
            }
        }

        private clsFinishedFlightSearch GetObjectSearch()
        {
            clsFinishedFlightSearch obj = new clsFinishedFlightSearch();
            obj.StartDate = txtStartDate.Value;
            obj.FinishDate = txtFinishDate.Value;
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
            if(txtValidateHour.Value!="")
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

        private void LoadDataGrid(clsFinishedFlightSearch objSearch)
        {
            DataTable dt = null;

            if (ddlSelect.Value == "0")
                dt = new FinishedFlightsDAL().GetTableBySearch(objSearch);
            else if (ddlSelect.Value == "1")
                dt = new FinishedFlightsDAL().GetTableBySearchMBNC(objSearch);
            else if (ddlSelect.Value == "2")
                dt = new FinishedFlightsDAL().GetTableBySearchMBATA(objSearch);
            else if (ddlSelect.Value == "3")
                dt = new FinishedFlightsDAL().GetTableBySearchMBBCS(objSearch);
            else if (ddlSelect.Value == "4")
                dt = new FinishedFlightsDAL().GetTableBySearchMBVIA(objSearch);
            else if (ddlSelect.Value == "5")
                dt = new FinishedFlightsDAL().GetTableBySearchMBSAMECAL(objSearch);
            else if (ddlSelect.Value == "7")
                dt = new FinishedFlightsDAL().GetTableBySearch7(objSearch);
            else if (ddlSelect.Value == "8")
                dt = new FinishedFlightsDAL().GetTableBySearch8(objSearch);
            else if (ddlSelect.Value == "9")
                dt = new FinishedFlightsDAL().GetTableBySearch9(objSearch);
            else if (ddlSelect.Value == "10")
                dt = new FinishedFlightsDAL().GetTableBySearch10(objSearch);
            else if (ddlSelect.Value == "11")
                dt = new FinishedFlightsDAL().GetTableBySearch11(objSearch);
            else if (ddlSelect.Value == "12")
                dt = new FinishedFlightsDAL().GetTableBySearch12(objSearch);
            else if (ddlSelect.Value == "13")
                dt = new FinishedFlightsDAL().GetTableBySearch13(objSearch);
            else if (ddlSelect.Value == "14")
                dt = new FinishedFlightsDAL().GetTableBySearch14(objSearch);
            else
                dt = new FinishedFlightsDAL().GetTableBySearch(objSearch);

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
                case "UpdateListObject":
                    kq = UpdateListObject(_arg[0]);
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
            this.FillDropdownList<FlyPurpose>(ddlPURPOSE, new FlyPurposeDAL().GetAllObject(), "PURPOSE_CODE", "PURPOSE_CODE", "--");
            this.FillDropdownList<Aero>(ddlTO_AIRP, new AeroDAL().GetListAll(), "AE_CODE", "AE_CODE", "--");
            this.FillDropdownList<Aero>(ddlFROM_AIRP, new AeroDAL().GetListAll(), "AE_CODE", "AE_CODE", "--");

            this.FillDropdownList<Oper>(ddlOPER, new OperDAL().GetAllObject(), "OPER_ICAO", "OPER_ICAO", "--");

        }
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/FinishFlights/EditFinishFlight.aspx" + "?Menu_ID=" + Request.Params["Menu_Id"] + "&ID=0");
        }
        private string UpdateListObject(string thamso)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var lisObj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<prjInfo.FinishedFlights>>(thamso, dateTimeConverter);
            int dem = 0;
            foreach (var obj in lisObj)
            {
                obj.LASTUSER = _user.UserName;
                bool kq = new FinishedFlightsDAL().Update(obj);
                string ax = kq.ToString() == true.ToString() ? "Update sussess" : "Update error";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermMasterScDAL]", 0, $"[UpdateObject] [{ax}]", 0.0);
                if (kq) dem++;
            }
            return $"Update sussess: {dem}/{lisObj.Count}!";
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
            clsFinishedFlightSearch objSearch = GetObjectSearch();
            if (IsSearch)
                objSearch.PageIndex = 0;
            else
                objSearch.PageIndex = PhanTrang1.PageIndex;
            objSearch.PageSize = PhanTrang1.PageSize;
            IsSearch = false;
            DataTable dt = new DataTable();
            if (!lnkDelete.Visible)
            {
                if (ddlSelect.Value == "0")
                    dt = new FinishedFlightsDAL().GetTableBySearch(objSearch);
                else if (ddlSelect.Value == "1")
                    dt = new FinishedFlightsDAL().GetTableBySearchMBNC(objSearch);
                else if (ddlSelect.Value == "2")
                    dt = new FinishedFlightsDAL().GetTableBySearchMBATA(objSearch);
                else if (ddlSelect.Value == "3")
                    dt = new FinishedFlightsDAL().GetTableBySearchMBBCS(objSearch);
                else if (ddlSelect.Value == "4")
                    dt = new FinishedFlightsDAL().GetTableBySearchMBVIA(objSearch);
                else if (ddlSelect.Value == "5")
                    dt = new FinishedFlightsDAL().GetTableBySearchMBSAMECAL(objSearch);
                else if (ddlSelect.Value == "7")
                    dt = new FinishedFlightsDAL().GetTableBySearch7(objSearch);
                else if (ddlSelect.Value == "8")
                    dt = new FinishedFlightsDAL().GetTableBySearch8(objSearch);
                else if (ddlSelect.Value == "9")
                    dt = new FinishedFlightsDAL().GetTableBySearch9(objSearch);
                else if (ddlSelect.Value == "10")
                    dt = new FinishedFlightsDAL().GetTableBySearch10(objSearch);
                else if (ddlSelect.Value == "11")
                    dt = new FinishedFlightsDAL().GetTableBySearch11(objSearch);
                else if (ddlSelect.Value == "12")
                    dt = new FinishedFlightsDAL().GetTableBySearch12(objSearch);
                else if (ddlSelect.Value == "13")
                    dt = new FinishedFlightsDAL().GetTableBySearch13(objSearch);
                else if (ddlSelect.Value == "14")
                    dt = new FinishedFlightsDAL().GetTableBySearch14(objSearch);
                else
                    dt = new FinishedFlightsDAL().GetTableBySearch(objSearch);
            }                
            //else
            //    dt = new FinishedFlightsDAL().GetTableDelete(objSearch);
            if (dt == null)
            {
                grdSource1.DataSource = dt;
                grdSource1.DataBind();
                return;
            }
            if (dt.Rows.Count > 0)
            {
                PhanTrang1.TotalRecord = Convert.ToInt32(dt.Rows[0]["SumRecord"].ToString());
            }           
            if (PhanTrang1.TotalRecord > 0)
                lit.Text = "<b>Totals Record :</b>  " + PhanTrang1.TotalRecord;
            else
                lit.Text = "<b>Totals Record :</b>  0";
            grdSource1.DataSource = dt;
            grdSource1.DataBind();
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            GridView ax = new GridView();
            if (!lnkDelete.Visible)
                ax.DataSource = new FinishedFlightsDAL().ExportBySearch(GetObjectSearch());
            ax.DataBind();
            string html = this.RenderToHTML(ax);
            this.CreateExcel(html, "Finished_Flights_" + DateTime.Now.ToFileTime() + ".xls");
        }
        public override void VerifyRenderingInServerForm(Control control) { }
    }
}