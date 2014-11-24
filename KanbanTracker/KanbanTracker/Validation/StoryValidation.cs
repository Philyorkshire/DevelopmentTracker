using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Reflection;
using KanbanTracker.Classes;

namespace KanbanTracker.Validation
{
    public class StoryValidation
    {
        public string StoryContentsNotEmpty(Story story)
        {
            var properties = story.GetType().GetProperties();
            List<string> missingFields = null;

            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.GetConstantValue() == "")
                {
                    missingFields.Add("item");
                }
            }

            return missingFields.ToString();
        }
    }
}