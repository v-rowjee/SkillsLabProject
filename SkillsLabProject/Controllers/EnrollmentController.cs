using SkillsLabProject.BL.BL;
using SkillsLabProject.Common.Enums;
using SkillsLabProject.Common.Models;
using SkillsLabProject.Common.Models.ViewModels;
using SkillsLabProject.Custom;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SkillsLabProject.Controllers
{
    [UserSession]
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
        [CustomAuthorization("Employee,Manager,Admin")]
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
        [CustomAuthorization("Employee,Manager,Admin")]
        public async Task<JsonResult> Enroll(int trainingId, List<HttpPostedFileBase> files)
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;

            if (files != null && files.Any())
            {
                var enrollmentWithProofs = new EnrollmentViewModel() 
                { 
                    EmployeeId = _employeeBL.GetEmployee(loggeduser).EmployeeId,
                    TrainingId = trainingId,
                    ProofUrls = new List<string>(),
                    Status = Status.Pending
                };
                foreach (var file in files)
                {
                    FileStream stream;
                    if (file.ContentLength > 0)
                    {
                        string path = Path.Combine(Server.MapPath("~/Content/Uploads/"), file.FileName);
                        file.SaveAs(path);
                        stream = new FileStream(Path.Combine(path), FileMode.Open);
                        string downloadUrl = await Task.Run(() => _enrollmentBL.UploadAndGetDownloadUrl(stream, file.FileName));
                        enrollmentWithProofs.ProofUrls.Add(downloadUrl);
                    }
                }
                var result = _enrollmentBL.AddEnrollment(enrollmentWithProofs);

                if (result)
                {
                    return Json(new { result = "Success", url = Url.Action("Index", "Enrollment") });
                }
                else
                {
                    return Json(new { result = "Error" });
                }
            }
            else
            {
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