using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Units
{
    public class UnitInventory
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
        /// Constructor. Builds the Items list and flags equipped items.
        /// </summary>
        /// <exception cref="UnmatchedEquippedItemException"></exception>
        public UnitInventory(InventoryConfig config, List<string> data, IDictionary<string, Item> items)
        {
            this.Items = new List<UnitInventoryItem>();
            this.EmptySlotCount = 0;

            foreach (int index in config.Slots)
            {
                string name = DataParser.OptionalString(data, index, "Item Name");
                if (string.IsNullOrEmpty(name))
                {
                    EmptySlotCount++;
                    continue;
                }

                this.Items.Add(new UnitInventoryItem(name, items));
            }

            //Find the all equipped items and flag them
            string equippedItemName = DataParser.OptionalString(data, config.PrimaryEquippedItem, "Equipped Item");
            if (!string.IsNullOrEmpty(equippedItemName))
            {
                UnitInventoryItem equipped = this.Items.FirstOrDefault(i => i.FullName == equippedItemName);
                if (equipped == null)
                    throw new UnmatchedEquippedItemException(equippedItemName);
                equipped.IsPrimaryEquipped = true;
            }

            foreach (int index in config.SecondaryEquippedItems)
            {
                string secondaryEquippedItemName = DataParser.OptionalString(data, index, "Equipped Item");
                if (string.IsNullOrEmpty(secondaryEquippedItemName))
                    continue;

                UnitInventoryItem equipped = this.Items.FirstOrDefault(i => i.FullName == secondaryEquippedItemName);
                if (equipped == null)
                    throw new UnmatchedEquippedItemException(secondaryEquippedItemName);
                equipped.IsSecondaryEquipped = true;
            }
        }

        /// <summary>
        /// Returns the item flagged as the primary equipped item, if any.
        /// </summary>
        public UnitInventoryItem GetPrimaryEquippedItem()
        {
            return this.Items.SingleOrDefault(i => i.IsPrimaryEquipped);
        }

        /// <summary>
        /// Returns a list of all items flagged as secondary equipped.
        /// </summary>
        public List<UnitInventoryItem> GetSecondaryEquippedItems()
        {
            return this.Items.Where(i => i.IsSecondaryEquipped).ToList();
        }

        /// <summary>
        /// Returns a list of all items flagged as either primary or secondary equipped.
        /// </summary>
        /// <returns></returns>
        public List<UnitInventoryItem> GetAllEquippedItems()
        {
            return this.Items.Where(i => i.IsPrimaryEquipped || i.IsSecondaryEquipped).ToList();
        }

        /// <summary>
        /// Returns a list of all items that are NOT flagged as either primary or secondary equipped.
        /// </summary>
        /// <returns></returns>
        public List<UnitInventoryItem> GetAllUnequippedItems()
        {
            return this.Items.Where(i => !i.IsPrimaryEquipped && !i.IsSecondaryEquipped).ToList();
        }
    }
}
