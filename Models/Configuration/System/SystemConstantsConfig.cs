using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Constants"</c> object data.
    /// </summary>
    public class SystemConstantsConfig
    {
        #region Optional Fields

        /// <summary>
        /// Optional. Container object for a system's currency configuration.
        /// </summary>
        public CurrencyConstsConfig Currency { get; set; } = null;

        /// <summary>
        /// Optional. List of the weapon rank letters for this system, in order from lowest to highest. (ex. "E","D","C"...)
        /// </summary>
        public List<string> WeaponRanks { get; set; } = new List<string>();

        /// <summary>
        /// Optional. Flag indicating whether or not units can equip items not present in their inventory. Defaults to false.
        /// </summary>
        public bool AllowNonInventoryEquippedItems { get; set; } = false;


        #endregion Optional Fields
    }
}
