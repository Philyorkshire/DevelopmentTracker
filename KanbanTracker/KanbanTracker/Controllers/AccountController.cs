/****************************** Development Tracker 2014 ******************************\
Project:      Development Tracker
Github: https://github.com/Philyorkshire/DevelopmentTracker
Author: Phillip Marsden - C3348183
Assignment: Software Engineering, Task B

The overall purpose of the application is to provide a tool that can be used to aid the software development process within an organization.
 * Essentially the product should be a “Software Development  Accounting Framework (A tool support for Software Engineering)”.

All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using KanbanTracker.Classes;
using KanbanTracker.Models;
using MongoDB.Driver;

namespace KanbanTracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly MongoCollection<User> _open;

        public AccountController()
        {
            _open = UserDb.Open();
        }

        [Auth]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            var httpCookie = HttpContext.Request.Cookies["sid"];
            if (httpCookie != null && httpCookie.Value != string.Empty)
            {
                Validation.UserValidation.DestroySession(httpCookie.Value);
                httpCookie.Expires = DateTime.Now.AddDays(-1d);
                Auth.Authenticated = false;
            }

            @ViewBag.Info = "Logged out successfully";
            return View("Login");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public RedirectToRouteResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid && Validation.UserValidation.Login(model))
            {
                Response.SetCookie(new HttpCookie("sid", Validation.UserValidation.GetSession(model)));
                Response.Cookies["sid"].Expires = DateTime.Now.AddMinutes(30);

                @ViewBag.Info = (string.Format("Welcome, {0}", model.Email));
                return RedirectToAction("dashboard", "projects");
            }

            // If we got this far, something failed, redisplay form
            @ViewBag.Info = "Login Failed";
            return RedirectToAction("login");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid && !Validation.UserValidation.UserExists(model.Email))
            {
                var user = new User
                {
                    UserName = model.Email,
                    PasswordHash = PasswordHash.CreateHash(model.ConfirmPassword)
                };

                _open.Save(user);
                return View("Index");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}
