using SkillsLabProject.BL.BL;
using SkillsLabProject.Common.Models.ViewModels;
using SkillsLabProject.Custom;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SkillsLabProject.Controllers
{
    [UserSession]
    [Notification]
    public class NotificationController : Controller
    {
        private readonly IEmployeeBL _employeeBL;
        private readonly INotificationBL _notificationBL;

        public NotificationController(IEmployeeBL employeeBL, INotificationBL notificationBL)
        {
            _employeeBL = employeeBL;
            _notificationBL = notificationBL;
        }

        // GET: Notification
        [HttpGet]
        [CustomAuthorization("Employee,Manager,Admin")]
        public async Task<ActionResult> Index()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var employee = await _employeeBL.GetEmployeeAsync(loggeduser);
            ViewBag.Employee = employee;

            var notifications = await _notificationBL.GetAllByEmployeeAsync(employee);
            ViewBag.Notifications = notifications;

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> MarkAsRead(int id)
        {
            var response = await _notificationBL.MarkAsReadAsync(id);

            if (response.IsSuccess)
            {
                response.RedirectUrl = Url.Action("Index", "Notification");
            }

            return Json(response);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var response = await _notificationBL.DeleteAsync(id);

            if (response.IsSuccess)
            {
                response.RedirectUrl = Url.Action("Index", "Notification");
            }

            return Json(response);
        }

    }
}