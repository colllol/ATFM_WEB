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
    public partial class ATMGetData : PageCoreAdmin
    {
        private const string _AliasSession = "ATMGET";
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

                txtBEGINDATE.Value = DateTime.Now.ToString("dd/MM/yyyy");
                LoadData();
                LoadDataNam();
                this.PopulateTreeView(DateTime.Now.Year, 0, null);
            }
        }

        private void LoadDataNam()
        {

            for (int year = 2018; year <= 2030; year++)
            {
                cboNam.Items.Add(year.ToString());
            }
        }
        private TreeNode AddNodeAndDescendents(int year, int month)
        {
            string m;
            if (month < 10)
            {
                m = "0" + month.ToString();
            }
            else m = month.ToString();
            TreeNode node = new TreeNode
            {
                Text = "Tháng " + m,
                Value = m
            };

            for (int day = 1; day <= System.DateTime.DaysInMonth(year, month); day++)
            {
                string d;
                if (day < 10)
                {
                    d = "0" + day.ToString();
                }
                else d = day.ToString();
                TreeNode child = new TreeNode
                {
                    Text = "Ngày" + d.ToString(),
                    Value = d.ToString()
                };
                node.ChildNodes.Add(child);
            }
            return node;
        }
        private void PopulateTreeView(int year, int parentId, TreeNode treeNode)
        {

            for (int month = 1; month <= 12; month++)
            {
                var root = AddNodeAndDescendents(year, month);
                TreeView1.Nodes.Add(root);
            }
            TreeView1.CollapseAll();
        }
        #region function

        protected void TreeViewTabs_SelectedNodeChanged(object sender, EventArgs e)
        {
            //LoadData();
            string _t = TreeView1.SelectedNode.Value;
            string _v = TreeView1.SelectedNode.Value;
            string _y = cboNam.SelectedValue;

            if (TreeView1.SelectedNode.Text.IndexOf("Ngày") == 0)
            {
                string _m = TreeView1.SelectedNode.Parent.Value;
                string _d = TreeView1.SelectedNode.Value;
                string _pathdate = _y + "_" + _m + "_" + _d;
                LoadData(_pathdate);
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        public void LoadData(string _pathdate)
        {
            UserDAL _obj = new UserDAL();
            string _area = ddlKV.SelectedValue.ToString();
            DataTable t = _obj.getATM(_pathdate, _area);
            grdSource.DataSource = t;
            grdSource.DataBind();
        }
        public void LoadData()
        {
            //List<prjInfo.Document> t = new DocumentDAL().GetListPage(CustomPaging1.PageSize, CustomPaging1.PageIndex, CustomPaging1.ValueSearch);
            //CustomPaging1.TotalsRecord = t.GetTotalRecord<prjInfo.Document>();
            //grdSource.DataSource = t;
            // grdSource.DataBind();
            string _date = txtBEGINDATE.Value;

            string _d = _date.Substring(0, 2);
            string _m = _date.Substring(3, 2);
            string _y = _date.Substring(6, 4);
            string _pathdate = _y + "_" + _m + "_" + _d;

            UserDAL _obj = new UserDAL();
            //DataTable t = _obj.getATM(_pathdate, "MB");
            string _area = ddlKV.SelectedValue.ToString();
            DataTable t = _obj.getATM(_pathdate, _area);
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
        public string _pathdate;
        protected void grdSource_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string _date = txtBEGINDATE.Value;
            //'2017_01_20'
            //17/05/2018
            string path = System.Configuration.ConfigurationManager.AppSettings["ATMPATH"];
            string _d = _date.Substring(0, 2);
            string _m = _date.Substring(3, 2);
            string _y = _date.Substring(6, 4);
            _pathdate = _y + "/" + _m + "/" + _d;
            UserDAL _obj = new UserDAL();
            string _pathdatesave = _y + "_" + _m + "_" + _d;
            string Name = e.CommandArgument.ToString();
            switch (e.CommandName)
            {
                case "DeleteByID":
                    // Response.Redirect(path + "MB/" + _pathdate + "/" + Name.ToString());                   
                    string _r = System.Configuration.ConfigurationManager.AppSettings["FileUploadPathMB"];
                    string filePath = _r + "\\" + _y + "\\" + _m + "\\" + _d + "\\" + Name.ToString();
                    File.Delete(filePath);
                    string sFileExt = "*.*";

                    _obj.DeleATMExp(Name.ToString(), "MB", _pathdatesave);
                    LoadData();

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
