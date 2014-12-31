﻿/****************************** Development Tracker 2014 ******************************\
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