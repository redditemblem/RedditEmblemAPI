using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.TerrainEffects;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Services.Helpers;
using System;
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
        /// The layer the terrain effect will be rendered on.
        /// </summary>
        public TerrainEffectLayer Layer { get; set; }

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
            this.SpriteURL = ParseHelper.SafeURLParse(data, config.SpriteURL, "Sprite URL", true);
            this.Size = ParseHelper.OptionalInt_NonZeroPositive(data, config.Size, "Size");
            this.Layer = GetTerrainEffectLayerEnum(data.ElementAtOrDefault<string>(config.Layer));
            this.TextFields = ParseHelper.StringListParse(data, config.TextFields);

            this.HPModifier = ParseHelper.OptionalInt_Any(data, config.HPModifier, "HP Modifier");

            this.CombatStatModifiers = new Dictionary<string, int>();
            foreach(NamedStatConfig stat in config.CombatStatModifiers)
            {
                int val = ParseHelper.Int_Any(data, stat.Value, stat.SourceName + " Modifier");
                if (val == 0) continue;

                this.CombatStatModifiers.Add(stat.SourceName, val);
            }
                

            this.StatModifiers = new Dictionary<string, int>();
            foreach(NamedStatConfig stat in config.StatModifiers)
            {
                int val = ParseHelper.Int_Any(data, stat.Value, stat.SourceName + " Modifier");
                if (val == 0) continue;

                this.StatModifiers.Add(stat.SourceName, val);
            }
        }

        /// <summary>
        /// Converts the string value of <paramref name="layer"/> into the corresponding <c>TerrainEffectLayer</c> object.
        /// </summary>
        /// <param name="layer"></param>
        /// <exception cref="UnmatchedTerrainEffectLayerException"></exception>
        /// <returns></returns>
        private TerrainEffectLayer GetTerrainEffectLayerEnum(string layer)
        {
            if (string.IsNullOrEmpty(layer))
                return TerrainEffectLayer.Below;

            object layerEnum;
            if (!Enum.TryParse(typeof(TerrainEffectLayer), layer, out layerEnum))
                throw new UnmatchedTerrainEffectLayerException(layer);

            return (TerrainEffectLayer)layerEnum;
        }

        #region Static Functions

        public static IDictionary<string, TerrainEffect> BuildDictionary(TerrainEffectsConfig config)
        {
            IDictionary<string, TerrainEffect> terrainEffects = new Dictionary<string, TerrainEffect>();

            foreach (IList<object> row in config.Query.Data)
            {
                try
                {
                    IList<string> effect = row.Select(r => r.ToString()).ToList();
                    string name = ParseHelper.SafeStringParse(effect, config.Name, "Name", false);
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!terrainEffects.TryAdd(name, new TerrainEffect(config, effect)))
                        throw new NonUniqueObjectNameException("terrain effect");
                }
                catch (Exception ex)
                {
                    throw new TerrainEffectProcessingException((row.ElementAtOrDefault(config.Name) ?? string.Empty).ToString(), ex);
                }
            }

            return terrainEffects;
        }

        #endregion
    }

    //Note: enum equals (=) value is important for calculating the z-index of the sprite render
    public enum TerrainEffectLayer
    {
        Below = 0,
        Above = 1
    }
}
