using Hangfire;
using Hangfire.SqlServer;
using System.Collections.Generic;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Configuration;
using SkillsLabProject.BL.BL;

namespace SkillsLabProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private IEnumerable<IDisposable> GetHangfireServers()
        {
            var hangfireConnectionString = ConfigurationManager.AppSettings["HangfireConnectionString"].ToString();

            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(hangfireConnectionString);

            yield return new BackgroundJobServer();
        }
        protected void Application_Start()
        {
            UnityConfig.RegisterComponents();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            HangfireAspNet.Use(GetHangfireServers);
            RecurringJob.AddOrUpdate<ITrainingBL>("EnrollmentProcessingJob", x =>  x.AutoCloseTrainingAsync(), Cron.Minutely);
        }
    }
}
