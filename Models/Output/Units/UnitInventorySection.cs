using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.Units
{
    public class UnitInventorySection
    {
        #region Attributes

        public string SectionTitle { get; set; }
        public IList<UnitInventoryItem> Items { get; set; }

        #endregion 

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnitInventorySection(string sectionTitle)
        {
            this.SectionTitle = sectionTitle;
            this.Items = new List<UnitInventoryItem>();
        }
    }
}
