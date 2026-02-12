using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange;
using RedditEmblemAPI.Models.Output.System.StatusConditions;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RedditEmblemAPI.Models.Output.Units
{
    /// <summary>
    /// Object representing a single Unit.
    /// </summary>
    public partial class Unit
    {
        #region Attributes

        /// <summary>
        /// The unit's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The set of digits present at the end of the unit's <c>Name</c>, if any.
        /// </summary>
        public string UnitNumber { get; set; }

        /// <summary>
        /// The player that controls the unit.
        /// </summary>
        public string Player { get; set; }

        /// <summary>
        /// The unit's character application URL link.
        /// </summary>
        public string CharacterApplicationURL { get; set; }

        /// <summary>
        /// List of the unit's text fields.
        /// </summary>
        public List<string> TextFields { get; set; }

        /// <summary>
        /// Container for information about rendering a unit.
        /// </summary>
        public UnitSpriteData Sprite { get; set; }

        /// <summary>
        /// Container for information about the unit's location on the map.
        /// </summary>
        public UnitLocationData Location { get; set; }

        /// <summary>
        /// A list of the unit's classes.
        /// </summary>
        [JsonIgnore]
        public List<IClass> ClassList { get; set; }

        /// <summary>
        /// The unit's movement type. Only used if classes are not provided.
        /// </summary>
        private string UnitMovementType { get; set; }

        /// <summary>
        /// The unit's affiliation.
        /// </summary>
        [JsonIgnore]
        public IAffiliation AffiliationObj { get; set; }

        /// <summary>
        /// Container for information about the unit's raw numbers.
        /// </summary>
        public UnitStatsData Stats { get; set; }

        /// <summary>
        /// List of the unit's tags.
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// Description of how the unit behaves.
        /// </summary>
        public string Behavior { get; set; }

        /// <summary>
        /// List of the status conditions the unit possesses.
        /// </summary>
        public List<UnitStatus> StatusConditions { get; set; }

        /// <summary>
        /// List of the skill subsections the unit possesses.
        /// </summary>
        public List<UnitSkillSubsection> SkillSubsections { get; set; }

        /// <summary>
        /// Container for information about a unit's movement/item ranges.
        /// </summary>
        public UnitRangeData Ranges { get; set; }

        #region JSON Serialization Only

        /// <summary>
        /// The unit's name, with special characters swapped for alphabetical ones to help with searching.
        /// </summary>
        [JsonProperty]
        private string NormalizedName { get; set; }

        /// <summary>
        /// The unit's movement type
        /// </summary>
        [JsonProperty]
        private string MovementType { get { return GetUnitMovementType(); } }

        /// <summary>
        /// Only for JSON serialization. A list of the unit's classes.
        /// </summary>
        [JsonProperty]
        private IEnumerable<string> Classes { get { return this.ClassList.Select(c => c.Name); } }

        /// <summary>
        /// Only for JSON serialization. The unit's affiliation name.
        /// </summary>
        [JsonProperty]
        private string Affiliation { get { return AffiliationObj.Name; } }

        /// <summary>
        /// Only for JSON serialization. True if the unit has any skill in any subsection. Used to control whether or not the skill section displays.
        /// </summary>
        [JsonProperty]
        private bool HasSkills { get { return this.SkillSubsections.Any(s => s.Skills.Any()); } }


        #endregion JSON Serialization Only

        #endregion Attributes

        #region Constants

        private static Regex nonSpacingMarkRegex = new Regex(@"\p{Mn}");
        private static Regex unitNumberRegex = new Regex(@"\s([0-9]+$)"); //matches digits at the end of a string (ex. "Swordmaster _05_")

        #endregion

        /// <summary>
        /// Main constructor.
        /// </summary>
        public Unit(UnitsConfig config, IEnumerable<string> data, SystemInfo system)
        {
            //Basic fields
            this.Name = DataParser.String(data, config.Name, "Name");
            this.NormalizedName = RemoveDiacritics(this.Name);
            this.UnitNumber = ExtractUnitNumberFromName(this.Name);
            this.Player = DataParser.OptionalString(data, config.Player, "Player");
            this.CharacterApplicationURL = DataParser.OptionalString_URL(data, config.CharacterApplicationURL, "Character Application URL");
            this.TextFields = DataParser.List_Strings(data, config.TextFields);
            this.Tags = DataParser.List_StringCSV(data, config.Tags).Distinct().ToList();
            this.Behavior = DataParser.OptionalString(data, config.Behavior, "Behavior");

            //Complex container objects
            this.Sprite = new UnitSpriteData(config, data);
            this.Location = new UnitLocationData(config, data);
            this.Stats = new UnitStatsData(config, data);
            this.Ranges = new UnitRangeData();

            string affiliation = DataParser.String(data, config.Affiliation, "Affiliation");
            this.AffiliationObj = System.Affiliation.MatchName(system.Affiliations, affiliation);

            //Classes and movement types
            //The unit movement type field itself is optional, but if it is present it must be populated
            this.ClassList = BuildClasses(data, config.Classes, system.Classes);
            if (config.MovementType > -1) this.UnitMovementType = DataParser.String(data, config.MovementType, "Movement Type");
            else this.UnitMovementType = string.Empty;

            MatchTags(system.Tags);

            this.SkillSubsections =  UnitSkillSubsection.BuildList(data, config.SkillSubsections, system.Skills);
            this.StatusConditions = BuildUnitStatusConditions(data, config.StatusConditions, system.StatusConditions);

            Constructor_Unit_3H(config, data, system);
            Constructor_Unit_Engage(config, data, system);
            Constructor_Unit_Inventory(config, data, system);
        }

        #region Build Functions

        /// <summary>
        /// Builds and returns a list of the unit's status conditions.
        /// </summary>
        private List<UnitStatus> BuildUnitStatusConditions(IEnumerable<string> data, List<UnitStatusConditionConfig> configs, IDictionary<string, IStatusCondition> statuses)
        {
            List<UnitStatus> statusConditions = new List<UnitStatus>();
            foreach (UnitStatusConditionConfig config in configs)
            {
                string name = DataParser.OptionalString(data, config.Name, "Status Condition Name");
                if (string.IsNullOrEmpty(name)) continue;

                statusConditions.Add(new UnitStatus(data, config, statuses));
            }

            return statusConditions;
        }

        /// <summary>
        /// Iterates through the values in <paramref name="data"/> at <paramref name="indexes"/> and attempts to match them to a <c>IClass</c> from <paramref name="classes"/>.
        /// </summary>
        /// /// <remarks>
        /// Depends on the following being built beforehand:
        /// <list type="bullet">
        /// <item>Tags</item>
        /// </list>
        /// </remarks>
        /// <exception cref="UnmatchedClassException"></exception>
        private List<IClass> BuildClasses(IEnumerable<string> data, List<int> indexes, IDictionary<string, IClass> classes)
        {
            List<IClass> unitClasses = new List<IClass>();

            foreach (int index in indexes)
            {
                string name = DataParser.OptionalString(data, index, "Class Name");
                if (string.IsNullOrEmpty(name))
                    continue;

                IClass match = Class.MatchName(classes, name);
                unitClasses.Add(match);

                //Append class tags to unit's tags
                this.Tags = this.Tags.Union(match.Tags).Distinct().ToList();
            }

            //If we have class fields configured, error if we found no values
            if (indexes.Count > 0 && !unitClasses.Any())
                throw new Exception("Unit must have at least one class defined.");

            return unitClasses;
        }

        #endregion Build Functions

        #region Match Functions

        /// <summary>
        /// Dependent on <c>this.Tags</c> already being built. Iterates through the values in <c>this.Tags</c> and attempts to match them a <c>Tag</c> from <paramref name="tags"/>.
        /// </summary>
        private void MatchTags(IDictionary<string, ITag> tags)
        {
            if (!tags.Any()) return;

            List<ITag> matched = Tag.MatchNames(tags, this.Tags);
            
            //Apply the unit aura from the first valid tag encountered
            if(string.IsNullOrEmpty(this.Sprite.Aura))
            {
                ITag aura = matched.FirstOrDefault(t => !string.IsNullOrEmpty(t.UnitAura));
                if(aura != null)
                    this.Sprite.Aura = aura.UnitAura;
            }
        }

        #endregion Match Functions

        private string RemoveDiacritics(string name)
        {
            string normalizedText = name.Normalize(NormalizationForm.FormD);
            return nonSpacingMarkRegex.Replace(normalizedText, string.Empty);
        }

        /// <summary>
        /// Regex evaluates <paramref name="name"/> to extract the unit number. Returns it if one is found, else returns an empty string.
        /// </summary>
        private string ExtractUnitNumberFromName(string name)
        {
            Match numberMatch = unitNumberRegex.Match(name);
            if (numberMatch.Success)
                return numberMatch.Value.Trim();
            return string.Empty;
        }

        /// <summary>
        /// Returns the unit's movement type, and accounts for things like effects and classes.
        /// </summary>
        public string GetUnitMovementType()
        {
            OverrideMovementTypeEffect_Status statusOverride = this.StatusConditions.SelectMany(s => s.StatusObj.Effects).OfType<OverrideMovementTypeEffect_Status>().FirstOrDefault();
            if (statusOverride != null)
                return statusOverride.MovementType;

            OverrideMovementTypeEffect_Skill skillOverride = this.GetFullSkillsList().SelectMany(s => s.Effects).OfType<OverrideMovementTypeEffect_Skill>().FirstOrDefault();
            if(skillOverride != null)
                return skillOverride.MovementType;

            // Try to retrieve the first class's movement type first.
            // If there is none, then we fall back on trying to grab from the unit's movement type field instead.
            string movementType = this.ClassList.FirstOrDefault()?.MovementType;
            if (string.IsNullOrEmpty(movementType)) movementType = this.UnitMovementType;

            return movementType;
        }

        /// <summary>
        /// Returns complete list of skills on the unit, including skills from things like the equipped item or emblems, ignoring subsection organization.
        /// </summary>
        public IEnumerable<ISkill> GetFullSkillsList()
        {
            IEnumerable<ISkill> skills = this.SkillSubsections.SelectMany(s => s.Skills.Select(s => s.SkillObj));

            //Union w/ equipped item skills
            List<UnitInventoryItem> equipped = this.Inventory.GetAllEquippedItems();
            foreach(UnitInventoryItem unitItem in equipped)
                skills = skills.Union(unitItem.Item.EquippedSkills.Select(s => s.SkillObj));
            
            //Union w/ emblem skills
            if(this.Emblem != null)
            {
                skills = skills.Union(this.Emblem.SyncSkills.Select(s => s.SkillObj));
                if (this.Emblem.IsEngaged)
                    skills = skills.Union(this.Emblem.EngageSkills.Select(s => s.SkillObj));
            }

            return skills;
        }

        #region Static Functions

        /// <summary>
        /// Iterates through the data in <paramref name="config"/>'s <c>Query</c> and builds a <c>Unit</c> from each valid row.
        /// </summary>
        /// <exception cref="UnitProcessingException"></exception>
        public static List<Unit> BuildList(UnitsConfig config, SystemInfo system)
        {
            List<Unit> units = new List<Unit>();
            if (config == null || config.Queries == null)
                return units;

            //Create units
            foreach (List<object> row in config.Queries.SelectMany(q => q.Data))
            {
                string name = string.Empty;
                try
                {
                    //Convert objects to strings
                    IEnumerable<string> unit = row.Select(r => r.ToString());
                    name = DataParser.OptionalString(unit, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    if (units.Any(u => u.Name == name))
                        throw new NonUniqueObjectNameException("unit");

                    units.Add(new Unit(config, unit, system));
                }
                catch (Exception ex)
                {
                    throw new UnitProcessingException(name, ex);
                }
            }

            return units;
        }

        #endregion Static Functions
    }
}