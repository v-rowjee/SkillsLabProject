using SkillsLabProject.BL.RepositoryBL;
using SkillsLabProject.BLL;
using SkillsLabProject.DAL.Enums;
using SkillsLabProject.DAL.Models;
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
        private readonly AppUserBL _appUserBL;
        private readonly IRepositoryBL<Department> _departmentBL;

        public RegisterController(AppUserBL appUserBL, IRepositoryBL<Department> departmentRepository)
        {
            _appUserBL = appUserBL;
            _departmentBL = departmentRepository;
        }


        // GET: Register/
        public ActionResult Index()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser != null)
            {
                return Redirect("Home/Index");
            }

            var departments = _departmentBL.GetAll().GetModelList();
            ViewBag.Departments = departments;
            return View();
        }

        // POST: Register/Register
        [HttpPost]
        public JsonResult Register(RegisterViewModel model)
        {
            model.RoleId = (int) Role.Employee;
            var result = _appUserBL.RegisterUser(model);

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