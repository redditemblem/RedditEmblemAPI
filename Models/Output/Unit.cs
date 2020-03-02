using Newtonsoft.Json;
using RedditEmblemAPI.Models.Common;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output
{
    /// <summary>
    /// Object representing a single Unit.
    /// </summary>
    public class Unit
    {
        public Unit()
        {
            this.TextFields = new List<string>();
            this.ClassList = new List<Class>();
            this.Tags = new List<string>();
            this.CalculatedStats = new Dictionary<string, int>();
            this.Stats = new Dictionary<string, ModifiedStatValue>();
            this.Inventory = new List<Item>();
            this.Skills = new List<Skill>();
            this.MovementRange = new List<Coordinate>();
            this.AttackRange = new List<Coordinate>();
            this.UtilityRange = new List<Coordinate>();
        }

        /// <summary>
        /// The unit's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The set of digits present at the end of the unit's <c>Name</c>, if any.
        /// </summary>
        public string UnitNumber { get; set; }

        /// <summary>
        /// The sprite image URL for the unit.
        /// </summary>
        public string SpriteURL { get; set; }

        /// <summary>
        /// List of the unit's text fields.
        /// </summary>
        public IList<string> TextFields { get; set; }

        /// <summary>
        /// The unit's location on the map.
        /// </summary>
        public Coordinate Coordinates { get; set; }

        /// <summary>
        /// The unit's current level.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// A list of the unit's classes.
        /// </summary>
        [JsonIgnore]
        public IList<Class> ClassList { get; set; }

        /// <summary>
        /// Only for JSON serialization. A list of the unit's classes.
        /// </summary>
        [JsonProperty]
        private IList<string> Classes { get { return this.ClassList.Select(c => c.Name).ToList();  } }

        /// <summary>
        /// The unit's affiliation.
        /// </summary>
        public string Affiliation { get; set; }

        /// <summary>
        /// The unit's earned experience.
        /// </summary>
        public int Experience { get; set; }

        /// <summary>
        /// The amount of currency the unit has in their wallet.
        /// </summary>
        public int HeldCurrency { get; set; }

        /// <summary>
        /// Container object for HP values.
        /// </summary>
        public HP HP { get; set; }

        /// <summary>
        /// List of the unit's tags.
        /// </summary>
        public IList<string> Tags { get; set; }

        /// <summary>
        /// Collection of the unit's calculated combat stats.
        /// </summary>
        public Dictionary<string, int> CalculatedStats { get; set; }

        /// <summary>
        /// Collection of the unit's stat values.
        /// </summary>
        public Dictionary<string, ModifiedStatValue> Stats { get; set; }

        /// <summary>
        /// List of the items the unit is carrying.
        /// </summary>
        public IList<Item> Inventory { get; set; }

        /// <summary>
        /// List of the skills the unit possesses.
        /// </summary>
        public IList<Skill> Skills { get; set; }

        #region Movement_And_Range

        /// <summary>
        /// Flag indicating whether or not a unit's turn has been processed.
        /// </summary>
        public bool HasMoved { get; set; }

        /// <summary>
        /// The size of the unit in grid tiles. Defaults to 1.
        /// </summary>
        public int UnitSize { get; set; }

        /// <summary>
        /// The <c>Tile</c> that this unit is drawn at.
        /// </summary>
        [JsonIgnore]
        public Tile AnchorTile { get; set; }

        /// <summary>
        /// The <c>Tile</c> that this unit's range originates from.
        /// </summary>
        [JsonIgnore]
        public Tile OriginTile { get; set; }

        /// <summary>
        /// List of tiles that the unit is capable of moving to.
        /// </summary>
        public IList<Coordinate> MovementRange { get; set; }

        /// <summary>
        /// List of tiles that the unit is capable of attacking.
        /// </summary>
        public IList<Coordinate> AttackRange { get; set; }

        /// <summary>
        /// List of tiles that the unit is capable of using a utility item on.
        /// </summary>
        public IList<Coordinate> UtilityRange { get; set; }

        #endregion
    }
}
