using SkillsLabProject.BL.BL;
using SkillsLabProject.Common.Enums;
using SkillsLabProject.Common.Models;
using SkillsLabProject.Common.Models.ViewModels;
using SkillsLabProject.Custom;
using System.Collections.Generic;
using System.IO;
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
        private IProofBL _proofBL;
        public EnrollmentController(IEmployeeBL employeeBL,IEnrollmentBL enrollmentBL, ITrainingBL trainingBL, IPreRequisiteBL preRequisiteBL, IProofBL proofBL)
        {
            _employeeBL = employeeBL;
            _enrollmentBL = enrollmentBL;
            _trainingBL = trainingBL;
            _preRequisiteBL = preRequisiteBL;
            _proofBL = proofBL;
        }
        // GET: Enrollment
        [CustomAuthorization("Employee,Manager,Admin")]
        public ActionResult Index()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser == null) return RedirectToAction("Index", "Login");
            
            var employee = _employeeBL.GetEmployee(loggeduser);
            ViewBag.Employee = employee;

            var enrollments = _enrollmentBL.GetAllEnrollments().Where(x => x.EmployeeId==employee.EmployeeId).ToList();

            var trainings = new List<TrainingModel>();
            var enrollmentsViews = new List<EnrollmentViewModel>();
            foreach (var enrollment in enrollments)
            {
                var training = new TrainingModel();
                training = _trainingBL.GetTrainingById(enrollment.TrainingId);
                trainings.Add(training);

                var enrollmentView = new EnrollmentViewModel()
                {
                    EnrollmentId = enrollment.EnrollmentId,
                    Employee = employee,
                    Training = training,
                    Status = enrollment.Status,
                    Proofs = _proofBL.GetAllProofs().Where(x => x.EnrollmentId == enrollment.EnrollmentId).ToList(),
                };
                enrollmentsViews.Add(enrollmentView);
            }
            ViewBag.Enrollments = enrollmentsViews;
            return View();
        }
        // Get: All
        [CustomAuthorization("Manager")]
        public ActionResult All()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser == null) return RedirectToAction("Index", "Login");

            var employee = _employeeBL.GetEmployee(loggeduser);
            ViewBag.Employee = employee;
            var enrollments = _enrollmentBL.GetAllEnrollments().ToList();

            var enrollmentsViews = new List<EnrollmentViewModel>();
            foreach (var enrollment in enrollments)
            {
                var enrollmentView = new EnrollmentViewModel()
                {
                    EnrollmentId = enrollment.EnrollmentId,
                    Employee = _employeeBL.GetAllEmployees().Where(e => e.EmployeeId == enrollment.EmployeeId).FirstOrDefault(),
                    Training = _trainingBL.GetTrainingById(enrollment.TrainingId),
                    Status = enrollment.Status,
                    Proofs = _proofBL.GetAllProofs().Where(x => x.EnrollmentId == enrollment.EnrollmentId).ToList(),
                };
                enrollmentsViews.Add(enrollmentView);
            }
            ViewBag.Enrollments = enrollmentsViews;
            return View();
        }
        // Post: Delete
        [HttpPost]
        [CustomAuthorization("Employee,Manager,Admin")]
        public JsonResult Delete(int id)
        {
            var result = _enrollmentBL.DeleteEnrollment(id);
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
        [CustomAuthorization("Employee,Manager,Admin")]
        public async Task<JsonResult> Enroll(List<HttpPostedFileBase> files, int trainingId)
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            bool result;

            var prerequisites = _preRequisiteBL.GetAllPreRequisites().Where(p => p.TrainingId ==  trainingId).ToList();

            if (files != null && files.Any() && files.Count == prerequisites.Count)
            {
                var enrollmentWithProofs = new EnrollmentViewModel() 
                { 
                    Employee = _employeeBL.GetEmployee(loggeduser),
                    Training = new TrainingModel() { TrainingId = trainingId },
                    Proofs = new List<ProofModel>(),
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
                        string downloadUrl = await _enrollmentBL.UploadAndGetDownloadUrl(stream, file.FileName);
                        enrollmentWithProofs.Proofs.Add(new ProofModel() { Attachment = downloadUrl });
                    }
                }
                result = _enrollmentBL.AddEnrollment(enrollmentWithProofs);
            }
            else if(prerequisites.Count == 0)
            {
                var enrollment = new EnrollmentModel()
                {
                    EmployeeId = _employeeBL.GetEmployee(loggeduser).EmployeeId,
                    TrainingId = trainingId,
                    Status = Status.Pending
                };
                result = _enrollmentBL.AddEnrollment(enrollment);
            }
            else
            {
                return Json(new { result = "FileMissing" });
            }

            return result ? Json(new { result = "Success", url = Url.Action("Index", "Enrollment") }) : Json(new { result = "Error" });
        }
    }
}