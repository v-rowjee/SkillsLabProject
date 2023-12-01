using SkillsLabProject.BLL;
using SkillsLabProject.DAL;
using SkillsLabProject.Enums;
using SkillsLabProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkillsLabProject.Controllers
{
    public class RegisterController : Controller
    {
        public IAppUserBL AppUserBL;
        public IDepartmentBL DepartmentBL;

        public RegisterController(IAppUserBL appUserBL, IDepartmentBL departmentBL)
        {
            AppUserBL = appUserBL;
            DepartmentBL = departmentBL;
        }


        // GET: Register/
        public ActionResult Index()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser != null)
            {
                return Redirect("Home/Index");
            }

            var departments = DepartmentBL.GetAllDepartments().ToList();
            ViewBag.Departments = departments;
            return View();
        }

        // POST: Register/Register
        [HttpPost]
        public JsonResult Register(RegisterViewModel model)
        {
            model.Role = RoleEnum.Employee;
            var result = AppUserBL.RegisterUser(model);

            if (result == "Success")
            {
                return Json(new { result = result, url = Url.Action("Index", "Login") });
            }
            else if (result == "DuplicatedEmail")
            {
                return Json(new { result = result, url = Url.Action("Index", "Login") });
            }
            else
            {
                return Json(new { result = "Error" });
            }
        }



    }
}