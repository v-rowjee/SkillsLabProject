using System.Web;
using System.Web.Optimization;

namespace SkillsLabProject
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jqueryval/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));


            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap/bootstrap.bundle.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap/bootstrap.min.css",
                "~/Content/icons/font-awesome.min.css",
                "~/Content/snackbar/snackbar.min.css",
                "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/Scripts/head").Include(
                "~/Scripts/jquery/jquery-{version}.js",
                "~/Scripts/modernizr/modernizr-*",
                "~/Scripts/snackbar/snackbar.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/body").Include(
                "~/Scripts/custom/site.js"));
        }
    }
}
