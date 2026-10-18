using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using Oracle.ManagedDataAccess.Client;
using prjComponents;

namespace prjApplication.Static.Craft
{
    public partial class EditCRAFT : PageCoreAdmin
    {
        private int RecordId
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["ID"], out id) ? id : 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (RecordId > 0)
            {
                btnSave.Enabled = _Role.R_Edit;
                LoadCraft();
            }
            else
            {
                btnSave.Enabled = _Role.R_Add;
            }
        }

        private void LoadCraft()
        {
            try
            {
                using (var connection = new OracleConnection(ConfigurationManager.ConnectionStrings["SlotsOracle"].ConnectionString))
                using (var command = new OracleCommand("SELECT CRAFT_ICAO, CRAFT_IATA, CRAFT_NAME FROM M_CRAFT WHERE ID = :id", connection))
                {
                    command.BindByName = true;
                    command.Parameters.Add("id", OracleDbType.Int32).Value = RecordId;
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            lblResult.InnerText = "Craft record not found.";
                            btnSave.Enabled = false;
                            return;
                        }
                        txtCraftIcao.Text = reader["CRAFT_ICAO"] as string ?? string.Empty;
                        txtCraftIata.Text = reader["CRAFT_IATA"] as string ?? string.Empty;
                        txtCraftName.Text = reader["CRAFT_NAME"] as string ?? string.Empty;
                    }
                }
            }
            catch
            {
                lblResult.InnerText = "Unable to load craft data.";
                btnSave.Enabled = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool isUpdate = RecordId > 0;
            if (isUpdate ? !_Role.R_Edit : !_Role.R_Add) return;

            try
            {
                string sql = isUpdate
                    ? "UPDATE M_CRAFT SET CRAFT_ICAO = :icao, CRAFT_IATA = :iata, CRAFT_NAME = :name WHERE ID = :id"
                    : "INSERT INTO M_CRAFT (CRAFT_ICAO, CRAFT_IATA, CRAFT_NAME) VALUES (:icao, :iata, :name)";
                using (var connection = new OracleConnection(ConfigurationManager.ConnectionStrings["SlotsOracle"].ConnectionString))
                using (var command = new OracleCommand(sql, connection))
                {
                    command.BindByName = true;
                    command.Parameters.Add("icao", OracleDbType.Varchar2, 200).Value = (object)txtCraftIcao.Text.Trim() ?? DBNull.Value;
                    command.Parameters.Add("iata", OracleDbType.Varchar2, 200).Value = (object)txtCraftIata.Text.Trim() ?? DBNull.Value;
                    command.Parameters.Add("name", OracleDbType.Varchar2, 200).Value = (object)txtCraftName.Text.Trim() ?? DBNull.Value;
                    if (isUpdate) command.Parameters.Add("id", OracleDbType.Int32).Value = RecordId;
                    connection.Open();
                    int affected = command.ExecuteNonQuery();
                    lblResult.InnerText = affected > 0 ? (isUpdate ? "Update success!" : "Insert success!") : "No record was changed.";
                }
            }
            catch
            {
                lblResult.InnerText = "Save failed. Please check the values and try again.";
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Static/Craft/ListCRAFT.aspx?Menu_ID=" + HttpUtility.UrlEncode(Request["Menu_ID"] ?? string.Empty));
        }
    }
}
