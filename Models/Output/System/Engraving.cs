using Newtonsoft.Json;
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
    #region Interface

    /// <inheritdoc cref="Engraving"/>
    public interface IEngraving : IMatchable
    {
        /// <inheritdoc cref="Engraving.SpriteURL"/>
        string SpriteURL { get; set; }

        /// <inheritdoc cref="Engraving.ItemStatModifiers"/>
        IDictionary<string, int> ItemStatModifiers { get; set; }

        /// <inheritdoc cref="Engraving.ItemRangeOverrides"/>
        IItemRange ItemRangeOverrides { get; set; }

        /// <inheritdoc cref="Engraving.CombatStatModifiers"/>
        IDictionary<string, int> CombatStatModifiers { get; set; }

        /// <inheritdoc cref="Engraving.StatModifiers"/>
        IDictionary<string, int> StatModifiers { get; set; }

        /// <inheritdoc cref="Engraving.Tags"/>
        List<ITag> Tags { get; set; }

        /// <inheritdoc cref="Engraving.TextFields"/>
        List<string> TextFields { get; set; }
    }

    #endregion Interface

    /// <summary>
    /// Object representing an item engraving definition in the team's system.
    /// </summary>
    public class Engraving : Matchable, IEngraving
    {
        #region Attributes

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
        public IItemRange ItemRangeOverrides { get; set; }

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
        public List<ITag> Tags { get; set; }

        /// <summary>
        /// List of the engraving's text fields.
        /// </summary>
        public List<string> TextFields { get; set; }

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        public Engraving(EngravingsConfig config, IEnumerable<string> data, IDictionary<string, ITag> tags)
        {
            this.Name = DataParser.String(data, config.Name, "Name");
            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.TextFields = DataParser.List_Strings(data, config.TextFields);

            IEnumerable<string> engravingTags = DataParser.List_StringCSV(data, config.Tags).Distinct();
            this.Tags = Tag.MatchNames(tags, engravingTags, false);

            this.ItemStatModifiers = DataParser.NamedStatDictionary_OptionalInt_Any(config.ItemStatModifiers, data, false, "{0} Modifier");
            this.ItemRangeOverrides = new ItemRange(config.ItemRangeOverrides, data);
            this.CombatStatModifiers = DataParser.NamedStatDictionary_OptionalInt_Any(config.CombatStatModifiers, data, false, "{0} Modifier");
            this.StatModifiers = DataParser.NamedStatDictionary_OptionalInt_Any(config.StatModifiers, data, false, "{0} Modifier");
        }

        public override void FlagAsMatched()
        {
            this.Matched = true;
            this.Tags.ForEach(t => t.FlagAsMatched());
        }

        #region Static Functions

        /// <summary>
        /// Iterates through the data in <paramref name="config"/>'s <c>Query</c> and builds an <c>IEngraving</c> from each valid row.
        /// </summary>
        /// <exception cref="EngravingProcessingException"></exception>
        public static IDictionary<string, IEngraving> BuildDictionary(EngravingsConfig config, IDictionary<string, ITag> tags)
        {
            IDictionary<string, IEngraving> engravings = new Dictionary<string, IEngraving>();
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
        /// Matches each of the strings in <paramref name="names"/> to an <c>IEngraving</c> in <paramref name="engravings"/> and returns the matches as a list.
        /// </summary>
        /// <param name="flagAsMatched">If true, calls <c>IMatchable.FlagAsMatched()</c> for all returned objects.</param>
        public static List<IEngraving> MatchNames(IDictionary<string, IEngraving> engravings, IEnumerable<string> names, bool flagAsMatched = true)
        {
            return names.Select(n => MatchName(engravings, n, flagAsMatched)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to an <c>IEngraving</c> in <paramref name="engravings"/> and returns it.
        /// </summary>
        /// <param name="flagAsMatched">If true, calls <c>IMatchable.FlagAsMatched()</c> for the returned object.</param>
        /// <exception cref="UnmatchedEngravingException"></exception>
        public static IEngraving MatchName(IDictionary<string, IEngraving> engravings, string name, bool flagAsMatched = true)
        {
            IEngraving match;
            if (!engravings.TryGetValue(name, out match))
                throw new UnmatchedEngravingException(name);

            if (flagAsMatched) match.FlagAsMatched();

            return match;
        }

        #endregion Static Functions
    }
}
