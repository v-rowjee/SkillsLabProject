using SkillsLabProject.BL.BL;
using SkillsLabProject.Common.Enums;
using SkillsLabProject.Common.Models.ViewModels;
using SkillsLabProject.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SkillsLabProject.Controllers
{
    [UserSession]
    public class TrainingController : Controller
    {
        private readonly IEmployeeBL _employeeBL;
        private readonly ITrainingBL _trainingBL;
        private readonly IPreRequisiteBL _preRequisiteBL;
        private readonly IDepartmentBL _departmentBL;
        private readonly IEnrollmentBL _enrollmentBL;
        private readonly INotificationBL _notificationBL;
        public TrainingController(IEmployeeBL employeeBL, ITrainingBL trainingBL, IPreRequisiteBL preRequisiteBL, IDepartmentBL departmentBL, IEnrollmentBL enrollmentBL, INotificationBL notificationBL)
        {
            _employeeBL = employeeBL;
            _trainingBL = trainingBL;
            _preRequisiteBL = preRequisiteBL;
            _departmentBL = departmentBL;
            _enrollmentBL = enrollmentBL;
            _notificationBL = notificationBL;
        }

        [HttpGet]
        [CustomAuthorization("Employee,Manager,Admin")]
        public async Task<ActionResult> Index()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var employee = await _employeeBL.GetEmployeeAsync(loggeduser);
            ViewBag.Employee = employee;

            var notificationCount = await _notificationBL.GetNotificationCountAsync(employee);
            ViewBag.NotificationCount = notificationCount;

            var trainings = (await _trainingBL.GetAllTrainingViewModelsAsync(employee.EmployeeId)).Where(t => t.Deadline >= DateTime.Now.AddDays(-7)).ToList();
            ViewBag.Trainings = trainings;

            return View();
        }

        [HttpGet]
        [CustomAuthorization("Employee,Manager,Admin")]
        public async Task<ActionResult> View(int? id)
        {
            if (id == null) return RedirectToAction("Index");
            var training = await _trainingBL.GetTrainingByIdAsync((int)id);
            if (training == null) return RedirectToAction("Index");
            ViewBag.Training = training;

            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var employee = await _employeeBL.GetEmployeeAsync(loggeduser);
            ViewBag.Employee = employee;

            var notificationCount = await _notificationBL.GetNotificationCountAsync(employee);
            ViewBag.NotificationCount = notificationCount;

            var preRequisites = (await _preRequisiteBL.GetAllPreRequisitesAsync()).Where(p => p.TrainingId == training.TrainingId).ToList();
            ViewBag.Prerequisites = preRequisites.Any() ? preRequisites : null;

            var enrolledStatus = (await _enrollmentBL.GetAllEnrollmentsAsync(employee)).Where(e => e.Training.TrainingId == training.TrainingId).Select(e => e.Status).FirstOrDefault().ToString();
            ViewBag.EnrolledStatus = enrolledStatus;

            var enrollmentId = (await _enrollmentBL.GetAllEnrollmentsAsync(employee)).Where(e => e.Training.TrainingId == training.TrainingId).Select(e => e.EnrollmentId).FirstOrDefault();
            ViewBag.EnrollmentId = enrollmentId;

            ViewBag.IsEnrolled = enrolledStatus != "0";
            ViewBag.IsEmployee = loggeduser.CurrentRole == Role.Employee;

            return View();
        }
        [HttpGet]
        [CustomAuthorization("Admin")]
        public async Task<ActionResult> Create()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var employee = await _employeeBL.GetEmployeeAsync(loggeduser);
            ViewBag.Employee = employee;

            var notificationCount = await _notificationBL.GetNotificationCountAsync(employee);
            ViewBag.NotificationCount = notificationCount;

            var departments = await _departmentBL.GetAllDepartmentsAsync();
            ViewBag.Departments = departments;

            var prerequisiteDetails = (await _preRequisiteBL.GetAllPreRequisitesAsync()).Select(p => p.Detail).Distinct().ToList();
            ViewBag.PreRequisiteDetails = prerequisiteDetails;
            return View();
        }
        [HttpPost]
        [CustomAuthorization("Admin")]
        public async Task<JsonResult> Create(TrainingViewModel training)
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            ViewBag.Employee = _employeeBL.GetEmployeeAsync(loggeduser);

            var result = await _trainingBL.AddTrainingAsync(training);

            if (result)
            {
                return Json(new { result = "Success", url = Url.Action("Index", "Training") });
            }
            else
            {
                return Json(new { result = "Error" });
            }
        }
        [HttpGet]
        [CustomAuthorization("Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) return RedirectToAction("Index");

            var training = await _trainingBL.GetTrainingByIdAsync((int)id);
            if (training == null) return RedirectToAction("Index");
            ViewBag.Training = training;

            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var employee = await _employeeBL.GetEmployeeAsync(loggeduser);
            ViewBag.Employee = employee;

            var notificationCount = await _notificationBL.GetNotificationCountAsync(employee);
            ViewBag.NotificationCount = notificationCount;

            var departments = await _departmentBL.GetAllDepartmentsAsync();
            ViewBag.Departments = departments;



            var preRequisites = (await _preRequisiteBL.GetAllPreRequisitesAsync()).Where(p => p.TrainingId == training.TrainingId).ToList();
            ViewBag.Prerequisites = preRequisites.Any() ? preRequisites : null;

            var prerequisiteDetails = (await _preRequisiteBL.GetAllPreRequisitesAsync()).Select(p => p.Detail).Distinct().ToList();
            ViewBag.PreRequisiteDetails = prerequisiteDetails;

            return View();
        }
        [HttpPost]
        [CustomAuthorization("Admin")]
        public async Task<JsonResult> Edit(TrainingViewModel training)
        {
            var result = await _trainingBL.UpdateTrainingAsync(training);
            if (result)
            {
                return Json(new { result = "Success", url = Url.Action("View", "Training",training.TrainingId) });
            }
            else
            {
                return Json(new { result = "Error" });
            }
        }
        [HttpPost]
        [CustomAuthorization("Admin")]
        public async Task<JsonResult> Delete(int id)
        {
            var result = await _trainingBL.DeleteTrainingAsync(id);
            if (result.IsSuccess)
            {
                result.RedirectUrl = Url.Action("Index", "Training");
            }
            return Json(result);
        }

        [HttpPost]
        [CustomAuthorization("Admin")]
        public async Task<JsonResult> Close(int id)
        {
            var result = await _trainingBL.CloseTrainingAsync(id);
            if (result)
            {
                return Json(new { result = "Success", url = Url.Action("Index", "Training") });
            }
            else
            {
                return Json(new { result = "Error" });
            }
        }

    }
}