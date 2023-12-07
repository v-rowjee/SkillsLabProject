using SkillsLabProject.BLL;
using SkillsLabProject.Custom;
using SkillsLabProject.DAL.Common;
using SkillsLabProject.Enums;
using SkillsLabProject.Models;
using SkillsLabProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JEDI_Carpool.Controllers
{
    public class LoginController : Controller
    {
        private IAppUserBL AppUserBL;
        private IEmployeeBL EmployeeBL;

        public LoginController(IAppUserBL appUserBL,IEmployeeBL employeeBL)
        {
            this.AppUserBL = appUserBL;
            this.EmployeeBL = employeeBL;
        }

        // GET: Login
        public ActionResult Index()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser != null)
            {
                return Redirect("Training/Index");
            }
            return View();
        }

        // POST: Login/Authenticate
        [HttpPost]
        public JsonResult Authenticate(LoginViewModel model)
        {
            var IsUserValid = AppUserBL.AuthenticateUser(model);
            if (IsUserValid)
            {
                this.Session["CurrentUser"] = model;
                var employee = EmployeeBL.GetEmployee(model);
                this.Session["CurrentRole"] = employee.Role.ToString();
            }
            return Json(new { result = IsUserValid, url = Url.Action("Index", "Training") });

        }
    }
}