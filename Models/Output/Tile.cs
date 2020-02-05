using Newtonsoft.Json;
using RedditEmblemAPI.Models.Common;

namespace RedditEmblemAPI.Models.Output
{
    public class Tile
    {
        public Coordinate Coordinate { get; set; }
        [JsonIgnore]
        public Unit Unit;
        public string OccupyingUnitName { get { return (this.Unit == null ? string.Empty : this.Unit.Name); } }
    }
}
