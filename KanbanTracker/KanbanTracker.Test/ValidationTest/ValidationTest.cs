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

using KanbanTracker.Account;
using KanbanTracker.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KanbanTracker.Test.ValidationTest
{
    [TestClass]
    public class ValidationTest
    {
        private const string TestPassword = "testHashing";

        /// <summary>
        /// Validates a password hash can be created.
        /// </summary>
        [TestMethod]
        public void CheckPasswordHash()
        {
            var hashPassword = PasswordHash.CreateHash(TestPassword);
            Assert.AreNotEqual(hashPassword, TestPassword);
        }

        /// <summary>
        /// Validates that a hash password can be validated with correct ASCII password and hash.
        /// </summary>
        [TestMethod]
        public void ValidateHashedPassword()
        {
            var hashPassword = PasswordHash.CreateHash(TestPassword);
            var validatePasswordHash = PasswordHash.ValidatePassword(TestPassword, hashPassword);
            Assert.IsTrue(validatePasswordHash);
        }
    }
}