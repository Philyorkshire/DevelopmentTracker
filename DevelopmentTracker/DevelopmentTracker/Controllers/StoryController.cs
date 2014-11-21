using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using DevelopmentTracker.Classes;
using DevelopmentTracker.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DevelopmentTracker.Controllers
{
 
    public class StoryController : ApiController
    {
        private readonly MongoCollection<Story> _stories; 

        public StoryController()
        {
            _stories = StoryDb.Open();
        }

        public IEnumerable<Story> Get()
        {
            return _stories.FindAll();
        }

        public HttpResponseMessage GetStoryById(string id)
        {
            var story = _stories.FindOneById(ObjectId.Parse(id));
            return story != null ? Request.CreateResponse(HttpStatusCode.OK, story)
                : Request.CreateErrorResponse(HttpStatusCode.NotFound, "Story could not be found");
        }
    }
}
