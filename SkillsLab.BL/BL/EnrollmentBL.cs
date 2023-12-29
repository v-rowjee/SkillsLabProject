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
using System.Reflection;

namespace SkillsLabProject.BL.BL
{
    public interface IEnrollmentBL
    {
        IEnumerable<EnrollmentViewModel> GetAllEnrollments(EmployeeModel employee);
        EnrollmentViewModel GetEnrollmentById(int enrollmentId);
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
        public IEnumerable<EnrollmentViewModel> GetAllEnrollments(EmployeeModel currentEmployee)
        {
            var enrollments = _enrollmentDAL.GetAll().ToList();
            var enrollmentsViews = new List<EnrollmentViewModel>();
            foreach (var enrollment in enrollments)
            {
                var employee = _employeeDAL.GetEmployeeById(enrollment.EmployeeId);
                var training = _trainingDAL.GetById(enrollment.TrainingId);
                var proofs = _proofDAL.GetAll().Where(p => p.EnrollmentId == enrollment.EnrollmentId).ToList();

                var enrollmentView = new EnrollmentViewModel()
                {
                    EnrollmentId = enrollment.EnrollmentId,
                    Employee = employee,
                    Training = training,
                    Status = enrollment.Status,
                    Proofs = proofs,
                };
                if (currentEmployee.Role == Role.Employee)
                {
                    if (currentEmployee.EmployeeId != enrollment.EmployeeId) continue;
                }
                else if (currentEmployee.Role == Role.Manager)
                {
                    if (currentEmployee.Department.DepartmentId != employee.Department.DepartmentId) continue;
                }
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
            var isApproved = _enrollmentDAL.Update(model);

            if (isApproved)
            {
                var enrollment = _enrollmentDAL.GetById(model.EnrollmentId);
                var employee = _employeeDAL.GetEmployeeById(enrollment.EmployeeId);
                var training = _trainingDAL.GetById(enrollment.TrainingId);

                string subject = "Training Enrollment Approved";
                string body = $@"<html><body>
                              <p>Dear {employee.FirstName},</p>
                              <p>Congratulations! Your {training.Title} training enrollment has been approved by your manager {manager.FirstName} {manager.LastName}, on {DateTime.Now.ToString("dddd, dd MMMM yyyy")} at {DateTime.Now.ToString("hh:mm tt")}.</p>
                              <p>Please check the details and make necessary arrangements.</p>
                              <p>Best regards,<br/>{employee.Department.Title}, SkillsLab</p>
                              </body></html>";
                string recipientEmail = employee.Email;
                string ccEmail = manager.Email;

                Task.Run(() => _emailService.SendEmail(subject, body, recipientEmail, ccEmail));
            }
            return isApproved;
        }
        public bool DeclineEnrollment(EnrollmentModel model)
        {
            model.Status = Status.Declined;
            return _enrollmentDAL.Update(model);
        }

        public async Task<string> Enroll(LoginViewModel loggeduser, int trainingId, List<HttpPostedFileBase> files)
        {
            var employee = _employeeDAL.GetEmployee(loggeduser);

            var prerequisites = _preRequisiteDAL.GetAll().Where(p => p.TrainingId == trainingId).ToList();

            if (prerequisites.Count == 0)
            {
                var enrollment = new EnrollmentModel
                {
                    EmployeeId = _employeeDAL.GetEmployee(loggeduser).EmployeeId,
                    TrainingId = trainingId,
                    Status = Status.Pending
                };
                var resultEnrollemnt = _enrollmentDAL.Add(enrollment);
                if (resultEnrollemnt)
                {
                    sendEmailToManager(employee, trainingId);
                }
                return resultEnrollemnt ? "Success" : "Error";
            }

            if (files == null || !files.Any() || files.Count < prerequisites.Count)
            {
                return "FileMissing";
            }

            var enrollmentWithProofs = new EnrollmentViewModel
            {
                Employee = employee,
                Training = new TrainingModel { TrainingId = trainingId },
                Proofs = new List<ProofModel>(),
                Status = Status.Pending
            };

            var invalidFiles = ValidateFiles(files);
            if (invalidFiles.Count > 0) return "InvalidType";


            foreach (var file in files)
            {
                string tempFilePath = Path.GetTempFileName();

                file.SaveAs(tempFilePath);

                var fileName = GenerateUniqueFileName(file);

                using (FileStream stream = new FileStream(tempFilePath, FileMode.Open))
                {
                    string downloadUrl = await _enrollmentDAL.UploadAndGetDownloadUrlAsync(stream, fileName);
                    enrollmentWithProofs.Proofs.Add(new ProofModel { Attachment = downloadUrl });
                    File.Delete(tempFilePath);
                }
            }

            var result = _enrollmentDAL.Add(enrollmentWithProofs);

            if (result)
            {
                sendEmailToManager(employee,trainingId);
            }

            return result ? "Success" : "Error";
        }

        private List<string> ValidateFiles(List<HttpPostedFileBase> files)
        {
            List<string> incorrectFiles = new List<string>();

            foreach (var file in files)
            {
                if (file == null || file.ContentLength == 0)
                {
                    continue;
                }

                string fileExtension = Path.GetExtension(file.FileName);

                List<string> allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png" };

                if (!allowedExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase))
                {
                    incorrectFiles.Add(file.FileName);
                }
            }
            return incorrectFiles;
        }


        private string GenerateUniqueFileName(HttpPostedFileBase file)
        {
            string fileName = Path.GetFileName(file.FileName);
            string fileExtension = Path.GetExtension(file.FileName);
            var uniqueFileName = $@"{fileName}-{Guid.NewGuid()}{fileExtension}";
            return uniqueFileName;
        }

        private void sendEmailToManager(EmployeeModel employee, int trainingId)
        {
            var manager = _employeeDAL.GetAllEmployees().Where(e => e.Department.DepartmentId == employee.Department.DepartmentId).FirstOrDefault();
            var training = _trainingDAL.GetById(trainingId);

            string subject = "Waiting For Approval";
            string body = $@"
            <html>
              <body>
                <p>Dear {manager.FirstName},</p>
                <p>This is to inform you that {employee.FirstName} {employee.LastName} has enrolled in the {training.Title} training program.</p>
                <p>Enrollment Details:</p>
                <ul>
                  <li><strong>Employee:</strong> {employee.FirstName} {employee.LastName}</li>
                  <li><strong>Training Program:</strong> {training.Title}</li>
                  <li><strong>Enrollment Date:</strong> {DateTime.Now:dddd, dd MMMM yyyy} at {DateTime.Now.ToString("hh:mm tt")}</li>
                </ul>
                <p>Please review and provide any necessary approvals or feedback.</p>
                <p>Best regards,<br/>Your System Administrator</p>
              </body>
            </html>";
            string recipientEmail = manager.Email;
            string ccEmail = employee.Email;

            Task.Run(() => _emailService.SendEmail(subject, body, recipientEmail, ccEmail));
        }
    }
}
