using SkillsLabProject.DAL.Models;
using SkillsLabProject.DAL.RepositoryDAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.BL.RepositoryBL
{
    public class RepositoryBL<T> : IRepositoryBL<T>
    {
        private readonly IRepositoryDAL<T> _repository;

        public RepositoryBL(IRepositoryDAL<T> repository)
        {
            _repository = repository;
        }

        public ResultModel<T> Add(T entity)
        {
            return _repository.Add(entity);
        }

        public ResultModel<T> Delete(int id)
        {
            return _repository.Delete(id);
        }

        public ResultModel<T> Get(int id)
        {
            return _repository.Get(id);
        }

        public ResultModel<T> Get(string sql, List<SqlParameter> parameters)
        {
            return _repository.Get(sql, parameters);
        }

        public ResultModel<T> GetAll()
        {
            return _repository.GetAll();
        }

        public ResultModel<T> Update(T entity)
        {
            return _repository.Update(entity);
        }
    }
}
