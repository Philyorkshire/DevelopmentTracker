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

using System.Threading.Tasks;
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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
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
            if (ModelState.IsValid && !KanbanTracker.Validation.UserValidation.UserExists(model.Email))
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
