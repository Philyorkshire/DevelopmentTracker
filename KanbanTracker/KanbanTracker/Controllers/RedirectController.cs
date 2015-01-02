using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KanbanTracker.Controllers
{
    public class RedirectController : Controller
    {
        public ActionResult Login()
        {
            var view = new RedirectToRouteResult(new RouteValueDictionary
                        {
                            {"controller", "account"},
                            {"action", "login"}
                        });
            return view;
        }
    }
}
