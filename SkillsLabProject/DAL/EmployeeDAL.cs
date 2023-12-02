using SkillsLabProject.Models;
using SkillsLabProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillsLabProject.DAL.Common;
using SkillsLabProject.Enums;
using System.Reflection;

namespace SkillsLabProject.DAL
{
    public interface IEmployeeDAL
    {
        IEnumerable<EmployeeModel> GetAllEmployees();
        EmployeeModel GetEmployee(LoginViewModel model);
        bool UpdateEmployee(EmployeeModel employee);
        bool DeleteEmployee(int employeeId);
    }
    public class EmployeeDAL : IEmployeeDAL
    {
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
    }
}
