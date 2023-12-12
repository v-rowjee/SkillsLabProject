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
                    EmployeeId = enrollment.EmployeeId,
                    TrainingId = enrollment.TrainingId,
                    Status = enrollment.Status,
                    ProofUrls = _proofBL.GetAllProofs().Where(x => x.EnrollmentId == enrollment.EnrollmentId).Select(x => x.Attachment).ToList(),
                };
                enrollmentsViews.Add(enrollmentView);

            }
            ViewBag.Trainings = trainings;
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
                        string downloadUrl = await _enrollmentBL.UploadAndGetDownloadUrl(stream, file.FileName);
                        enrollmentWithProofs.ProofUrls.Add(downloadUrl);
                    }
                }
                result = _enrollmentBL.AddEnrollment(enrollmentWithProofs);
            }
            else
            {
                var enrollment = new EnrollmentModel()
                {
                    EmployeeId = _employeeBL.GetEmployee(loggeduser).EmployeeId,
                    TrainingId = trainingId,
                    Status = Status.Pending
                };
                result = _enrollmentBL.AddEnrollment(enrollment);
            }

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