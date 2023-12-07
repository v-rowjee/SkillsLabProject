using System.Web;
using System.Web.Optimization;

namespace SkillsLabProject
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jQuery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jQueryVal/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                        "~/Scripts/Moment/moment.min.js",
                        "~/Scripts/Moment/moment-with-locales.min.js"
                ));


            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                "~/Scripts/Bootstrap/bootstrap.bundle.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/Bootstrap/bootstrap.min.css",
                "~/Content/font-awesome.min.css",
                "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/Scripts/head").Include(
                "~/Scripts/jQuery/jquery-{version}.js",
                "~/Scripts/modernizr-*",
                "~/Scripts/Moment/moment.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/body").Include(
                "~/Scripts/site.js"));
        }
    }
}
