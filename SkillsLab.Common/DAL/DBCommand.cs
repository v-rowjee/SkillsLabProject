using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SkillsLabProject.Common.DAL
{
    public class DBCommand
    {
        private static async Task<SqlDataReader> ExecuteReaderAsync(string query, List<SqlParameter> parameters = null)
        {
            DAL dal = new DAL();
            SqlCommand command = new SqlCommand(query, dal.Connection);

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters.ToArray());
            }

            command.CommandType = CommandType.Text;
            await dal.OpenConnectionAsync().ConfigureAwait(false);
            return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection).ConfigureAwait(false);
        }

        private static async Task<bool> ExecuteNonQueryAsync(string query, List<SqlParameter> parameters = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                DAL dal = new DAL();
                SqlCommand command = new SqlCommand(query, dal.Connection);

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }

                command.CommandType = commandType;
                await dal.OpenConnectionAsync().ConfigureAwait(false);
                int rowsAffected = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                dal.CloseConnection();

                return rowsAffected > 0;
            }
            catch
            {
                return false;
                throw;
            }
        }

        public static Task<SqlDataReader> GetDataAsync(string query)
        {
            return ExecuteReaderAsync(query);
        }

        public static Task<SqlDataReader> GetDataWithConditionAsync(string query, List<SqlParameter> parameters)
        {
            return ExecuteReaderAsync(query, parameters);
        }

        public static Task<bool> InsertDataAsync(string query, List<SqlParameter> parameters)
        {
            return ExecuteNonQueryAsync(query, parameters);
        }

        public static Task<bool> UpdateDataAsync(string query, List<SqlParameter> parameters)
        {
            return ExecuteNonQueryAsync(query, parameters);
        }

        public static Task<bool> DeleteDataAsync(string query, SqlParameter parameter)
        {
            return ExecuteNonQueryAsync(query, new List<SqlParameter> { parameter });
        }

        public static Task<bool> ExecuteStoredProcedureAsync(string storedProcedureName, List<SqlParameter> parameters)
        {
            return ExecuteNonQueryAsync(storedProcedureName, parameters, CommandType.StoredProcedure);
        }
    }

}