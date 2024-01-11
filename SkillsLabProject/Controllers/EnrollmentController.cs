using SkillsLabProject.BL.BL;
using SkillsLabProject.BL.Services;
using SkillsLabProject.Common.Enums;
using SkillsLabProject.Common.Models;
using SkillsLabProject.Common.Models.ViewModels;
using SkillsLabProject.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SkillsLabProject.Controllers
{
    [UserSession]
    public class EnrollmentController : Controller
    {
        private readonly IEmployeeBL _employeeBL;
        private readonly IEnrollmentBL _enrollmentBL;
        private readonly ITrainingBL _trainingBL;
        private readonly IPreRequisiteBL _preRequisiteBL;
        private readonly IProofBL _proofBL;
        private readonly IDeclinedEnrollmentBL _declinedEnrollmentBL;
        private readonly IEmailService _emailService;
        private readonly IDepartmentBL _departmentBL;
        private readonly INotificationBL _notificationBL;
        public EnrollmentController(IEmployeeBL employeeBL,IEnrollmentBL enrollmentBL, ITrainingBL trainingBL, IPreRequisiteBL preRequisiteBL, IProofBL proofBL, IDeclinedEnrollmentBL declinedEnrollmentBL,IDepartmentBL departmentBL, IEmailService emailService, INotificationBL notificationBL)
        {
            _employeeBL = employeeBL;
            _enrollmentBL = enrollmentBL;
            _trainingBL = trainingBL;
            _preRequisiteBL = preRequisiteBL;
            _proofBL = proofBL;
            _declinedEnrollmentBL = declinedEnrollmentBL;
            _departmentBL = departmentBL;
            _emailService = emailService;
            _notificationBL = notificationBL;
        }

        // GET: Enrollment
        [HttpGet]
        [CustomAuthorization("Employee,Manager,Admin")]
        public async Task<ActionResult> Index()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;            
            var employee = await _employeeBL.GetEmployeeAsync(loggeduser);
            ViewBag.Employee = employee;

            var enrollments = (await _enrollmentBL.GetAllEnrollmentsAsync(employee)).ToList();
            ViewBag.Enrollments = enrollments;

            var notificationCount = await _notificationBL.GetNotificationCountAsync(employee);
            ViewBag.NotificationCount = notificationCount;

            var trainings = await _trainingBL.GetAllTrainingViewModelsAsync();
            var departments = await _departmentBL.GetAllDepartmentsAsync();

            if (employee.CurrentRole == Role.Admin)
            {
                var departmentsWithEnrollments = enrollments.Select(e => e.Employee.Department.DepartmentId).Distinct();
                departments = (await _departmentBL.GetAllDepartmentsAsync()).Where(d => departmentsWithEnrollments.Contains(d.DepartmentId)).ToList();
            }
            else
            {
                departments = new List<DepartmentModel>() { employee.Department };
                var trainingEnrollment = (from enrollment in enrollments
                                         join training in trainings
                                         on enrollment.Training.TrainingId equals training.TrainingId
                                         select training);
                trainings = trainingEnrollment.Distinct().ToList();
            }
            ViewBag.Departments = departments;
            ViewBag.Trainings = trainings;


            return View();
        }
        // GET: View
        [HttpGet]
        [CustomAuthorization("Employee,Manager,Admin")]
        public async Task<ActionResult> View(int? id)
        {
            if (id == null) return RedirectToAction("Index");

            var enrollment = await _enrollmentBL.GetEnrollmentByIdAsync((int)id);
            if (enrollment == null) return RedirectToAction("Index");

            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var employee = await _employeeBL.GetEmployeeAsync(loggeduser);
            ViewBag.Employee = employee;

            var notificationCount = await _notificationBL.GetNotificationCountAsync(employee);
            ViewBag.NotificationCount = notificationCount;

            if (employee.CurrentRole == Role.Employee && enrollment.Employee.EmployeeId != employee.EmployeeId)
            {
                return RedirectToAction("Unauthorized","Error");
            }
            ViewBag.Enrollment = enrollment;

            var declineReason = (await _declinedEnrollmentBL.GetAllDeclinedEnrollmentsAsync()).Where(d => d.EnrollmentId == id).Select(d => d.Reason).FirstOrDefault();
            ViewBag.DeclineReason = declineReason;

            var proofs = (await _proofBL.GetAllProofsAsync()).Where(p => p.EnrollmentId == enrollment.EnrollmentId).ToList();
            ViewBag.Proofs = proofs;

            var preRequisites = (await _preRequisiteBL.GetAllPreRequisitesAsync()).Where(p => p.TrainingId == enrollment.Training.TrainingId).ToList();
            ViewBag.Prerequisites = preRequisites;

            return View();
        }

        // Post: Approve
        [HttpPost]
        [CustomAuthorization("Manager")]
        public async Task<JsonResult> Approve(EnrollmentModel model) {

            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var manager = await _employeeBL.GetEmployeeAsync(loggeduser);

            var result = await _enrollmentBL.ApproveEnrollmentAsync(model, manager);
            if (result)
            {
                return Json(new { result = "Success" });
            }
            else
            {
                return Json(new { result = "Error" });
            }
        }

        // Post: Decline
        [HttpPost]
        [CustomAuthorization("Manager")]
        public async Task<JsonResult> Decline(EnrollmentModel model)
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var manager = await _employeeBL.GetEmployeeAsync(loggeduser);

            var result = await _enrollmentBL.DeclineEnrollmentAsync(model, manager);
            if (result)
            {
                return Json(new { result = "Success" });
            }
            else
            {
                return Json(new { result = "Error" });
            }
        }

        // Post: Delete
        [HttpPost]
        [CustomAuthorization("Employee,Admin")]
        public async Task<JsonResult> Delete(int id)
        {
            var result = await _enrollmentBL.DeleteEnrollmentAsync(id);
            if (result)
            {
                return Json(new { result = "Success", url = Url.Action("Index", "Enrollment") });
            }
            else
            {
                return Json(new { result = "Error" });
            }
        }

        // POST: Enroll
        [HttpPost]
        [CustomAuthorization("Employee")]
        public async Task<JsonResult> Enroll(List<HttpPostedFileBase> files, int trainingId)
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var result = await _enrollmentBL.EnrollAsync(loggeduser, trainingId, files);

            return Json(new { result });
        }


        [HttpPost]
        [CustomAuthorization("Admin")]
        public async Task<FileResult> Export(int trainingId)
        {
            var fileContent = await _enrollmentBL.ExportAsync(trainingId);

            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet","ExportedEnrollemnts.xlsx");
        }
    }
}