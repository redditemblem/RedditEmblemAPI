using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.TileObjects;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.System.Interfaces;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System
{
    public class TileObject : IMatchable
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

        public TileObject(TileObjectsConfig config, IEnumerable<string> data)
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
            if (config == null || config.Query == null)
                return tileObjects;

            foreach (List<object> row in config.Query.Data)
            {
                try
                {
                    IEnumerable<string> tileObj = row.Select(r => r.ToString());
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

        /// <summary>
        /// Matches each of the strings in <paramref name="names"/> to a <c>TileObject</c> in <paramref name="tileObjects"/> and returns the matches as a list.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned objects to true.</param>
        public static List<TileObject> MatchNames(IDictionary<string, TileObject> tileObjects, IEnumerable<string> names, Coordinate coord, bool skipMatchedStatusSet = false)
        {
            return names.Select(n => MatchName(tileObjects, n, coord, skipMatchedStatusSet)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to a <c>TileObject</c> in <paramref name="tileObjects"/> and returns it.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned object to true.</param>
        /// <exception cref="UnmatchedTileObjectException"></exception>
        public static TileObject MatchName(IDictionary<string, TileObject> tileObjects, string name, Coordinate coord, bool skipMatchedStatusSet = false)
        {
            TileObject match;
            if (!tileObjects.TryGetValue(name, out match))
                throw new UnmatchedTileObjectException(coord, name);

            if (!skipMatchedStatusSet) match.Matched = true;

            return match;
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
