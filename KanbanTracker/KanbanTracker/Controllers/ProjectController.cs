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

        public IEnumerable<Project> GetAllProjects()
        {
            return _open.FindAll();
        }

        public HttpResponseMessage GetProjectById(string id)
        {
            var story = _open.FindOneById(ObjectId.Parse(id));
            return story != null ? Request.CreateResponse(HttpStatusCode.OK, story)
                : Request.CreateErrorResponse(HttpStatusCode.NotFound, "Story could not be found");
        }

        public HttpResponseMessage PostNewProject([FromBody]Project project)
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

        public HttpResponseMessage DeleteProject(string id)
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

        // Project stories

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
                    Update.PushAllWrapped("Stories", newStory));

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
                return Request.CreateResponse(HttpStatusCode.Accepted, "Story deleted: " + storyId);
            }

            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Story could not be deleted");
            }
        }

        // Project bugs

        [Route("project/{projectId}/bugs")]
        public HttpResponseMessage GetAllProjectBugs(string projectId)
        {
            var project = _open.FindOneById(ObjectId.Parse(projectId));
            return project != null ? Request.CreateResponse(HttpStatusCode.OK, project.Bugs)
                : Request.CreateErrorResponse(HttpStatusCode.NotFound, "Bugs could not be found");
        }

        [Route("project/{projectId}/bugs/{bugId}")]
        public HttpResponseMessage GetAProjectBug(string projectId, string bugId)
        {
            var project = _open.FindOneById(ObjectId.Parse(projectId));
            var bugs = project.Bugs.Find(b => b.Id == bugId);

            return bugs != null ? Request.CreateResponse(HttpStatusCode.OK, bugs)
                : Request.CreateErrorResponse(HttpStatusCode.NotFound, "Bug could not be found");
        }

        [Route("project/{projectId}/bugs/{bugId}")]
        public HttpResponseMessage DeleteAProjectBug(string projectId, string bugId)
        {
            var query = Query.And(Query.EQ("_id", ObjectId.Parse(projectId)));

            var update = Update.Pull("Bugs", new BsonDocument{
                             { "_id", ObjectId.Parse(bugId) }
                });

            try
            {
                _open.Update(query, update);
                return Request.CreateResponse(HttpStatusCode.Accepted, "Story deleted: " + bugId);
            }

            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Story could not be deleted");
            }
        }

        [Route("project/{projectId}/bugs")]
        public HttpResponseMessage PostANewProjectBug(string projectId, [FromBody]Bug bug)
        {
            var newBug = new Bug
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Title = bug.Title,
                Description = bug.Description,
                Status = "In Progress",
                Created = DateTime.Now
            };

            try
            {
                _open.Update(Query.EQ("_id", (ObjectId.Parse(projectId))),
                    Update.PushAllWrapped("Bugs", newBug));

                return Request.CreateResponse(HttpStatusCode.Accepted, newBug);
            }

            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    "Request not accepted, please check documentation");
            }
        }
    }
}