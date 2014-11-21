using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DevelopmentTracker.Classes
{
    public abstract class Task
    {
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        [BsonDateTimeOptions(Representation = BsonType.Document)]
        public DateTime Created { get; set; }
    }
}