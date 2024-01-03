using SkillsLabProject.Common.Exceptions;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SkillsLabProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UnityConfig.RegisterComponents();
        }

        protected void Application_Error()
        {
            var error = Server.GetLastError();
            var exception = new CustomException(error);
            exception.Log();
        }
    }
}
