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

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KanbanTracker.Account;
using KanbanTracker.Models;
using KanbanTracker.Validation;
using MongoDB.Bson;
using MongoDB.Driver;

namespace KanbanTracker.Controllers
{
    public class UserController : ApiController
    {
        private readonly MongoCollection<User> _open;

        public UserController()
        {
            _open = UserDb.Open();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _open.FindAll();
        }

        public HttpResponseMessage GetUser(string id)
        {
            User user = _open.FindOneById(ObjectId.Parse(id));
            return user != null
                ? Request.CreateResponse(HttpStatusCode.OK, user)
                : Request.CreateErrorResponse(HttpStatusCode.NotFound, "User could not be found");
        }

        public HttpResponseMessage PostNewUser([FromBody] User user)
        {
            if (!UserValidation.UserExists(user.UserName))
            {
                var newUser = new User
                {
                    UserName = user.UserName,
                    PasswordHash = PasswordHash.CreateHash(user.PasswordHash)
                };

                _open.Save(newUser);
                return Request.CreateResponse(HttpStatusCode.Accepted, "User created: " + user.UserName);
            }

            return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "This user already exists");
        }

        public HttpResponseMessage DeleteUser(string id)
        {
            try
            {
                _open.Remove(new QueryDocument("_id", new BsonObjectId(new ObjectId(id))));
                return Request.CreateResponse(HttpStatusCode.NoContent, "User deleted: " + id);
            }

            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User could not be deleted");
            }
        }
    }
}