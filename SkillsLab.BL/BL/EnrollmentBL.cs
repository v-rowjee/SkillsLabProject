using OfficeOpenXml;
using SkillsLabProject.BL.Services;
using SkillsLabProject.Common.Enums;
using SkillsLabProject.Common.Models;
using SkillsLabProject.Common.Models.ViewModels;
using SkillsLabProject.DAL.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SkillsLabProject.BL.BL
{
    public interface IEnrollmentBL
    {
        IEnumerable<EnrollmentViewModel> GetAllEnrollments(EmployeeModel employee);
        EnrollmentViewModel GetEnrollmentById(int enrollmentId);
        bool ApproveEnrollment(EnrollmentModel model, EmployeeModel manager);
        bool DeclineEnrollment(EnrollmentModel model, EmployeeModel manager);
        bool DeleteEnrollment(int enrollmentId);
        byte[] Export(int trainingId);
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
                CreatedOn = enrollmentModel.CreatedOn,
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
                    CreatedOn= enrollment.CreatedOn,
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
        public bool DeclineEnrollment(EnrollmentModel model, EmployeeModel manager)
        {
            model.Status = Status.Declined;
            var isDeclined = _enrollmentDAL.Update(model);

            if (isDeclined)
            {
                var enrollment = _enrollmentDAL.GetById(model.EnrollmentId);
                var employee = _employeeDAL.GetEmployeeById(enrollment.EmployeeId);
                var training = _trainingDAL.GetById(enrollment.TrainingId);

                string subject = "Training Enrollment Declined";
                string body = $@"<html><body>
                              <p>Dear {employee.FirstName},</p>
                              <p>We regret to inform you that your {training.Title} training enrollment has been declined by your manager {manager.FirstName} {manager.LastName}, on {DateTime.Now.ToString("dddd, dd MMMM yyyy")} at {DateTime.Now.ToString("hh:mm tt")}.</p>
                              <p>Please check the details and make necessary arrangements.</p>
                              <p>Regards,<br/>{employee.Department.Title}, SkillsLab</p>
                              </body></html>";
                string recipientEmail = employee.Email;
                string ccEmail = manager.Email;

                Task.Run(() => _emailService.SendEmail(subject, body, recipientEmail, ccEmail));
            }
            return isDeclined;
        }

        public byte[] Export(int trainingId)
        {
            var enrollments = _enrollmentDAL.GetAll().Where(e => e.TrainingId == trainingId).ToList();
            var training = _trainingDAL.GetById(trainingId);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Enrollments");

                worksheet.Cells["A1"].Value = "Training";
                worksheet.Cells["B1"].Value = training.Title;

                worksheet.Cells["A3"].Value = "Employee's Name";
                worksheet.Cells["B3"].Value = "Phone Number";
                worksheet.Cells["C3"].Value = "Email";
                worksheet.Cells["D3"].Value = "Manager's Name";
                worksheet.Cells["E3"].Value = "Enrollment Date";

                for (int i = 0; i < enrollments.Count; i++)
                {
                    var employee = _employeeDAL.GetEmployeeById(enrollments[i].EmployeeId);
                    var manager = _employeeDAL.GetAllEmployees().Where(e => e.Department.DepartmentId == employee.Department.DepartmentId).FirstOrDefault();

                    worksheet.Cells[i + 4, 1].Value = employee.FirstName + " " + employee.LastName;
                    worksheet.Cells[i + 4, 2].Value = employee.PhoneNumber;
                    worksheet.Cells[i + 4, 3].Value = employee.Email;
                    worksheet.Cells[i + 4, 4].Value = manager.FirstName + " " + manager.LastName;
                    worksheet.Cells[i + 4, 5].Value = enrollments[i].CreatedOn.ToString("f");
                }

                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A3:E3"].Style.Font.Bold = true;

                worksheet.Cells["A1:B1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["A1:E1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                worksheet.Cells["A3:E3"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["A3:E3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                worksheet.Cells.AutoFitColumns();

                return package.GetAsByteArray();
            }
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
                    sendEmailAwaitingReviewToManager(employee, trainingId);
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
                sendEmailAwaitingReviewToManager(employee,trainingId);
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

        private void sendEmailAwaitingReviewToManager(EmployeeModel employee, int trainingId)
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
