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

        [JsonIgnore]
        public bool Matched { get; set; }

        public string Name { get; set; }

        public string SpriteURL { get; set; }

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

        public IList<string> TextFields { get; set; }

        #endregion

        public TerrainEffect(TerrainEffectsConfig config, IList<string> data)
        {
            this.Matched = false;

            this.Name = data.ElementAtOrDefault<string>(config.Name);
            this.SpriteURL = data.ElementAtOrDefault<string>(config.SpriteURL);
            this.Size = ParseHelper.OptionalSafeIntParse(data.ElementAtOrDefault<string>(config.Size), "Size", true, 1);
            this.TextFields = ParseHelper.StringListParse(data, config.TextFields);

            this.HPModifier = ParseHelper.OptionalSafeIntParse(data.ElementAtOrDefault<string>(config.HPModifier), "HP Modifier", false, 0);

            this.CombatStatModifiers = new Dictionary<string, int>();
            foreach(NamedStatConfig stat in config.CombatStatModifiers)
                this.CombatStatModifiers.Add(stat.SourceName, ParseHelper.SafeIntParse(data.ElementAtOrDefault<string>(stat.Value), stat.SourceName + " Modifier", false));

            this.StatModifiers = new Dictionary<string, int>();
            foreach(NamedStatConfig stat in config.StatModifiers)
                this.StatModifiers.Add(stat.SourceName, ParseHelper.SafeIntParse(data.ElementAtOrDefault<string>(stat.Value), stat.SourceName + " Modifier", false));
        }
    }
}
