using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.TerrainEffects;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System
{
    public class TerrainEffect
    {
        #region Attributes

        /// <summary>
        /// Flag indicating whether or not this terrain effect was found on a tile. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; set; }

        /// <summary>
        /// The name of the terrain effect.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The sprite image URL for the terrain effect.
        /// </summary>
        public string SpriteURL { get; set; }

        /// <summary>
        /// The size of the terrain effect in map tiles. Defaults to 1.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// The value by which the terrain effect modifies a unit's HP. Assumed to be a percentage.
        /// </summary>
        public int HPModifier { get; set; }

        /// <summary>
        /// List of modifiers that the terrain effect can apply to a unit's combat stats.
        /// </summary>
        public IDictionary<string, int> CombatStatModifiers { get; set; }

        /// <summary>
        /// List of modifiers that the terrain effect can apply to a unit's stats.
        /// </summary>
        public IDictionary<string, int> StatModifiers { get; set; }

        /// <summary>
        /// List of text fields for the terrain effect.
        /// </summary>
        public IList<string> TextFields { get; set; }

        #endregion

        public TerrainEffect(TerrainEffectsConfig config, IList<string> data)
        {
            this.Matched = false;

            this.Name = ParseHelper.SafeStringParse(data, config.Name, "Name", true);
            this.SpriteURL = ParseHelper.SafeStringParse(data, config.SpriteURL, "Sprite URL", true);
            this.Size = ParseHelper.OptionalSafeIntParse(data, config.Size, "Size", true, 1);
            this.TextFields = ParseHelper.StringListParse(data, config.TextFields);

            this.HPModifier = ParseHelper.OptionalSafeIntParse(data, config.HPModifier, "HP Modifier", false, 0);

            this.CombatStatModifiers = new Dictionary<string, int>();
            foreach(NamedStatConfig stat in config.CombatStatModifiers)
            {
                int val = ParseHelper.SafeIntParse(data, stat.Value, stat.SourceName + " Modifier", false);
                if (val == 0)
                    continue;

                this.CombatStatModifiers.Add(stat.SourceName, val);
            }
                

            this.StatModifiers = new Dictionary<string, int>();
            foreach(NamedStatConfig stat in config.StatModifiers)
            {
                int val = ParseHelper.SafeIntParse(data, stat.Value, stat.SourceName + " Modifier", false);
                if (val == 0)
                    continue;

                this.StatModifiers.Add(stat.SourceName, val);
            }
        }
    }
}
