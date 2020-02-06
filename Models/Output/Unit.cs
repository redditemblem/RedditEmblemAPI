﻿using RedditEmblemAPI.Models.Common;
using System.Collections.Generic;

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
            this.Tags = new List<string>();
            this.Stats = new Dictionary<string, ModifiedStatValue>();
            this.Inventory = new List<Item>();
            this.Skills = new List<Skill>();
        }

        /// <summary>
        /// The unit's name.
        /// </summary>
        public string Name { get; set; }

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
        /// The unit's class.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// The unit's affiliation.
        /// </summary>
        public string Affiliation { get; set; }

        /// <summary>
        /// The unit's earned experience.
        /// </summary>
        public int Experience { get; set; }

        /// <summary>
        /// Container object for HP values.
        /// </summary>
        public HP HP { get; set; }

        /// <summary>
        /// List of the unit's tags.
        /// </summary>
        public IList<string> Tags { get; set; }

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
    }
}
