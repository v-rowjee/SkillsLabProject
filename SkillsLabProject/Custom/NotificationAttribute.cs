using SkillsLabProject.BL.BL;
using SkillsLabProject.Common.Enums;
using SkillsLabProject.Common.Models.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SkillsLabProject.Custom
{
    public class NotificationAttribute : ActionFilterAttribute
    {
        public override async void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var employeeBL = DependencyResolver.Current.GetService<IEmployeeBL>();
            var notificationBL = DependencyResolver.Current.GetService<INotificationBL>();

            var loggedUser = filterContext.HttpContext.Session["CurrentUser"] as LoginViewModel;

            if (loggedUser != null)
            {
                var employee = await employeeBL.GetEmployeeAsync(loggedUser);
                //filterContext.Controller.ViewBag.Employee = employee;

                Enum.TryParse(filterContext.HttpContext.Session["CurrentRole"] as string, out Role role);
                //filterContext.Controller.ViewBag.Employee.Role = role;
                var notifications = (await notificationBL.GetAllByEmployeeAsync(employee)).Where(n => !n.IsRead && n.EmployeeRole == role).ToList();
                filterContext.Controller.ViewBag.Notifications = notifications;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
