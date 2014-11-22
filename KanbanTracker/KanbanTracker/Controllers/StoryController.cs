using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KanbanTracker.Classes;
using KanbanTracker.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace KanbanTracker.Controllers
{

    public class StoryController : ApiController
    {
        private MongoCollection<Story> _open;

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
            var entry = new Story
            {
                Title = story.Title,
                Description = story.Description,
                Status = story.Status,
                Created = DateTime.Now
            };

            try
            {
                _open.Insert(entry);
                return Request.CreateResponse(HttpStatusCode.Accepted, entry);
            }

            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    "Request not accepted, please check documentation");
            }
        }

        [Route("api/story/{id}")]
        public HttpResponseMessage PostStoryUpdate(string id, [FromBody]Story story)
        {

            try
            {
                var query = Query<Story>.EQ(s => s.Id, (id));
                var update = Update<Story>
                    .Set(s => s.Title, story.Title)
                    .Set(s => s.Description, story.Description)
                    .Set(s => s.Status, story.Status);

                _open.Update(query, update);

                return Request.CreateResponse(HttpStatusCode.Accepted, story);
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
                _open.Remove(new QueryDocument("_id", new BsonObjectId(new ObjectId(id))));
                return Request.CreateResponse(HttpStatusCode.NoContent, "Story deleted: " + id);
            }

            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Story could not be deleted");
            }
        }
    }
}