using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.TerrainEffects;
using RedditEmblemAPI.Services.Helpers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output
{
    public class TerrainEffect
    {
        [JsonIgnore]
        public bool Matched { get; set; }

        public string Name { get; set; }

        public string SpriteURL { get; set; }

        public int Size { get; set; }

        public IList<string> TextFields { get; set; }

        public TerrainEffect(TerrainEffectsConfig config, IList<string> data)
        {
            this.Matched = false;

            this.Name = data.ElementAtOrDefault<string>(config.Name);
            this.SpriteURL = data.ElementAtOrDefault<string>(config.SpriteURL);
            this.Size = ParseHelper.OptionalSafeIntParse(data.ElementAtOrDefault<string>(config.Size), "Size", true, 1);
            this.TextFields = ParseHelper.StringListParse(data, config.TextFields);
        }
    }
}
