using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KanbanTracker.Classes;
using KanbanTracker.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;

namespace KanbanTracker.Test.StoryControllerTest
{
    [TestClass]
    public class StoriesControllerTest
    {
        public StoryController Controller;

        public StoriesControllerTest()
        {
            var controller = new StoryController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            Controller = controller;
        }

        public Story story = new Story()
        {
            Title = "Unit Test Story",
            Description = "This is a unit test story description",
            Status = "TEST-STORY",
            Tags = new List<string> {"UnitTest"}
        };

        public Story GetStory()
        {
            var enumerable = Controller.Get();
            var firstStory = enumerable.First();
            return firstStory;
        }

        [TestMethod]
        public void GetAllStories()
        {
            var result = Controller.Get();

            var enumerable = result as Story[] ?? result.ToArray();
            var count = enumerable.Count();

            Assert.IsNotNull(result);
            Assert.IsTrue(count > 1);
        }

        [TestMethod]
        public void GetStoryById()
        {
            var enumerable = Controller.Get();
            var firstStory = enumerable.First();
            var storyId = firstStory.Id;

            var result = Controller.GetStoryById(storyId);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public void PostNewStory()
        {
            var result = Controller.PostStory(story);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.Accepted);
        }

        [TestMethod]
        public void PostUpdateStory()
        {
            var storyTitle = GetStory().Title;

            var newStory = new Story
            {
                Title = "PostUpdateStory " + new Random().Next(0,1000),
                Description = "This is a unit test story description",
                Status = "TEST-STORY",
                Tags = new List<string> { "UnitTest" }
            };

            var postUpdateResult = Controller.PostStoryUpdate(GetStory().Id, newStory);
            var result = Controller.GetStoryById(GetStory().Id);

            Assert.AreNotEqual(GetStory().Title, storyTitle);
            Assert.AreEqual(HttpStatusCode.Accepted, postUpdateResult.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public void DeleteStory()
        {
            var enumerable = Controller.Get();
            var firstStory = enumerable.First();
            var storyId = firstStory.Id;

            var result = Controller.DeleteStory(storyId);
            var requestDeletedStory = Controller.GetStoryById(storyId);

            Assert.AreEqual(requestDeletedStory.StatusCode, HttpStatusCode.NotFound);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.NoContent);
        }
    }
}