using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Classes;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System
{
    public class Class
    {
        #region Attributes

        /// <summary>
        /// Flag indicating whether or not this class was found on a unit. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; set; }

        /// <summary>
        /// The class's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The class's movement type. (i.e. Foot/Mounted/etc.)
        /// </summary>
        public string MovementType { get; set; }

        /// <summary>
        /// List of the class's tags.
        /// </summary>
        [JsonIgnore]
        public List<string> Tags { get; set; }

        /// <summary>
        /// List of the class's text fields.
        /// </summary>
        public List<string> TextFields { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public Class(ClassesConfig config, List<string> data)
        {
            this.Matched = false;
            this.Name = DataParser.String(data, config.Name, "Name");
            this.MovementType = DataParser.String(data, config.MovementType, "Movement Type");

            this.Tags = DataParser.List_StringCSV(data, config.Tags).Distinct().ToList();
            this.TextFields = DataParser.List_Strings(data, config.TextFields);
        }

        #region Static Functions

        public static IDictionary<string, Class> BuildDictionary(ClassesConfig config)
        {
            IDictionary<string, Class> classes = new Dictionary<string, Class>();

            foreach (List<object> row in config.Query.Data)
            {
                try
                {
                    List<string> cls = row.Select(r => r.ToString()).ToList();
                    string name = DataParser.OptionalString(cls, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!classes.TryAdd(name, new Class(config, cls)))
                        throw new NonUniqueObjectNameException("class");
                }
                catch (Exception ex)
                {
                    throw new ClassProcessingException((row.ElementAtOrDefault(config.Name) ?? string.Empty).ToString(), ex);
                }
            }

            return classes;
        }

        #endregion

    }
}
