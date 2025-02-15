using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.Units
{
    public class UnitSkillSubsection
    {
        #region Attributes

        /// <summary>
        /// List of all unit skills that belong to this subsection.
        /// </summary>
        public List<UnitSkill> Skills { get; set; }

        #endregion Attributes

        /// <summary>
        /// Default constructor.
        /// </summary>
        public UnitSkillSubsection() 
        {
            this.Skills = new List<UnitSkill>();
        }

        #region Static Methods

        /// <summary>
        /// Builds a list of <c>UnitSkillSubsections</c> using <paramref name="data"/> and <paramref name="configs"/> and returns it.
        /// </summary>
        public static List<UnitSkillSubsection> BuildList(IEnumerable<string> data, List<UnitSkillSubsectionConfig> configs, IReadOnlyDictionary<string, Skill> skills)
        {
            List<UnitSkillSubsection> subsections = new List<UnitSkillSubsection>();
            foreach (UnitSkillSubsectionConfig subsectionConfig in configs)
            {
                UnitSkillSubsection subsection = new UnitSkillSubsection();
                foreach (UnitSkillConfig config in subsectionConfig.Skills)
                {
                    string name = DataParser.OptionalString(data, config.Name, "Skill Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    subsection.Skills.Add(new UnitSkill(data, config, skills));
                }
                
                subsections.Add(subsection);
            }

            return subsections;
        }

        #endregion Static Methods
    }
}
