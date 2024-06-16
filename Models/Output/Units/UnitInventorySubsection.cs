using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Units
{
    public class UnitInventorySubsection
    {
        #region Attributes

        /// <summary>
        /// List of the items the unit is carrying.
        /// </summary>
        public List<UnitInventoryItem> Items { get; set; }

        /// <summary>
        /// Counter indicating the number of empty slots in the inventory.
        /// </summary>
        public int EmptySlotCount { get; set; }

        #endregion Attributes

        /// <summary>
        /// Default constructor.
        /// </summary>
        public UnitInventorySubsection() 
        {
            this.Items = new List<UnitInventoryItem>();
            this.EmptySlotCount = 0;
        }

        /// <summary>
        /// Uses the <paramref name="config"/> and <paramref name="data"/> to initialize a new <c>UnitInventoryItem</c> and adds it to <c>this.Items</c>. If the <paramref name="data"/> is empty, increments <c>this.EmptySlotCount</c> instead.
        /// </summary>
        public void AddUnitInventoryItem(UnitInventoryItemConfig config, IEnumerable<string> data, IDictionary<string, Item> items, IDictionary<string, Engraving> engravings)
        {
            string name = DataParser.OptionalString(data, config.Name, "Item Name");
            if (string.IsNullOrEmpty(name))
            {
                this.EmptySlotCount++;
                return;
            }

            int uses = DataParser.OptionalInt_Positive(data, config.Uses, "Item Uses");
            IEnumerable<string> itemEngravings = DataParser.List_Strings(data, config.Engravings).Distinct();
            this.Items.Add(new UnitInventoryItem(name, uses, itemEngravings, items, engravings));
        }

        /// <summary>
        /// Inserts <paramref name="item"/> at the FRONT of <c>this.Items</c>.
        /// </summary>
        public void InsertUnitInventoryItem(UnitInventoryItem item)
        {
            this.Items.Insert(0, item);
        }
    }
}
