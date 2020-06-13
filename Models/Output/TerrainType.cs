using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.TerrainTypes;
using RedditEmblemAPI.Models.Exceptions;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output
{
    /// <summary>
    /// Object representing a TerrainType definition in the team's system.
    /// </summary>
    public class TerrainType
    {
        /// <summary>
        /// Flag indicating whether or not this terrain type was found on a tile. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; set; }

        /// <summary>
        /// The name of the terrain type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of stat modifiers applied by the terrain type.
        /// </summary>
        public IDictionary<string, int> StatModifiers { get; set; }

        /// <summary>
        /// List of movement costs for the terrain type.
        /// </summary>
        [JsonIgnore]
        public IDictionary<string, int> MovementCosts { get; set; }

        /// <summary>
        /// Flag indicating whether or not item ranges can pass through the terrain type.
        /// </summary>
        [JsonIgnore]
        public bool BlocksItems { get; set; }

        /// <summary>
        /// List of text fields for the terrain type.
        /// </summary>
        public IList<string> TextFields { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="AnyIntegerException"></exception>
        public TerrainType(TerrainTypesConfig config, IList<string> data)
        {
            this.Matched = false;
            this.Name = data.ElementAtOrDefault(config.Name).Trim();
            this.BlocksItems = ((data.ElementAtOrDefault(config.BlocksItems) ?? "No").Trim() == "Yes");

            this.StatModifiers = new Dictionary<string, int>();
            foreach(NamedStatConfig stat in config.StatModifiers)
            {
                int val;
                if (!int.TryParse(data.ElementAtOrDefault(stat.Value), out val))
                    throw new AnyIntegerException(stat.SourceName, data.ElementAtOrDefault(stat.Value));
                this.StatModifiers.Add(stat.SourceName, val);
            }

            this.MovementCosts = new Dictionary<string, int>();
            foreach (NamedStatConfig stat in config.MovementCosts)
            {
                int val;
                if (!int.TryParse(data.ElementAtOrDefault(stat.Value), out val))
                    throw new AnyIntegerException(stat.SourceName, data.ElementAtOrDefault(stat.Value));
                this.MovementCosts.Add(stat.SourceName, val);
            }

            this.TextFields = ParseHelper.StringListParse(data, config.TextFields);
        }
    }
}