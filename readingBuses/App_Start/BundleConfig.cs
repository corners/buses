using System.Web;
using System.Web.Optimization;

namespace readingBuses
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/reset.css",
                        "~/Content/departures.css"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryvar").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.validate.js"));

            var jqueryCdn = @"http://ajax.aspnetcdn.com/ajax/jQuery/jquery-2.1.1.min.js";
            bundles.Add(new ScriptBundle("~/bundles/jquery", jqueryCdn).Include(
                        "~/Scripts/jquery-{version}.js"));

            var knockoutCdn = @"http://ajax.aspnetcdn.com/ajax/knockout/knockout-3.1.0.js";
            bundles.Add(new ScriptBundle("~/bundles/knockout", knockoutCdn).Include(
                        "~/Scripts/knockout-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/departures").Include(
                        "~/Scripts/departures-{version}.js"));

            // todo phase out
            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/knockout-{version}.js",
                        "~/Scripts/departures-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/routes").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/knockout-{version}.js",
                        "~/Scripts/underscore.js"));

            //Scripts.DefaultTagFormat = @"<script src=""{0}"" async></script>";

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
