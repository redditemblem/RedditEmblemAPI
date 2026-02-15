using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.Units
{
    #region Interface

    /// <inheritdoc cref="UnitSkillSubsection"/>
    public interface IUnitSkillSubsection
    {
        /// <inheritdoc cref="UnitSkillSubsection.Skills"/>
        List<IUnitSkill> Skills { get; set; }
    }

    #endregion Interface

    public class UnitSkillSubsection : IUnitSkillSubsection
    {
        #region Attributes

        /// <summary>
        /// List of all unit skills that belong to this subsection.
        /// </summary>
        public List<IUnitSkill> Skills { get; set; }

        #endregion Attributes

        /// <summary>
        /// Default constructor.
        /// </summary>
        public UnitSkillSubsection() 
        {
            this.Skills = new List<IUnitSkill>();
        }

        #region Static Methods

        /// <summary>
        /// Builds a list of <c>UnitSkillSubsections</c> using <paramref name="data"/> and <paramref name="configs"/> and returns it.
        /// </summary>
        public static List<IUnitSkillSubsection> BuildList(IEnumerable<string> data, List<UnitSkillSubsectionConfig> configs, IDictionary<string, ISkill> skills)
        {
            List<IUnitSkillSubsection> subsections = new List<IUnitSkillSubsection>();
            foreach (UnitSkillSubsectionConfig subsectionConfig in configs)
            {
                IUnitSkillSubsection subsection = new UnitSkillSubsection();
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
