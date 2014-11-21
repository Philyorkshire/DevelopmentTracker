﻿using DevelopmentTracker.Classes;
using MongoDB.Driver;

namespace DevelopmentTracker.Models
{
    public static class StoryDb
    {
        public static MongoCollection<Story> Open()
        {
            var client = new MongoClient("mongodb://localhost");
            var server = client.GetServer();
            var db = server.GetDatabase("StoryDb");
            return db.GetCollection<Story>("Stories");
        }
    }
}