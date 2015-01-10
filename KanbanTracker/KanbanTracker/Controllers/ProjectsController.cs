using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using KanbanTracker.Classes;
using KanbanTracker.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace KanbanTracker.Controllers
{
    [Auth]
    public class ProjectsController : Controller
    {
        private readonly MongoCollection<Project> _open;

        public ProjectsController()
        {
            _open = ProjectDb.Open();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public RedirectToRouteResult Create(ProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var project = new Project
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Title = model.Title,
                    Owner = model.Owner,
                    Description = model.Description,
                    Created = DateTime.Now,
                    DueDate = model.DueDate,
                    Stories = new List<Story>(),
                    Bugs = new List<Bug>()
                };

                _open.Save(project);
                @ViewBag.Info = (string.Format("Project created: {0}", project.Title));
            }

            return RedirectToAction("index", "projects"); 
        }

        public ActionResult Create()
        {
            @ViewBag.users = UserDb.Open();
            return View();
        }

        public ActionResult Dashboard(string id)
        {
            @ViewBag.id = id;
            return View();
        }

        public ActionResult Story_Create(string id)
        {
            return View();
        }

        public ActionResult Bug_Create(string id)
        {
            return View();
        }

        public ActionResult Story_Edit(string id, string storyId)
        {
            @ViewBag.id = id;
            @ViewBag.storyId = storyId;
            return View();
        }

        public ActionResult Bug_Edit(string id, string storyId)
        {
            @ViewBag.id = id;
            @ViewBag.bugId = storyId; 
            return View(); 
        }

        [HttpPost]
        public RedirectToRouteResult Story_Edit(string id, StoryViewModel model)
        {
            var project = _open.FindOneById(ObjectId.Parse(model.ProjectId));
            var story = project.Stories.Find(s => s.Id == model.Id);

            var status = model.Status != story.Status;
            var description = model.Description != story.Description;
            var assigned = model.Assigned != story.Assigned;

            var comment = new Comment
            {
                Created = DateTime.Now,
                Description = "",
                OwnerId = Classes.User.CurrentUser.Id
            };

            if (status)
            {
                comment.Description += string.Format("Status changed from '{1}' => '{0}'. ",
                    model.Status, story.Status);
            }

            if (description)
            {
                comment.Description += string.Format("Description changed from '{1}' => '{0}'. ",
                    model.Description, story.Description);
            }

            if (assigned)
            {
                comment.Description += string.Format("Assigned changed from '{1}' => '{0}'. ",
                    Classes.User.GetUserFromId(model.Assigned), Classes.User.GetUserFromId(story.Assigned));
            }

                story.Status = model.Status;
                story.Description = model.Description;
                story.Assigned = model.Assigned;

            if (comment.Description != "")
            {
                comment.Description = comment.Description.Insert(0, string.Format("'{0}' updated ", story.Title));
                story.Comments.Add(comment);
            }

            _open.Save(project);

            return RedirectToAction("dashboard", "projects", new { id = model.ProjectId });
        }

        [HttpPost]
        public RedirectToRouteResult Bug_Edit(string id, StoryViewModel model)
        {
            var project = _open.FindOneById(ObjectId.Parse(model.ProjectId));
            var bug = project.Bugs.Find(s => s.Id == model.Id);

            var status = model.Status != bug.Status;
            var description = model.Description != bug.Description;
            var assigned = model.Assigned != bug.Assigned;

            var comment = new Comment
            {
                Created = DateTime.Now,
                Description = "",
                OwnerId = Classes.User.CurrentUser.Id
            };

            if (status)
            {
                comment.Description += string.Format("Status changed from '{1}' => '{0}'. ",
                    model.Status, bug.Status);
            }

            if (description)
            {
                comment.Description += string.Format("Description changed from '{1}' => '{0}'. ",
                    model.Description, bug.Description);
            }

            if (assigned)
            {
                comment.Description += string.Format("Assigned changed from '{1}' => '{0}'. ",
                    Classes.User.GetUserFromId(model.Assigned), Classes.User.GetUserFromId(bug.Assigned));
            }

            bug.Status = model.Status;
            bug.Description = model.Description;
            bug.Assigned = model.Assigned;

            if (comment.Description != "")
            {
                comment.Description = comment.Description.Insert(0, string.Format("'{0}' updated ", bug.Title));
                bug.Comments.Add(comment);
            }

            _open.Save(project);

            return RedirectToAction("dashboard", "projects", new { id = model.ProjectId });
        }

        public RedirectToRouteResult Story_Delete(string id, string storyId)
        {
            if (ModelState.IsValid)
            {
                var query = Query.And(Query.EQ("_id", ObjectId.Parse(id)));
                var update = Update.Pull("Stories", new BsonDocument{
                             { "_id", ObjectId.Parse(storyId) }
                });

                _open.Update(query, update);
            }
            
            return RedirectToAction("dashboard", "projects", new { id });
        }

        [HttpPost]
        public RedirectToRouteResult Story_Create(string id, StoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var story = new Story
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Title = model.Title,
                    Description = model.Description,
                    Assigned = model.Assigned,
                    Created = DateTime.Now,
                    Status = model.Status,
                    Comments = new List<Comment>()
                };

                var project = _open.FindOneById(ObjectId.Parse(id));
                project.Stories.Add(story);
                _open.Save(project);

                @ViewBag.info = (string.Format("Story created: {0}", story.Title));
            }

            return RedirectToAction("index", "projects"); 
        }

        [HttpPost]
        public RedirectToRouteResult Bug_Create(string id, BugViewModel model) 
        {
            if (ModelState.IsValid)
            {
                var bug = new Bug
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Title = model.Title,
                    Description = model.Description,
                    Assigned = model.Assigned,
                    Created = DateTime.Now,
                    Status = model.Status,
                    Comments = new List<Comment>()
                };

                var project = _open.FindOneById(ObjectId.Parse(id));
                project.Bugs.Add(bug);
                _open.Save(project);

                @ViewBag.info = (string.Format("Bug created: {0}", bug.Title));
            }

            return RedirectToAction("index", "projects");
        }

        [HttpPost]
        public RedirectToRouteResult Comment_Create(CommentViewModel model)
        {

            var comment = new Comment 
            {
                Description = model.Description,
                OwnerId = Classes.User.CurrentUser.Id,
                Created = DateTime.Now
            };

            var project = _open.FindOneById(ObjectId.Parse(model.ProjectId));

            if (model.ElementType != "bug")
            {
                var element = project.Stories.Find(s => s.Id == model.ElementId);
                element.Comments.Add(comment);
            }

            else
            {
                var element = project.Bugs.Find(s => s.Id == model.ElementId);
                element.Comments.Add(comment);
            }

            _open.Save(project);

            return RedirectToAction("index", "projects"); 
        }

        public RedirectToRouteResult Delete(string id)
        {
            _open.Remove(new QueryDocument("_id", new BsonObjectId(new ObjectId(id))));
            return RedirectToAction("index", "projects"); 
        }

        public ActionResult Story(string id, string storyId)
        {
            @ViewBag.id = id;
            @ViewBag.story = storyId;

            return View();
        }

    }
}