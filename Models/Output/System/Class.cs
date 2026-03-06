using Newtonsoft.Json;
using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Configuration.System.Classes;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Match;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System
{
    #region Interface

    /// <inheritdoc cref="Class"/>
    public interface IClass : IMatchable
    {
        /// <inheritdoc cref="Class.MovementType"/>
        string MovementType { get; set; }

        /// <inheritdoc cref="Class.BattleStyle"/>
        IBattleStyle BattleStyle { get; set; }

        /// <inheritdoc cref="Class.Tags"/>
        List<string> Tags { get; set; }

        /// <inheritdoc cref="Class.TextFields"/>
        List<string> TextFields { get; set; }
    }

    #endregion Interface

    public class Class : Matchable, IClass
    {
        #region Attributes

        /// <summary>
        /// The class's movement type. (i.e. Foot/Mounted/etc.)
        /// </summary>
        public string MovementType { get; set; }

        /// <summary>
        /// The class's battle style.
        /// </summary>
        [JsonIgnore]
        public IBattleStyle BattleStyle { get; set; }

        /// <summary>
        /// List of the class's tags.
        /// </summary>
        [JsonIgnore]
        public List<string> Tags { get; set; }

        /// <summary>
        /// List of the class's text fields.
        /// </summary>
        public List<string> TextFields { get; set; }

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        public Class(ClassesConfig config, IEnumerable<string> data, bool isUnitMovementTypeConfigured, IDictionary<string, IBattleStyle> battleStyles)
        {
            this.Name = DataParser.String(data, config.Name, "Name");
            
            //The movement type field value on the Units sheet is required if the field is configured
            //If we can depend on falling back onto the unit's value, the class value can be optional
            if(isUnitMovementTypeConfigured) this.MovementType = DataParser.OptionalString(data, config.MovementType, "Movement Type");
            else this.MovementType = DataParser.String(data, config.MovementType, "Movement Type");

            string battleStyle = DataParser.OptionalString(data, config.BattleStyle, "Battle Style");
            if(!string.IsNullOrEmpty(battleStyle))
                this.BattleStyle = System.BattleStyle.MatchName(battleStyles, battleStyle, false);

            this.Tags = DataParser.List_StringCSV(data, config.Tags).Distinct().ToList();
            this.TextFields = DataParser.List_Strings(data, config.TextFields);
        }

        public override void FlagAsMatched()
        {
            this.Matched = true;
            this.BattleStyle?.FlagAsMatched();
        }

        #region Static Functions

        /// <summary>
        /// Iterates through <paramref name="config"/>'s queried data and builds an <c>IClass</c> from each valid row.
        /// </summary>
        /// <exception cref="ClassProcessingException"></exception>
        public static IDictionary<string, IClass> BuildDictionary(ClassesConfig config, bool isUnitMovementTypeConfigured, IDictionary<string, IBattleStyle> battleStyles)
        {
            IDictionary<string, IClass> classes = new Dictionary<string, IClass>();
            if (config?.Queries is null) return classes;

            foreach (IList<object> row in config.Queries.SelectMany(q => q.Data))
            {
                string name = string.Empty;
                try
                {
                    IEnumerable<string> cls = row.Select(r => r.ToString());
                    name = DataParser.OptionalString(cls, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!classes.TryAdd(name, new Class(config, cls, isUnitMovementTypeConfigured, battleStyles)))
                        throw new NonUniqueObjectNameException("class");
                }
                catch (Exception ex)
                {
                    throw new ClassProcessingException(name, ex);
                }
            }

            return classes;
        }

        /// <summary>
        /// Matches each string in <paramref name="names"/> to an <c>IClass</c> in <paramref name="classes"/> and returns the matches as a list.
        /// </summary>
        /// <param name="flagAsMatched">If true, calls <c>IMatchable.FlagAsMatched()</c> for all returned objects.</param>
        public static List<IClass> MatchNames(IDictionary<string, IClass> classes, IEnumerable<string> names, bool flagAsMatched = true)
        {
            return names.Select(n => MatchName(classes, n, flagAsMatched)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to an <c>IClass</c> in <paramref name="classes"/> and returns it.
        /// </summary>
        /// <param name="flagAsMatched">If true, calls <c>IMatchable.FlagAsMatched()</c> for the returned object.</param>
        /// <exception cref="UnmatchedClassException"></exception>
        public static IClass MatchName(IDictionary<string, IClass> classes, string name, bool flagAsMatched = true)
        {
            IClass match;
            if (!classes.TryGetValue(name, out match))
                throw new UnmatchedClassException(name);

            if (flagAsMatched) match.FlagAsMatched();

            return match;
        }

        #endregion Static Functions

    }
}
