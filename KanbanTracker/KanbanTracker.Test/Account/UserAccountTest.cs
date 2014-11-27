using System.Net;
using System.Net.Http;
using System.Web.Http;
using KanbanTracker.Controllers;
using KanbanTracker.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KanbanTracker.Test.Account
{
    [TestClass]
    public class UserAccountTest
    {

        public UserController Controller;

        public UserAccountTest()
        {
            var controller = new UserController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            Controller = controller;
        }

        [TestMethod]
        public void GetUserById()
        {
            var user = UserDb.Open();
            var firstUser = user.FindOne().Id;
            var request = Controller.GetUser(firstUser);

            Assert.AreEqual(request.StatusCode, HttpStatusCode.OK);
        }
    }
}
