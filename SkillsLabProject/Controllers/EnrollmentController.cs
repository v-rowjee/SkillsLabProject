using SkillsLabProject.BL.BL;
using SkillsLabProject.BL.Services;
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
        private IDeclinedEnrollmentBL _declinedEnrollmentBL;
        private IEmailService _emailService;
        public EnrollmentController(IEmployeeBL employeeBL,IEnrollmentBL enrollmentBL, ITrainingBL trainingBL, IPreRequisiteBL preRequisiteBL, IProofBL proofBL, IDeclinedEnrollmentBL declinedEnrollmentBL, IEmailService emailService)
        {
            _employeeBL = employeeBL;
            _enrollmentBL = enrollmentBL;
            _trainingBL = trainingBL;
            _preRequisiteBL = preRequisiteBL;
            _proofBL = proofBL;
            _declinedEnrollmentBL = declinedEnrollmentBL;
            _emailService = emailService;
        }
        // GET: Enrollment
        [CustomAuthorization("Employee,Manager")]
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
        [CustomAuthorization("Manager,Admin")]
        public ActionResult All()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var manager = _employeeBL.GetEmployee(loggeduser);
            ViewBag.Employee = manager;

            List<EnrollmentViewModel> enrollments;
            if (Session["CurrentRole"] as string == "Admin")
            {
                enrollments = _enrollmentBL.GetAllEnrollments().ToList();
            }
            else
            {
                enrollments = _enrollmentBL.GetAllEnrollmentsOfManager(manager.EmployeeId).ToList();
            }
            ViewBag.Enrollments = enrollments;

            return View();
        }
        // GET: View
        [HttpGet]
        [CustomAuthorization("Manager,Admin")]
        public ActionResult View(int? id)
        {
            if (id == null) return RedirectToAction("All");
            var enrollment = _enrollmentBL.GetEnrollmentById((int)id);
            if (enrollment == null) return RedirectToAction("All");
            ViewBag.Enrollment = enrollment;

            var declineReason = _declinedEnrollmentBL.GetAllDeclinedEnrollments().Where(d => d.EnrollmentId == id).Select(d => d.Reason).FirstOrDefault();
            ViewBag.DeclineReason = declineReason;

            var proofs = _proofBL.GetAllProofs().Where(p => p.EnrollmentId == enrollment.EnrollmentId).ToList();
            ViewBag.Proofs = proofs;

            var preRequisites = _preRequisiteBL.GetAllPreRequisites().Where(p => p.TrainingId == enrollment.Training.TrainingId).ToList();
            ViewBag.Prerequisites = preRequisites;

            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var employee = _employeeBL.GetEmployee(loggeduser);
            ViewBag.Employee = employee;
            
            return View();
        }

        // Post: Approve
        [HttpPost]
        [CustomAuthorization("Manager")]
        public JsonResult Approve(EnrollmentModel model) {

            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var manager = _employeeBL.GetEmployee(loggeduser);

            var result = _enrollmentBL.ApproveEnrollment(model, manager);
            if (result)
            {
                return Json(new { result = "Success" });
            }
            else
            {
                return Json(new { result = "Error" });
            }
        }

        // Post: Decline
        [HttpPost]
        [CustomAuthorization("Manager")]
        public JsonResult Decline(EnrollmentModel model)
        {
            model.Status = Status.Declined;
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
        [CustomAuthorization("Employee,Manager")]
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
        [CustomAuthorization("Employee")]
        public async Task<JsonResult> Enroll(List<HttpPostedFileBase> files, int trainingId)
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var result = await _enrollmentBL.Enroll(loggeduser, trainingId, files);

            switch (result)
            {
                case "Success":
                    return Json(new { result = "Success" });
                case "FileMissing":
                    return Json(new { result = "FileMissing" });
                default:
                    return Json(new { result = "Error" });
            }
        }

    }
}