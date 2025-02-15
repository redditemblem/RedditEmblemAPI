using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Classes;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Interfaces;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedditEmblemAPI.Models.Output.System
{
    public class Class : IMatchable
    {
        #region Attributes

        /// <summary>
        /// Flag indicating whether or not this class was found on a unit. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; private set; }

        /// <summary>
        /// The class's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The class's movement type. (i.e. Foot/Mounted/etc.)
        /// </summary>
        public string MovementType { get; set; }

        /// <summary>
        /// The class's battle style.
        /// </summary>
        [JsonIgnore]
        public BattleStyle BattleStyle { get; set; }

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
        public Class(ClassesConfig config, IEnumerable<string> data, IReadOnlyDictionary<string, BattleStyle> battleStyles)
        {
            this.Matched = false;
            this.Name = DataParser.String(data, config.Name, "Name");
            this.MovementType = DataParser.String(data, config.MovementType, "Movement Type");

            string battleStyle = DataParser.OptionalString(data, config.BattleStyle, "Battle Style");
            if(!string.IsNullOrEmpty(battleStyle))
                this.BattleStyle = BattleStyle.MatchName(battleStyles, battleStyle, true);

            this.Tags = DataParser.List_StringCSV(data, config.Tags).Distinct().ToList();
            this.TextFields = DataParser.List_Strings(data, config.TextFields);
        }

        /// <summary>
        /// Sets the <c>Matched</c> flag for this <c>Class</c> to true. Additionally, calls <c>FlagAsMatched()</c> for all of its <c>IMatchable</c> child attributes.
        /// </summary>
        public void FlagAsMatched()
        {
            this.Matched = true;

            if(this.BattleStyle != null)
                this.BattleStyle.FlagAsMatched();
        }

        #region Static Functions

        /// <summary>
        /// Iterates through the data in <paramref name="config"/>'s <c>Query</c> and builds a <c>Class</c> from each valid row.
        /// </summary>
        /// <exception cref="ClassProcessingException"></exception>
        public static IReadOnlyDictionary<string, Class> BuildDictionary(ClassesConfig config, IReadOnlyDictionary<string, BattleStyle> battleStyles)
        {
            ConcurrentDictionary<string, Class> classes = new ConcurrentDictionary<string, Class>();
            if (config == null || config.Queries == null)
                return classes.ToFrozenDictionary();

            try
            {
                Parallel.ForEach(config.Queries.SelectMany(q => q.Data), row =>
                {
                    string name = string.Empty;
                    try
                    {
                        IEnumerable<string> cls = row.Select(r => r.ToString());
                        name = DataParser.OptionalString(cls, config.Name, "Name");
                        if (string.IsNullOrEmpty(name)) return;

                        if (!classes.TryAdd(name, new Class(config, cls, battleStyles)))
                            throw new NonUniqueObjectNameException("class");
                    }
                    catch (Exception ex)
                    {
                        throw new ClassProcessingException(name, ex);
                    }
                });
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
            
            return classes.ToFrozenDictionary();
        }

        /// <summary>
        /// Matches each of the strings in <paramref name="names"/> to a <c>Class</c> in <paramref name="classes"/> and returns the matches as a list.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned objects to true.</param>
        public static List<Class> MatchNames(IReadOnlyDictionary<string, Class> classes, IEnumerable<string> names, bool skipMatchedStatusSet = false)
        {
            return names.Select(n => MatchName(classes, n, skipMatchedStatusSet)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to a <c>Class</c> in <paramref name="classes"/> and returns it.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned object to true.</param>
        /// <exception cref="UnmatchedClassException"></exception>
        public static Class MatchName(IReadOnlyDictionary<string, Class> classes, string name, bool skipMatchedStatusSet = false)
        {
            Class match;
            if (!classes.TryGetValue(name, out match))
                throw new UnmatchedClassException(name);

            if (!skipMatchedStatusSet) match.FlagAsMatched();

            return match;
        }

        #endregion

    }
}
