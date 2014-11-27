using System.Threading.Tasks;
using System.Web.Mvc;
using KanbanTracker.Classes;
using KanbanTracker.Classes;
using KanbanTracker.Models;
using KanbanTracker.Validation;
using MongoDB.Driver;

namespace KanbanTracker.Controllers
{
    public class AccountController : Controller
    {
        private MongoCollection<User> _open;

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
            if (ModelState.IsValid && !UserValidation.UserExists(model.Email))
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
