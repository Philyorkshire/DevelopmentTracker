using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KanbanTracker.Classes
{
    public class Project
    {
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [BsonDateTimeOptions(Representation = BsonType.Document)]
        public DateTime Created { get; set; }

        [BsonDateTimeOptions(Representation = BsonType.Document)]
        public DateTime DueDate { get; set; }

        public Team Team { get; set; }
    }
}