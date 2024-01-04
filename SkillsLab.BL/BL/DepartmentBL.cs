using System.Collections.Generic;
using System.Threading.Tasks;
using SkillsLabProject.Common.Models;
using SkillsLabProject.DAL.DAL;

namespace SkillsLabProject.BL.BL
{
    public interface IDepartmentBL
    {
        Task<IEnumerable<DepartmentModel>> GetAllDepartmentsAsync();
        Task<DepartmentModel> GetDepartmentByIdAsync(int departmentId);
        Task<bool> AddDepartmentAsync(DepartmentModel model);
        Task<bool> UpdateDepartmentAsync(DepartmentModel model);
        Task<bool> DeleteDepartmentAsync(int departmentId);
    }

    public class DepartmentBL : IDepartmentBL
    {
        private readonly IDepartmentDAL _departmentDAL;

        public DepartmentBL(IDepartmentDAL departmentDAL)
        {
            _departmentDAL = departmentDAL;
        }

        public async Task<bool> AddDepartmentAsync(DepartmentModel department)
        {
            return await _departmentDAL.AddAsync(department);
        }

        public async Task<bool> DeleteDepartmentAsync(int departmentId)
        {
            return await _departmentDAL.DeleteAsync(departmentId);
        }

        public async Task<DepartmentModel> GetDepartmentByIdAsync(int departmentId)
        {
            return await _departmentDAL.GetByIdAsync(departmentId);
        }

        public async Task<IEnumerable<DepartmentModel>> GetAllDepartmentsAsync()
        {
            return await _departmentDAL.GetAllAsync();
        }

        public async Task<bool> UpdateDepartmentAsync(DepartmentModel department)
        {
            return await _departmentDAL.UpdateAsync(department);
        }
    }
}