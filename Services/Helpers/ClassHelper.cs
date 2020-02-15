using RedditEmblemAPI.Models.Configuration.System.Classes;
using RedditEmblemAPI.Models.Exceptions;
using RedditEmblemAPI.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Services.Helpers
{
    public class ClassHelper : Helper
    {
        public static IDictionary<string, Class> Process(ClassesConfig config)
        {
            IDictionary<string, Class> classes = new Dictionary<string, Class>();

            foreach (IList<object> row in config.Query.Data)
            {
                try
                {
                    //Convert objects to strings
                    IList<string> cls = row.Select(r => r.ToString()).ToList();

                    //Skip blank items
                    if (string.IsNullOrEmpty(cls.ElementAtOrDefault(config.ClassName)))
                        continue;

                    Class temp = new Class()
                    {
                        Name = cls.ElementAtOrDefault(config.ClassName).Trim(),
                        MovementType = (cls.ElementAtOrDefault(config.MovementType) ?? string.Empty).Trim()
                    };

                    BuildTextFieldList(temp, cls, config.TextFields);
                    BuildTagsList(temp, cls.ElementAtOrDefault(config.Tags) ?? string.Empty);

                    classes.Add(temp.Name, temp);
                }
                catch (Exception ex)
                {
                    throw new ClassProcessingException(row.ElementAtOrDefault(config.ClassName).ToString(), ex);
                }
            }

            return classes;
        }

        private static void BuildTextFieldList(Class cls, IList<string> data, IList<int> configFields)
        {
            foreach (int field in configFields)
                if (!string.IsNullOrEmpty(data.ElementAtOrDefault(field)))
                    cls.TextFields.Add(data.ElementAtOrDefault(field));
        }

        private static void BuildTagsList(Class cls, string tagsCSV)
        {
            foreach (string tag in tagsCSV.Split(','))
                if (!string.IsNullOrEmpty(tag))
                    cls.Tags.Add(tag.Trim());
        }
    }
}
