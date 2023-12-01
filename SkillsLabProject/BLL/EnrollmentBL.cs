using SkillsLabProject.Models;
using SkillsLabProject.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillsLabProject.DAL.Common;
using SkillsLabProject.Models.ViewModels;

namespace SkillsLabProject.BLL
{
    public interface IEnrollmentBL
    {
        IEnumerable<EnrollmentModel> GetAllEnrollments();
        EnrollmentModel GetEnrollmentById(int enrollmentId);
        bool AddEnrollment(EnrollmentModel model);
        bool UpdateEnrollment(EnrollmentModel model);
        bool DeleteEnrollment(int enrollmentId);

    }
    public class EnrollmentBL : IEnrollmentBL
    {
        private readonly IEnrollmentDAL _enrollmentDAL;

        public EnrollmentBL(IEnrollmentDAL enrollmentDAL)
        {
            _enrollmentDAL = enrollmentDAL;
        }

        public bool AddEnrollment(EnrollmentModel enrollment)
        {
            return _enrollmentDAL.Add(enrollment);
        }
        public bool DeleteEnrollment(int enrollmentId)
        {
            return _enrollmentDAL.Delete(enrollmentId);
        }
        public EnrollmentModel GetEnrollmentById(int enrollmentId)
        {
            return _enrollmentDAL.GetById(enrollmentId);
        }
        public IEnumerable<EnrollmentModel> GetAllEnrollments()
        {
            return _enrollmentDAL.GetAll();
        }
        public bool UpdateEnrollment(EnrollmentModel enrollment)
        {
            return _enrollmentDAL.Update(enrollment);
        }
    }
}
