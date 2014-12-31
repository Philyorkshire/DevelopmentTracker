/****************************** Development Tracker 2014 ******************************\
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
