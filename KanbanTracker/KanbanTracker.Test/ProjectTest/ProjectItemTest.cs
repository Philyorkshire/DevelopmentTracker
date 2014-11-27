using System.Collections.Generic;
using System.Net;
using KanbanTracker.Classes;
using KanbanTracker.Controllers;
using KanbanTracker.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;

namespace KanbanTracker.Test.ProjectTest
{
    [TestClass]
    public class ProjectItemTest
    {
        private MongoCollection<Project> _open;

        public ProjectItemTest()
        {
            _open = ProjectDb.Open();
        }

        [TestMethod]
        public void GetAllProjects()
        {
            var projects = _open.FindAll();
            Assert.IsInstanceOfType(projects, typeof (IEnumerable<Project>));
        }

        [TestMethod]
        public void GetProjectById()
        {
            var project = _open.FindOne();
            var controller = new ProjectController();
        }
    }
}
