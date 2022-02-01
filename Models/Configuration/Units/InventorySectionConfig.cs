using Newtonsoft.Json;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.Units
{
  public class InventorySectionConfig
  {
    #region Required Fields

    /// <summary>
    /// Optional. Cell index of a unit's equipped item name.
    /// </summary>
    [JsonRequired]
    public IList<int> EquippedItems { get; set; }

    /// <summary>
    /// Required. List of cell indexes for a unit's inventory items.
    /// </summary>
    [JsonRequired]
    public IList<int> Slots { get; set; }

    #endregion

    #region Optional Fields

    /// <summary>
    /// Optional. The subheader title for this particular section of the Inventory in the UI, if any.
    /// </summary>
    public string SectionTitle { get; set; } = string.Empty;

    #endregion
  }
}