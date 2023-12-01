using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace SkillsLabProject.DAL.Common
{
    public class DBCommand
    {
        public static DataTable GetData(string query)
        {
            DAL dal = new DAL();
            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand(query, dal.Connection))
            {
                cmd.CommandType = CommandType.Text;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }
            }

            dal.CloseConnection();

            return dt;
        }

        public static bool InsertUpdateData(string query, List<SqlParameter> parameters)
        {
            try
            {
                DAL dal = new DAL();
                int rowsAffected = 0;

                using (SqlCommand cmd = new SqlCommand(query, dal.Connection))
                {
                    cmd.CommandType = CommandType.Text;
                    if (parameters != null)
                    {
                        parameters.ForEach(parameter =>
                        {
                            cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                        });
                    }
                    rowsAffected = cmd.ExecuteNonQuery();
                }
                dal.CloseConnection();

                return rowsAffected > 0;
            }
            catch
            {
                return false;
            }

        }

        public static DataTable GetDataWithCondition(string query, List<SqlParameter> parameters)
        {
            DAL dal = new DAL();
            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand(query, dal.Connection))
            {
                cmd.CommandType = CommandType.Text;
                if (parameters != null)
                {
                    parameters.ForEach(parameter =>
                    {
                        cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                    });
                }
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }

            }

            dal.CloseConnection();

            return dt;
        }

        public static bool DeleteData(string query, SqlParameter parameter)
        {
            try
            {
                DAL dal = new DAL();
                int rowsAffected = 0;

                using (SqlCommand cmd = new SqlCommand(query, dal.Connection))
                {
                    cmd.CommandType = CommandType.Text;
                    if (parameter != null)
                    {
                        cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                    }
                    rowsAffected = cmd.ExecuteNonQuery();
                }
                dal.CloseConnection();
                return rowsAffected > 0;
            }
            catch
            {
                return false;
            }
        }

    }
}