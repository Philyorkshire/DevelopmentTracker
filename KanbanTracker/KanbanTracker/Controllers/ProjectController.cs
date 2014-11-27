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
    public class ProjectController : ApiController
    {
        private MongoCollection<Project> _open;

        public ProjectController()
        {
            _open = ProjectDb.Open();
        }

        public IEnumerable<Project> Get()
        {
            return _open.FindAll();
        }

        public HttpResponseMessage GetProjectById(string id)
        {
            var story = _open.FindOneById(ObjectId.Parse(id));
            return story != null ? Request.CreateResponse(HttpStatusCode.OK, story)
                : Request.CreateErrorResponse(HttpStatusCode.NotFound, "Story could not be found");
        }

        public HttpResponseMessage PostStory([FromBody]Project project)
        {
            var newProject = new Project
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Title = project.Title,
                Description = project.Description,
                Created = DateTime.Now,
                DueDate = DateTime.Now,
                Stories = new List<Story>()
            };

            try
            {
                _open.Insert(newProject);
                return Request.CreateResponse(HttpStatusCode.Accepted, newProject);
            }

            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    "Request not accepted, please check documentation - Insert");
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

        // stories

        [Route("project/{id}/stories")]
        public HttpResponseMessage GetAllProjectStories(string id)
        {
            var project = _open.FindOneById(ObjectId.Parse(id));
            return project != null ? Request.CreateResponse(HttpStatusCode.OK, project.Stories)
                : Request.CreateErrorResponse(HttpStatusCode.NotFound, "Story could not be found");
        }

        [Route("project/{projectId}/stories/{storyId}")]
        public HttpResponseMessage GetAProjectStory(string projectId, string storyId)
        {
            var project = _open.FindOneById(ObjectId.Parse(projectId));
            var story = project.Stories.Find(s => s.Id == storyId);

            return story != null ? Request.CreateResponse(HttpStatusCode.OK, story)
                : Request.CreateErrorResponse(HttpStatusCode.NotFound, "Story could not be found");
        }

        [Route("project/{id}/stories")]
        public HttpResponseMessage PostProjectStory(string id, [FromBody]Story story)
        {
            var newStory = new Story
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Title = story.Title,
                Description = story.Description,
                Status = "In Progress",
                Created = DateTime.Now
            };
            try
            {
                _open.Update(Query.EQ("_id", (ObjectId.Parse(id))),
                    Update.PushAllWrapped("stories", newStory));

                return Request.CreateResponse(HttpStatusCode.Accepted, newStory);
            }

            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    "Request not accepted, please check documentation");
            }
        }

        [Route("project/{projectId}/stories/{storyId}")]
        public HttpResponseMessage DeleteAProjectStory(string projectId, string storyId)
        {
            var query = Query.And(Query.EQ("_id", ObjectId.Parse(projectId)));

            var update = Update.Pull("Stories", new BsonDocument{
                             { "_id", ObjectId.Parse(storyId) }
                });

            try
            {
                _open.Update(query, update);
                return Request.CreateResponse(HttpStatusCode.OK, "Story deleted: " + storyId);
            }

            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Story could not be deleted");
            }
        }
    }
}
