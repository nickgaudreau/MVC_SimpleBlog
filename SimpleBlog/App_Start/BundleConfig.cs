using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace SimpleBlog.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Style Admin
            // how we will target them from Views : new StyleBundle("~/admin/styles")
            bundles.Add(new StyleBundle("~/admin/styles")
                .Include("~/Content/Styles/bootstrap.css")
                .Include("~/Content/Styles/admin.css"));

            // Style Main
            bundles.Add(new StyleBundle("~/styles")
                .Include("~/Content/Styles/bootstrap.css")
                .Include("~/Content/Styles/Site.css"));


            // Script -> Order is important!!
            bundles.Add(new ScriptBundle("~/admin/scripts")
                .Include("~/Scripts/jquery-2.1.4.js")
                .Include("~/Scripts/jquery.validate.js")
                .Include("~/Scripts/jquery.validate.unobtrusive.js")
                .Include("~/SimpleBlog/Scripts/bootstrap.js")
                );

            bundles.Add(new ScriptBundle("~/scripts")
                .Include("~/Scripts/jquery-2.1.4.js")
                .Include("~/Scripts/jquery.validate.js")
                .Include("~/Scripts/jquery.validate.unobtrusive.js")
                .Include("~/SimpleBlog/Scripts/bootstrap.js")
                );

        }
    }
}