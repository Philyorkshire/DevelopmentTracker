using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KanbanTracker.Classes;
using KanbanTracker.Controllers;
using KanbanTracker.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;

namespace KanbanTracker.Test.ProjectControllerTest
{
    [TestClass]
    public class ProjectBugsTest
    {
        private readonly MongoCollection<Project> _open;
        private readonly ProjectController _controller;

        public ProjectBugsTest()
        {
            _open = ProjectDb.Open();
            _controller = new ProjectController { Request = new HttpRequestMessage() };
            _controller.Request.SetConfiguration(new HttpConfiguration());
        }

        private string GetAProjectId()
        {
            var projectId = _open.FindOne().Id;
            return projectId;
        }

        [TestMethod]
        public void GetAllProjectBugs()
        {
            var projectId = GetAProjectId();
            var projects = _open.FindOneById(ObjectId.Parse(projectId));
            var projectDbCount = projects.Bugs.Count;

            var request = _controller.GetAllProjectBugs(projectId);
            IEnumerable<Bug> responseProject;
            request.TryGetContentValue(out responseProject);

            var response = responseProject.Count();

            Assert.AreEqual(projectDbCount, response);
        }

        [TestMethod]
        public void GetAProjectBug()
        {
            var project = _open.FindOne();
            var bug = project.Bugs.FirstOrDefault();

            if (bug == null || bug.Id == null) return;

            var request = _controller.GetAProjectBug(project.Id, bug.Id);

            Bug responseBug;
            request.TryGetContentValue(out responseBug);

            Assert.IsNotNull(responseBug);
            Assert.AreEqual(bug.Title, responseBug.Title);
        }

        [TestMethod]
        public void CreateANewBug()
        {
            var newBug = new Bug
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Title = "Unit Test - CreateANewBug()",
                Description = "Created at ProjectBugsTest.cs",
                Status = "In Progress",
                Created = DateTime.Now
            };

            var projectId = GetAProjectId();
            var request = _controller.PostANewProjectBug(projectId, newBug);

            Assert.AreEqual(HttpStatusCode.Accepted, request.StatusCode);
        }

        [TestMethod]
        public void DeleteProjectBug()
        {
            var project = _open.FindOne();
            var bug = project.Bugs.FirstOrDefault();

            if (bug == null || bug.Id == null) return;

            var beforeRequest = _controller.GetAProjectBug(project.Id, bug.Id);
            var request = _controller.DeleteAProjectBug(project.Id, bug.Id);
            var afterRequest = _controller.GetAProjectStory(project.Id, bug.Id);

            Assert.AreEqual(HttpStatusCode.OK, beforeRequest.StatusCode);
            Assert.AreEqual(HttpStatusCode.Accepted, request.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, afterRequest.StatusCode);
        }
    }
}
