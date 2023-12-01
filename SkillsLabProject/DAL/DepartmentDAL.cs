using SkillsLabProject.Models;
using SkillsLabProject.DAL.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.DAL
{
    public interface IDepartmentDAL : IDAL<DepartmentModel>
    {

    }
    public class DepartmentDAL : IDepartmentDAL
    {
        private const string AddDepartmentQuery = @"
            INSERT [dbo].[Department] (DepartmentId, Title) VALUES (@DepartmentId, @Title);
        ";
        private const string GetAllDepartmentsQuery = @"
            SELECT DepartmentId, Title
            FROM [dbo].[Department]
        ";
        private const string GetDepartmentQuery = @"
            SELECT DepartmentId, Title
            FROM [dbo].[Department]
            WHERE [DepartmentId] = @DepartmentId
        ";
        private const string UpdateDepartmentQuery = @"
            UPDATE [dbo].[Department]
            SET DepartmentId=@DepartmentId, Title=@Title
            WHERE DepartmentId=@DepartmentId;
        ";
        private const string DeleteDepartmentQuery = @"
            DELETE FROM [dbo].[Department] WHERE DepartmentId=@DepartmentId
        ";

        public bool Add(DepartmentModel model)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@DepartmentId", model.DepartmentId));
            parameters.Add(new SqlParameter("@Title", model.Title));


            var DepartmentInserted = DBCommand.InsertUpdateData(AddDepartmentQuery, parameters);

            return DepartmentInserted;
        }

        public bool Delete(int DepartmentId)
        {
            var parameter = new SqlParameter("@DepartmentId", DepartmentId);
            return DBCommand.DeleteData(DeleteDepartmentQuery, parameter);
        }

        public IEnumerable<DepartmentModel> GetAll()
        {
            var Departments = new List<DepartmentModel>();
            DepartmentModel Department;

            var dt = DBCommand.GetData(GetAllDepartmentsQuery);
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
            var Department = new DepartmentModel();
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@DepartmentId", DepartmentId));

            var dt = DBCommand.GetDataWithCondition(GetDepartmentQuery, parameters);
            foreach (DataRow row in dt.Rows)
            {
                Department.DepartmentId = int.Parse(row["DepartmentId"].ToString());
                Department.Title = row["Title"].ToString();
            }
            return Department;
        }

        public bool Update(DepartmentModel model)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@DepartmentId", model.DepartmentId));
            parameters.Add(new SqlParameter("@Title", model.Title));

            var DepartmentUpdated = DBCommand.InsertUpdateData(UpdateDepartmentQuery, parameters);

            return DepartmentUpdated;
        }
    }
}
