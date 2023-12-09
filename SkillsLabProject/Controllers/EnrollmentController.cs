using SkillsLabProject.BL.BL;
using SkillsLabProject.Common.Enums;
using SkillsLabProject.Common.Models;
using SkillsLabProject.Common.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace SkillsLabProject.Controllers
{
    public class EnrollmentController : Controller
    {
        private IEmployeeBL _employeeBL;
        private IEnrollmentBL _enrollmentBL;
        private ITrainingBL _trainingBL;
        private IPreRequisiteBL _preRequisiteBL;
        public EnrollmentController(IEmployeeBL employeeBL,IEnrollmentBL enrollmentBL, ITrainingBL trainingBL, IPreRequisiteBL preRequisiteBL)
        {
            _employeeBL = employeeBL;
            _enrollmentBL = enrollmentBL;
            _trainingBL = trainingBL;
            _preRequisiteBL = preRequisiteBL;
        }
        // GET: Enrollment
        public ActionResult Index()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser == null) return RedirectToAction("Index", "Login");
            
            var employee = _employeeBL.GetEmployee(loggeduser);
            ViewBag.Employee = employee;

            var enrollments = _enrollmentBL.GetAllEnrollments().Where(x => x.EmployeeId==employee.EmployeeId);

            var trainings = new List<TrainingModel>();
            foreach (var enrollment in enrollments)
            {
                var training = new TrainingModel();
                training = _trainingBL.GetTrainingById(enrollment.TrainingId);
                trainings.Add(training);
            }
            ViewBag.Trainings = trainings;
            return View();
        }

        // POST: Enroll
        [HttpPost]
        public JsonResult Enroll(int trainingId, List<HttpPostedFileBase> files)
        {
            if (files != null && files.Any())
            {
                foreach (var item in files)
                {
                    if (item.ContentLength > 0)
                    {
                        var loggeduser = Session["CurrentUser"] as LoginViewModel;
                        var enrollment = new EnrollmentModel()
                        {
                            EmployeeId = _employeeBL.GetEmployee(loggeduser).EmployeeId,
                            TrainingId = trainingId,
                            Status = Status.Pending
                        };

                        var result = _enrollmentBL.AddEnrollment(enrollment);

                        if (result)
                        {
                            return Json(new { result = "Success", url = Url.Action("Index", "Enrollment") });
                        }
                        else
                        {
                            return Json(new { result = "Error" });
                        }
                    }
                }
                return Json(new { result = "No file found." });
            }
            else
            {
                var loggeduser = Session["CurrentUser"] as LoginViewModel;
                var enrollment = new EnrollmentModel()
                {
                    EmployeeId = _employeeBL.GetEmployee(loggeduser).EmployeeId,
                    TrainingId = trainingId,
                    Status = Status.Pending
                };

                var result = _enrollmentBL.AddEnrollment(enrollment);

                if (result)
                {
                    return Json(new { result = "Success", url = Url.Action("Index", "Enrollment") });
                }
                else
                {
                    return Json(new { result = "Error" });
                }
            }
        }
    }
}