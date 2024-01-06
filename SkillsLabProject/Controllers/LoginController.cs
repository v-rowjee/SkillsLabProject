using SkillsLabProject.Common.Models.ViewModels;
using System.Web.Mvc;
using SkillsLabProject.BL.BL;
using System.Threading.Tasks;

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
        public async Task<JsonResult> Authenticate(LoginViewModel model)
        {
            var response = await _appUserBL.AuthenticateUserAsync(model);

            if (response.IsSuccess)
            {
                Session["CurrentUser"] = model;
                response.RedirectUrl = Url.Action("Role", "Common");
            }

            return Json(response);
        }
    }
}