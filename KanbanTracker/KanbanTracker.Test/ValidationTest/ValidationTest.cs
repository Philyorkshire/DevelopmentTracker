using System;
using KanbanTracker.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KanbanTracker.Validation;

namespace KanbanTracker.Test.ValidationTest
{
    [TestClass]
    public class ValidationTest
    {
        private const string TestPassword = "testHashing";

        public StoryValidation Validation;

        [TestMethod]
        public void CheckPasswordHash()
        {
            var hashPassword = PasswordHash.CreateHash(TestPassword);
            Assert.AreNotEqual(hashPassword, TestPassword);
        }

        [TestMethod]
        public void ValidateHashedPassword()
        {
            var hashPassword = PasswordHash.CreateHash(TestPassword);
            var validatePasswordHash = PasswordHash.ValidatePassword(TestPassword, hashPassword);
            Assert.IsTrue(validatePasswordHash);
        }
    }
}
