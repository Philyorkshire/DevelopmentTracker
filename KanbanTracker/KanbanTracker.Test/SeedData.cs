using System;
using System.Collections.Generic;
using KanbanTracker.Classes;
using KanbanTracker.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;

namespace KanbanTracker.Test
{
    [TestClass]
    public class SeedData
    {
        private readonly MongoCollection<Project> _open;

        public SeedData()
        {
            _open = ProjectDb.Open();
        }

        [TestMethod]
        public void RemoveAllProjects()
        {
            _open.RemoveAll();
        }

        [TestMethod]
        public void SeedDataObjects()
        {
            var projects = new List<Project>
            {
                new Project
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Title = "Unit Testing 1",
                    Description = "Seeded data",
                    Created = DateTime.Now,
                    DueDate = DateTime.Now,
                    Stories = new List<Story>
                    {
                        new Story
                        {
                            Id = ObjectId.GenerateNewId().ToString(),
                            Title = "Unit Testing",
                            Description = "Seeded data",
                            Created = DateTime.Now,
                            Status = "In-progress",
                            Tags = new List<string>
                            {
                                "unitTests"
                            }

                        }
                    }
                },

                new Project
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Title = "Unit Testing 2",
                    Description = "Seeded data",
                    Created = DateTime.Now,
                    DueDate = DateTime.Now,
                    Stories = new List<Story>
                    {
                        new Story
                        {
                            Id = ObjectId.GenerateNewId().ToString(),
                            Title = "Unit Testing",
                            Description = "Seeded data",
                            Created = DateTime.Now,
                            Status = "In-progress",
                            Tags = new List<string>
                            {
                                "unitTests"
                            }

                        }
                    }
                },

                new Project
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Title = "Unit Testing 3",
                    Description = "Seeded data",
                    Created = DateTime.Now,
                    DueDate = DateTime.Now,
                    Stories = new List<Story>
                    {
                        new Story
                        {
                            Id = ObjectId.GenerateNewId().ToString(),
                            Title = "Unit Testing",
                            Description = "Seeded data",
                            Created = DateTime.Now,
                            Status = "In-progress",
                            Tags = new List<string>
                            {
                                "unitTests"
                            }

                        }
                    }
                }


            };

            _open.InsertBatch(projects);
        }
    }
}
