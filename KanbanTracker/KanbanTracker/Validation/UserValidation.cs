using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using KanbanTracker.Account;
using KanbanTracker.Models;
using Microsoft.AspNet.Identity;
using MongoDB.AspNet.Identity;
using MongoDB.Driver.Builders;


namespace KanbanTracker.Validation
{
    public static class UserValidation
    {
        public static bool UserExists(string email)
        {
            var users = UserDb.Open();

            var count = users.FindAs<User>(Query.EQ("UserName", email)).Count();

            if (count > 1)
            {
                return true;
            }
            return false;
        }
    }
}