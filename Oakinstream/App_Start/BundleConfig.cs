using System.Web;
using System.Web.Optimization;

namespace Oakinstream
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/aos.css",
                      "~/Content/Style/About.css",
                      "~/Content/Style/AboutFiles.css",
                      "~/Content/Style/AboutImages.css",
                      "~/Content/Style/Account.css",
                      "~/Content/Style/Admin.css",
                      "~/Content/Style/Blog.css",
                      "~/Content/Style/BlogCategory.css",
                      "~/Content/Style/BlogImages.css",
                      "~/Content/Style/CheckingAccount.css",
                      "~/Content/Style/Contact.css",
                      "~/Content/Style/Home.css",
                      "~/Content/Style/HomeImages.css",
                      "~/Content/Style/ProjectCategory.css",
                      "~/Content/Style/ProjectFiles.css",
                      "~/Content/Style/ProjectImages.css",
                      "~/Content/Style/Projects.css"));
        }
    }
}
