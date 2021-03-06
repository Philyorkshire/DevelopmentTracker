﻿/****************************** Development Tracker 2014 ******************************\
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
    public class ProjectItemCommentsTest
    {
        private readonly MongoCollection<Project> _open;
        private readonly ProjectController _controller;

        public ProjectItemCommentsTest()
        {
            _open = ProjectDb.Open();
            _controller = new ProjectController {Request = new HttpRequestMessage()};
            _controller.Request.SetConfiguration(new HttpConfiguration());
        }

        /// <summary>
        /// Returns project id
        /// </summary>
        /// <returns>id</returns>
        private string GetAProjectId()
        {
            var projectId = _open.FindOne().Id;
            return projectId;
        }

        /// <summary>
        /// Returns project story by id
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>Story</returns>
        private Story GetProjectStory(out string projectId)
        {
            projectId = GetAProjectId();
            var project = _open.FindOneById(ObjectId.Parse(projectId));
            var story = project.Stories.FirstOrDefault();
            return story;
        }

        /// <summary>
        /// Validate API GET request returns all project story comments.
        /// </summary>
        [TestMethod]
        public void GetAllProjectItemComments()
        {
            string projectId;
            var story = GetProjectStory(out projectId);

            if (story == null || story.Id == null) return;
            var commentCount = story.Comments.Count;

            var request = _controller.GetAllProjectElementComments(projectId, "stories", story.Id);
            List<Comment> responseStory;
            request.TryGetContentValue(out responseStory);

            var response = responseStory.Count;

            Assert.IsNotNull(response);
            Assert.AreEqual(commentCount, response);
        }

        /// <summary>
        /// Validate API GET request returns a project story comment by id.
        /// </summary>
        [TestMethod]
        public void GetAProjectItemComment()
        {
            string projectId;
            var story = GetProjectStory(out projectId);

            var comment = story.Comments.FirstOrDefault();
            var request = _controller.GetAProjectElementComment(projectId, "stories", story.Id, comment.Id);

            Comment responseComment;
            request.TryGetContentValue(out responseComment);

            Assert.IsNotNull(request);
            Assert.AreEqual(HttpStatusCode.OK, request.StatusCode);
            Assert.AreEqual(comment.Description, responseComment.Description);
        }

        /// <summary>
        /// Validate API POST request creates new comment.
        /// </summary>
        [TestMethod]
        public void CreateNewCommentToProjectItem()
        {
            string projectId;
            var story = GetProjectStory(out projectId);

            var comment = new Comment
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Created = DateTime.Now,
                Description = "SUCCESSS!",
                OwnerId = "54a4d1d0d7aa0b1ae48da9f4"
            };

            var request = _controller.PostAProjectElementComment(projectId, "stories", story.Id, comment);
            Comment responseComment;
            request.TryGetContentValue(out responseComment);

            Assert.AreEqual(HttpStatusCode.Accepted, request.StatusCode);
        }
    }
}