using RedditEmblemAPI.Models.Configuration.Common;
using System;

namespace RedditEmblemAPI.Models.Configuration.System.Classes
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Classes"</c> object data.
    /// </summary>
    public class ClassesConfig : MultiQueryable
    {
        #region Optional Fields

        /// <summary>
        /// Optional. Location of a class's movement type value.
        /// </summary>
        public (int, int) MovementType { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of a class's battle style.
        /// </summary>
        public (int, int) BattleStyle { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Collection of locations of a class's tag field(s).
        /// </summary>
        public (int, int)[] Tags { get; set; } = Array.Empty<(int, int)>();

        /// <summary>
        /// Optional. Collection of locations of a class's text fields.
        /// </summary>
        public (int, int)[] TextFields { get; set; } = Array.Empty<(int, int)>();

        #endregion Optional Fields
    }
}
