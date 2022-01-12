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
        public IList<string> Tags { get; set; }

        /// <summary>
        /// List of the class's text fields.
        /// </summary>
        public IList<string> TextFields { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public Class(ClassesConfig config, IList<string> data)
        {
            this.Matched = false;
            this.Name = ParseHelper.SafeStringParse(data, config.Name, "Name", true);
            this.MovementType = ParseHelper.SafeStringParse(data, config.MovementType, "Movement Type", true);

            this.Tags = ParseHelper.StringCSVParse(data, config.Tags);
            this.TextFields = ParseHelper.StringListParse(data, config.TextFields);
        }

        #region Static Functions

        public static IDictionary<string, Class> BuildDictionary(ClassesConfig config)
        {
            IDictionary<string, Class> classes = new Dictionary<string, Class>();

            foreach (IList<object> row in config.Query.Data)
            {
                try
                {
                    IList<string> cls = row.Select(r => r.ToString()).ToList();
                    string name = ParseHelper.SafeStringParse(cls, config.Name, "Name", false);
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
