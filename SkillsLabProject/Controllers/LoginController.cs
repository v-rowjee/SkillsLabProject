using SkillsLabProject.Common.Models.ViewModels;
using System.Web.Mvc;
using SkillsLabProject.BL.BL;

namespace SkillsLabProject.Controllers
{
    public class LoginController : Controller
    {
        private IAppUserBL _appUserBL;

        public LoginController(IAppUserBL appUserBL)
        {
            _appUserBL = appUserBL;
        }

        // GET: Login
        public ActionResult Index()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser != null)
            {
                return RedirectToAction("Role", "Common");
            }
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