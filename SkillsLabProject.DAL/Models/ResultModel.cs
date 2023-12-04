using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.DAL.Models
{
    public class ResultModel<T>
    {
        private readonly List<ValidationResult> ValidationResults;
        private List<T> ModelList;
        private T Model;
        private int AddedRows;
        private int UpdatedRows;
        private int DeletedRows;

        public ResultModel()
        {
            ValidationResults = new List<ValidationResult>();
            ModelList = new List<T>();
            Model = Activator.CreateInstance<T>();
        }

        public bool IsSuccess => ValidationResults.Count == 0;
        public void AddValidationResult(ValidationResult validationResult) => ValidationResults.Add(validationResult);
        public List<ValidationResult> GetValidationResults() => ValidationResults;

        public List<T> GetModelList() => ModelList;
        public List<T> SetModelList(List<T> modelList) => ModelList = modelList;

        public T GetModel() => Model;
        public T SetModel(T model) => Model = model;

        public int SetAddedRows(int rows) => AddedRows = rows;
        public int SetUpdatedRows(int rows) => UpdatedRows = rows;
        public int SetDeleteRows(int rows) => DeletedRows = rows;

        public int GetAddedRows() => AddedRows;
        public int GetUpdatedRows() => UpdatedRows;
        public int GetDeletedRows() => DeletedRows;

    }
}
