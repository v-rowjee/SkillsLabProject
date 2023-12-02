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
        bool UpdateEmployee(EmployeeModel model);
        bool DeleteEmployee(int employeeId);
    }
    public class EmployeeDAL : IEmployeeDAL
    {
        private const string GetEmployeeQuery = @"
            SELECT e.EmployeeId, e.FirstName, e.LastName, e.NIC, e.PhoneNumber, e.DepartmentId, e.RoleId, a.Email, d.Title
            FROM [dbo].[Employee] as e
            INNER JOIN [dbo].[AppUser] as a ON e.EmployeeId = a.EmployeeId
            INNER JOIN [dbo].[Department] as d ON e.DepartmentId = d.DepartmentId
            WHERE a.[Email] = @Email
        ";
        private const string GetAllEmployeesQuery = @"
            SELECT e.EmployeeId, FirstName, LastName, NIC, PhoneNumber, e.DepartmentId, RoleId, a.Email, d.Title
            FROM [dbo].[Employee] as e
            INNER JOIN [dbo].[AppUser] as a ON e.EmployeeId = a.EmployeeId
            INNER JOIN [dbo].[Department] as d ON e.DepartmentId = d.DepartmentId
        ";
        private const string UpdateEmployeeQuery = @"
            UPDATE [dbo].[Employee] e
            INNER JOIN [dbo].[AppUser] as a ON e.EmployeeId = a.EmployeeId
            INNER JOIN [dbo].[Department] as d ON e.DepartmentId = d.DepartmentId
            SET FirstName=@FirstName, LastName=@LastName, NIC=@NIC, PhoneNumber=@PhoneNumber, e.DepartmentId=@DepartmentId, RoleId=@RoleId,  a.Email=@Email, d.Title=@Title
            WHERE EmployeeId=@EmployeeId;
        ";
        private const string DeleteEmployeeQuery = @"
            DELETE FROM AppUser WHERE EmployeeId=@EmployeeId;
            DELETE FROM Enrollement WHERE EmployeeId=@EmployeeId;
            DELETE FROM Employee WHERE EmployeeId=@EmployeeId
        ";


        public IEnumerable<EmployeeModel> GetAllEmployees()
        {
            var employees = new List<EmployeeModel>();
            EmployeeModel employee;

            var dt = DBCommand.GetData(GetAllEmployeesQuery);
            foreach (DataRow row in dt.Rows)
            {
                employee = new EmployeeModel();
                employee.EmployeeId = int.Parse(row["EmployeeId"].ToString());
                employee.FirstName = row["FirstName"].ToString();
                employee.LastName = row["LastName"].ToString();
                employee.NIC = row["NIC"].ToString();
                employee.PhoneNumber = row["PhoneNumber"].ToString();

                employee.Email = row["Email"].ToString();

                var department = new DepartmentModel();
                department.DepartmentId = int.Parse(row["DepartmentId"].ToString());
                department.Title = row["Title"].ToString();
                employee.Department = department;

                employee.Role = (Role) int.Parse(row["RoleId"].ToString());

                employees.Add(employee);
            }

            return employees;
        }

        public EmployeeModel GetEmployee(LoginViewModel model)
        {
            var employee = new EmployeeModel();
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Email", model.Email));

            var dt = DBCommand.GetDataWithCondition(GetEmployeeQuery, parameters);
            foreach (DataRow row in dt.Rows)
            {
                employee.EmployeeId = int.Parse(row["EmployeeId"].ToString());
                employee.FirstName = row["FirstName"].ToString();
                employee.LastName = row["LastName"].ToString();
                employee.NIC = row["NIC"].ToString();
                employee.PhoneNumber = row["PhoneNumber"].ToString();

                employee.Email = row["Email"].ToString();

                var department = new DepartmentModel();
                department.DepartmentId = int.Parse(row["DepartmentId"].ToString());
                department.Title = row["Title"].ToString();
                employee.Department = department;

                employee.Role = (Role)int.Parse(row["RoleId"].ToString());
            }

            return employee;
        }

        public bool UpdateEmployee(EmployeeModel model)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@EmployeeId", model.EmployeeId));
            parameters.Add(new SqlParameter("@FirstName", model.FirstName));
            parameters.Add(new SqlParameter("@LastName", model.LastName));
            parameters.Add(new SqlParameter("@NIC", model.NIC));
            parameters.Add(new SqlParameter("@Email", model.Email));
            parameters.Add(new SqlParameter("@PhoneNumber", model.PhoneNumber));
            parameters.Add(new SqlParameter("@DepartmentId", model.Department.DepartmentId));
            parameters.Add(new SqlParameter("@Title", model.Department.Title));
            parameters.Add(new SqlParameter("@RoleId", (int) model.Role));

            var isValid = DBCommand.InsertUpdateData(UpdateEmployeeQuery, parameters);

            return isValid;
        }

        public bool DeleteEmployee(int employeeId)
        {
            var parameter = new SqlParameter("@EmployeeId", employeeId);
            return DBCommand.DeleteData(DeleteEmployeeQuery, parameter);
        }
    }
}
