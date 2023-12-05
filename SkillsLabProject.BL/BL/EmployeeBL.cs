using SkillsLabProject.Models;
using SkillsLabProject.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillsLabProject.Models.ViewModels;
using SkillsLabProject.DAL.Models;
using SkillsLabProject.DAL.RepositoryDAL;
using System.Data.SqlClient;

namespace SkillsLabProject.BLL
{
    public interface IEmployeeBL
    {
        IEnumerable<Employee> GetAllEmployees();
        Employee GetEmployee(LoginViewModel model);
        bool UpdateEmployee(Employee employee);
        bool DeleteEmployee(int employeeId);
    }
    public class EmployeeBL
    {
        private readonly IRepositoryDAL<Employee> _employeeDAL;

        public EmployeeBL(IRepositoryDAL<Employee> employeeDAL)
        {
            _employeeDAL = employeeDAL;
        }
    }
}
