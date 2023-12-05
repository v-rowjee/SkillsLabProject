using SkillsLabProject.DAL;
using SkillsLabProject.DAL.Models;
using SkillsLabProject.DAL.RepositoryDAL;
using SkillsLabProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkillsLabProject.BLL
{
    public interface IDepartmentBL
    {
        IEnumerable<Department> GetAllDepartments();
        Department GetDepartmentById(int departmentId);
        bool AddDepartment(Department model);
        bool UpdateDepartment(Department model);
        bool DeleteDepartment(int departmentId);

    }
    public class DepartmentBL
    {
        private readonly IRepositoryDAL<Department> _departmentDAL;

        public DepartmentBL(IRepositoryDAL<Department> departmentDAL)
        {
            _departmentDAL = departmentDAL;
        }
    }
}