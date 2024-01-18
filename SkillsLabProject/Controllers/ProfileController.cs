using SkillsLabProject.BL.BL;
using SkillsLabProject.Common.Models.ViewModels;
using SkillsLabProject.Custom;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SkillsLabProject.Controllers
{
    [UserSession]
    public class ProfileController : Controller
    {
        private readonly IEmployeeBL _employeeBL;
        private readonly INotificationBL _notificationBL;

        public ProfileController(IEmployeeBL employeeBL, INotificationBL notificationBL)
        {
            _employeeBL = employeeBL;
            _notificationBL = notificationBL;
        }

        // GET: Dojo
        [HttpGet]
        [CustomAuthorization("Employee,Manager,Admin")]
        public async Task<ActionResult> Index()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var employee = await _employeeBL.GetEmployeeAsync(loggeduser);
            ViewBag.Employee = employee;

            var notificationCount = await _notificationBL.GetNotificationCountAsync(employee);
            ViewBag.NotificationCount = notificationCount;
            return View();
        }
    }
}