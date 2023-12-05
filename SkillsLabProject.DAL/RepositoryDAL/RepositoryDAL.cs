﻿using SkillsLabProject.Common.DAL;
using SkillsLabProject.Common.Exceptions;
using SkillsLabProject.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.DAL.RepositoryDAL
{
    public class RepositoryDAL<T> : IRepositoryDAL<T>
    {
        public ResultModel<T> Add(T model)
        {
            ResultModel<T> result = new ResultModel<T>();

            try
            {
                DataAccessLayer dal = new DataAccessLayer();
                int rowAdded = dal.Insert(model);
                result.SetAddedRows(rowAdded);
                dal.CloseConnection();
            }
            catch (Exception error)
            {
                result.AddValidationResult(new ValidationResult()
                {
                    ErrorMessage = $"Unable to add {model.GetType()}",
                    Success = false,
                    Exception = error
                });
                CustomException exception = new CustomException(error.Message, error);
                exception.Log();
            }
            return result;
        }

        public ResultModel<T> Delete(int id)
        {
            ResultModel<T> result = new ResultModel<T>();

            try
            {
                DataAccessLayer dal = new DataAccessLayer();
                int rowDeleted = dal.Delete(id);
                result.SetDeleteRows(rowDeleted);
                dal.CloseConnection();
            }
            catch (Exception error)
            {
                result.AddValidationResult(new ValidationResult()
                {
                    ErrorMessage = $"Unable to delete with id: {id}",
                    Success = false,
                    Exception = error
                });
                CustomException customException = new CustomException(error.Message, error);
                customException.Log();
            }
            return result;
        }

        public ResultModel<T> Get(int id)
        {
            throw new NotImplementedException();
        }

        public ResultModel<T> Get(string query, List<SqlParameter> parameters)
        {
            ResultModel<T> result = new ResultModel<T>();
            DataAccessLayer dal = new DataAccessLayer();
            var results = dal.GetData<T>(query, parameters);
            result.SetModelList(results);
            return result;
        }

        public ResultModel<T> GetAll()
        {
            ResultModel<T> result = new ResultModel<T>();
            DataAccessLayer dal = new DataAccessLayer();
            var results = dal.GetData<T>();
            result.SetModelList(results);
            return result;
        }

        public ResultModel<T> Update(T model)
        {
            ResultModel<T> result = new ResultModel<T>();
            DataAccessLayer dal = new DataAccessLayer();
            var results = dal.Update(model);
            result.SetUpdatedRows(results);
            return result;
        }
    }
}
