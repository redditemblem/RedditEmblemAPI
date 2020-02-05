using RedditEmblemAPI.Models.Configuration.Team;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output
{
    public class Map
    {
        public Map(string mapImageURL, string chapterPostURL, MapConstantsConfig config)
        {
            this.MapImageURL = mapImageURL;
            this.ChapterPostURL = chapterPostURL;
            this.Constants = config;
            this.Tiles = new List<List<Tile>>();
        }

        public string MapImageURL { get; set; }
        public string ChapterPostURL { get; set; }
        public MapConstantsConfig Constants { get; set; }
        public IList<List<Tile>> Tiles { get; set; }
    }
}
