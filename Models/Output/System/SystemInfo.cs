using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System;
using RedditEmblemAPI.Models.Output.System.Interfaces;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Models.Output.System.StatusConditions;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System
{
    public class SystemInfo
    {
        #region Constants

        /// <summary>
        /// Container for system constants.
        /// </summary>
        public SystemConstantsConfig Constants { get; set; }

        /// <summary>
        /// Container for interface label constants.
        /// </summary>
        public InterfaceLabelsConfig InterfaceLabels { get; set; }

        #endregion Constants

        #region Required Data

        /// <summary>
        /// Container dictionary for data about terrain types.
        /// </summary>
        [JsonConverter(typeof(OmitUnmatchedObjectsFromIMatchableDictionaryConverter))]
        public IReadOnlyDictionary<string, TerrainType> TerrainTypes { get; set; }

        /// <summary>
        /// Container dictionary for data about classes.
        /// </summary>
        [JsonConverter(typeof(OmitUnmatchedObjectsFromIMatchableDictionaryConverter))]
        public IReadOnlyDictionary<string, Class> Classes { get; set; }

        /// <summary>
        /// Container dictionary for data about affiliations.
        /// </summary>
        [JsonConverter(typeof(OmitUnmatchedObjectsFromIMatchableDictionaryConverter))]
        public IReadOnlyDictionary<string, Affiliation> Affiliations { get; set; }

        /// <summary>
        /// Container dictionary for data about items.
        /// </summary>
        [JsonConverter(typeof(OmitUnmatchedObjectsFromIMatchableDictionaryConverter))]
        public IReadOnlyDictionary<string, Item> Items { get; set; }

        #endregion

        #region Optional Data

        /// <summary>
        /// Container dictionary for data about tile objects.
        /// </summary>
        [JsonConverter(typeof(OmitUnmatchedObjectsFromIMatchableDictionaryConverter))]
        public IReadOnlyDictionary<string, TileObject> TileObjects { get; set; }

        /// <summary>
        /// Container dictionary for data about skills.
        /// </summary>
        [JsonConverter(typeof(OmitUnmatchedObjectsFromIMatchableDictionaryConverter))]
        public IReadOnlyDictionary<string, Skill> Skills { get; set; }

        /// <summary>
        /// Container dictionary for data about status conditions.
        /// </summary>
        [JsonConverter(typeof(OmitUnmatchedObjectsFromIMatchableDictionaryConverter))]
        public IReadOnlyDictionary<string, StatusCondition> StatusConditions { get; set; }

        /// <summary>
        /// Container dictionary for data about tags.
        /// </summary>
        [JsonConverter(typeof(OmitUnmatchedObjectsFromIMatchableDictionaryConverter))]
        public IReadOnlyDictionary<string, Tag> Tags { get; set; }

        /// <summary>
        /// Container list for data about weapon rank bonuses.
        /// </summary>
        [JsonIgnore]
        public IReadOnlyCollection<WeaponRankBonus> WeaponRankBonuses { get; set; }

        /// <summary>
        /// Container dictionary for data about engravings.
        /// </summary>
        [JsonConverter(typeof(OmitUnmatchedObjectsFromIMatchableDictionaryConverter))]
        public IReadOnlyDictionary<string, Engraving> Engravings { get; set; }

        /// <summary>
        /// Container dictionary for data about combat arts.
        /// </summary>
        [JsonConverter(typeof(OmitUnmatchedObjectsFromIMatchableDictionaryConverter))]
        public IReadOnlyDictionary<string, CombatArt> CombatArts { get; set; }

        /// <summary>
        /// Container dictionary for data about battalions.
        /// </summary>
        [JsonConverter(typeof(OmitUnmatchedObjectsFromIMatchableDictionaryConverter))]
        public IReadOnlyDictionary<string, Battalion> Battalions { get; set; }

        /// <summary>
        /// Container dictionary for data about gambits.
        /// </summary>
        [JsonConverter(typeof(OmitUnmatchedObjectsFromIMatchableDictionaryConverter))]
        public IReadOnlyDictionary<string, Gambit> Gambits { get; set; }

        /// <summary>
        /// Container dictionary for data about adjutants.
        /// </summary>
        [JsonConverter(typeof(OmitUnmatchedObjectsFromIMatchableDictionaryConverter))]
        public IReadOnlyDictionary<string, Adjutant> Adjutants { get; set; }

        /// <summary>
        /// Container dictionary for data about battle styles.
        /// </summary>
        [JsonConverter(typeof(OmitUnmatchedObjectsFromIMatchableDictionaryConverter))]
        public IReadOnlyDictionary<string, BattleStyle> BattleStyles { get; set; }

        /// <summary>
        /// Container dictionary for data about emblems.
        /// </summary>
        [JsonConverter(typeof(OmitUnmatchedObjectsFromIMatchableDictionaryConverter))]
        public IReadOnlyDictionary<string, Emblem> Emblems { get; set; }

        /// <summary>
        /// Container dictionary for data about emblem engage attacks.
        /// </summary>
        [JsonConverter(typeof(OmitUnmatchedObjectsFromIMatchableDictionaryConverter))]
        public IReadOnlyDictionary<string, EngageAttack> EngageAttacks { get; set; }

        #endregion Optional Data

        /// <summary>
        /// Constructor.
        /// </summary>
        public SystemInfo(SystemConfig config, bool isUnitMovementTypeConfigured)
        {
            //Copy over constants from config
            this.Constants = config.Constants;
            this.InterfaceLabels = config.InterfaceLabels;

            ParseOptionalData(config, isUnitMovementTypeConfigured);
            ParseRequiredData(config); //some required data reliant on optional data
        }

        #region Parsers

        /// <summary>
        /// Helper function for the constructor. Parses data into dictionaries.
        /// </summary>
        /// <param name="config"></param>
        private void ParseRequiredData(SystemConfig config)
        {
            this.Affiliations = Affiliation.BuildDictionary(config.Affiliations);
            this.TerrainTypes = TerrainType.BuildDictionary(config.TerrainTypes, this.Affiliations);
            this.Items = Item.BuildDictionary(config.Items, this.Skills, this.Tags, this.Engravings);
        }

        /// <summary>
        /// Helper function for the constructor. Parses data into dictionaries.
        /// </summary>
        private void ParseOptionalData(SystemConfig config, bool isUnitMovementTypeConfigured)
        {
            //Non-dependent objects
            this.TileObjects = TileObject.BuildDictionary(config.TileObjects);
            this.BattleStyles = BattleStyle.BuildDictionary(config.BattleStyles);
            this.Skills = Skill.BuildDictionary(config.Skills);
            this.StatusConditions = StatusCondition.BuildDictionary(config.StatusConditions);
            this.WeaponRankBonuses = WeaponRankBonus.BuildList(config.WeaponRankBonuses);
            this.Gambits = Gambit.BuildDictionary(config.Gambits);
            this.Emblems = Emblem.BuildDictionary(config.Emblems);
            this.EngageAttacks = EngageAttack.BuildDictionary(config.EngageAttacks);
            this.Adjutants = Adjutant.BuildDictionary(config.Adjutants);

            //Dependent objects
            this.Tags = Tag.BuildDictionary(config.Tags); //Lots of things are dependent on tags, so do it first

            this.Classes = Class.BuildDictionary(config.Classes, isUnitMovementTypeConfigured, this.BattleStyles);
            this.CombatArts = CombatArt.BuildDictionary(config.CombatArts, this.Tags);
            this.Battalions = Battalion.BuildDictionary(config.Battalions, this.Gambits);
            this.Engravings = Engraving.BuildDictionary(config.Engravings, this.Tags);
        }

        #endregion
    }
}
