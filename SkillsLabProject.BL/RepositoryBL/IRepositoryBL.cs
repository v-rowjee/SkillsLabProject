using SkillsLabProject.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.BL.RepositoryBL
{
    public interface IRepositoryBL<T>
    {
        ResultModel<T> GetAll();
        ResultModel<T> Get(int id);
        ResultModel<T> Get(string sql, List<SqlParameter> parameters);
        ResultModel<T> Add(T entity);
        ResultModel<T> Update(T entity);
        ResultModel<T> Delete(int id);
    }
}
