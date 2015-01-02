using System;
using System.Collections.Generic;
using System.Web.Mvc;
using KanbanTracker.Classes;
using KanbanTracker.Models;
using MongoDB.Bson;
using MongoDB.Driver;

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
                    Description = model.Description,
                    Created = DateTime.Now,
                    DueDate = model.DueDate
                };

                _open.Save(project);
                @ViewBag.Info = (string.Format("Project created: {0}", project.Title));
            }

            return RedirectToAction("dashboard", "projects");
        }

        public ActionResult Create()
        {
            return View();
        }

        
        public ActionResult Dashboard()
        {
            return View();
        } 

    }
}