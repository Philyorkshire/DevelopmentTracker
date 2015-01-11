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
        private readonly MongoCollection<Project> _open;

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
            Project story = _open.FindOneById(ObjectId.Parse(id));
            return story != null
                ? Request.CreateResponse(HttpStatusCode.OK, story)
                : Request.CreateErrorResponse(HttpStatusCode.NotFound, "Story could not be found");
        }

        public HttpResponseMessage PostNewProject([FromBody] Project project)
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

        [Route("api/project/{id}/stories")]
        public HttpResponseMessage GetAllProjectStories(string id)
        {
            Project project = _open.FindOneById(ObjectId.Parse(id));
            return project != null
                ? Request.CreateResponse(HttpStatusCode.OK, project.Stories)
                : Request.CreateErrorResponse(HttpStatusCode.NotFound, "Story could not be found");
        }

        [Route("api/project/{projectId}/stories/{storyId}")]
        public HttpResponseMessage GetAProjectStory(string projectId, string storyId)
        {
            Project project = _open.FindOneById(ObjectId.Parse(projectId));
            Story story = project.Stories.Find(s => s.Id == storyId);

            return story != null
                ? Request.CreateResponse(HttpStatusCode.OK, story)
                : Request.CreateErrorResponse(HttpStatusCode.NotFound, "Story could not be found");
        }

        [Route("api/project/{id}/stories")]
        public HttpResponseMessage PostProjectStory(string id, [FromBody] Story story)
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
                _open.Update(Query.EQ("_id", ObjectId.Parse(id)),
                    Update.PushAllWrapped("Stories", newStory));

                return Request.CreateResponse(HttpStatusCode.Accepted, newStory);
            }

            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    "Request not accepted, please check documentation");
            }
        }

        [Route("api/project/{projectId}/stories/{storyId}")]
        public HttpResponseMessage DeleteAProjectStory(string projectId, string storyId)
        {
            IMongoQuery query = Query.And(Query.EQ("_id", ObjectId.Parse(projectId)));
            UpdateBuilder update = Update.Pull("Stories", new BsonDocument
            {
                {"_id", ObjectId.Parse(storyId)}
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

        [Route("api/project/{projectId}/bugs")]
        public HttpResponseMessage GetAllProjectBugs(string projectId)
        {
            Project project = _open.FindOneById(ObjectId.Parse(projectId));
            return project != null
                ? Request.CreateResponse(HttpStatusCode.OK, project.Bugs)
                : Request.CreateErrorResponse(HttpStatusCode.NotFound, "Bugs could not be found");
        }

        [Route("api/project/{projectId}/bugs/{bugId}")]
        public HttpResponseMessage GetAProjectBug(string projectId, string bugId)
        {
            Project project = _open.FindOneById(ObjectId.Parse(projectId));
            Bug bugs = project.Bugs.Find(b => b.Id == bugId);

            return bugs != null
                ? Request.CreateResponse(HttpStatusCode.OK, bugs)
                : Request.CreateErrorResponse(HttpStatusCode.NotFound, "Bug could not be found");
        }

        [Route("api/project/{projectId}/bugs/{bugId}")]
        public HttpResponseMessage DeleteAProjectBug(string projectId, string bugId)
        {
            IMongoQuery query = Query.And(Query.EQ("_id", ObjectId.Parse(projectId)));

            UpdateBuilder update = Update.Pull("Bugs", new BsonDocument
            {
                {"_id", ObjectId.Parse(bugId)}
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

        [Route("api/project/{projectId}/bugs")]
        public HttpResponseMessage PostANewProjectBug(string projectId, [FromBody] Bug bug)
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

        // Project element comments

        [Route("api/project/{projectId}/{task}/{elementId}/comments")]
        public HttpResponseMessage GetAllProjectElementComments(string projectId, string task, string elementId)
        {
            Project project = _open.FindOneById(ObjectId.Parse(projectId));

            if (task == "bugs")
            {
                Bug bug = project.Bugs.Find(b => b.Id == elementId);
                List<Comment> comments = bug.Comments;

                return comments != null
                    ? Request.CreateResponse(HttpStatusCode.OK, comments)
                    : Request.CreateErrorResponse(HttpStatusCode.NotFound, "Comment could not be found");
            }

            if (task == "stories")
            {
                Story story = project.Stories.Find(s => s.Id == elementId);
                List<Comment> comments = story.Comments;

                return comments != null
                    ? Request.CreateResponse(HttpStatusCode.OK, comments)
                    : Request.CreateErrorResponse(HttpStatusCode.NotFound, "Comment could not be found");
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect URI request");
        }

        [Route("api/project/{projectId}/{task}/{elementId}/comments/{commentId}")]
        public HttpResponseMessage GetAProjectElementComment(string projectId, string task, string elementId,
            string commentId)
        {
            Project project = _open.FindOneById(ObjectId.Parse(projectId));

            if (task == "bugs")
            {
                Bug bug = project.Bugs.Find(b => b.Id == elementId);
                Comment comment = bug.Comments.Find(c => c.Id == commentId);

                return comment != null
                    ? Request.CreateResponse(HttpStatusCode.OK, comment)
                    : Request.CreateErrorResponse(HttpStatusCode.NotFound, "Comment could not be found");
            }

            if (task == "stories")
            {
                Story story = project.Stories.Find(s => s.Id == elementId);
                Comment comment = story.Comments.Find(c => c.Id == commentId);

                return comment != null
                    ? Request.CreateResponse(HttpStatusCode.OK, comment)
                    : Request.CreateErrorResponse(HttpStatusCode.NotFound, "Comment could not be found");
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect URI request");
        }

        [Route("api/project/{projectId}/{task}/{elementId}/comments")]
        public HttpResponseMessage PostAProjectElementComment(string projectId, string task, string elementId,
            [FromBody] Comment comment)
        {
            Project project = _open.FindOneById(ObjectId.Parse(projectId));

            var nComment = new Comment
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Created = DateTime.Now,
                Description = comment.Description,
                OwnerId = "1234567890",
                Uri = comment.Uri
            };

            if (task == "bugs")
            {
                Bug bug = project.Bugs.Find(b => b.Id == elementId);
                List<Comment> comments = bug.Comments;

                comments.Add(nComment);
                _open.Save(project);

                return comments != null
                    ? Request.CreateResponse(HttpStatusCode.Accepted, comments)
                    : Request.CreateErrorResponse(HttpStatusCode.NotFound, "Comment could not be created");
            }

            if (task == "stories")
            {
                Story story = project.Stories.Find(s => s.Id == elementId);
                List<Comment> comments = story.Comments;

                comments.Add(nComment);
                _open.Save(project);

                return comments != null
                    ? Request.CreateResponse(HttpStatusCode.Accepted, comments)
                    : Request.CreateErrorResponse(HttpStatusCode.NotFound, "Comment could not be created");
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect URI request");
        }

        [Route("api/project/{projectId}/{task}/{elementId}/comments/{commentId}")]
        public HttpResponseMessage DeleteAProjectElementComments(string projectId, string task, string elementId,
            string commentId)
        {
            IMongoQuery query = Query.And(Query.EQ("_id", ObjectId.Parse(projectId)));

            if (task == "bugs")
            {
                UpdateBuilder update = Update.Pull("Bug.Comment", new BsonDocument
                {
                    {"_id", ObjectId.Parse(commentId)}
                });

                try
                {
                    _open.Update(query, update);
                    return Request.CreateResponse(HttpStatusCode.Accepted, "Comment deleted: " + commentId);
                }

                catch
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Comment could not be deleted");
                }
            }

            if (task == "stories")
            {
                UpdateBuilder update = Update.Pull("Story.Comment", new BsonDocument
                {
                    {"_id", ObjectId.Parse(commentId)}
                });

                try
                {
                    _open.Update(query, update);
                    return Request.CreateResponse(HttpStatusCode.Accepted, "Comment deleted: " + commentId);
                }

                catch
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Comment could not be deleted");
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect URI request");
        }
    }
}