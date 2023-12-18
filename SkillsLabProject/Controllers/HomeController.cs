using SkillsLabProject.BL.BL;
using SkillsLabProject.Common.Enums;
using SkillsLabProject.Common.Models.ViewModels;
using SkillsLabProject.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Security;

namespace SkillsLabProject.Controllers
{
    public class HomeController : Controller
    {
        private IEmployeeBL _employeeBL;
        public HomeController(IEmployeeBL employeeBL)
        {
            _employeeBL = employeeBL;
        }
        // GET: Home
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Training");
        }

        //GET: Role
        [HttpGet]
        public ActionResult Role()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser == null) return RedirectToAction("Index", "Login");
            var employee = _employeeBL.GetEmployee(loggeduser);
            ViewBag.Employee = employee;

            var roles = _employeeBL.GetUserRoles(employee.EmployeeId).ToList();
            ViewBag.Roles = roles;

            return View();
        }
        [HttpPost]
        public JsonResult Role(string role)
        {
            if(Enum.TryParse(role, out Role roleEnum))
            {
                Session["CurrentRole"] = roleEnum.ToString();
                return Json(new { result = "Success", url = Url.Action("Index", "Training") });
            }
            return Json(new { result = "Error" });
        }

        // GET: Home/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}