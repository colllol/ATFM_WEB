using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CustomControl;
using Oracle.ManagedDataAccess.Client;
using prjComponents;

namespace prjApplication.Static.Craft
{
    public partial class ListCRAFT : PageCoreAdmin
    {
        private const string SearchSessionKey = "ListCRAFT.Search";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtSearch.Text = Session[SearchSessionKey] as string ?? string.Empty;
                btnAdd.Enabled = _Role.R_Add;
                LoadData();
            }
        }

        private string CurrentSearch
        {
            get { return (txtSearch.Text ?? string.Empty).Trim(); }
        }

        private void LoadData()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["SlotsOracle"].ConnectionString;
                string predicate = "(:search IS NULL OR UPPER(CRAFT_ICAO) LIKE :search OR UPPER(CRAFT_IATA) LIKE :search OR UPPER(CRAFT_NAME) LIKE :search)";
                using (var connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    using (var countCommand = new OracleCommand("SELECT COUNT(*) FROM M_CRAFT WHERE " + predicate, connection))
                    {
                        countCommand.BindByName = true;
                        AddSearchParameter(countCommand, CurrentSearch);
                        CustomPaging1.TotalsRecord = Convert.ToInt32(countCommand.ExecuteScalar());
                    }

                    int pageIndex = CustomPaging1.PageIndex;
                    int pageSize = CustomPaging1.PageSize;
                    int offset = pageIndex * pageSize;
                    const string sql = "SELECT ID, CRAFT_ICAO, CRAFT_IATA, CRAFT_NAME FROM M_CRAFT WHERE "
                        + "(:search IS NULL OR UPPER(CRAFT_ICAO) LIKE :search OR UPPER(CRAFT_IATA) LIKE :search OR UPPER(CRAFT_NAME) LIKE :search) "
                        + "ORDER BY CRAFT_ICAO, ID OFFSET :offset ROWS FETCH NEXT :pageSize ROWS ONLY";
                    using (var command = new OracleCommand(sql, connection))
                    using (var adapter = new OracleDataAdapter(command))
                    {
                        command.BindByName = true;
                        AddSearchParameter(command, CurrentSearch);
                        command.Parameters.Add("offset", OracleDbType.Int32).Value = offset;
                        command.Parameters.Add("pageSize", OracleDbType.Int32).Value = pageSize;
                        var data = new DataTable();
                        adapter.Fill(data);
                        grdSource.DataSource = data;
                        grdSource.DataBind();
                    }
                }
            }
            catch
            {
                grdSource.DataSource = null;
                grdSource.DataBind();
                this.AlertMessage("Unable to load M_CRAFT data.");
            }
        }

        private static void AddSearchParameter(OracleCommand command, string search)
        {
            OracleParameter parameter = command.Parameters.Add("search", OracleDbType.Varchar2, 500, ParameterDirection.Input);
            parameter.Value = string.IsNullOrWhiteSpace(search)
                ? (object)DBNull.Value
                : "%" + search.ToUpperInvariant() + "%";
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Session[SearchSessionKey] = CurrentSearch;
            CustomPaging1.PageIndex = 0;
            LoadData();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (!_Role.R_Add) return;
            Response.Redirect("~/Static/Craft/EditCRAFT.aspx?Menu_ID=" + HttpUtility.UrlEncode(Request["Menu_ID"] ?? string.Empty));
        }

        protected void grdSource_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out id)) return;

            if (e.CommandName == "EditByID")
            {
                if (!_Role.R_Edit) return;
                Response.Redirect("~/Static/Craft/EditCRAFT.aspx?Menu_ID="
                    + HttpUtility.UrlEncode(Request["Menu_ID"] ?? string.Empty) + "&ID=" + id);
            }
            else if (e.CommandName == "DeleteByID")
            {
                if (!_Role.R_Del) return;
                try
                {
                    using (var connection = new OracleConnection(ConfigurationManager.ConnectionStrings["SlotsOracle"].ConnectionString))
                    using (var command = new OracleCommand("DELETE FROM M_CRAFT WHERE ID = :id", connection))
                    {
                        command.BindByName = true;
                        command.Parameters.Add("id", OracleDbType.Int32).Value = id;
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    this.AlertMessage("Delete success!");
                }
                catch
                {
                    this.AlertMessage("Delete failed. Check whether this craft is referenced by other data.");
                }
                LoadData();
            }
        }

        protected void grdSource_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.TableSection = TableRowSection.TableHeader;
        }

        protected void pages_IndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                const string sql = "SELECT ID, CRAFT_ICAO, CRAFT_IATA, CRAFT_NAME FROM M_CRAFT "
                    + "WHERE (:search IS NULL OR UPPER(CRAFT_ICAO) LIKE :search OR UPPER(CRAFT_IATA) LIKE :search OR UPPER(CRAFT_NAME) LIKE :search) "
                    + "ORDER BY CRAFT_ICAO, ID";
                using (var connection = new OracleConnection(ConfigurationManager.ConnectionStrings["SlotsOracle"].ConnectionString))
                using (var command = new OracleCommand(sql, connection))
                using (var adapter = new OracleDataAdapter(command))
                {
                    command.BindByName = true;
                    AddSearchParameter(command, CurrentSearch);
                    var data = new DataTable();
                    adapter.Fill(data);
                    grdExport.DataSource = data;
                    grdExport.DataBind();
                }

                grdExport.Visible = true;
                this.CreateExcel(this.RenderToHTML(grdExport), "LIST_CRAFT_" + DateTime.Now.ToFileTime() + ".xls");
                grdExport.Visible = false;
            }
            catch
            {
                grdExport.Visible = false;
                this.AlertMessage("Export M_CRAFT data failed.");
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
        }
    }
}
