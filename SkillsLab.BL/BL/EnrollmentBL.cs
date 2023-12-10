using SkillsLabProject.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillsLabProject.Common.DAL;
using SkillsLabProject.Common.Models.ViewModels;
using SkillsLabProject.DAL.DAL;
using System.IO;
using SkillsLabProject.Common.Enums;

namespace SkillsLabProject.BL.BL
{
    public interface IEnrollmentBL
    {
        IEnumerable<EnrollmentModel> GetAllEnrollments();
        EnrollmentModel GetEnrollmentById(int enrollmentId);
        bool AddEnrollment(EnrollmentModel model);
        bool AddEnrollment(EnrollmentViewModel model);
        bool UpdateEnrollment(EnrollmentModel model);
        bool DeleteEnrollment(int enrollmentId);
        Task<string> UploadAndGetDownloadUrl(FileStream stream, string fileName);

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
            enrollment.Status = Status.Pending;
            return _enrollmentDAL.Add(enrollment);
        }
        public bool AddEnrollment(EnrollmentViewModel enrollment)
        {
            enrollment.Status = Status.Pending;
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

        public Task<string> UploadAndGetDownloadUrl(FileStream stream, string fileName)
        {
            var uniqueFileName = $@"{fileName}-{Guid.NewGuid()}-{DateTime.Now.Ticks}.txt";
            var downloadUrl = _enrollmentDAL.UploadAndGetDownloadUrlAsync(stream, uniqueFileName);
            return downloadUrl;
        }
    }
}
