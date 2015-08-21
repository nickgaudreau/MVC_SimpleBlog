using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using NHibernate.Linq;
using SimpleBlog.Models;
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
            

            var user = Database.Session.Query<User>().FirstOrDefault(u => u.Username == form.Username);
            // prevent timing attack -> ig input good username = 900ms, if bad = 500ms
            //(looking to see if hit correct username by F12 browser/network loading speed..now good or not we get ~900ms)
            if(user == null)
                SimpleBlog.Models.User.FakeHash();

            if (user == null || !user.CheckPassword(form.Password))
            {
                ModelState.AddModelError("Username", "Username / Password is incorrect!");
            }

            // check for invalid input
            if (!ModelState.IsValid)
            {
                form.Valid = "! - The form is INVALID";
                return View(form);
            }

            // Authentication for RoleProvider.cs - GetRolesForUser(username)
            FormsAuthentication.SetAuthCookie(user.Username, true);
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