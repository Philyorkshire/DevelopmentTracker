using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using DevelopmentTracker.Classes;
using DevelopmentTracker.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace DevelopmentTracker.Controllers
{
 
    public class StoryController : ApiController
    {
        private readonly MongoCollection<Story> _open; 

        public StoryController()
        {
            _open = StoryDb.Open();
        }

        public IEnumerable<Story> Get()
        {
            return _open.FindAll();
        }

        public HttpResponseMessage GetStoryById(string id)
        {
            var story = _open.FindOneById(ObjectId.Parse(id));
            return story != null ? Request.CreateResponse(HttpStatusCode.OK, story)
                : Request.CreateErrorResponse(HttpStatusCode.NotFound, "Story could not be found");
        }

        public HttpResponseMessage PostStory([FromBody]Story story)
        {
            try
            {
                var entry = new Story
                {
                    Title = story.Title,
                    Description = story.Description,
                    Status = story.Status,
                    Created = DateTime.Now
                };

                _open.Insert(entry);

                return Request.CreateResponse(HttpStatusCode.Accepted, entry);
            }

            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    "Request not accepted, please check documentation");
            }
        }

        public HttpResponseMessage PostStoryUpdate(string id, [FromBody]Story story)
        {
            try
            {
                _open.Update(
                    Query.EQ(name: "_id", value: id),
                    Update.Set("title", story.Title)
                    );

                return Request.CreateResponse(HttpStatusCode.Accepted, "Request Accepted");
            }

            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    "Request not accepted, please check documentation");
            }
        }

        public HttpResponseMessage DeleteStory(string id)
        {
            try
            {
                _open.Remove(new QueryDocument("_id", new BsonObjectId(id)));
                return Request.CreateResponse(HttpStatusCode.Accepted, "Story deleted: " + id);
            }

            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Story could not be deleted");
            }
        }
    }
}
