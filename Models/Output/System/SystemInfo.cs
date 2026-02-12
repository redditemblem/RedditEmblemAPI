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
        public IDictionary<string, ITerrainType> TerrainTypes { get; set; }

        /// <summary>
        /// Container dictionary for data about classes.
        /// </summary>
        public IDictionary<string, IClass> Classes { get; set; }

        /// <summary>
        /// Container dictionary for data about affiliations.
        /// </summary>
        public IDictionary<string, IAffiliation> Affiliations { get; set; }

        /// <summary>
        /// Container dictionary for data about items.
        /// </summary>
        public IDictionary<string, IItem> Items { get; set; }

        #endregion

        #region Optional Data

        /// <summary>
        /// Container dictionary for data about tile objects.
        /// </summary>
        public IDictionary<string, ITileObject> TileObjects { get; set; }

        /// <summary>
        /// Container dictionary for data about skills.
        /// </summary>
        public IDictionary<string, ISkill> Skills { get; set; }

        /// <summary>
        /// Container dictionary for data about status conditions.
        /// </summary>
        public IDictionary<string, IStatusCondition> StatusConditions { get; set; }

        /// <summary>
        /// Container dictionary for data about tags.
        /// </summary>
        public IDictionary<string, ITag> Tags { get; set; }

        /// <summary>
        /// Container list for data about weapon rank bonuses.
        /// </summary>
        [JsonIgnore]
        public List<IWeaponRankBonus> WeaponRankBonuses { get; set; }

        /// <summary>
        /// Container dictionary for data about engravings.
        /// </summary>
        public IDictionary<string, IEngraving> Engravings { get; set; }

        /// <summary>
        /// Container dictionary for data about combat arts.
        /// </summary>
        public IDictionary<string, ICombatArt> CombatArts { get; set; }

        /// <summary>
        /// Container dictionary for data about battalions.
        /// </summary>
        public IDictionary<string, IBattalion> Battalions { get; set; }

        /// <summary>
        /// Container dictionary for data about gambits.
        /// </summary>
        public IDictionary<string, IGambit> Gambits { get; set; }

        /// <summary>
        /// Container dictionary for data about adjutants.
        /// </summary>
        public IDictionary<string, IAdjutant> Adjutants { get; set; }

        /// <summary>
        /// Container dictionary for data about battle styles.
        /// </summary>
        public IDictionary<string, IBattleStyle> BattleStyles { get; set; }

        /// <summary>
        /// Container dictionary for data about emblems.
        /// </summary>
        public IDictionary<string, IEmblem> Emblems { get; set; }

        /// <summary>
        /// Container dictionary for data about emblem engage attacks.
        /// </summary>
        public IDictionary<string, IEngageAttack> EngageAttacks { get; set; }


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

        public void RemoveUnusedObjects()
        {
            //REQUIRED
            CullDictionary(this.TerrainTypes);
            CullDictionary(this.Affiliations);
            CullDictionary(this.Items);

            //OPTIONAL
            CullDictionary(this.TileObjects);
            CullDictionary(this.Classes);
            CullDictionary(this.Skills);
            CullDictionary(this.StatusConditions);
            CullDictionary(this.Tags);
            CullDictionary(this.CombatArts);
            CullDictionary(this.Battalions);
            CullDictionary(this.Gambits);
            CullDictionary(this.Engravings);
            CullDictionary(this.BattleStyles);
            CullDictionary(this.Emblems);
            CullDictionary(this.EngageAttacks);
            CullDictionary(this.Adjutants);
        }

        private void CullDictionary<T>(IDictionary<string, T> dictionary) where T : IMatchable
        { 
            foreach (string key in dictionary.Keys)
                if (!dictionary[key].Matched)
                    dictionary.Remove(key);
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
            //Lots of things are dependent on tags, so I'm putting it first.
            this.Tags = Tag.BuildDictionary(config.Tags);

            //Non-dependent objects
            this.TileObjects = TileObject.BuildDictionary(config.TileObjects);
            this.BattleStyles = BattleStyle.BuildDictionary(config.BattleStyles);
            this.Skills = Skill.BuildDictionary(config.Skills);
            this.StatusConditions = StatusCondition.BuildDictionary(config.StatusConditions);
            this.WeaponRankBonuses = WeaponRankBonus.BuildList(config.WeaponRankBonuses);
            this.Engravings = Engraving.BuildDictionary(config.Engravings, this.Tags);
            this.Gambits = Gambit.BuildDictionary(config.Gambits);
            this.Emblems = Emblem.BuildDictionary(config.Emblems);
            this.EngageAttacks = EngageAttack.BuildDictionary(config.EngageAttacks);
            this.Adjutants = Adjutant.BuildDictionary(config.Adjutants);

            //Dependent objects
            this.Classes = Class.BuildDictionary(config.Classes, isUnitMovementTypeConfigured, this.BattleStyles);
            this.CombatArts = CombatArt.BuildDictionary(config.CombatArts, this.Tags);
            this.Battalions = Battalion.BuildDictionary(config.Battalions, this.Gambits);
        }

        #endregion
    }
}
