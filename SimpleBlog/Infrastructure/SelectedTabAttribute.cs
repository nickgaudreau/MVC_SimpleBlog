using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SimpleBlog.Infrastructure
{
    // Accept only class or methods
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SelectedTabAttribute : ActionFilterAttribute
    {
        private readonly string _selectedTab;

        public SelectedTabAttribute(string selectedTab)
        {
            _selectedTab = selectedTab;
        }

        // Call before the Action Get exec... the [SelectedTab("Users")]
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            // comm with our view viewbag
            filterContext.Controller.ViewBag.SelectedTab = _selectedTab;
        }



    }

}