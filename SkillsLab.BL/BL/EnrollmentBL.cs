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
using System.Web;
using SkillsLabProject.BL.Services;

namespace SkillsLabProject.BL.BL
{
    public interface IEnrollmentBL
    {
        IEnumerable<EnrollmentViewModel> GetAllEnrollments();
        IEnumerable<EnrollmentViewModel> GetAllEnrollmentsOfEmployee(int employeeId);
        IEnumerable<EnrollmentViewModel> GetAllEnrollmentsOfManager(int managerId);
        EnrollmentViewModel GetEnrollmentById(int enrollmentId);
        bool AddEnrollment(EnrollmentModel model);
        bool AddEnrollment(EnrollmentViewModel model);
        bool UpdateEnrollment(EnrollmentModel model);
        bool ApproveEnrollment(EnrollmentModel model, EmployeeModel manager);
        bool DeclineEnrollment(EnrollmentModel model);
        bool DeleteEnrollment(int enrollmentId);
        Task<string> Enroll(LoginViewModel loggeduser, int trainingId, List<HttpPostedFileBase> files);

    }
    public class EnrollmentBL : IEnrollmentBL
    {
        private readonly IEnrollmentDAL _enrollmentDAL;
        private readonly ITrainingDAL _trainingDAL;
        private readonly IEmployeeDAL _employeeDAL;
        private readonly IProofDAL _proofDAL;
        private readonly IPreRequisiteDAL _preRequisiteDAL;
        private readonly IEmailService _emailService;

        public EnrollmentBL(IEnrollmentDAL enrollmentDAL, ITrainingDAL trainingDAL, IEmployeeDAL employeeDAL, IProofDAL proofDAL, IPreRequisiteDAL preRequisiteDAL, IEmailService emailService)
        {
            _enrollmentDAL = enrollmentDAL;
            _trainingDAL = trainingDAL;
            _employeeDAL = employeeDAL;
            _proofDAL = proofDAL;
            _preRequisiteDAL = preRequisiteDAL;
            _emailService = emailService;
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
        public EnrollmentViewModel GetEnrollmentById(int enrollmentId)
        {
            var enrollmentModel = _enrollmentDAL.GetById(enrollmentId);
            var enrollmentViewModel = new EnrollmentViewModel()
            {
                EnrollmentId = enrollmentId,
                Employee = _employeeDAL.GetEmployeeById(enrollmentModel.EmployeeId),
                Training = _trainingDAL.GetById(enrollmentModel.TrainingId),
                Proofs = _proofDAL.GetAll().Where(x => x.EnrollmentId == enrollmentModel.EnrollmentId).ToList(),
                Status = enrollmentModel.Status,
            };
            return enrollmentViewModel;
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
        public IEnumerable<EnrollmentViewModel> GetAllEnrollments()
        {
            var enrollments = _enrollmentDAL.GetAll().ToList();
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
        public bool UpdateEnrollment(EnrollmentModel model)
        {
            var enrollment = _enrollmentDAL.GetById(model.EnrollmentId);
            enrollment.Status = model.Status;

            if (model.DeclinedReason != null)
            {
                enrollment.DeclinedReason = model.DeclinedReason;
            }

            return _enrollmentDAL.Update(enrollment);
        }

        public bool ApproveEnrollment(EnrollmentModel model, EmployeeModel manager)
        {
            model.Status = Status.Approved;
            var result = _enrollmentDAL.Update(model);
            if (!result) return false;

            var enrollment = _enrollmentDAL.GetById(model.EnrollmentId);
            var employee = _employeeDAL.GetEmployeeById(enrollment.EmployeeId);
            var training = _trainingDAL.GetById(enrollment.TrainingId);

            var emailViewModel = new EmailViewModel()
            {
                Employee = employee,
                Manager = manager,
                Training = training,
            };

            return _emailService.SendEmail(emailViewModel);
        }
        public bool DeclineEnrollment(EnrollmentModel model)
        {
            model.Status = Status.Declined;
            return _enrollmentDAL.Update(model);
        }

        public async Task<string> Enroll(LoginViewModel loggeduser, int trainingId, List<HttpPostedFileBase> files)
        {
            var prerequisites = _preRequisiteDAL.GetAll().Where(p => p.TrainingId == trainingId).ToList();

            if (prerequisites.Count == 0)
            {
                var enrollment = new EnrollmentModel()
                {
                    EmployeeId = _employeeDAL.GetEmployee(loggeduser).EmployeeId,
                    TrainingId = trainingId,
                    Status = Status.Pending
                };
                return _enrollmentDAL.Add(enrollment) ? "Success" : "Error";
            }
            else if(files != null && files.Any() && files.Count < prerequisites.Count)
            {
                return "FileMissing";
            }
            else if(files != null && files.Any() && files.Count >= prerequisites.Count)
            {
                var enrollmentWithProofs = new EnrollmentViewModel()
                {
                    Employee = _employeeDAL.GetEmployee(loggeduser),
                    Training = new TrainingModel() { TrainingId = trainingId },
                    Proofs = new List<ProofModel>(),
                    Status = Status.Pending
                };
                foreach (var file in files)
                {
                    if (file.ContentLength > 0)
                    {
                        string tempFilePath = Path.GetTempFileName();

                        file.SaveAs(tempFilePath);

                        using (FileStream stream = new FileStream(tempFilePath, FileMode.Open))
                        {
                            string uniqueFileName = GenerateUniqueFileName(file);
                            string downloadUrl = await _enrollmentDAL.UploadAndGetDownloadUrlAsync(stream, file.FileName);
                            enrollmentWithProofs.Proofs.Add(new ProofModel() { Attachment = downloadUrl });
                        }

                        System.IO.File.Delete(tempFilePath);
                    }
                }
                return _enrollmentDAL.Add(enrollmentWithProofs) ? "Success" : "Error";
            }
            else
            {
                return "FileMissing";
            }
        }
        private string GenerateUniqueFileName(HttpPostedFileBase file)
        {
            string fileName = Path.GetFileName(file.FileName);
            string fileExtension = Path.GetExtension(file.FileName);
            var uniqueFileName = $@"{fileName}-{Guid.NewGuid()}{fileExtension}";
            return uniqueFileName;
        }
    }
}
