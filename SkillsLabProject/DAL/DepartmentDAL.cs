using SkillsLabProject.Models;
using SkillsLabProject.DAL.Common;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
namespace SkillsLabProject.DAL
{
    public interface IDepartmentDAL : IDAL<DepartmentModel>
    {
    }
    public class DepartmentDAL : IDepartmentDAL
    {
        public bool Add(DepartmentModel model)
        {
            const string AddDepartmentQuery = @"
                INSERT [dbo].[Department] (DepartmentId, Title) VALUES (@DepartmentId, @Title);
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@DepartmentId", model.DepartmentId),
                new SqlParameter("@Title", model.Title)
            };
            return DBCommand.InsertUpdateData(AddDepartmentQuery, parameters);
        }
        public bool Delete(int DepartmentId)
        {
            const string DeleteDepartmentQuery = @"
                DELETE FROM [dbo].[Department] WHERE DepartmentId=@DepartmentId
            ";
            var parameter = new SqlParameter("@DepartmentId", DepartmentId);
            return DBCommand.DeleteData(DeleteDepartmentQuery, parameter);
        }
        public IEnumerable<DepartmentModel> GetAll()
        {
            const string GetAllDepartmentsQuery = @"
                SELECT DepartmentId, Title
                FROM [dbo].[Department]
            ";
            var dt = DBCommand.GetData(GetAllDepartmentsQuery);
            var Departments = new List<DepartmentModel>();
            DepartmentModel Department;
            foreach (DataRow row in dt.Rows)
            {
                Department = new DepartmentModel();
                Department.DepartmentId = int.Parse(row["DepartmentId"].ToString());
                Department.Title = row["Title"].ToString();

                Departments.Add(Department);
            }
            return Departments;
        }
        public DepartmentModel GetById(int DepartmentId)
        {
            const string GetDepartmentQuery = @"
                SELECT DepartmentId, Title
                FROM [dbo].[Department]
                WHERE [DepartmentId] = @DepartmentId
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@DepartmentId", DepartmentId)
            };
            var dt = DBCommand.GetDataWithCondition(GetDepartmentQuery, parameters);
            var Department = new DepartmentModel();
            foreach (DataRow row in dt.Rows)
            {
                Department.DepartmentId = int.Parse(row["DepartmentId"].ToString());
                Department.Title = row["Title"].ToString();
            }
            return Department;
        }
        public bool Update(DepartmentModel model)
        {
            const string UpdateDepartmentQuery = @"
                UPDATE [dbo].[Department]
                SET DepartmentId=@DepartmentId, Title=@Title
                WHERE DepartmentId=@DepartmentId;
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@DepartmentId", model.DepartmentId),
                new SqlParameter("@Title", model.Title)
            };
            return DBCommand.InsertUpdateData(UpdateDepartmentQuery, parameters);
        }
    }
}