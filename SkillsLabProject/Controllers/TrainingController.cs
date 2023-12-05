using SkillsLabProject.BL.BL;
using SkillsLabProject.BLL;
using SkillsLabProject.DAL.Models;
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
        private IPreRequisiteBL _preRequisiteBL;
        public TrainingController(IEmployeeBL employeeBL, ITrainingBL trainingBL, IPreRequisiteBL preRequisiteBL)
        {
            _employeeBL = employeeBL;
            _trainingBL = trainingBL;
            _preRequisiteBL = preRequisiteBL;
        }

        // GET: Training
        public ActionResult Index()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser != null)
            {
                var employee = _employeeBL.GetEmployee(loggeduser);
                ViewBag.Employee = employee;
                var trainings = _trainingBL.GetAllTrainings().Where(t => t.Deadline >= DateTime.Now).OrderBy(t => t.Deadline);
                ViewBag.Trainings = trainings;
                return View();
            }
            return RedirectToAction("Index","Login");
        }

        // GET: Training/View/#
        public ActionResult View(int? id)
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;

            if (loggeduser == null) return RedirectToAction("Index","Login"); 
            ViewBag.Employee = _employeeBL.GetEmployee(loggeduser);

            if (id == null) return RedirectToAction("Index");
            var training = _trainingBL.GetTrainingById((int)id);

            if (training == null) return RedirectToAction("Index");
            ViewBag.Training = training;
            var preRequisites = new List<PreRequisite>();//_preRequisiteBL.GetAllPreRequisites().Where(p => p.TrainingId == training.TrainingId).ToList();
            ViewBag.Prerequisites = preRequisites.Any() ? preRequisites : null;
            
            return View();
        }
    }
}