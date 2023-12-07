using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SkillsLabProject.Custom
{
    public class CustomAuthorizationAttribute : ActionFilterAttribute
    {
        public string[] AuthorizedRoles { get; set; }
        public CustomAuthorizationAttribute(string roles) 
        {
            AuthorizedRoles = roles.Split(',');
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var controller = filterContext.Controller as Controller;
            if (controller != null && controller.Session["CurrentRole"] != null)
            {
                var currentRole = controller.Session["CurrentRole"] as string;
                if(!AuthorizedRoles.Contains(currentRole))
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "Unauthorized" }));
                }
            }
        }
    }
}