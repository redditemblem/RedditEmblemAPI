using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.Classes
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Classes"</c> object data.
    /// </summary>
    public class ClassesConfig
    {
        #region RequiredFields

        [JsonRequired]
        public Query Query { get; set; }

        /// <summary>
        /// Cell index of a class's name value.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        /// <summary>
        /// Cell index of a class's movement type value.
        /// </summary>
        [JsonRequired]
        public int MovementType { get; set; }

        #endregion

        #region OptionalFields

        /// <summary>
        /// Cell index of a class's tags value.
        /// </summary>
        public int Tags { get; set; } = -1;

        /// <summary>
        /// List of cell indexes for a class's text fields.
        /// </summary>
        public IList<int> TextFields { get; set; } = new List<int>();

        #endregion
    }
}
