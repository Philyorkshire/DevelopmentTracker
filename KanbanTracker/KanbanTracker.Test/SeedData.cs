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
        private string id { get; set; }

        public string GenerateId()
        {
            var ID = ObjectId.GenerateNewId().ToString();
            id = ID;
            return ID;
        }

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
                    Id = GenerateId(),
                    Uri = "http://localhost:62168/project/" + id,
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
                            },

                            Comments = new List<Comment>
                            {
                                new Comment
                                {
                                    Id = ObjectId.GenerateNewId().ToString(),
                                    Description = "This is a new comment - seeded",
                                    Created = DateTime.Now,
                                    OwnerId = "123456789"
                                }
                            }
                        }
                    },

                    Bugs = new List<Bug>
                    {
                        new Bug
                        {
                            Id = ObjectId.GenerateNewId().ToString(),
                            Title = "Unit Testing",
                            Description = "Seeded data",
                            Created = DateTime.Now,
                            Status = "In-progress",
                            Tags = new List<string>
                            {
                                "unitTests"
                            },

                            Comments = new List<Comment>
                            {
                                new Comment
                                {
                                    Id = ObjectId.GenerateNewId().ToString(),
                                    Description = "This is a new comment - seeded",
                                    Created = DateTime.Now,
                                    OwnerId = "123456789"
                                }
                            }
                        }
                    }

                    
                },

                new Project
                {
                    Id = GenerateId(),
                    Uri = "http://localhost:62168/project/" + id,
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
                            },

                            Comments = new List<Comment>
                            {
                                new Comment
                                {
                                    Id = ObjectId.GenerateNewId().ToString(),
                                    Description = "This is a new comment - seeded",
                                    Created = DateTime.Now,
                                    OwnerId = "123456789"
                                }
                            }

                        }
                    },

                    Bugs = new List<Bug>
                    {
                        new Bug
                        {
                            Id = ObjectId.GenerateNewId().ToString(),
                            Title = "Unit Testing",
                            Description = "Seeded data",
                            Created = DateTime.Now,
                            Status = "In-progress",
                            Tags = new List<string>
                            {
                                "unitTests"
                            },

                            Comments = new List<Comment>
                            {
                                new Comment
                                {
                                    Id = ObjectId.GenerateNewId().ToString(),
                                    Description = "This is a new comment - seeded",
                                    Created = DateTime.Now,
                                    OwnerId = "123456789"
                                }
                            }
                        }
                    }
                },

                new Project
                {
                    Id = GenerateId(),
                    Uri = "http://localhost:62168/project/" + id,
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
                            },

                            Comments = new List<Comment>
                            {
                                new Comment
                                {
                                    Id = ObjectId.GenerateNewId().ToString(),
                                    Description = "This is a new comment - seeded",
                                    Created = DateTime.Now,
                                    OwnerId = "123456789"
                                }
                            }

                        }
                    },

                    Bugs = new List<Bug>
                    {
                        new Bug
                        {
                            Id = ObjectId.GenerateNewId().ToString(),
                            Title = "Unit Testing",
                            Description = "Seeded data",
                            Created = DateTime.Now,
                            Status = "In-progress",
                            Tags = new List<string>
                            {
                                "unitTests"
                            },

                            Comments = new List<Comment>
                            {
                                new Comment
                                {
                                    Id = ObjectId.GenerateNewId().ToString(),
                                    Description = "This is a new comment - seeded",
                                    Created = DateTime.Now,
                                    OwnerId = "123456789"
                                }
                            }
                        }
                    }
                }
            };

            _open.InsertBatch(projects);
        }
    }
}
