using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using KanbanTracker.Controllers;
using KanbanTracker.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;

namespace KanbanTracker.Test.User
{
    [TestClass]
    public class UserControllerTest
    {
        private MongoCollection<Classes.User> _open;
        private readonly UserController _controller;

        public UserControllerTest()
        {
            _open = UserDb.Open();
            _controller = new UserController { Request = new HttpRequestMessage() };
            _controller.Request.SetConfiguration(new HttpConfiguration());
        }

        public IEnumerable<Classes.User> GetAllUsers()
        {
            return _open.FindAll();
        }

        [TestMethod]
        public void CreateNewUser()
        {
            var user = new Classes.User
            {
                UserName = "UnitTest",
            };
            var action = _controller.PostNewUser(user);

            //Assert.AreEqual(action.StatusCode, HttpStatusCode.Accepted);
        }
    }
}
