using OfficeOpenXml;
using SkillsLabProject.BL.Services;
using SkillsLabProject.Common.Enums;
using SkillsLabProject.Common.Models;
using SkillsLabProject.Common.Models.ViewModels;
using SkillsLabProject.DAL.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Drawing;
using SkillsLab.Common.Models;

namespace SkillsLabProject.BL.BL
{
    public interface IEnrollmentBL
    {
        Task<IEnumerable<EnrollmentViewModel>> GetAllEnrollmentsAsync(EmployeeModel employee);
        Task<EnrollmentViewModel> GetEnrollmentByIdAsync(int enrollmentId);
        Task<bool> ApproveEnrollmentAsync(EnrollmentModel model, EmployeeModel manager);
        Task<bool> DeclineEnrollmentAsync(EnrollmentModel model, EmployeeModel manager);
        Task<bool> DeleteEnrollmentAsync(int enrollmentId);
        Task<byte[]> ExportAsync(int trainingId);
        Task<string> EnrollAsync(LoginViewModel loggeduser, int trainingId, List<HttpPostedFileBase> files);
    }
    public class EnrollmentBL : IEnrollmentBL
    {
        private readonly IEnrollmentDAL _enrollmentDAL;
        private readonly ITrainingDAL _trainingDAL;
        private readonly IEmployeeDAL _employeeDAL;
        private readonly IProofDAL _proofDAL;
        private readonly IPreRequisiteDAL _preRequisiteDAL;
        private readonly IDepartmentDAL _departmentDAL;
        private readonly IEmailService _emailService;
        private readonly INotificationDAL _notificationDAL;

        public EnrollmentBL(IEnrollmentDAL enrollmentDAL, ITrainingDAL trainingDAL, IEmployeeDAL employeeDAL, IProofDAL proofDAL, IPreRequisiteDAL preRequisiteDAL,IDepartmentDAL departmentDAL, IEmailService emailService, INotificationDAL notificationDAL)
        {
            _enrollmentDAL = enrollmentDAL;
            _trainingDAL = trainingDAL;
            _employeeDAL = employeeDAL;
            _proofDAL = proofDAL;
            _preRequisiteDAL = preRequisiteDAL;
            _departmentDAL = departmentDAL;
            _emailService = emailService;
            _notificationDAL = notificationDAL;
        }
        public async Task<bool> DeleteEnrollmentAsync(int enrollmentId)
        {
            return await _enrollmentDAL.DeleteAsync(enrollmentId);
        }

