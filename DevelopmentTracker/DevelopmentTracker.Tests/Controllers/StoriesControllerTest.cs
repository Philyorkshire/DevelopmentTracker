using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Net.Http;
using DevelopmentTracker.Classes;
using DevelopmentTracker.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevelopmentTracker.Tests.Controllers
{
    [TestClass]
    public class StoriesControllerTest
    {
        [TestMethod]
        public void GetAllStories()
        {
            // Arrange
            var controller = new StoryController();

            // Act
            IEnumerable<Story> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
        }

        //TODO: Implement test.
        [TestMethod]
        public void GetAllStoriesById()
        {
            // Arrange
            var controller = new StoryController();

        }
    }
}
