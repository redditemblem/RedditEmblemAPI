using RedditEmblemAPI.Models.Configuration.System.Skills;
using RedditEmblemAPI.Models.Exceptions;
using RedditEmblemAPI.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Services.Helpers
{
    public class SkillHelper : Helper
    {
        public static IList<Skill> Process(SkillsConfig config)
        {
            IList<Skill> skills = new List<Skill>();

            foreach (IList<object> row in config.Query.Data)
            {
                try
                {
                    //Convert objects to strings
                    IList<string> skill = row.Select(r => r.ToString()).ToList();

                    //Skip blank items
                    if (string.IsNullOrEmpty(skill.ElementAtOrDefault(config.SkillName)))
                        continue;

                    Skill temp = new Skill()
                    {
                        Name = skill.ElementAtOrDefault(config.SkillName).Trim(),
                        SpriteURL = (skill.ElementAtOrDefault(config.SpriteURL) ?? string.Empty).Trim()
                    };

                    foreach (int Value in config.TextFields)
                        temp.TextFields.Add(skill.ElementAtOrDefault(Value) ?? string.Empty);

                    skills.Add(temp);
                }
                catch (Exception ex)
                {
                    throw new SkillProcessingException(row.ElementAtOrDefault(config.SkillName).ToString(), ex);
                }
            }

            return skills;
        }
    }
}
