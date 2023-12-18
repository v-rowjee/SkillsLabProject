using SkillsLabProject.Common.Enums;
using SkillsLabProject.Common.Models.ViewModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using SkillsLabProject.Common.DAL;
using SkillsLabProject.Common.Models;
using System.Web.UI.WebControls;

namespace SkillsLabProject.DAL.DAL
{
    public interface IEmployeeDAL
    {
        bool DeleteEmployee(int employeeId);
        IEnumerable<EmployeeModel> GetAllEmployees();
        EmployeeModel GetEmployee(LoginViewModel model);
        EmployeeModel GetEmployeeById(int employeeId);
        bool UpdateEmployee(EmployeeModel employee);
        List<Role> GetUserRoles(int employeeId);
    }
    public class EmployeeDAL : IEmployeeDAL
    {
        public bool DeleteEmployee(int employeeId)
        {
            const string DeleteEmployeeQuery = @"
                DELETE FROM AppUser WHERE EmployeeId=@EmployeeId;
                DELETE FROM Enrollement WHERE EmployeeId=@EmployeeId;
                DELETE FROM Employee WHERE EmployeeId=@EmployeeId
            ";
            var parameter = new SqlParameter("@EmployeeId", employeeId);
            return DBCommand.DeleteData(DeleteEmployeeQuery, parameter);
        }
        public IEnumerable<EmployeeModel> GetAllEmployees()
        {
            const string GetAllEmployeesQuery = @"
                SELECT e.EmployeeId, FirstName, LastName, NIC, PhoneNumber, e.DepartmentId, RoleId, a.Email, d.Title
                FROM [dbo].[Employee] as e
                INNER JOIN [dbo].[AppUser] as a ON e.EmployeeId = a.EmployeeId
                INNER JOIN [dbo].[Department] as d ON e.DepartmentId = d.DepartmentId
            ";
            var dt = DBCommand.GetData(GetAllEmployeesQuery);
            var employees = new List<EmployeeModel>();
            EmployeeModel employee;
            foreach (DataRow row in dt.Rows)
            {
                employee = new EmployeeModel
                {
                    EmployeeId = int.Parse(row["EmployeeId"].ToString()),
                    FirstName = row["FirstName"].ToString(),
                    LastName = row["LastName"].ToString(),
                    NIC = row["NIC"].ToString(),
                    PhoneNumber = row["PhoneNumber"].ToString(),
                    Email = row["Email"].ToString(),
                    Department = new DepartmentModel
                    {
                        DepartmentId = int.Parse(row["DepartmentId"].ToString()),
                        Title = row["Title"].ToString()
                    },
                    Role = (Role)int.Parse(row["RoleId"].ToString())
                };
                employees.Add(employee);
            }
            return employees;
        }
        public EmployeeModel GetEmployee(LoginViewModel login)
        {
            const string GetEmployeeQuery = @"
                SELECT e.EmployeeId, e.FirstName, e.LastName, e.NIC, e.PhoneNumber, e.DepartmentId, e.RoleId, a.Email, d.Title
                FROM [dbo].[Employee] as e
                INNER JOIN [dbo].[AppUser] as a ON e.EmployeeId = a.EmployeeId
                INNER JOIN [dbo].[Department] as d ON e.DepartmentId = d.DepartmentId
                WHERE a.[Email] = @Email
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Email", login.Email)
            };
            var dt = DBCommand.GetDataWithCondition(GetEmployeeQuery, parameters);
            var employee = new EmployeeModel();
            foreach (DataRow row in dt.Rows)
            {
                employee.EmployeeId = int.Parse(row["EmployeeId"].ToString());
                employee.FirstName = row["FirstName"].ToString();
                employee.LastName = row["LastName"].ToString();
                employee.NIC = row["NIC"].ToString();
                employee.PhoneNumber = row["PhoneNumber"].ToString();
                employee.Email = row["Email"].ToString();
                employee.Department = new DepartmentModel
                {
                    DepartmentId = int.Parse(row["DepartmentId"].ToString()),
                    Title = row["Title"].ToString()
                };
                employee.Role = (Role)int.Parse(row["RoleId"].ToString());
            }
            return employee;
        }

        public EmployeeModel GetEmployeeById(int employeeId)
        {
            const string GetEmployeeQuery = @"
                SELECT e.EmployeeId, e.FirstName, e.LastName, e.NIC, e.PhoneNumber, e.DepartmentId, e.RoleId, a.Email, d.Title
                FROM [dbo].[Employee] as e
                INNER JOIN [dbo].[AppUser] as a ON e.EmployeeId = a.EmployeeId
                INNER JOIN [dbo].[Department] as d ON e.DepartmentId = d.DepartmentId
                WHERE e.[EmployeeId] = @EmployeeId
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@EmployeeId", employeeId)
            };
            var dt = DBCommand.GetDataWithCondition(GetEmployeeQuery, parameters);
            var employee = new EmployeeModel();
            foreach (DataRow row in dt.Rows)
            {
                employee.EmployeeId = int.Parse(row["EmployeeId"].ToString());
                employee.FirstName = row["FirstName"].ToString();
                employee.LastName = row["LastName"].ToString();
                employee.NIC = row["NIC"].ToString();
                employee.PhoneNumber = row["PhoneNumber"].ToString();
                employee.Email = row["Email"].ToString();
                employee.Department = new DepartmentModel
                {
                    DepartmentId = int.Parse(row["DepartmentId"].ToString()),
                    Title = row["Title"].ToString()
                };
                employee.Role = (Role)int.Parse(row["RoleId"].ToString());
            }
            return employee;
        }

        public bool UpdateEmployee(EmployeeModel employee)
        {
            const string UpdateEmployeeQuery = @"
                UPDATE [dbo].[Employee] e
                INNER JOIN [dbo].[AppUser] as a ON e.EmployeeId = a.EmployeeId
                INNER JOIN [dbo].[Department] as d ON e.DepartmentId = d.DepartmentId
                SET FirstName=@FirstName, LastName=@LastName, NIC=@NIC, PhoneNumber=@PhoneNumber, e.DepartmentId=@DepartmentId, RoleId=@RoleId,  a.Email=@Email, d.Title=@Title
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
                new SqlParameter("@RoleId", (int)employee.Role)
            };
            return DBCommand.InsertUpdateData(UpdateEmployeeQuery, parameters);
        }

        public List<Role> GetUserRoles(int employeeId)
        {
            const string GetRolesQuery = @"
                SELECT u.RoleId
                FROM [dbo].[UserRole] u
                INNER JOIN [dbo].[AppUser] a ON u.AppUserId = a.AppUserId
                INNER JOIN [dbo].[Employee] e ON a.EmployeeId = e.EmployeeId
                WHERE e.EmployeeId = @EmployeeId
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@EmployeeId", employeeId)
            };
            var dt = DBCommand.GetDataWithCondition(GetRolesQuery, parameters);
            var roles = new List<Role>();
            foreach (DataRow row in dt.Rows)
            {
                Role role = (Role)int.Parse(row["RoleId"].ToString());
                roles.Add(role);
            }
            return roles;
        }
    }
}