﻿using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Classes;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Interfaces;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System
{
    public class Class : IMatchable
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
        public Class(ClassesConfig config, IEnumerable<string> data)
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
            if (config == null || config.Query == null)
                return classes;

            foreach (List<object> row in config.Query.Data)
            {
                try
                {
                    IEnumerable<string> cls = row.Select(r => r.ToString());
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

        /// <summary>
        /// Matches each of the strings in <paramref name="names"/> to a <c>Class</c> in <paramref name="classes"/> and returns the matches as a list.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned objects to true.</param>
        public static List<Class> MatchNames(IDictionary<string, Class> classes, IEnumerable<string> names, bool skipMatchedStatusSet = false)
        {
            return names.Select(n => MatchName(classes, n, skipMatchedStatusSet)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to a <c>Class</c> in <paramref name="classes"/> and returns it.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned object to true.</param>
        /// <exception cref="UnmatchedClassException"></exception>
        public static Class MatchName(IDictionary<string, Class> classes, string name, bool skipMatchedStatusSet = false)
        {
            Class match;
            if (!classes.TryGetValue(name, out match))
                throw new UnmatchedClassException(name);

            if (!skipMatchedStatusSet) match.Matched = true;

            return match;
        }

        #endregion

    }
}
