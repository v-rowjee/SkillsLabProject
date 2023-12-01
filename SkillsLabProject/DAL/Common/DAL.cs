using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SkillsLabProject.DAL.Common
{
    public class DAL
    {
        // public const string connectionString = @"server=jedicarpooldbserver.database.windows.net;database=JEDICarpool_db;uid=wbpoc;pwd=sql@tfs2008";
        private readonly string _connectionString;

        public SqlConnection Connection;

        public DAL()
        {
            _connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Connection = new SqlConnection(_connectionString);
            OpenConnection();
        }

        public void OpenConnection()
        {
            try
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }

                Connection.Open();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void OpenConnection(string connectionString)
        {
            try
            {
                Connection = new SqlConnection(connectionString);
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }

                Connection.Open();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
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