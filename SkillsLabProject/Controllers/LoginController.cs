using SkillsLabProject.BLL;
using SkillsLabProject.DAL.Common;
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
        public IAppUserBL AppUserBL;

        public LoginController(IAppUserBL appUserBL)
        {
            this.AppUserBL = appUserBL;
        }

        // GET: Login
        public ActionResult Index()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser != null)
            {
                return Redirect("Home/Index");
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
            }
            return Json(new { result = IsUserValid, url = Url.Action("Index", "Home") });

        }
    }
}