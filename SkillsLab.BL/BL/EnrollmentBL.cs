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
        IEnumerable<EnrollmentViewModel> GetAllEnrollmentsOfEmployee(int employeeId);
        IEnumerable<EnrollmentViewModel> GetAllEnrollmentsOfManager(int managerId);
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
        private readonly ITrainingDAL _trainingDAL;
        private readonly IEmployeeDAL _employeeDAL;
        private readonly IProofDAL _proofDAL;

        public EnrollmentBL(IEnrollmentDAL enrollmentDAL, ITrainingDAL trainingDAL, IEmployeeDAL employeeDAL, IProofDAL proofDAL)
        {
            _enrollmentDAL = enrollmentDAL;
            _trainingDAL = trainingDAL;
            _employeeDAL = employeeDAL;
            _proofDAL = proofDAL;
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
        public IEnumerable<EnrollmentViewModel> GetAllEnrollmentsOfEmployee(int employeeId)
        {
            var enrollments = _enrollmentDAL.GetAll().Where(e => e.EmployeeId == employeeId).ToList();
            var enrollmentsViews = new List<EnrollmentViewModel>();
            foreach (var enrollment in enrollments)
            {
                var employee = _employeeDAL.GetEmployeeById(enrollment.EmployeeId);
                var training = _trainingDAL.GetById(enrollment.TrainingId);
                var proofs = _proofDAL.GetAll().Where(x => x.EnrollmentId == enrollment.EnrollmentId).ToList();

                var enrollmentView = new EnrollmentViewModel()
                {
                    EnrollmentId = enrollment.EnrollmentId,
                    Employee = employee,
                    Training = training,
                    Status = enrollment.Status,
                    Proofs = proofs,
                };
                enrollmentsViews.Add(enrollmentView);
            }
            return enrollmentsViews;
        }
        public IEnumerable<EnrollmentViewModel> GetAllEnrollmentsOfManager(int managerId)
        {
            var enrollments = _enrollmentDAL.GetAll().ToList();
            var enrollmentsViews = new List<EnrollmentViewModel>();
            foreach (var enrollment in enrollments)
            {
                var manager = _employeeDAL.GetEmployeeById(managerId);
                var employee = _employeeDAL.GetEmployeeById(enrollment.EmployeeId);
                var training = _trainingDAL.GetById(enrollment.TrainingId);
                var proofs = _proofDAL.GetAll().Where(x => x.EnrollmentId == enrollment.EnrollmentId).ToList();

                if (employee.Department.DepartmentId != manager.Department.DepartmentId) continue;

                var enrollmentView = new EnrollmentViewModel()
                {
                    EnrollmentId = enrollment.EnrollmentId,
                    Employee = employee,
                    Training = training,
                    Status = enrollment.Status,
                    Proofs = proofs,
                };
                enrollmentsViews.Add(enrollmentView);
            }
            return enrollmentsViews;
        }
        public bool UpdateEnrollment(EnrollmentModel enrollment)
        {
            return _enrollmentDAL.Update(enrollment);
        }

        public Task<string> UploadAndGetDownloadUrl(FileStream stream, string fileName)
        {
            var uniqueFileName = $@"{fileName}-{Guid.NewGuid()}";
            var downloadUrl = _enrollmentDAL.UploadAndGetDownloadUrlAsync(stream, uniqueFileName);
            return downloadUrl;
        }
    }
}
