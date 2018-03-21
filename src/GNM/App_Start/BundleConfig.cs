using System.Web;
using System.Web.Optimization;

namespace GNM
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                         "~/Content/admin/js/jquery.min.js",
                         "~/Content/admin/js/intlTelInput.js",
                         "~/Scripts/yalgaar.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/ValidationJS").Include(
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/AdminJS").Include(
                "~/Content/admin/js/jquery.min.js",
                 "~/Content/admin/js/bootstrap.min.js",
                 "~/Content/admin/js/nicescroll/jquery.nicescroll.min.js",
                 "~/Content/admin/js/moment/moment.min.js",
                 "~/Content/admin/js/datepicker/daterangepicker.js",
                 "~/Content/admin/js/custom.js",
                 "~/Content/admin/js/datatables/js/jquery.dataTables.js",
                 "~/Content/admin/js/intlTelInput.js",
                 "~/Scripts/yalgaar.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/admin/bootstrap.min.css",
                "~/Content/admin/fonts/css/font-awesome.min.css",
                "~/Content/admin/custom.css"));
        }
    }
}
