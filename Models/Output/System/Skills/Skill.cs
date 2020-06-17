using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Skills;
using RedditEmblemAPI.Models.Output.System.Skills.Effects;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills
{
    /// <summary>
    /// Object representing a Skill definition in the team's system. 
    /// </summary>
    public class Skill
    {
        /// <summary>
        /// Flag indicating whether or not this skill was found on a unit. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; set; }

        /// <summary>
        /// The name of the skill.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The sprite image URL for the skill.
        /// </summary>
        public string SpriteURL { get; set; }

        /// <summary>
        /// List of the skill's text fields.
        /// </summary>
        public IList<string> TextFields { get; set; }

        /// <summary>
        /// The effect the skill applies, if any.
        /// </summary>
        [JsonIgnore]
        public ISkillEffect Effect { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Skill(SkillsConfig config, IList<string> data)
        {
            this.Name = (data.ElementAtOrDefault<string>(config.Name) ?? string.Empty).Trim();
            this.SpriteURL = (data.ElementAtOrDefault<string>(config.SpriteURL) ?? string.Empty).Trim();
            this.TextFields = ParseHelper.StringListParse(data, config.TextFields);
            this.Effect = BuildSkillEffect((data.ElementAtOrDefault<string>(config.Effect.Type) ?? string.Empty).Trim(),
                                           (data.ElementAtOrDefault<string>(config.Effect.Parameter1) ?? string.Empty).Trim(),
                                           (data.ElementAtOrDefault<string>(config.Effect.Parameter2) ?? string.Empty).Trim() );
        }

        private ISkillEffect BuildSkillEffect(string effectType, string param1, string param2)
        {
            switch (effectType)
            {
                case "FlatUnitStatModifer": return new FlatUnitStatModiferEffect(param1, param2);
            }

            return null;
        }
    }
}
