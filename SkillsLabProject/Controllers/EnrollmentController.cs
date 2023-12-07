using SkillsLabProject.BLL;
using SkillsLabProject.Models;
using SkillsLabProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}