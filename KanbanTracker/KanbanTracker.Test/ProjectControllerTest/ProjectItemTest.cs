﻿using System;
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
    public class ProjectItemTest
    {
        private MongoCollection<Project> _open;
        private ProjectController controller;

        public ProjectItemTest()
        {
            _open = ProjectDb.Open();
            controller = new ProjectController { Request = new HttpRequestMessage() };
            controller.Request.SetConfiguration(new HttpConfiguration());
        }

        [TestMethod]
        public void GetAllProjects()
        {
            var projects = _open.FindAll();

            var result = controller.GetAllProjects();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(projects, typeof (IEnumerable<Project>));
            Assert.AreEqual(result.Count(), projects.Count());
        }

        [TestMethod]
        public void GetProjectById()
        {
            var getProject = _open.FindOne();

            var request = controller.GetProjectById(getProject.Id);
            Project value;
            request.TryGetContentValue(out value);

            Assert.IsNotNull(request);
            Assert.AreEqual(getProject.Id, value.Id);
        }

        [TestMethod]
        public void CreateNewProject()
        {
            var newProject = new Project
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Title = "Unit Testing",
                Description = "",
                Created = DateTime.Now,
                DueDate = DateTime.Now,
                Stories = new List<Story>()
            };

            var projectId = newProject.Id;
            var insertResult = _open.Insert(newProject);
            var request = controller.GetProjectById(projectId);

            Project value;
            request.TryGetContentValue(out value);

            Assert.IsTrue(insertResult.Ok);
            Assert.AreEqual(projectId, value.Id);
        }

        [TestMethod]
        public void DeleteAProject()
        {
            var projectId = _open.FindOne().Id;
            var beforeRequest = controller.GetProjectById(projectId);

            _open.Remove(new QueryDocument("_id", new BsonObjectId(new ObjectId(projectId))));

            var afterRequest = controller.GetProjectById(projectId);

            Assert.AreEqual(HttpStatusCode.OK, beforeRequest.StatusCode, "Status should be OK");
            Assert.AreEqual(HttpStatusCode.NotFound, afterRequest.StatusCode, "Status should be not found");
        }
    }
}