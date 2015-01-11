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

        [BsonRepresentation(BsonType.ObjectId)]
        public string Owner { get; set; }

        [BsonDateTimeOptions(Representation = BsonType.Document)]
        public DateTime Created { get; set; }

        [BsonDateTimeOptions(Representation = BsonType.Document)]
        public DateTime DueDate { get; set; }

        public List<Story> Stories { get; set; }
        public List<Bug> Bugs { get; set; }

        public int DateUntilCompletion(DateTime date)
        {
            TimeSpan duration = DateTime.Now - date;
            return duration.Days;
        }
    }
}