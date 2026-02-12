using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.Classes
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Classes"</c> object data.
    /// </summary>
    public class ClassesConfig : MultiQueryable
    {
        #region Optional Fields

        /// <summary>
        /// Optional. Cell index of a class's movement type value.
        /// </summary>
        public int MovementType { get; set; } = -1;

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

        #endregion Optional Fields
    }
}
