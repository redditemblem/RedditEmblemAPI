using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Tags;
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
    public class Tag : IMatchable
    {
        #region Attributes

        /// <summary>
        /// Flag indicating whether or not this status was found on a unit. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; private set; }

        /// <summary>
        /// The name of the tag.
        /// </summary>
        public string Name { get; set; }

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

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public Tag(TagsConfig config, IEnumerable<string> data)
        {
            this.Matched = false;
            this.Name = DataParser.String(data, config.Name, "Name");
            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.ShowOnUnit = (DataParser.OptionalBoolean_YesNo(data, config.ShowOnUnit, "Show On Unit") && !string.IsNullOrEmpty(this.SpriteURL));
            this.UnitAura = DataParser.OptionalString_HexCode(data, config.UnitAura, "Unit Aura");
        }

        /// <summary>
        /// Sets the <c>Matched</c> flag for this <c>Tag</c> to true. Additionally, calls <c>FlagAsMatched()</c> for all of its <c>IMatchable</c> child attributes.
        /// </summary>
        public void FlagAsMatched()
        {
            this.Matched = true;
        }

        #region Static Functions

        /// <summary>
        /// Iterates through the data in <paramref name="config"/>'s <c>Query</c> and builds a <c>Tag</c> from each valid row.
        /// </summary>
        /// <exception cref="TagProcessingException"></exception>
        public static IReadOnlyDictionary<string, Tag> BuildDictionary(TagsConfig config)
        {
            ConcurrentDictionary<string, Tag> tags = new ConcurrentDictionary<string, Tag>();
            if (config == null || config.Queries == null)
                return tags.ToFrozenDictionary();

            try
            {
                Parallel.ForEach(config.Queries.SelectMany(q => q.Data), row =>
                {
                    string name = string.Empty;
                    try
                    {
                        IEnumerable<string> tag = row.Select(r => r.ToString());
                        name = DataParser.OptionalString(tag, config.Name, "Name");
                        if (string.IsNullOrEmpty(name)) return;

                        if (!tags.TryAdd(name, new Tag(config, tag)))
                            throw new NonUniqueObjectNameException("tag");
                    }
                    catch (Exception ex)
                    {
                        throw new TagProcessingException(name, ex);
                    }
                });
            }
            catch(AggregateException ex)
            {
                throw ex.InnerException;
            }
            
            return tags.ToFrozenDictionary();
        }

        /// <summary>
        /// Matches each of the strings in <paramref name="names"/> to a <c>Tag</c> in <paramref name="tags"/> and returns the matches as a list.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned objects to true.</param>
        public static List<Tag> MatchNames(IReadOnlyDictionary<string, Tag> tags, IEnumerable<string> names, bool skipMatchedStatusSet = false)
        {
            return names.Select(n => MatchName(tags, n, skipMatchedStatusSet)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to a <c>Tag</c> in <paramref name="tags"/> and returns it.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned object to true.</param>
        /// <exception cref="UnmatchedTagException"></exception>
        public static Tag MatchName(IReadOnlyDictionary<string, Tag> tags, string name, bool skipMatchedStatusSet = false)
        {
            Tag match;
            if (!tags.TryGetValue(name, out match))
                throw new UnmatchedTagException(name);

            if (!skipMatchedStatusSet) match.FlagAsMatched();

            return match;
        }

        #endregion
    }
}