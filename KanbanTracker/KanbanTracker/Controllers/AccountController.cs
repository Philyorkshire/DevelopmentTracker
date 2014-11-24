using System.Threading.Tasks;
using System.Web.Mvc;
using KanbanTracker.Classes;
using KanbanTracker.Models;
using KanbanTracker.Validation;
using Microsoft.AspNet.Identity;
using MongoDB.AspNet.Identity;
using MongoDB.Driver;

namespace KanbanTracker.Controllers
{
    public class AccountController : Controller
    {
        private MongoCollection<IUser> _open;

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
            var validation = new UserValidation();

            if (ModelState.IsValid && !validation.UserExists(model))
            {
                var user = new IdentityUser(model.Email)
                {
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
