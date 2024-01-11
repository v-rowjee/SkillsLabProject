using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkillsLabProject.Custom
{
    public class UserSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["CurrentUser"] == null)
            {
                filterContext.Result = new RedirectResult("~/Login/Index");
            }
        }
    }
}