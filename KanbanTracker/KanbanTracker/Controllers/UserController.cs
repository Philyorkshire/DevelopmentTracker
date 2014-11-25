using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KanbanTracker.Account;
using KanbanTracker.Classes;
using KanbanTracker.Models;
using KanbanTracker.Validation;
using MongoDB.AspNet.Identity;
using MongoDB.Bson;
using MongoDB.Driver;

namespace KanbanTracker.Controllers
{
    public class UserController : ApiController
    {
        private MongoCollection<User> _open;

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
            var user = _open.FindOneById(ObjectId.Parse(id));
            return user != null ? Request.CreateResponse(HttpStatusCode.OK, user)
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
