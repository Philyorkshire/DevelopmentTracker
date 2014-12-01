using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KanbanTracker.Classes
{
    public class Comment
    {
        public string Uri { get; set; }

        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Description { get; set; }
        [BsonDateTimeOptions(Representation = BsonType.Document)]
        public DateTime Created { get; set; }
        public string OwnerId { get; set; }
    }
}