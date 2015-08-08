using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleBlog.ViewModels;

namespace SimpleBlog.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult Login()
        {
            return View(new AuthLogin()
            {
                Test = "Test value obj instanciate with default GET",
                Valid = "",
                Username = "Default"
            });
        }

        // Post here
        [HttpPost]
        public ActionResult Login(AuthLogin form)
        {
            // we use the form to change data and pass it to next view
            form.Test = "Value of Test changes after POST ";
            form.Valid = "The form info is valid";

            // check for invalid input
            if (!ModelState.IsValid)
            {
                form.Valid = "! - The form is INVALID";
                return View(form);
            }

            // check is user/email valid loop back from db per say
            if (form.Username != "admin")
            {
                ModelState.AddModelError("Username", "USernamer or password was not found!");
                form.Valid = "! - The form is INVALID";
                return View(form);
            }

            return View(form);
        }
    }

    
}