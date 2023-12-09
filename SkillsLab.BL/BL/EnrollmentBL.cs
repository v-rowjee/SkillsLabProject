using SkillsLabProject.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillsLabProject.Common.DAL;
using SkillsLabProject.Common.Models.ViewModels;
using SkillsLabProject.DAL.DAL;

namespace SkillsLabProject.BL.BL
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
