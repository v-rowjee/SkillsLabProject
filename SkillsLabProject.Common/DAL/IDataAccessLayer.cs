using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.Common.DAL
{
    public interface IDataAccessLayer
    {
        void OpenConnection();
        string OpenConnection(string connectionString);
        void CloseConnection();
        List<T> GetData<T>(string query, List<SqlParameter> parameters = null);
        int Insert<T>(T item);
        int Update<T>(T item);
        int Delete<T>(T item);
    }
}
