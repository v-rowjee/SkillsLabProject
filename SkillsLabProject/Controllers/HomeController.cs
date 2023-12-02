using SkillsLabProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkillsLabProject.BLL;

namespace SkillsLabProject.Controllers
{
    public class HomeController : Controller
    {
        private IEmployeeBL EmployeeBL;
        public HomeController(IEmployeeBL employeeBL)
        {
            this.EmployeeBL = employeeBL;
        }

        // GET: Home
        public ActionResult Index()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var view = View();

            if (loggeduser != null)
            {
                var employee = EmployeeBL.GetEmployee(loggeduser);
                ViewBag.Employee = employee;

                view.MasterName = "~/Views/Shared/_Layout.cshtml";
            }
            else
            {
                view.MasterName = "~/Views/Shared/_GuestLayout.cshtml";
            }

            return view;
        }

        // GET: Home/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}