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
using KanbanTracker.Classes;
using KanbanTracker.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

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
            _open.Remove(Query.And(Query.EQ("Description", "Seeded data")));
        }

        [TestMethod]
        public void SeedDataObjects()
        {
            var projects = new List<Project>
            {
                new Project
                {
                    Id = GenerateId(),
                    Uri = "http://phillip-pc:62168/api/project/" + id,
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
                            Status = "backlog",
                            Assigned = "marsden@phillip.com",
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
                                    OwnerId = "54a4d1d0d7aa0b1ae48da9f4"
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
                            Status = "readytogo",
                            Assigned = "marsden@phillip.com",
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
                                    OwnerId = "54a4d1d0d7aa0b1ae48da9f4"
                                }
                            }
                        }
                    }

                    
                },

                new Project
                {
                    Id = GenerateId(),
                    Uri = "http://phillip-pc:62168/api/project/" + id,
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
                            Status = "readytogo",
                            Assigned = "marsden@phillip.com",
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
                                    OwnerId = "54a4d1d0d7aa0b1ae48da9f4"
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
                            Status = "readytogo",
                            Assigned = "marsden@phillip.com",
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
                                    OwnerId = "54a4d1d0d7aa0b1ae48da9f4"
                                }
                            }
                        }
                    }
                },

                new Project
                {
                    Id = GenerateId(),
                    Uri = "http://phillip-pc:62168/api/project/" + id,
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
                            Status = "readytogo",
                            Assigned = "marsden@phillip.com",
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
                                    OwnerId = "54a4d1d0d7aa0b1ae48da9f4"
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
                            Status = "readytogo",
                            Assigned = "marsden@phillip.com",
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
                                    OwnerId = "54a4d1d0d7aa0b1ae48da9f4"
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
