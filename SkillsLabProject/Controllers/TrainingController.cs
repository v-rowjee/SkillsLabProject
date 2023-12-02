using SkillsLabProject.BLL;
using SkillsLabProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkillsLabProject.Controllers
{
    public class TrainingController : Controller
    {
        private IEmployeeBL _employeeBL;
        private ITrainingBL _trainingBL;
        public TrainingController(IEmployeeBL employeeBL, ITrainingBL trainingBL)
        {
            _employeeBL = employeeBL;
            _trainingBL = trainingBL;
        }

        // GET: Training
        public ActionResult Index()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser != null)
            {
                var employee = _employeeBL.GetEmployee(loggeduser);
                ViewBag.Employee = employee;
                var trainings = _trainingBL.GetAllTrainings().OrderBy(x => x.PriorityDepartmentId == employee.Department.DepartmentId);
                ViewBag.Training = trainings;

                return View();
            }
            return RedirectToAction("Index","Login");
        }
    }
}