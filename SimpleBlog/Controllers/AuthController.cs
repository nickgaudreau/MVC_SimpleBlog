using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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
                Valid = ""
            });
        }

        // Param -> string returnUrl get automatically what the RequestReturnUrl is from the Model Binder
        [HttpPost] //to take post submit form
        public ActionResult Login(AuthLogin form, string returnUrl)
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

            // Authentication for RoleProvider.cs - GetRolesForUser(username)
            FormsAuthentication.SetAuthCookie(form.Username, true);
            // THEN AUTHORIZATION OCCURS -> RoleProvider.cs - GetRolesForUser(username)

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                // might want to check that we don't redirect to an external domain for hacking safe
                return Redirect(returnUrl);
            }

            return RedirectToRoute("Home");
        }

        // this not need to be post
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToRoute("Home");
        }
    }

    
}