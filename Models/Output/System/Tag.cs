using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Tags;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Match;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System
{
    #region Interface

    /// <inheritdoc cref="Tag"/>
    public interface ITag : IMatchable
    {
        /// <inheritdoc cref="Tag.SpriteURL"/>
        string SpriteURL { get; set; }

        /// <inheritdoc cref="Tag.ShowOnUnit"/>
        bool ShowOnUnit { get; set; }

        /// <inheritdoc cref="Tag.UnitAura"/>
        string UnitAura { get; set; }
    }

    #endregion Interface

    public class Tag : Matchable, ITag
    {
        #region Attributes

        /// <summary>
        /// The sprite image URL for the tag.
        /// </summary>
        public string SpriteURL { get; set; }

        /// <summary>
        /// Flag indicating if the tag's sprite should be displayed over the map sprite of units who possess it.
        /// </summary>
        public bool ShowOnUnit { get; set; }

        /// <summary>
        /// The unit aura color for the tag.
        /// </summary>
        [JsonIgnore]
        public string UnitAura { get; set; }

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        public Tag(TagsConfig config, IEnumerable<string> data)
        {
            this.Name = DataParser.String(data, config.Name, "Name");
            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.ShowOnUnit = (DataParser.OptionalBoolean_YesNo(data, config.ShowOnUnit, "Show On Unit") && !string.IsNullOrEmpty(this.SpriteURL));
            this.UnitAura = DataParser.OptionalString_HexCode(data, config.UnitAura, "Unit Aura");
        }

        #region Static Functions

        /// <summary>
        /// Iterates through <paramref name="config"/>'s queried data and builds an <c>ITag</c> from each valid row.
        /// </summary>
        /// <exception cref="TagProcessingException"></exception>
        public static IDictionary<string, ITag> BuildDictionary(TagsConfig config)
        {
            IDictionary<string, ITag> tags = new Dictionary<string, ITag>();
            if (config?.Queries is null) return tags;

            foreach (IList<object> row in config.Queries.SelectMany(q => q.Data))
            {
                string name = string.Empty;
                try
                {
                    IEnumerable<string> tag = row.Select(r => r.ToString());
                    name = DataParser.OptionalString(tag, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!tags.TryAdd(name, new Tag(config, tag)))
                        throw new NonUniqueObjectNameException("tag");
                }
                catch (Exception ex)
                {
                    throw new TagProcessingException(name, ex);
                }
            }

            return tags;
        }

        /// <summary>
        /// Matches each string in <paramref name="names"/> to an <c>ITag</c> in <paramref name="tags"/> and returns the matches as a list.
        /// </summary>
        /// <param name="flagAsMatched">If true, calls <c>IMatchable.FlagAsMatched()</c> for all returned objects.</param>
        public static List<ITag> MatchNames(IDictionary<string, ITag> tags, IEnumerable<string> names, bool flagAsMatched = true)
        {
            return names.Select(n => MatchName(tags, n, flagAsMatched)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to an <c>ITag</c> in <paramref name="tags"/> and returns it.
        /// </summary>
        /// <param name="flagAsMatched">If true, calls <c>IMatchable.FlagAsMatched()</c> for the returned object.</param>
        /// <exception cref="UnmatchedTagException"></exception>
        public static ITag MatchName(IDictionary<string, ITag> tags, string name, bool flagAsMatched = true)
        {
            ITag match;
            if (!tags.TryGetValue(name, out match))
                throw new UnmatchedTagException(name);

            if (flagAsMatched) match.FlagAsMatched();

            return match;
        }

        #endregion Static Functions
    }
}