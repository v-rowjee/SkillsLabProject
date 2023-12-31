﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SkillsLabProject.Common.DAL
{
    public class DBCommand
    {
        public static DataTable GetData(string query)
        {
            DAL dal = new DAL();
            DataTable dataTable = new DataTable();
            using (SqlCommand command = new SqlCommand(query, dal.Connection))
            {
                command.CommandType = CommandType.Text;
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                {
                    dataAdapter.Fill(dataTable);
                }
            }
            dal.CloseConnection();
            return dataTable;
        }
        public static DataTable GetDataWithCondition(string query, List<SqlParameter> parameters)
        {
            DAL dal = new DAL();
            DataTable dt = new DataTable();
            using (SqlCommand command = new SqlCommand(query, dal.Connection))
            {
                command.CommandType = CommandType.Text;
                if (parameters != null)
                {
                    parameters.ForEach(parameter =>
                    {
                        command.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                    });
                }
                using (SqlDataAdapter sda = new SqlDataAdapter(command))
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
                using (SqlCommand command = new SqlCommand(query, dal.Connection))
                {
                    command.CommandType = CommandType.Text;
                    if (parameters != null)
                    {
                        parameters.ForEach(parameter =>
                        {
                            command.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                        });
                    }
                    rowsAffected = command.ExecuteNonQuery();
                }
                dal.CloseConnection();
                return rowsAffected > 0;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public static bool DeleteData(string query, SqlParameter parameter)
        {
            try
            {
                DAL dal = new DAL();
                int rowsAffected = 0;
                using (SqlCommand command = new SqlCommand(query, dal.Connection))
                {
                    command.CommandType = CommandType.Text;
                    if (parameter != null)
                    {
                        command.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                    }
                    rowsAffected = command.ExecuteNonQuery();
                }
                dal.CloseConnection();
                return rowsAffected > 0;
            }
            catch
            {
                return false;
                throw;
            }
        }

        public static bool ExecuteStoredProcedure(string storedProcedureName, List<SqlParameter> parameters)
        {
            try
            {
                DAL dal = new DAL();
                int rowsAffected = 0;
                using (SqlCommand command = new SqlCommand(storedProcedureName, dal.Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        parameters.ForEach(parameter =>
                        {
                            command.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                        });
                    }
                    rowsAffected = command.ExecuteNonQuery();
                }
                dal.CloseConnection();
                return rowsAffected > 0;
            }
            catch
            {
                return false;
                throw;
            }
        }

    }
}