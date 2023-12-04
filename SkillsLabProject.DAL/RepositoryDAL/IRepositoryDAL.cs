using SkillsLabProject.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.DAL.RepositoryDAL
{
    public interface IRepositoryDAL<T>
    {
        ResultModel<T> GetAll();
        ResultModel<T> Get(int id);
        ResultModel<T> Get(string query, List<SqlParameter> parameters);
        ResultModel<T> Add(T entity);
        ResultModel<T> Update(T entity);
        ResultModel<T> Delete(int id);
    }
}
