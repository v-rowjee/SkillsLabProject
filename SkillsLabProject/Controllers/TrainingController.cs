using SkillsLabProject.BL.BL;
using SkillsLabProject.Common.Models.ViewModels;
using SkillsLabProject.Custom;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SkillsLabProject.Controllers
{
    [UserSession]
    public class TrainingController : Controller
    {
        private IEmployeeBL _employeeBL;
        private ITrainingBL _trainingBL;
        private IPreRequisiteBL _preRequisiteBL;
        private IDepartmentBL _departmentBL;
        public TrainingController(IEmployeeBL employeeBL, ITrainingBL trainingBL, IPreRequisiteBL preRequisiteBL, IDepartmentBL departmentBL)
        {
            _employeeBL = employeeBL;
            _trainingBL = trainingBL;
            _preRequisiteBL = preRequisiteBL;
            _departmentBL = departmentBL;
        }
        [HttpGet]
        [CustomAuthorization("Employee,Manager,Admin")]
        public ActionResult Index()
        {
            ViewBag.CurrentRole = Session["CurrentRole"];
            var loggeduser = Session["CurrentUser"] as LoginViewModel;

            var employee = _employeeBL.GetEmployee(loggeduser);
            ViewBag.Employee = employee;
            var trainings = _trainingBL.GetAllTrainings().Where(t => t.Deadline >= DateTime.Now);
            ViewBag.Trainings = trainings;
            return View();
        }
                    
        [HttpGet]
        [CustomAuthorization("Employee,Manager,Admin")]
        public ActionResult View(int? id)
        {
            if (id == null) return RedirectToAction("Index");
            var training = _trainingBL.GetTrainingById((int)id);
            if (training == null) return RedirectToAction("Index");
            ViewBag.Training = training;
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            ViewBag.Employee = _employeeBL.GetEmployee(loggeduser);
            var preRequisites = _preRequisiteBL.GetAllPreRequisites().Where(p => p.TrainingId == training.TrainingId).ToList();
            ViewBag.Prerequisites = preRequisites.Any() ? preRequisites : null;
            return View();
        }
        [HttpGet]
        [CustomAuthorization("Admin")]
        public ActionResult Create()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            ViewBag.Employee = _employeeBL.GetEmployee(loggeduser);

            var departments = _departmentBL.GetAllDepartments();
            ViewBag.Departments = departments;

            var prerequisiteDetails = _preRequisiteBL.GetAllPreRequisites().Select(p => p.Detail).Distinct().ToList();
            ViewBag.PreRequisiteDetails = prerequisiteDetails;
            return View();
        }
        [HttpPost]
        [CustomAuthorization("Admin")]
        public JsonResult Create(TrainingViewModel training)
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            ViewBag.Employee = _employeeBL.GetEmployee(loggeduser);

            var result = _trainingBL.AddTraining(training);

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
        public ActionResult Edit(int? id)
        {
            if (id == null) return RedirectToAction("Index");

            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            ViewBag.Employee = _employeeBL.GetEmployee(loggeduser);

            var departments = _departmentBL.GetAllDepartments();
            ViewBag.Departments = departments;

            var training = _trainingBL.GetTrainingById((int)id);
            if (training == null) return RedirectToAction("Index");
            ViewBag.Training = training;

            var preRequisites = _preRequisiteBL.GetAllPreRequisites().Where(p => p.TrainingId == training.TrainingId).ToList();
            ViewBag.Prerequisites = preRequisites.Any() ? preRequisites : null;

            var prerequisiteDetails = _preRequisiteBL.GetAllPreRequisites().Select(p => p.Detail).Distinct().ToList();
            ViewBag.PreRequisiteDetails = prerequisiteDetails;

            return View();
        }
        [HttpPost]
        [CustomAuthorization("Admin")]
        public JsonResult Edit(TrainingViewModel training)
        {
            var result = _trainingBL.UpdateTraining(training);
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
        public JsonResult Delete(int id)
        {
            var result = _trainingBL.DeleteTraining(id);
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