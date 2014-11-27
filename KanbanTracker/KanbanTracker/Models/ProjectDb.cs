using KanbanTracker.Classes;
using MongoDB.Driver;

namespace KanbanTracker.Models
{
    public static class ProjectDb
    {
        public static MongoCollection<Project> Open()
        {
            var client = new MongoClient("mongodb://localhost");
            var server = client.GetServer();
            var db = server.GetDatabase("ProjectDb");
            return db.GetCollection<Project>("Projects");
        }
    }
}