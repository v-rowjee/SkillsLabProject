using SkillsLabProject.Common.Models.ViewModels;
using System.Web.Mvc;
using SkillsLabProject.BL.BL;

namespace SkillsLabProject.Controllers
{
    public class LoginController : Controller
    {
        private IAppUserBL _appUserBL;
        private IEmployeeBL _employeeBL;

        public LoginController(IAppUserBL appUserBL,IEmployeeBL employeeBL)
        {
            _appUserBL = appUserBL;
            _employeeBL = employeeBL;
        }

        // GET: Login
        public ActionResult Index()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser != null)
            {
                return RedirectToAction("Role", "Common");
            }
            var x = 0;
            x = 0 / x;
            return View();
        }

        // POST: Login/Authenticate
        [HttpPost]
        public JsonResult Authenticate(LoginViewModel model)
        {
            var IsUserValid = _appUserBL.AuthenticateUser(model);
            if (IsUserValid)
            {
                Session["CurrentUser"] = model;
            }
            return Json(new { result = IsUserValid, url = Url.Action("Role", "Common") });

        }
    }
}