using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.TileObjects;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System
{
    public class TileObject
    {
        #region Attributes

        /// <summary>
        /// Flag indicating whether or not this tile object was found on a tile. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; set; }

        /// <summary>
        /// The name of the tile object.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The sprite image URL for the tile object.
        /// </summary>
        public string SpriteURL { get; set; }

        /// <summary>
        /// The size of the tile object in map tiles. Defaults to 1.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// The layer the tile object will be rendered on.
        /// </summary>
        public TileObjectLayer Layer { get; set; }

        /// <summary>
        /// Container object for the tile object's range.
        /// </summary>
        public TileObjectRange Range { get; set; }

        /// <summary>
        /// The value by which the tile object modifies a unit's HP. Assumed to be a percentage.
        /// </summary>
        public int HPModifier { get; set; }

        /// <summary>
        /// List of modifiers that the tile object can apply to a unit's combat stats.
        /// </summary>
        public IDictionary<string, int> CombatStatModifiers { get; set; }

        /// <summary>
        /// List of modifiers that the tile object can apply to a unit's stats.
        /// </summary>
        public IDictionary<string, int> StatModifiers { get; set; }

        /// <summary>
        /// List of text fields for the tile object.
        /// </summary>
        public List<string> TextFields { get; set; }

        #endregion

        public TileObject(TileObjectsConfig config, List<string> data)
        {
            this.Matched = false;

            this.Name = DataParser.String(data, config.Name, "Name");
            this.SpriteURL = DataParser.String_URL(data, config.SpriteURL, "Sprite URL");
            this.Size = DataParser.OptionalInt_NonZeroPositive(data, config.Size, "Size");
            this.Layer = GetTerrainObjectLayerEnum(data.ElementAtOrDefault<string>(config.Layer));
            this.TextFields = DataParser.List_Strings(data, config.TextFields);

            if (config.Range != null) this.Range = new TileObjectRange(config.Range, data);
            else this.Range = new TileObjectRange();

            this.HPModifier = DataParser.OptionalInt_Any(data, config.HPModifier, "HP Modifier");

            this.CombatStatModifiers = new Dictionary<string, int>();
            foreach (NamedStatConfig stat in config.CombatStatModifiers)
            {
                int val = DataParser.Int_Any(data, stat.Value, stat.SourceName + " Modifier");
                if (val == 0) continue;

                this.CombatStatModifiers.Add(stat.SourceName, val);
            }


            this.StatModifiers = new Dictionary<string, int>();
            foreach (NamedStatConfig stat in config.StatModifiers)
            {
                int val = DataParser.Int_Any(data, stat.Value, stat.SourceName + " Modifier");
                if (val == 0) continue;

                this.StatModifiers.Add(stat.SourceName, val);
            }
        }

        /// <summary>
        /// Converts the string value of <paramref name="layer"/> into the corresponding <c>TerrainObjectLayer</c> value.
        /// </summary>
        /// <exception cref="UnmatchedTileObjectLayerException"></exception>
        private TileObjectLayer GetTerrainObjectLayerEnum(string layer)
        {
            if (string.IsNullOrEmpty(layer))
                return TileObjectLayer.Below;

            object layerEnum;
            if (!Enum.TryParse(typeof(TileObjectLayer), layer, out layerEnum))
                throw new UnmatchedTileObjectLayerException(layer);

            return (TileObjectLayer)layerEnum;
        }

        #region Static Functions

        public static IDictionary<string, TileObject> BuildDictionary(TileObjectsConfig config)
        {
            IDictionary<string, TileObject> tileObjects = new Dictionary<string, TileObject>();

            foreach (List<object> row in config.Query.Data)
            {
                try
                {
                    List<string> tileObj = row.Select(r => r.ToString()).ToList();
                    string name = DataParser.OptionalString(tileObj, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!tileObjects.TryAdd(name, new TileObject(config, tileObj)))
                        throw new NonUniqueObjectNameException("tile object");
                }
                catch (Exception ex)
                {
                    throw new TileObjectProcessingException((row.ElementAtOrDefault(config.Name) ?? string.Empty).ToString(), ex);
                }
            }

            return tileObjects;
        }

        #endregion
    }

    //Note: enum equals (=) value is important for calculating the z-index of the sprite render
    public enum TileObjectLayer
    {
        Below = 0,
        Above = 1
    }
}
