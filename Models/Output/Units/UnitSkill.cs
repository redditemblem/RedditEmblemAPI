using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.Units
{
    /// <summary>
    /// Object representing a <c>Skill</c> present on a <c>Unit</c>.
    /// </summary>
    public class UnitSkill
    {
        #region Attributes

        /// <summary>
        /// The full name of the skill pulled from raw <c>Unit</c> data.
        /// </summary>
        [JsonIgnore]
        public string FullName { get; set; }

        /// <summary>
        /// The <c>Skill</c> object.
        /// </summary>
        [JsonIgnore]
        public Skill SkillObj { get; set; }

        /// <summary>
        /// Dictionary of additional stat values for this skill.
        /// </summary>
        public IDictionary<string, int> AdditionalStats { get; set; }

        #region JSON Serialization

        /// <summary>
        /// Only for JSON serialization. The name of the skill. 
        /// </summary>
        [JsonProperty]
        private string Name { get { return this.SkillObj.Name; } }


        #endregion JSON Serialization

        #endregion Attributes

        /// <summary>
        /// Initalizes a new skill from a unit and matches it to a <c>Skill</c> in <paramref name="skills"/>.
        /// </summary>
        public UnitSkill(IEnumerable<string> data, UnitSkillConfig config, IDictionary<string, Skill> skills, bool skipMatchedStatusSet = false)
        {
            this.FullName = DataParser.String(data, config.Name, "Skill Name");
            this.AdditionalStats = DataParser.NamedStatDictionary_OptionalInt_Any(config.AdditionalStats, data, false, this.FullName + " {0}");

            this.SkillObj = Skill.MatchName(skills, this.FullName.Trim(), skipMatchedStatusSet);
        }
    }
}
