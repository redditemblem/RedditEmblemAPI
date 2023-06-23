using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.Classes
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Classes"</c> object data.
    /// </summary>
    public class ClassesConfig : IQueryable
    {
        #region Required Fields

        [JsonRequired]
        public Query Query { get; set; }

        /// <summary>
        /// Required. Cell index of a class's name value.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        /// <summary>
        /// Required. Cell index of a class's movement type value.
        /// </summary>
        [JsonRequired]
        public int MovementType { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. Cell index of a class's battle style.
        /// </summary>
        public int BattleStyle { get; set; } = -1;

        /// <summary>
        /// Optional. List of cell indexes for a class's tag field(s).
        /// </summary>
        public List<int> Tags { get; set; } = new List<int>();

        /// <summary>
        /// Optional. List of cell indexes for a class's text fields.
        /// </summary>
        public List<int> TextFields { get; set; } = new List<int>();

        #endregion
    }
}
