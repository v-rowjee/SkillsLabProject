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

            var enrollments = _enrollmentBL.GetAllEnrollmentsOfEmployee(employee.EmployeeId).ToList();

            ViewBag.Enrollments = enrollments;
            return View();
        }
        // Get: All
        [CustomAuthorization("Manager")]
        public ActionResult All()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var manager = _employeeBL.GetEmployee(loggeduser);
            ViewBag.Employee = manager;

            var enrollments = _enrollmentBL.GetAllEnrollmentsOfManager(manager.EmployeeId).ToList();
            ViewBag.Enrollments = enrollments;

            return View();
        }
        // GET: View
        [HttpGet]
        [CustomAuthorization("Manager")]
        public ActionResult View(int? id)
        {
            if (id == null) return RedirectToAction("All");
            var enrollment = _enrollmentBL.GetEnrollmentById((int)id);
            if (enrollment == null) return RedirectToAction("All");
            ViewBag.Enrollment = enrollment;

            var proofs = _proofBL.GetAllProofs().Where(p => p.EnrollmentId == enrollment.EnrollmentId).ToList();
            ViewBag.Proofs = proofs;

            var preRequisites = _preRequisiteBL.GetAllPreRequisites().Where(p => p.TrainingId == enrollment.Training.TrainingId).ToList();
            ViewBag.Prerequisites = preRequisites;

            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var employee = _employeeBL.GetEmployee(loggeduser);
            ViewBag.Employee = employee;
            
            return View();
        }

        // Post: Edit
        [HttpPost]
        [CustomAuthorization("Manager")]
        public JsonResult Edit(EnrollmentModel model) {
            var result = _enrollmentBL.UpdateEnrollment(model);
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

            var prerequisites = _preRequisiteBL.GetAllPreRequisites().Where(p => p.TrainingId == trainingId).ToList();

            if (files != null && files.Any() && files.Count >= prerequisites.Count)
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

            return result ? Json(new { result = "Success" }) : Json(new { result = "Error" });
        }
    }
}