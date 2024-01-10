using Hangfire;
using Hangfire.SqlServer;
using System.Collections.Generic;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Configuration;
using SkillsLabProject.BL.BL;
using System.Diagnostics;
using SkillsLabProject.DAL.DAL;
using SkillsLabProject.App_Start;

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
                .UseSqlServerStorage(hangfireConnectionString)
                .UseActivator(new ContainerJobActivator(UnityConfig.Container));

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
            //BackgroundJob.Schedule<ITrainingBL>(t => t.AutoCloseTrainingAsync() , TimeSpan.FromSeconds(30));
            RecurringJob.AddOrUpdate<ITrainingBL>("EnrollmentProcessingJob", t =>  t.AutoCloseTrainingAsync(), Cron.Daily);
        }
    }
}
