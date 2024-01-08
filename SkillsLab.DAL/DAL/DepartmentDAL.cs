using SkillsLabProject.Common.Enums;
using SkillsLabProject.Common.DAL;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using SkillsLabProject.Common.Models;
using System.Threading.Tasks;

namespace SkillsLabProject.DAL.DAL
{
    public interface IDepartmentDAL : IDAL<DepartmentModel>
    {
    }
    public class DepartmentDAL : IDepartmentDAL
    {
        public async Task<bool> AddAsync(DepartmentModel model)
        {
            const string AddDepartmentQuery = @"
                INSERT [dbo].[Department] (DepartmentId, Title) VALUES (@DepartmentId, @Title);
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@DepartmentId", model.DepartmentId),
                new SqlParameter("@Title", model.Title)
            };
            return await DBCommand.InsertDataAsync(AddDepartmentQuery, parameters).ConfigureAwait(false);
        }
        public async Task<bool> DeleteAsync(int DepartmentId)
        {
            const string DeleteDepartmentQuery = @"
                DELETE FROM [dbo].[Department] WHERE DepartmentId=@DepartmentId
            ";
            var parameter = new SqlParameter("@DepartmentId", DepartmentId);
            return await DBCommand.DeleteDataAsync(DeleteDepartmentQuery, parameter).ConfigureAwait(false);
        }
        public async Task<IEnumerable<DepartmentModel>> GetAllAsync()
        {
            const string GetAllDepartmentsQuery = @"
                SELECT DepartmentId, Title
                FROM [dbo].[Department]
            ";

            var Departments = new List<DepartmentModel>();

            using (SqlDataReader dataReader = await DBCommand.GetDataAsync(GetAllDepartmentsQuery).ConfigureAwait(false))
            {
                while (await dataReader.ReadAsync().ConfigureAwait(false))
                {
                    DepartmentModel Department = new DepartmentModel
                    {
                        DepartmentId = dataReader.GetByte(dataReader.GetOrdinal("DepartmentId")),
                        Title = dataReader["Title"].ToString()
                    };

                    Departments.Add(Department);
                }
            }

            return Departments;
        }

        public async Task<DepartmentModel> GetByIdAsync(int departmentId)
        {
            const string GetDepartmentQuery = @"
                SELECT DepartmentId, Title
                FROM [dbo].[Department]
                WHERE [DepartmentId] = @DepartmentId
            ";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@DepartmentId", departmentId)
            };

            var department = new DepartmentModel();

            using (SqlDataReader dataReader = await DBCommand.GetDataWithConditionAsync(GetDepartmentQuery, parameters).ConfigureAwait(false))
            {
                while (await dataReader.ReadAsync().ConfigureAwait(false))
                {
                    department.DepartmentId = dataReader.GetByte(dataReader.GetOrdinal("DepartmentId"));
                    department.Title = dataReader["Title"].ToString();
                }
            }

            return department;
        }

        public async Task<bool> UpdateAsync(DepartmentModel model)
        {
            const string UpdateDepartmentQuery = @"
                UPDATE [dbo].[Department]
                SET Title=@Title
                WHERE DepartmentId=@DepartmentId;
            ";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@DepartmentId", model.DepartmentId),
                new SqlParameter("@Title", model.Title)
            };

            return await DBCommand.UpdateDataAsync(UpdateDepartmentQuery, parameters).ConfigureAwait(false);
        }

    }
}