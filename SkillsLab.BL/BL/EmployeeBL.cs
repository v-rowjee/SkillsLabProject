using System.Collections.Generic;
using System.Threading.Tasks;
using SkillsLabProject.Common.Models;
using SkillsLabProject.DAL.DAL;
using SkillsLabProject.Common.Models.ViewModels;
using SkillsLabProject.Common.Enums;

namespace SkillsLabProject.BL.BL
{
    public interface IEmployeeBL
    {
        Task<IEnumerable<EmployeeModel>> GetAllEmployeesAsync();
        Task<EmployeeModel> GetEmployeeAsync(LoginViewModel model);
        Task<EmployeeModel> GetEmployeeByIdAsync(int employeeId);
        Task<bool> UpdateEmployeeAsync(EmployeeModel employee);
        Task<bool> DeleteEmployeeAsync(int employeeId);
        Task<List<Role>> GetUserRolesAsync(int employeeId);
    }

    public class EmployeeBL : IEmployeeBL
    {
        private readonly IEmployeeDAL _employeeDAL;

        public EmployeeBL(IEmployeeDAL employeeDAL)
        {
            _employeeDAL = employeeDAL;
        }

        public async Task<EmployeeModel> GetEmployeeAsync(LoginViewModel model)
        {
            return await _employeeDAL.GetEmployeeAsync(model);
        }

        public async Task<EmployeeModel> GetEmployeeByIdAsync(int employeeId)
        {
            return await _employeeDAL.GetEmployeeByIdAsync(employeeId);
        }

        public async Task<IEnumerable<EmployeeModel>> GetAllEmployeesAsync()
        {
            return await _employeeDAL.GetAllEmployeesAsync();
        }

        public async Task<bool> UpdateEmployeeAsync(EmployeeModel employee)
        {
            return await _employeeDAL.UpdateEmployeeAsync(employee);
        }

        public async Task<bool> DeleteEmployeeAsync(int employeeId)
        {
            return await _employeeDAL.DeleteEmployeeAsync(employeeId);
        }

        public async Task<List<Role>> GetUserRolesAsync(int employeeId)
        {
            return await _employeeDAL.GetUserRolesAsync(employeeId);
        }
    }
}