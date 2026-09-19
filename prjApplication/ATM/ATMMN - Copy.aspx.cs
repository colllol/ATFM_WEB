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
using System.Data;

namespace prjApplication.ATM
{
    public partial class ATMMN : PageCoreAdmin
    {
        private const string _AliasSession = "ATMMN";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                  
                    Session.SetValueForControlSearch(this, _AliasSession);
                }
                catch
                {
                    Session.SetValueSearch(_AliasSession, " 1=1");
                }
                LoadData();
                txtBEGINDATE.Value = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }

        #region function
        public void LoadData()
        {
            //List<prjInfo.Document> t = new DocumentDAL().GetListPage(CustomPaging1.PageSize, CustomPaging1.PageIndex, CustomPaging1.ValueSearch);
            //CustomPaging1.TotalsRecord = t.GetTotalRecord<prjInfo.Document>();
            //grdSource.DataSource = t;
            // grdSource.DataBind();
            UserDAL _obj = new UserDAL();
            DataTable t = _obj.getATMExp("1");
            grdSource.DataSource = t;
            grdSource.DataBind();
        }
        private void PutclsSearchDetail()
        {
          
        }


        #endregion

        #region control-event
        protected void pages_IndexChanged(object sender, EventArgs e)
        {

            LoadData();
        }

        protected void linkSearch_Click(object sender, EventArgs e)
        {
            
        }

       

        protected void btnAddCountry_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Static/Document/EditDocument.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }

        #endregion

        #region grd event

        protected void grdSource_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string _date = txtBEGINDATE.Value;
            //'2017_01_20'
            //17/05/2018
            string path = System.Configuration.ConfigurationManager.AppSettings["ATMPATH"];
            string _d = _date.Substring(0, 2);
            string _m = _date.Substring(3, 2);
            string _y = _date.Substring(6, 4);
            string _pathdate = _y + "_" + _m + "_" + _d;

            string Name = e.CommandArgument.ToString();
            switch (e.CommandName)
            {
                case "DeleteByID":
                    Response.Redirect(path + "MN/" + _pathdate + "/" + Name.ToString() + ".xls");                   
                    break;
                case "Edit":
                    //Response.Redirect("~/Static/Document/EditDocument.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString() + "&ID=" + id.ToString());
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

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
           
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
    }
}
