using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjInfo;
using prjComponents;
using System.IO;
using TuesPechkin;

namespace prjApplication.PermIMP
{
    public partial class ListPermNoIMP : PageCoreAdmin
    {
        private const string _AliasSession = "ListPermNoIMP";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        #region function
        public void LoadData()
        {
            string where = GetWhereCondition();
            List<PermNoIMP> t = new PermNoImpDAL().GetPagePermNoIMP(PhanTrang1.PageSize, PhanTrang1.PageIndex, where);
            if (t.Count == 0)
                PhanTrang1.TotalRecord = 0;
            else
                PhanTrang1.TotalRecord = (int)t[0].RECORD_SUM;
            rptSource.DataSource = t;
            rptSource.DataBind();
        }

        #endregion

        #region control-event

        protected void linkSearch_Click(object sender, EventArgs e)
        {
            PhanTrang1.PageIndex = 0;
            LoadData();
        }

        public string GetWhereCondition()
        {
            string where = " 1=1";
            if (!String.IsNullOrEmpty(txtSearchCALLSIGN.Value.Trim()))
                where += " AND " + string.Format(" CALLSIGN like N'%{0}%'", UltilFunc.SqlFormatText(txtSearchCALLSIGN.Value.Trim()).ToUpper());
            if (!String.IsNullOrEmpty(txtSearchCRAFT.Value.Trim()))
                where += " AND " + string.Format(" CRAFT like N'%{0}%'", UltilFunc.SqlFormatText(txtSearchCRAFT.Value.Trim()).ToUpper()); 
             //if (!String.IsNullOrEmpty(txtSearchCRAFT.Value.Trim()))
             //   where += " AND " + string.Format(" DAYFLY like N'%{0}%'", UltilFunc.SqlFormatText(txtSearchDAYFLY.Value.Trim()).ToUpper());
            //if (!String.IsNullOrEmpty(txtSearchDAILY.Value.Trim()))
            //    where += " AND " + string.Format(" DAILY like N'%{0}%'", UltilFunc.SqlFormatText(txtSearchDAILY.Value.Trim()).ToUpper());
            if (!String.IsNullOrEmpty(txtSearchETA.Value.Trim()))
                where += " AND " + string.Format(" ETA like N'%{0}%'", UltilFunc.SqlFormatText(txtSearchETA.Value.Trim()).ToUpper());
            if (!String.IsNullOrEmpty(txtSearchETD.Value.Trim()))
                where += " AND " + string.Format(" ETD like N'%{0}%'", UltilFunc.SqlFormatText(txtSearchETD.Value.Trim()).ToUpper());
            if (!String.IsNullOrEmpty(txtSearchPERMNBR.Value.Trim()))
                where += " AND " + string.Format(" PERMNBR like N'%{0}%'", UltilFunc.SqlFormatText(txtSearchPERMNBR.Value.Trim()).ToUpper());
            if (!String.IsNullOrEmpty(txtSearchPERMTYPE.Value.Trim()))
                where += " AND " + string.Format(" PERMTYPE like N'%{0}%'", UltilFunc.SqlFormatText(txtSearchPERMTYPE.Value.Trim()).ToUpper());
            if (!String.IsNullOrEmpty(txtSearchREGISTRATION.Value.Trim()))
                where += " AND " + string.Format(" REGISTRATION like N'%{0}%'", UltilFunc.SqlFormatText(txtSearchREGISTRATION.Value.Trim()).ToUpper());
            if (!String.IsNullOrEmpty(txtSearchREMARK.Value.Trim()))
                where += " AND " + string.Format(" REMARK like N'%{0}%'", UltilFunc.SqlFormatText(txtSearchREMARK.Value.Trim()).ToUpper());
            if (!String.IsNullOrEmpty(txtSearchVIA.Value.Trim()))
                where += " AND " + string.Format(" VIA like N'%{0}%'", UltilFunc.SqlFormatText(txtSearchVIA.Value.Trim()).ToUpper());
            if (!String.IsNullOrEmpty(txtSearchTO_AIRP.Value.Trim()))
                where += " AND " + string.Format(" TO_AIRP like N'%{0}%'", UltilFunc.SqlFormatText(txtSearchTO_AIRP.Value.Trim()).ToUpper());
            if (!String.IsNullOrEmpty(txtSearchFROM_AIRP.Value.Trim()))
                where += " AND " + string.Format(" FROM_AIRP like N'%{0}%'", UltilFunc.SqlFormatText(txtSearchFROM_AIRP.Value.Trim()).ToUpper());
            //where += " AND ACTION ='TangChuyen' ";
            return HttpUtility.UrlEncode(where);
        }



        #endregion

        #region grd event

        protected void grdSource_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            switch (e.CommandName)
            {
                case "DeleteByID":
                    var kq = new PermNoImpDAL().DeletePermNoIMP(id.ToString());
                    if (kq) this.AlertMessage("Delete sussess!");
                    else this.AlertMessage("Delete error!");
                    LoadData();
                    break;
                case "Edit":
                    Response.Redirect("~/PermIMP/EditPermNoImp.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString() + "&ID=" + id.ToString());
                    break;
            }
        }
        protected void grdSource_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.TableSection = TableRowSection.TableHeader;
        }

        #endregion

