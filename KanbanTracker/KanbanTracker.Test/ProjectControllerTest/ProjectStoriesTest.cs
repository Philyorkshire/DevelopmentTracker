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
    public class ProjectStoriesTest
    {
        private readonly MongoCollection<Project> _open;
        private readonly ProjectController _controller;

        public ProjectStoriesTest()
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
        public void GetAllProjectStories()
        {
            var projectId = GetAProjectId();
            var projects = _open.FindOneById(ObjectId.Parse(projectId));
            var projectDbCount = projects.Stories.Count;

            var request = _controller.GetAllProjectStories(projectId);
            IEnumerable<Story> responseProject;
            request.TryGetContentValue(out responseProject);

            var response = responseProject.Count();

            Assert.AreEqual(projectDbCount, response);
        }

        [TestMethod]
        public void GetAProjectStory()
        {
            var project =  _open.FindOne();
            var story = project.Stories.FirstOrDefault();

            if(story == null || story.Id == null) return;

            var request = _controller.GetAProjectStory(project.Id, story.Id);

            Story responseStory;
            request.TryGetContentValue(out responseStory);

            Assert.IsNotNull(responseStory);
            Assert.AreEqual(story.Title, responseStory.Title);
        }

        [TestMethod]
        public void CreateANewStory()
        {
            var newStory = new Story
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Title = "Unit Test - CreateANewStory()",
                Description = "Created at ProjectStoriesTest.cs",
                Status = "In Progress",
                Created = DateTime.Now
            };

            var projectId = GetAProjectId();
            var request = _controller.PostProjectStory(projectId, newStory);

            Assert.AreEqual(HttpStatusCode.Accepted, request.StatusCode);
        }

        [TestMethod]
        public void DeleteAStory()
        {
            var project = _open.FindOne();
            var story = project.Stories.FirstOrDefault();

            if (story == null || story.Id == null) return;

            var beforeRequest = _controller.GetAProjectStory(project.Id, story.Id);
            var request = _controller.DeleteAProjectStory(project.Id, story.Id);
            var afterRequest = _controller.GetAProjectStory(project.Id, story.Id);

            Assert.AreEqual(HttpStatusCode.OK, beforeRequest.StatusCode);
            Assert.AreEqual(HttpStatusCode.Accepted, request.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, afterRequest.StatusCode);
        }
    }
}