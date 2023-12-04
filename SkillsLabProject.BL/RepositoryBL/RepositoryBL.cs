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
            var repository = new RepositoryDAL<T>();
            var result = repository.Add(entity);
            return result;
        }

        public ResultModel<T> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ResultModel<T> Get(int id)
        {
            throw new NotImplementedException();
        }

        public ResultModel<T> Get(string sql, List<SqlParameter> parameters)
        {
            throw new NotImplementedException();
        }

        public ResultModel<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public ResultModel<T> Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