        #region pdf excel 
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter htw = new HtmlTextWriter(sw);
            //GridView ax = grdSource;
            //ax.Columns[ax.Columns.Count - 1].Visible = false;
            //if (!String.IsNullOrEmpty(txtSearch_UserName.Text.Trim()))
            //    ax.DataSource = new PermScImpDAL().GetPagePermScIMPExport(GetWhereCondition());
            //else
            //    ax.DataSource = new PermScImpDAL().GetAllPermSCIMP();
            //ax.DataBind();
            //ax.RenderControl(htw);
            //string html = sw.ToString();
            //this.CreateExcel(html, "LISTPERMSC_" + DateTime.Now.ToFileTime() + ".xls");
        }

        protected void GridView_PreRender(object sender, EventArgs e)
        {
            GridView gv = (GridView)sender;

            if ((gv.ShowHeader == true && gv.Rows.Count > 0)
                || (gv.ShowHeaderWhenEmpty == true))
            {
                //Force GridView to use <thead> instead of <tbody> - 11/03/2013 - MCR.
                gv.HeaderRow.TableSection = TableRowSection.TableHeader;

            }
            if (gv.ShowFooter == true && gv.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody> - 11/03/2013 - MCR.
                gv.FooterRow.TableSection = TableRowSection.TableFooter;
            }

        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
        #endregion

        protected void PhanTrang1_Paging_IndexChange(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void lnkBindToPerm_Click(object sender, EventArgs e)
        {
            var ax = Convert.ToInt64(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PermNo_BindToPerm", new { P_OPER = txtOper.Value.ToUpper() }).ToString());
            linkSearch_Click(sender, e);
        }


        #region 02082018 change new
        protected void lblDelete_Click(object sender, EventArgs e)
        {
            var id = ((LinkButton)sender).Attributes["data-ID"].ToString();
            var kq = new PermNoImpDAL().DeletePermNoIMP(id.ToString());
            if (kq) this.AlertMessage("Delete sussess!");
            else this.AlertMessage("Delete error!");
            LoadData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            linkSearch_Click(sender, e);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int kq = 0;
            foreach (RepeaterItem item in rptSource.Items)
            {
                var txtCALLSIGN = (System.Web.UI.HtmlControls.HtmlInputText)item.FindControl("txtCALLSIGN");
                var txtREGISTRATION = (System.Web.UI.HtmlControls.HtmlInputText)item.FindControl("txtREGISTRATION");
                //var txtDAYFLY = (System.Web.UI.HtmlControls.HtmlInputText)item.FindControl("txtDAYFLY");
                var txtCRAFT = (System.Web.UI.HtmlControls.HtmlInputText)item.FindControl("txtCRAFT");
                var txtFROM_AIRP = (System.Web.UI.HtmlControls.HtmlInputText)item.FindControl("txtFROM_AIRP");
                var txtTO_AIRP = (System.Web.UI.HtmlControls.HtmlInputText)item.FindControl("txtTO_AIRP");
                var txtETD = (System.Web.UI.HtmlControls.HtmlInputText)item.FindControl("txtETD");
                var txtETA = (System.Web.UI.HtmlControls.HtmlInputText)item.FindControl("txtETA");
                var txtVIA = (System.Web.UI.HtmlControls.HtmlInputText)item.FindControl("txtVIA");
                var txtPERMTYPE = (System.Web.UI.HtmlControls.HtmlInputText)item.FindControl("txtPERMTYPE");
                var txtPERMNBR = (System.Web.UI.HtmlControls.HtmlInputText)item.FindControl("txtPERMNBR");
                var txtREMARK = (System.Web.UI.HtmlControls.HtmlInputText)item.FindControl("txtREMARK");
                var lblDelete = (LinkButton)item.FindControl("lblDelete");
                PermNoIMP _obj = new PermNoIMP();
                var id = lblDelete.Attributes["data-ID"].ToString();
                _obj.ID = Convert.ToInt64(id);
                _obj.CALLSIGN = txtCALLSIGN.Value;
                //_obj.DAYFLY = Convert.ToDateTime(txtDAYFLY.Value);
                _obj.TO_AIRP = txtTO_AIRP.Value;
                _obj.PERMTYPE = txtPERMTYPE.Value;
                _obj.CRAFT = txtCRAFT.Value;
                _obj.REMARK = txtREMARK.Value;
                _obj.ETA = txtETA.Value;
                _obj.ETD = txtETD.Value;
                _obj.FROM_AIRP = txtFROM_AIRP.Value;
                _obj.PERMNBR = txtPERMNBR.Value;
                _obj.VIA = txtVIA.Value;
                _obj.REGISTRATION = txtREGISTRATION.Value;
                var ax = new PermNoImpDAL().UpdatePermNoIMPAsync(_obj);
                if (ax) kq++;
            }
            this.AlertMessage($"Update sussess: {kq}/{rptSource.Items.Count}.");
            LoadData();
        }

        protected void lnkDeleteBy_Click(object sender, EventArgs e)
        {
            var ax = Convert.ToInt64(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "DeleteImpNoByOper", new { P_OPER = txtOper.Value.ToUpper() }));
            if (ax == 1)
                this.AlertMessage("Delete by oper:" + txtOper.Value + " sussess!");
            else this.AlertMessage("Delete by oper:" + txtOper.Value + " error!");
            LoadData();
        }

        #endregion


    }
}