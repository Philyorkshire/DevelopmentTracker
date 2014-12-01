using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KanbanTracker.Classes

{
    public class Project
    {
        public string Uri { get; set; }

        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [BsonDateTimeOptions(Representation = BsonType.Document)]
        public DateTime Created { get; set; }

        [BsonDateTimeOptions(Representation = BsonType.Document)]
        public DateTime DueDate { get; set; }

        public List<Story> Stories { get; set; }
        public List<Bug> Bugs { get; set; }
    } 
}