        public async Task<EnrollmentViewModel> GetEnrollmentByIdAsync(int enrollmentId)
        {
            var enrollmentModel = await _enrollmentDAL.GetByIdAsync(enrollmentId);
            var enrollmentViewModel = new EnrollmentViewModel()
            {
                EnrollmentId = enrollmentId,
                Employee = await _employeeDAL.GetEmployeeByIdAsync(enrollmentModel.EmployeeId),
                Training = await _trainingDAL.GetByIdAsync(enrollmentModel.TrainingId),
                Proofs = (await _proofDAL.GetAllAsync()).Where(x => x.EnrollmentId == enrollmentModel.EnrollmentId).ToList(),
                Status = enrollmentModel.Status,
                CreatedOn = enrollmentModel.CreatedOn,
            };
            return enrollmentViewModel;
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetAllEnrollmentsAsync(EmployeeModel currentEmployee)
        {
            var enrollments = (await _enrollmentDAL.GetAllAsync()).ToList();
            var enrollmentsViews = new List<EnrollmentViewModel>();
            foreach (var enrollment in enrollments)
            {
                var employee = await _employeeDAL.GetEmployeeByIdAsync(enrollment.EmployeeId);
                var training = await _trainingDAL.GetByIdAsync(enrollment.TrainingId);
                var proofs = (await _proofDAL.GetAllAsync()).Where(p => p.EnrollmentId == enrollment.EnrollmentId).ToList();

                var enrollmentView = new EnrollmentViewModel()
                {
                    EnrollmentId = enrollment.EnrollmentId,
                    Employee = employee,
                    Training = training,
                    Status = enrollment.Status,
                    Proofs = proofs,
                    CreatedOn = enrollment.CreatedOn,
                };
                if (currentEmployee.CurrentRole == Role.Employee)
                {
                    if (currentEmployee.EmployeeId != enrollment.EmployeeId) continue;
                }
                else if (currentEmployee.CurrentRole == Role.Manager)
                {
                    if (currentEmployee.Department.DepartmentId != employee.Department.DepartmentId) continue;
                }
                enrollmentsViews.Add(enrollmentView);
            }
            return enrollmentsViews;
        }

        public async Task<bool> ApproveEnrollmentAsync(EnrollmentModel model, EmployeeModel manager)
        {
            model.Status = Status.Approved;
            var isApproved = await _enrollmentDAL.UpdateAsync(model);

            if (isApproved)
            {
                var enrollment = await _enrollmentDAL.GetByIdAsync(model.EnrollmentId);
                var employee = await _employeeDAL.GetEmployeeByIdAsync(enrollment.EmployeeId);
                var training = await _trainingDAL.GetByIdAsync(enrollment.TrainingId);

                string subject = "Training Enrollment Approved";
                string body = $@"<html><body>
                              <p>Dear {employee.FirstName},</p>
                              <p>Congratulations! Your {training.Title} training enrollment has been approved by your manager {manager.FirstName} {manager.LastName}, on {DateTime.Now.ToString("dddd, dd MMMM yyyy")} at {DateTime.Now.ToString("hh:mm tt")}.</p>
                              <p>Please check the details and make necessary arrangements.</p>
                              <p>Best regards,<br/>{employee.Department.Title}, SkillsLab</p>
                              </body></html>";
                string recipientEmail = employee.Email;
                string ccEmail = manager.Email;

                var notification = new NotificationModel()
                {
                    EmployeeId = employee.EmployeeId,
                    EmployeeRole = Role.Employee,
                    Message = $"Training {training.Title} enrollment approved by manager {manager.FirstName} {manager.LastName}.",
                };
                await _notificationDAL.AddAsync(notification);
                _emailService.SendEmail(subject, body, recipientEmail, ccEmail);
            }
            return isApproved;
        }

        public async Task<bool> DeclineEnrollmentAsync(EnrollmentModel model, EmployeeModel manager)
        {
            model.Status = Status.Declined;
            var isDeclined = await _enrollmentDAL.UpdateAsync(model);

            if (isDeclined)
            {
                var enrollment = await _enrollmentDAL.GetByIdAsync(model.EnrollmentId);
                var employee = await _employeeDAL.GetEmployeeByIdAsync(enrollment.EmployeeId);
                var training = await _trainingDAL.GetByIdAsync(enrollment.TrainingId);

                string subject = "Training Enrollment Declined";
                string body = $@"<html><body>
                              <p>Dear {employee.FirstName},</p>
                              <p>We regret to inform you that your {training.Title} training enrollment has been declined by your manager {manager.FirstName} {manager.LastName}, on {DateTime.Now.ToString("dddd, dd MMMM yyyy")} at {DateTime.Now.ToString("hh:mm tt")}.</p>
                              <p>Please check the details and make necessary arrangements.</p>
                              <p>Regards,<br/>{employee.Department.Title}, SkillsLab</p>
                              </body></html>";
                string recipientEmail = employee.Email;
                string ccEmail = manager.Email;

                var notification = new NotificationModel()
                {
                    EmployeeId = employee.EmployeeId,
                    EmployeeRole = Role.Employee,
                    Message = $"Training {training.Title} enrollment declined by manager {manager.FirstName} {manager.LastName}.",
                };
                await _notificationDAL.AddAsync(notification);
                _emailService.SendEmail(subject, body, recipientEmail, ccEmail);
            }
            return isDeclined;
        }

        public async Task<byte[]> ExportAsync(int trainingId)
        {
            var trainingTitle = (await _trainingDAL.GetByIdAsync(trainingId)).Title;
            var enrollments = (await _enrollmentDAL.GetAllAsync()).Where(e => e.TrainingId == trainingId).ToList();

            ExcelPackage.LicenseContext = Enum.TryParse(ConfigurationManager.AppSettings["EPPlus:ExcelPackage.LicenseContext"], out LicenseContext context) ? context : LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Enrollments");

                worksheet.Cells["A1"].Value = "Training";
                worksheet.Cells["B1"].Value = trainingTitle;

                worksheet.Cells["A3"].Value = "Employee's Name";
                worksheet.Cells["B3"].Value = "Phone Number";
                worksheet.Cells["C3"].Value = "Email";
                worksheet.Cells["D3"].Value = "Manager's Name";
                worksheet.Cells["E3"].Value = "Enrollment Date";

                int rowsInserted = 0;
                for (int i = 0; i < enrollments.Count; i++)
                {
                    var training = await _trainingDAL.GetByIdAsync(enrollments[i].TrainingId);
                    if (!training.IsClosed) continue;
                    if (enrollments[i].Status != Status.Approved) continue;

                    rowsInserted++;
                    var employee = await _employeeDAL.GetEmployeeByIdAsync(enrollments[i].EmployeeId);
                    var manager = (await _employeeDAL.GetAllEmployeesAsync()).FirstOrDefault(e => e.Department.DepartmentId == employee.Department.DepartmentId);

                    worksheet.Cells[i + 4, 1].Value = employee.FirstName + " " + employee.LastName;
                    worksheet.Cells[i + 4, 2].Value = employee.PhoneNumber;
                    worksheet.Cells[i + 4, 3].Value = employee.Email;
                    worksheet.Cells[i + 4, 4].Value = manager.FirstName + " " + manager.LastName;
                    worksheet.Cells[i + 4, 5].Value = enrollments[i].CreatedOn.ToString("f");
                }
                if (rowsInserted == 0) return null;

                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A3:E3"].Style.Font.Bold = true;

                worksheet.Cells["A1:B1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["A1:B1"].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                worksheet.Cells["A3:E3"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["A3:E3"].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                worksheet.Cells.AutoFitColumns();

                return package.GetAsByteArray();
            }
        }

        public async Task<string> EnrollAsync(LoginViewModel loggeduser, int trainingId, List<HttpPostedFileBase> files)
        {
            var employee = await _employeeDAL.GetEmployeeAsync(loggeduser);

            var prerequisites = (await _preRequisiteDAL.GetAllAsync()).Where(p => p.TrainingId == trainingId).ToList();

            if (prerequisites.Count == 0)
            {
                var enrollment = new EnrollmentModel
                {
                    EmployeeId = (await _employeeDAL.GetEmployeeAsync(loggeduser)).EmployeeId,
                    TrainingId = trainingId,
                    Status = Status.Pending
                };
                var resultEnrollemnt = await _enrollmentDAL.AddAsync(enrollment);
                if (resultEnrollemnt)
                {
                    await SendEmailAwaitingReviewToManager(employee, trainingId);
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

            var result = await _enrollmentDAL.AddAsync(enrollmentWithProofs);

            if (result)
            {
                await SendEmailAwaitingReviewToManager(employee, trainingId);
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

        private async Task SendEmailAwaitingReviewToManager(EmployeeModel employee, int trainingId)
        {
            var managers = await _employeeDAL.GetAllEmployeesAsync();
            var training = await _trainingDAL.GetByIdAsync(trainingId);

            var manager = managers.FirstOrDefault(e => e.Department.DepartmentId == employee.Department.DepartmentId);

            if (manager != null)
            {
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
                            <li><strong>Enrollment Date:</strong> {DateTime.Now:dddd, dd MMMM yyyy} at {DateTime.Now:hh:mm tt}</li>
                        </ul>
                        <p>Please review and provide any necessary approvals or feedback.</p>
                        <p>Best regards,<br/>Your System Administrator</p>
                        </body>
                    </html>";
                string recipientEmail = manager.Email;
                string ccEmail = employee.Email;

                var notification = new NotificationModel()
                {
                    EmployeeId = manager.EmployeeId,
                    EmployeeRole = Role.Manager,
                    Message = $"Employee {employee.FirstName} {employee.LastName} waiting for approval of training {training.Title}.",
                };
                await _notificationDAL.AddAsync(notification);
                _emailService.SendEmail(subject, body, recipientEmail, ccEmail);
            }
        }
    }
}
