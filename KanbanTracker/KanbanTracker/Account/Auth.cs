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
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using KanbanTracker.Validation;
using MongoDB.Bson;

namespace KanbanTracker.Account
{
    public class Auth : ActionFilterAttribute
    {
        public static string SessionId = null;
        public static bool Authenticated = false;
        public static string Message = null;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpCookie httpCookie = filterContext.HttpContext.Request.Cookies.Get("sid");

            if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
            {
                DateTime datetimeSid = (ObjectId.Parse(httpCookie.Value).CreationTime);
                TimeSpan duration = DateTime.Now - datetimeSid;

                if (duration.Minutes > 10 || !UserValidation.CheckSession(ObjectId.Parse(httpCookie.Value)))
                {
                    filterContext.Result =
                        new RedirectToRouteResult(new RouteValueDictionary
                        {
                            {"controller", "account"},
                            {"action", "login"}
                        });

                    SessionId = null;
                    Message = "Session expired, please login";
                    Authenticated = false;
                }

                else
                {
                    SessionId = httpCookie.Value;
                    Message = null;
                    Authenticated = true;
                }
            }

            else
            {
                base.OnActionExecuting(filterContext);
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
    }
}