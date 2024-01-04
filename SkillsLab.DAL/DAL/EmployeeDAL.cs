using SkillsLabProject.Common.DAL;
using SkillsLabProject.Common.Enums;
using SkillsLabProject.Common.Models;
using SkillsLabProject.Common.Models.ViewModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SkillsLabProject.DAL.DAL
{
    public interface IEmployeeDAL
    {
        Task<bool> DeleteEmployeeAsync(int employeeId);
        Task<IEnumerable<EmployeeModel>> GetAllEmployeesAsync();
        Task<EmployeeModel> GetEmployeeAsync(LoginViewModel model);
        Task<EmployeeModel> GetEmployeeByIdAsync(int employeeId);
        Task<bool> UpdateEmployeeAsync(EmployeeModel employee);
        Task<List<Role>> GetUserRolesAsync(int employeeId);
    }
    public class EmployeeDAL : IEmployeeDAL
    {
        public async Task<bool> DeleteEmployeeAsync(int employeeId)
        {
            const string DeleteEmployeeQuery = @"
                DELETE FROM AppUser WHERE EmployeeId=@EmployeeId;
                DELETE FROM Enrollement WHERE EmployeeId=@EmployeeId;
                DELETE FROM Employee WHERE EmployeeId=@EmployeeId
            ";
            var parameter = new SqlParameter("@EmployeeId", employeeId);
            return await DBCommand.DeleteDataAsync(DeleteEmployeeQuery, parameter);
        }
        public async Task<IEnumerable<EmployeeModel>> GetAllEmployeesAsync()
        {
            const string GetAllEmployeesQuery = @"
                SELECT e.EmployeeId, FirstName, LastName, NIC, PhoneNumber, e.DepartmentId, a.Email, d.Title
                FROM [dbo].[Employee] as e
                INNER JOIN [dbo].[AppUser] as a ON e.EmployeeId = a.EmployeeId
                INNER JOIN [dbo].[Department] as d ON e.DepartmentId = d.DepartmentId
            ";

            var employees = new List<EmployeeModel>();

            using (SqlDataReader dataReader = await DBCommand.GetDataAsync(GetAllEmployeesQuery))
            {
                while (await dataReader.ReadAsync())
                {
                    var employee = new EmployeeModel
                    {
                        EmployeeId = dataReader.GetInt32(dataReader.GetOrdinal("EmployeeId")),
                        FirstName = dataReader["FirstName"].ToString(),
                        LastName = dataReader["LastName"].ToString(),
                        NIC = dataReader["NIC"].ToString(),
                        PhoneNumber = dataReader["PhoneNumber"].ToString(),
                        Email = dataReader["Email"].ToString(),
                        Department = new DepartmentModel
                        {
                            DepartmentId = dataReader.GetInt32(dataReader.GetOrdinal("DepartmentId")),
                            Title = dataReader["Title"].ToString()
                        },
                    };
                    employees.Add(employee);
                }
            }

            return employees;
        }

        public async Task<EmployeeModel> GetEmployeeAsync(LoginViewModel login)
        {
            const string GetEmployeeQuery = @"
                SELECT e.EmployeeId, e.FirstName, e.LastName, e.NIC, e.PhoneNumber, e.DepartmentId, a.Email, d.Title
                FROM [dbo].[Employee] as e
                INNER JOIN [dbo].[AppUser] as a ON e.EmployeeId = a.EmployeeId
                INNER JOIN [dbo].[Department] as d ON e.DepartmentId = d.DepartmentId
                WHERE a.[Email] = @Email
            ";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Email", login.Email)
            };

            var employee = new EmployeeModel();

            using (SqlDataReader dataReader = await DBCommand.GetDataWithConditionAsync(GetEmployeeQuery, parameters))
            {
                while (await dataReader.ReadAsync())
                {
                    employee.EmployeeId = dataReader.GetInt32(dataReader.GetOrdinal("EmployeeId"));
                    employee.FirstName = dataReader["FirstName"].ToString();
                    employee.LastName = dataReader["LastName"].ToString();
                    employee.NIC = dataReader["NIC"].ToString();
                    employee.PhoneNumber = dataReader["PhoneNumber"].ToString();
                    employee.Email = dataReader["Email"].ToString();
                    employee.Department = new DepartmentModel
                    {
                        DepartmentId = dataReader.GetInt32(dataReader.GetOrdinal("DepartmentId")),
                        Title = dataReader["Title"].ToString()
                    };
                }
            }
            return employee;
        }


        public async Task<EmployeeModel> GetEmployeeByIdAsync(int employeeId)
        {
            const string GetEmployeeQuery = @"
                SELECT e.EmployeeId, e.FirstName, e.LastName, e.NIC, e.PhoneNumber, e.DepartmentId, a.Email, d.Title
                FROM [dbo].[Employee] as e
                INNER JOIN [dbo].[AppUser] as a ON e.EmployeeId = a.EmployeeId
                INNER JOIN [dbo].[Department] as d ON e.DepartmentId = d.DepartmentId
                WHERE e.[EmployeeId] = @EmployeeId
            ";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@EmployeeId", employeeId)
            };

            var employee = new EmployeeModel();

            using (SqlDataReader dataReader = await DBCommand.GetDataWithConditionAsync(GetEmployeeQuery, parameters))
            {
                while (await dataReader.ReadAsync())
                {
                    employee.EmployeeId = dataReader.GetInt32(dataReader.GetOrdinal("EmployeeId"));
                    employee.FirstName = dataReader["FirstName"].ToString();
                    employee.LastName = dataReader["LastName"].ToString();
                    employee.NIC = dataReader["NIC"].ToString();
                    employee.PhoneNumber = dataReader["PhoneNumber"].ToString();
                    employee.Email = dataReader["Email"].ToString();
                    employee.Department = new DepartmentModel
                    {
                        DepartmentId = dataReader.GetInt32(dataReader.GetOrdinal("DepartmentId")),
                        Title = dataReader["Title"].ToString()
                    };
                }
            }
            return employee;
        }


        public async Task<bool> UpdateEmployeeAsync(EmployeeModel employee)
        {
            const string UpdateEmployeeQuery = @"
                UPDATE [dbo].[Employee] e
                INNER JOIN [dbo].[AppUser] as a ON e.EmployeeId = a.EmployeeId
                INNER JOIN [dbo].[Department] as d ON e.DepartmentId = d.DepartmentId
                SET FirstName=@FirstName, LastName=@LastName, NIC=@NIC, PhoneNumber=@PhoneNumber, e.DepartmentId=@DepartmentId, a.Email=@Email, d.Title=@Title
                WHERE EmployeeId=@EmployeeId;
            ";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@EmployeeId", employee.EmployeeId),
                new SqlParameter("@FirstName", employee.FirstName),
                new SqlParameter("@LastName", employee.LastName),
                new SqlParameter("@NIC", employee.NIC),
                new SqlParameter("@Email", employee.Email),
                new SqlParameter("@PhoneNumber", employee.PhoneNumber),
                new SqlParameter("@DepartmentId", employee.Department.DepartmentId),
                new SqlParameter("@Title", employee.Department.Title),
            };

            return await DBCommand.UpdateDataAsync(UpdateEmployeeQuery, parameters);
        }


        public async Task<List<Role>> GetUserRolesAsync(int employeeId)
        {
            const string GetRolesQuery = @"
                SELECT u.RoleId
                FROM [dbo].[UserRole] u
                INNER JOIN [dbo].[AppUser] a ON u.AppUserId = a.AppUserId
                WHERE a.EmployeeId = @EmployeeId
            ";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@EmployeeId", employeeId)
            };

            var roles = new List<Role>();

            using (SqlDataReader dataReader = await DBCommand.GetDataWithConditionAsync(GetRolesQuery, parameters))
            {
                while (await dataReader.ReadAsync())
                {
                    Role role = (Role)dataReader.GetInt32(dataReader.GetOrdinal("RoleId"));
                    roles.Add(role);
                }
            }

            return roles;
        }
    }
}