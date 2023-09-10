using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Units
{
    //Partial class for handling mechanics from Fire Emblem: Engage.
    public partial class Unit
    {
        #region Attributes

        /// <summary>
        /// Container for information about a unit's emblem.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public UnitEmblem Emblem { get; set; }

        #region JSON Serialization

        /// <summary>
        /// Only for JSON serialization. The unit's battle style.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        private string BattleStyle
        {
            get { return this.ClassList.FirstOrDefault()?.BattleStyle?.Name; }
        }

        #endregion JSON Serialization

        #endregion Attributes

        /// <summary>
        /// Partial constructor. Builds unit emblem.
        /// </summary>
        /// <remarks>
        /// Depends on the following being built beforehand:
        /// <list type="bullet">
        /// <item>Sprite</item>
        /// </list>
        /// </remarks>
        public void Constructor_Unit_Engage(UnitsConfig config, IEnumerable<string> data, SystemInfo system)
        {
            this.Emblem = BuildUnitEmblem(data, config.Emblem, system);
        }

        #region Build Functions

        /// <summary>
        /// Builds and returns the unit's emblem.
        /// </summary>
        /// <remarks>
        /// Depends on the following being built beforehand:
        /// <list type="bullet">
        /// <item>Sprite</item>
        /// </list>
        /// </remarks>
        private UnitEmblem BuildUnitEmblem(IEnumerable<string> data, UnitEmblemConfig config, SystemInfo systemData)
        {
            if (config == null) return null;

            string name = DataParser.OptionalString(data, config.Name, "Emblem");
            if (string.IsNullOrEmpty(name)) return null;

            UnitEmblem emblem = new UnitEmblem(config, data, systemData);

            //Set unit aura
            if (emblem.IsEngaged && !string.IsNullOrEmpty(emblem.Emblem.EngagedUnitAura))
                this.Sprite.Aura = emblem.Emblem.EngagedUnitAura;

            return emblem;
        }


        #endregion Build Functions
    }
}
