using SkillsLabProject.Common.Exceptions;
using System.Web.Mvc;
using System.Web.Routing;

namespace SkillsLab.Common.Exceptions
{
    public class GlobalExceptionHandler : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                CustomException customException = new CustomException(filterContext.Exception);
                customException.Log();

                filterContext.ExceptionHandled = true;

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    { "controller", "Error" },
                    { "action", "InternalServerError" }
                });
            }
        }
    }
}
