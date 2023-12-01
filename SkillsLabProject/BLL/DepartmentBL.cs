using SkillsLabProject.DAL;
using SkillsLabProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkillsLabProject.BLL
{
    public interface IDepartmentBL
    {
        IEnumerable<DepartmentModel> GetAllDepartments();
        DepartmentModel GetDepartmentById(int departmentId);
        bool AddDepartment(DepartmentModel model);
        bool UpdateDepartment(DepartmentModel model);
        bool DeleteDepartment(int DepartmentId);

    }
    public class DepartmentBL : IDepartmentBL
    {
        private readonly IDepartmentDAL _departmentDAL;

        public DepartmentBL(IDepartmentDAL departmentDAL)
        {
            _departmentDAL = departmentDAL;
        }

        public bool AddDepartment(DepartmentModel department)
        {
            return _departmentDAL.Add(department);
        }
        public bool DeleteDepartment(int DepartmentId)
        {
            return _departmentDAL.Delete(DepartmentId);
        }
        public DepartmentModel GetDepartmentById(int departmentId)
        {
            return _departmentDAL.GetById(departmentId);
        }
        public IEnumerable<DepartmentModel> GetAllDepartments()
        {
            return _departmentDAL.GetAll();
        }
        public bool UpdateDepartment(DepartmentModel department)
        {
            return _departmentDAL.Update(department);
        }
    }
}