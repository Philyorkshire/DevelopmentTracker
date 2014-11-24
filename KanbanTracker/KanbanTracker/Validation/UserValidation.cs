using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using KanbanTracker.Models;
using Microsoft.AspNet.Identity;
using MongoDB.Driver.Builders;


namespace KanbanTracker.Validation
{
    public class UserValidation
    {
        

        public bool UserExists(RegisterViewModel model)
        {
            var users = UserDb.Open();

            var count = users.FindAs<IUser>(Query.EQ("UserName", model.Email)).Count();

            if (count > 1)
            {
                return true;
            }
            return false;
        }
    }
}