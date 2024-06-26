﻿using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Items;
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
    /// <summary>
    /// Object representing an item engraving definition in the team's system.
    /// </summary>
    public class Engraving : IMatchable
    {
        #region Attributes

        /// <summary>
        /// Flag indicating whether or not this engraving was found on an item. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; private set; }

        /// <summary>
        /// The engraving's name. 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The sprite image URL for the engraving.
        /// </summary>
        public string SpriteURL { get; set; }

        /// <summary>
        /// Dictionary of the stat modifiers this engraving applies to items.
        /// </summary>
        [JsonIgnore]
        public IDictionary<string, int> ItemStatModifiers { get; set; }

        /// <summary>
        /// The engraving's item range overrides.
        /// </summary>
        [JsonIgnore]
        public ItemRange ItemRangeOverrides { get; set; }

        /// <summary>
        /// Dictionary of the combat stat modifiers this engraving applies to units.
        /// </summary>
        [JsonIgnore]
        public IDictionary<string, int> CombatStatModifiers { get; set; }

        /// <summary>
        /// Dictionary of the stat modifiers this engraving applies to units.
        /// </summary>
        [JsonIgnore]
        public IDictionary<string, int> StatModifiers { get; set; }

        /// <summary>
        /// The engraving's tags.
        /// </summary>
        [JsonIgnore]
        public List<Tag> Tags { get; set; }

        /// <summary>
        /// List of the engraving's text fields.
        /// </summary>
        public List<string> TextFields { get; set; }

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        public Engraving(EngravingsConfig config, IEnumerable<string> data, IDictionary<string, Tag> tags)
        {
            this.Matched = false;
            this.Name = DataParser.String(data, config.Name, "Name");
            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.TextFields = DataParser.List_Strings(data, config.TextFields);

            IEnumerable<string> engravingTags = DataParser.List_StringCSV(data, config.Tags).Distinct();
            this.Tags = Tag.MatchNames(tags, engravingTags, true);

            this.ItemStatModifiers = DataParser.NamedStatDictionary_OptionalInt_Any(config.ItemStatModifiers, data, false, "{0} Modifier");
            this.ItemRangeOverrides = new ItemRange(config.ItemRangeOverrides, data);
            this.CombatStatModifiers = DataParser.NamedStatDictionary_OptionalInt_Any(config.CombatStatModifiers, data, false, "{0} Modifier");
            this.StatModifiers = DataParser.NamedStatDictionary_OptionalInt_Any(config.StatModifiers, data, false, "{0} Modifier");
        }

        /// <summary>
        /// Sets the <c>Matched</c> flag for this <c>Engraving</c> to true. Additionally, calls <c>FlagAsMatched()</c> for all of its <c>IMatchable</c> child attributes.
        /// </summary>
        public void FlagAsMatched()
        {
            this.Matched = true;
            this.Tags.ForEach(t => t.FlagAsMatched());
        }

        #region Static Functions

        /// <summary>
        /// Iterates through the data in <paramref name="config"/>'s <c>Query</c> and builds an <c>Engraving</c> from each valid row.
        /// </summary>
        /// <exception cref="EngravingProcessingException"></exception>
        public static IDictionary<string, Engraving> BuildDictionary(EngravingsConfig config, IDictionary<string, Tag> tags)
        {
            IDictionary<string, Engraving> engravings = new Dictionary<string, Engraving>();
            if (config == null || config.Queries == null)
                return engravings;

            foreach (List<object> row in config.Queries.SelectMany(q => q.Data))
            {
                string name = string.Empty;
                try
                {
                    IEnumerable<string> engraving = row.Select(r => r.ToString());
                    name = DataParser.OptionalString(engraving, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!engravings.TryAdd(name, new Engraving(config, engraving, tags)))
                        throw new NonUniqueObjectNameException("engraving");
                }
                catch (Exception ex)
                {
                    throw new EngravingProcessingException(name, ex);
                }
            }

            return engravings;
        }

        /// <summary>
        /// Matches each of the strings in <paramref name="names"/> to an <c>Engraving</c> in <paramref name="engravings"/> and returns the matches as a list.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned objects to true.</param>
        public static List<Engraving> MatchNames(IDictionary<string, Engraving> engravings, IEnumerable<string> names, bool skipMatchedStatusSet = false)
        {
            return names.Select(n => MatchName(engravings, n, skipMatchedStatusSet)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to an <c>Engraving</c> in <paramref name="engravings"/> and returns it.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned object to true.</param>
        /// <exception cref="UnmatchedEngravingException"></exception>
        public static Engraving MatchName(IDictionary<string, Engraving> engravings, string name, bool skipMatchedStatusSet = false)
        {
            Engraving match;
            if (!engravings.TryGetValue(name, out match))
                throw new UnmatchedEngravingException(name);

            if (!skipMatchedStatusSet) match.FlagAsMatched();

            return match;
        }

        #endregion Static Functions
    }
}
