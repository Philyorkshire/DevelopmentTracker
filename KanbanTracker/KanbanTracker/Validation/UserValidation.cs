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

using System;
using KanbanTracker.Account;
using KanbanTracker.Models;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace KanbanTracker.Validation
{
    public class UserValidation
    {
        public static bool UserExists(string email)
        {
            var users = UserDb.Open();
            var count = users.FindAs<User>(Query.EQ("UserName", email)).Count();
            return count > 0;
        }

        public static bool Login(LoginViewModel user)
        {
            if (!UserExists(user.Email)) return false;

            var users = UserDb.Open();
            var userFind = users.FindOneAs<User>(Query.EQ("UserName", user.Email));
            var passwordVerify = PasswordHash.ValidatePassword(user.Password, userFind.PasswordHash);

            if (passwordVerify)
            {
                userFind.SessionId = ObjectId.GenerateNewId().ToString();
                users.Save(userFind);
            }

            return passwordVerify;
        }

        public static bool CheckSession(ObjectId sessionId)
        {
            var users = UserDb.Open();
            var userFind = users.FindOneAs<User>(Query.EQ("SessionId", sessionId));

            if (userFind == null) return false;
            {
                return true;
            }
        }

        public static string GetSession(LoginViewModel user)
        {
            var users = UserDb.Open();
            var userFind = users.FindOneAs<User>(Query.EQ("UserName", user.Email));

            return userFind.SessionId;
        }

        public static void DestroySession(string sessionId)
        {
            var users = UserDb.Open();
            var userFind = users.FindOneAs<User>(Query.EQ("SessionId", ObjectId.Parse(sessionId)));
            userFind.SessionId = null;
            users.Save(userFind);
        }
    }
}