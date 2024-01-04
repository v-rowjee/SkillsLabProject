using SkillsLabProject.BL.BL;
using SkillsLabProject.Common.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult> Index()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser != null)
            {
                return RedirectToAction("Index","Common");
            }
            var departments = (await DepartmentBL.GetAllDepartmentsAsync()).ToList();
            ViewBag.Departments = departments;
            return View();
        }

        // POST: Register/Register
        [HttpPost]
        public async Task<JsonResult> Register(RegisterViewModel model)
        {
            var results = await AppUserBL.RegisterUserAsync(model);
            if (results.Contains("Success"))
            {
                return Json(new { result = "Success", url = Url.Action("Index", "Login") });
            }
            else
            {
                return Json(new { result = results.ToArray(), url = Url.Action("Index", "Login") });
            }
        }
    }
}