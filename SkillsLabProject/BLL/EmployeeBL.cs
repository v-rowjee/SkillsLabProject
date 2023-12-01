using SkillsLabProject.Models;
using SkillsLabProject.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillsLabProject.Models.ViewModels;

namespace SkillsLabProject.BLL
{
    public interface IEmployeeBL
    {
        IEnumerable<EmployeeModel> GetAllEmployees();
        EmployeeModel GetEmployee(LoginViewModel model);
        bool UpdateEmployee(EmployeeModel employee);
        bool DeleteEmployee(int employeeId);
    }
    public class EmployeeBL : IEmployeeBL
    {
        private readonly IEmployeeDAL _employeeDAL;

        public EmployeeBL(IEmployeeDAL employeeDAL)
        {
            _employeeDAL = employeeDAL;
        }
        public EmployeeBL()
        {
            _employeeDAL = new EmployeeDAL();
        }

        public EmployeeModel GetEmployee(LoginViewModel model)
        { 
            return _employeeDAL.GetEmployee(model);
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
    }
}
