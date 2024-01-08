using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SkillsLabProject.Common.DAL
{
    public class DAL
    {
        private readonly string _connectionString;

        public SqlConnection Connection;

        public DAL()
        {
            _connectionString = ConfigurationManager.AppSettings["DBConnectionString"].ToString();
            Connection = new SqlConnection(_connectionString);
        }

        public async Task OpenConnectionAsync()
        {
            if (Connection.State == ConnectionState.Open)
            {
                Connection.Close();
            }
            await Connection.OpenAsync();
        }

        public async Task OpenConnectionAsync(string connectionString)
        {

            Connection = new SqlConnection(connectionString);
            if (Connection.State == ConnectionState.Open)
            {
                Connection.Close();
            }

            await Connection.OpenAsync();
        }

        public void CloseConnection()
        {
            if (Connection != null && Connection.State == ConnectionState.Open)
            {
                Connection.Close();
                Connection.Dispose();
            }
        }
    }

}