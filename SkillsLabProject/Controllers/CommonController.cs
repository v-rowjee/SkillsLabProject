using SkillsLabProject.BL.BL;
using SkillsLabProject.Common.Enums;
using SkillsLabProject.Common.Models.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SkillsLabProject.Controllers
{
    public class CommonController : Controller
    {
        private IEmployeeBL _employeeBL;
        public CommonController(IEmployeeBL employeeBL)
        {
            _employeeBL = employeeBL;
        }
        // GET: Home
        public ActionResult Index()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (loggeduser.CurrentRole == Common.Enums.Role.Manager)
            {
                return RedirectToAction("Index", "Enrollment");
            }
            return RedirectToAction("Index", "Training");
        }

        //GET: Role
        [HttpGet]
        public async Task<ActionResult> Role()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser == null) return RedirectToAction("Index", "Login");

            var employee = await _employeeBL.GetEmployeeAsync(loggeduser);
            ViewBag.Employee = employee;

            var roles = (await _employeeBL.GetUserRolesAsync(employee.EmployeeId)).ToList();
            ViewBag.Roles = roles;

            return View();
        }
        [HttpPost]
        public JsonResult Role(string role)
        {
            if(Enum.TryParse(role, out Role roleEnum))
            {
                var loggedUser = Session["CurrentUser"] as LoginViewModel;
                loggedUser.CurrentRole = roleEnum;
                Session["CurrentUser"] = loggedUser;
                if (roleEnum == Common.Enums.Role.Manager)
                {
                    return Json(new { result = "Success", url = Url.Action("Index", "Enrollment") });
                }
                return Json(new { result = "Success", url = Url.Action("Index", "Training") });
            }
            return Json(new { result = "Error" });
        }

        // GET: Common/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}