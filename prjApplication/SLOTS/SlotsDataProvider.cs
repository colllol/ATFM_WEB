using System.Configuration;
using System.Data;
using System.Linq;
using Oracle.ManagedDataAccess.Client;

namespace prjApplication.SLOTS
{
    internal sealed class SlotsDataProvider
    {
        private readonly string explicitConnectionString;

        public SlotsDataProvider()
        {
        }

        internal SlotsDataProvider(string connectionString)
        {
            explicitConnectionString = connectionString;
        }

        internal string ConnectionString
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(explicitConnectionString)) return explicitConnectionString;
                ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings["SlotsOracle"];
                if (setting == null || string.IsNullOrWhiteSpace(setting.ConnectionString))
                    throw new ConfigurationErrorsException("Thiếu connection string SlotsOracle trong Web.config.");
                return setting.ConnectionString;
            }
        }

        public DataTable ExecuteQuery(string commandText, params OracleParameter[] parameters)
        {
            DataTable table = new DataTable();
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (OracleCommand command = new OracleCommand(commandText, connection))
            using (OracleDataAdapter adapter = new OracleDataAdapter(command))
            {
                command.CommandType = CommandType.Text;
                command.BindByName = true;
                AddParameters(command, parameters);
                connection.Open();
                adapter.Fill(table);
            }
            return table;
        }

        public object ExecuteScalar(string commandText, params OracleParameter[] parameters)
        {
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (OracleCommand command = new OracleCommand(commandText, connection))
            {
                command.CommandType = CommandType.Text;
                command.BindByName = true;
                AddParameters(command, parameters);
                connection.Open();
                return command.ExecuteScalar();
            }
        }

        private static void AddParameters(OracleCommand command, OracleParameter[] parameters)
        {
            if (parameters == null) return;
            OracleParameter[] validParameters = parameters.Where(parameter => parameter != null).ToArray();
            if (validParameters.Length > 0) command.Parameters.AddRange(validParameters);
        }
    }
}
