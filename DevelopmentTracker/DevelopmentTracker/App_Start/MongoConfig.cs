﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DevelopmentTracker.Classes;
using DevelopmentTracker.Models;
using MongoDB.Driver.Linq;

namespace DevelopmentTracker
{
    public static class MongoConfig
    {
        public static void Seed()
        {
            var stories = StoryDb.Open();

            if (stories.AsQueryable().Any(s => s.Title == "Fix Banner 2")) return;
            var data = new List<Story>
            {
                new Story
                {
                    Status = "Not complete",
                    Title = "Fix Banner",
                    Description = "This is the description",
                    Created = DateTime.Now
                },
                new Story
                {
                    Status = "Not complete",
                    Title = "Fix Banner 2",
                    Description = "This is the description",
                    Created = DateTime.Now
                },
                new Story
                {
                    Status = "Not complete",
                    Title = "Fix Banner 3",
                    Description = "This is the description",
                    Created = DateTime.Now
                }
            };

            stories.InsertBatch(data);
        }
    }
}