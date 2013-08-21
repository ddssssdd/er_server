using ExpenseReportServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ExpenseReportServer.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginUsers user,string returnUrl)
        {
            //FormsAuthentication.Authenticate(user.Username, user.Password);
            bool result = Membership.ValidateUser(user.Username, user.Password);
            if (result)
            {
                FormsAuthentication.SetAuthCookie(user.Username, false);
                return this.Redirect(returnUrl ?? Url.Action("Index", "Home"));
            }
            else
            {

                return View();
            }

        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            IIdentity user = System.Threading.Thread.CurrentPrincipal.Identity;
            return View("Login");
        }

    }
}
