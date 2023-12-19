using SkillsLabProject.Common.Models;
using SkillsLabProject.DAL.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillsLabProject.Common.Models.ViewModels;
using SkillsLabProject.Common.Enums;

namespace SkillsLabProject.BL.BL
{
    public interface IEmployeeBL
    {
        IEnumerable<EmployeeModel> GetAllEmployees();
        EmployeeModel GetEmployee(LoginViewModel model);
        EmployeeModel GetEmployeeById(int employeeId);
        bool UpdateEmployee(EmployeeModel employee);
        bool DeleteEmployee(int employeeId);
        List<Role> GetUserRoles(int employeeId);
    }
    public class EmployeeBL : IEmployeeBL
    {
        private readonly IEmployeeDAL _employeeDAL;

        public EmployeeBL(IEmployeeDAL employeeDAL)
        {
            _employeeDAL = employeeDAL;
        }
        public EmployeeModel GetEmployee(LoginViewModel model)
        { 
            return _employeeDAL.GetEmployee(model);
        }
        public EmployeeModel GetEmployeeById(int employeeId)
        {
            return _employeeDAL.GetEmployeeById(employeeId);
        }
        public IEnumerable<EmployeeModel> GetAllEmployees() 
        { 
            return _employeeDAL.GetAllEmployees();
        }
        public bool UpdateEmployee(EmployeeModel employee) 
        { 
            return _employeeDAL.UpdateEmployee(employee); 
        }
        public bool DeleteEmployee(int employeeId) 
        { 
            return _employeeDAL.DeleteEmployee(employeeId);
        }
        public List<Role> GetUserRoles(int employeeId)
        {
            return _employeeDAL.GetUserRoles(employeeId);
        }
    }
}
