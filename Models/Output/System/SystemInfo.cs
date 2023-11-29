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
        public IDictionary<string, TerrainType> TerrainTypes { get; set; }

        /// <summary>
        /// Container dictionary for data about classes.
        /// </summary>
        public IDictionary<string, Class> Classes { get; set; }

        /// <summary>
        /// Container dictionary for data about affiliations.
        /// </summary>
        public IDictionary<string, Affiliation> Affiliations { get; set; }

        /// <summary>
        /// Container dictionary for data about items.
        /// </summary>
        public IDictionary<string, Item> Items { get; set; }

        #endregion

        #region Optional Data

        /// <summary>
        /// Container dictionary for data about tile objects.
        /// </summary>
        public IDictionary<string, TileObject> TileObjects { get; set; }

        /// <summary>
        /// Container dictionary for data about skills.
        /// </summary>
        public IDictionary<string, Skill> Skills { get; set; }

        /// <summary>
        /// Container dictionary for data about status conditions.
        /// </summary>
        public IDictionary<string, StatusCondition> StatusConditions { get; set; }

        /// <summary>
        /// Container dictionary for data about tags.
        /// </summary>
        public IDictionary<string, Tag> Tags { get; set; }

        /// <summary>
        /// Container list for data about weapon rank bonuses.
        /// </summary>
        [JsonIgnore]
        public List<WeaponRankBonus> WeaponRankBonuses { get; set; }

        /// <summary>
        /// Container dictionary for data about engravings.
        /// </summary>
        public IDictionary<string, Engraving> Engravings { get; set; }

        /// <summary>
        /// Container dictionary for data about combat arts.
        /// </summary>
        public IDictionary<string, CombatArt> CombatArts { get; set; }

        /// <summary>
        /// Container dictionary for data about battalions.
        /// </summary>
        public IDictionary<string, Battalion> Battalions { get; set; }

        /// <summary>
        /// Container dictionary for data about gambits.
        /// </summary>
        public IDictionary<string, Gambit> Gambits { get; set; }

        /// <summary>
        /// Container dictionary for data about adjutants.
        /// </summary>
        public IDictionary<string, Adjutant> Adjutants { get; set; }

        /// <summary>
        /// Container dictionary for data about battle styles.
        /// </summary>
        public IDictionary<string, BattleStyle> BattleStyles { get; set; }

        /// <summary>
        /// Container dictionary for data about emblems.
        /// </summary>
        public IDictionary<string, Emblem> Emblems { get; set; }

        /// <summary>
        /// Container dictionary for data about emblem engage attacks.
        /// </summary>
        public IDictionary<string, EngageAttack> EngageAttacks { get; set; }


        #endregion Optional Data

        /// <summary>
        /// Constructor.
        /// </summary>
        public SystemInfo(SystemConfig config)
        {
            //Copy over constants from config
            this.Constants = config.Constants;
            this.InterfaceLabels = config.InterfaceLabels;

            ParseOptionalData(config);
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
        private void ParseOptionalData(SystemConfig config)
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
            this.Classes = Class.BuildDictionary(config.Classes, this.BattleStyles);
            this.CombatArts = CombatArt.BuildDictionary(config.CombatArts, this.Tags);
            this.Battalions = Battalion.BuildDictionary(config.Battalions, this.Gambits);
        }

        #endregion
    }
}
