using RedditEmblemAPI.Models.Configuration.Skills;
using RedditEmblemAPI.Models.Exceptions;
using RedditEmblemAPI.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Services.Helpers
{
    public class SkillHelper
    {
        public static IList<Skill> Process(IList<IList<object>> data, SkillsConfig config)
        {
            IList<Skill> skills = new List<Skill>();

            foreach (IList<object> row in data)
            {
                try
                {
                    //Convert objects to strings
                    IList<string> skill = row.Select(r => r.ToString()).ToList();

                    Skill temp = new Skill()
                    {
                        Name = skill.ElementAtOrDefault<string>(config.SkillName),
                        SpriteURL = skill.ElementAtOrDefault<string>(config.SpriteURL)
                    };

                    foreach (int Value in config.TextFields)
                        temp.TextFields.Add(skill.ElementAtOrDefault<string>(Value));

                    skills.Add(temp);
                }
                catch (Exception ex)
                {
                    throw new SkillProcessingException(row.ElementAtOrDefault<object>(config.SkillName).ToString(), ex);
                }
            }

            return skills;
        }
    }
}
