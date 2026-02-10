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
    #region Interface

    /// <inheritdoc cref="TileObject"/>
    public interface ITileObject : IMatchable
    {
        /// <inheritdoc cref="TileObject.SpriteURL"/>
        string SpriteURL { get; set; }

        /// <inheritdoc cref="TileObject.Size"/>
        int Size { get; set; }

        /// <inheritdoc cref="TileObject.Layer"/>
        TileObjectLayer Layer { get; set; }

        /// <inheritdoc cref="TileObject.Range"/>
        ITileObjectRange Range { get; set; }

        /// <inheritdoc cref="TileObject.HPModifier"/>
        int HPModifier { get; set; }

        /// <inheritdoc cref="TileObject.CombatStatModifiers"/>
        IDictionary<string, int> CombatStatModifiers { get; set; }

        /// <inheritdoc cref="TileObject.StatModifiers"/>
        IDictionary<string, int> StatModifiers { get; set; }

        /// <inheritdoc cref="TileObject.TextFields"/>
        List<string> TextFields { get; set; }
    }

    #endregion Interface

    public class TileObject : Matchable, ITileObject
    {
        #region Attributes

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
        public ITileObjectRange Range { get; set; }

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

        #endregion Attributes

        public TileObject(TileObjectsConfig config, IEnumerable<string> data)
        {
            this.Name = DataParser.String(data, config.Name, "Name");
            this.SpriteURL = DataParser.String_URL(data, config.SpriteURL, "Sprite URL");
            this.Size = DataParser.OptionalInt_NonZeroPositive(data, config.Size, "Size");
            this.Layer = GetTerrainObjectLayerEnum(data.ElementAtOrDefault<string>(config.Layer));
            this.TextFields = DataParser.List_Strings(data, config.TextFields);

            if (config.Range != null) this.Range = new TileObjectRange(config.Range, data);
            else this.Range = new TileObjectRange();

            this.HPModifier = DataParser.OptionalInt_Any(data, config.HPModifier, "HP Modifier");
            this.CombatStatModifiers = DataParser.NamedStatDictionary_OptionalInt_Any(config.CombatStatModifiers, data, false, "{0} Modifier");
            this.StatModifiers = DataParser.NamedStatDictionary_OptionalInt_Any(config.StatModifiers, data, false, "{0} Modifier");
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

        /// <summary>
        /// Iterates through the data in <paramref name="config"/>'s <c>Query</c> and builds an <c>ITileObject</c> from each valid row.
        /// </summary>
        /// <exception cref="TileObjectProcessingException"></exception>
        public static IDictionary<string, ITileObject> BuildDictionary(TileObjectsConfig config)
        {
            IDictionary<string, ITileObject> tileObjects = new Dictionary<string, ITileObject>();
            if (config == null || config.Queries == null)
                return tileObjects;

            foreach (List<object> row in config.Queries.SelectMany(q => q.Data))
            {
                string name = string.Empty;
                try
                {
                    IEnumerable<string> tileObj = row.Select(r => r.ToString());
                    name = DataParser.OptionalString(tileObj, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!tileObjects.TryAdd(name, new TileObject(config, tileObj)))
                        throw new NonUniqueObjectNameException("tile object");
                }
                catch (Exception ex)
                {
                    throw new TileObjectProcessingException(name, ex);
                }
            }

            return tileObjects;
        }

        /// <summary>
        /// Matches each of the strings in <paramref name="names"/> to an <c>ITileObject</c> in <paramref name="tileObjects"/> and returns the matches as a list.
        /// </summary>
        /// <param name="flagAsMatched">If true, calls <c>IMatchable.FlagAsMatched()</c> for all returned objects.</param>
        public static List<ITileObject> MatchNames(IDictionary<string, ITileObject> tileObjects, IEnumerable<string> names, Coordinate coord, bool flagAsMatched = true)
        {
            return names.Select(n => MatchName(tileObjects, n, coord, flagAsMatched)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to an <c>ITileObject</c> in <paramref name="tileObjects"/> and returns it.
        /// </summary>
        /// <param name="flagAsMatched">If true, calls <c>IMatchable.FlagAsMatched()</c> for the returned object.</param>
        /// <exception cref="UnmatchedTileObjectException"></exception>
        public static ITileObject MatchName(IDictionary<string, ITileObject> tileObjects, string name, Coordinate coord, bool flagAsMatched = false)
        {
            ITileObject match;
            if (!tileObjects.TryGetValue(name, out match))
                throw new UnmatchedTileObjectException(coord, name);

            if (flagAsMatched) match.FlagAsMatched();

            return match;
        }

        #endregion Static Functions
    }

    //Note: enum equals (=) value is important for calculating the z-index of the sprite render
    public enum TileObjectLayer
    {
        Below = 0,
        Above = 1
    }
}
