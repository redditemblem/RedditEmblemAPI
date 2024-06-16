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
        /// List of unit's inventory subsections.
        /// </summary>
        public List<UnitInventorySubsection> Subsections { get; set; }

        #endregion Attributes

        /// <summary>
        /// Constructor. Builds the Items list and flags equipped items.
        /// </summary>
        /// <exception cref="UnmatchedEquippedItemException"></exception>
        public UnitInventory(InventoryConfig config, SystemInfo system, IEnumerable<string> data, UnitEmblem emblem)
        {
            this.Subsections = new List<UnitInventorySubsection>();
            foreach(InventorySubsectionConfig subsectionConfig in config.Subsections)
            {
                UnitInventorySubsection subsection = new UnitInventorySubsection();
                this.Subsections.Add(subsection);

                foreach (UnitInventoryItemConfig item in subsectionConfig.Slots)
                    subsection.AddUnitInventoryItem(item, data, system.Items, system.Engravings);
            }

            List<UnitInventoryItem> items = GetAllItems();

            //Find the all equipped items and flag them
            string equippedItemName = DataParser.OptionalString(data, config.PrimaryEquippedItem, "Equipped Item");
            if (!string.IsNullOrEmpty(equippedItemName))
            {
                UnitInventoryItem equipped = items.FirstOrDefault(i => i.FullName == equippedItemName);
                if (equipped == null)
                {
                    //Attempt to pick the equipped item off of an emblem, if one exists
                    equipped = emblem?.EngageWeapons.FirstOrDefault(i => i.FullName == equippedItemName);
                    if(equipped == null) {

                        if(!system.Constants.AllowNonInventoryEquippedItems)
                            throw new UnmatchedEquippedItemException(equippedItemName);

                        //If we're allowing non-inventory equipped items, add this missing item to the top of the unit's inventory
                        equipped = new UnitInventoryItem(equippedItemName, 0, new List<string>(), system.Items, system.Engravings);
                        equipped.IsNotInInventory = true;
                        this.Subsections.First().InsertUnitInventoryItem(equipped);
                    }
                }
                equipped.IsPrimaryEquipped = true;
            }

            foreach (int index in config.SecondaryEquippedItems)
            {
                string secondaryEquippedItemName = DataParser.OptionalString(data, index, "Equipped Item");
                if (string.IsNullOrEmpty(secondaryEquippedItemName))
                    continue;

                UnitInventoryItem equipped = items.FirstOrDefault(i => i.FullName == secondaryEquippedItemName);
                if (equipped == null)
                    throw new UnmatchedEquippedItemException(secondaryEquippedItemName);
                equipped.IsSecondaryEquipped = true;
            }
        }

        /// <summary>
        /// Returns a flat list of all <c>UnitInventoryItems</c>, ignoring which <c>this.Subsections</c> they belong to.
        /// </summary>
        public List<UnitInventoryItem> GetAllItems()
        {
            return this.Subsections.SelectMany(s => s.Items).ToList();
        }

        /// <summary>
        /// Returns the item flagged as the primary equipped item, if any.
        /// </summary>
        public UnitInventoryItem GetPrimaryEquippedItem()
        {
            return GetAllItems().SingleOrDefault(i => i.IsPrimaryEquipped);
        }

        /// <summary>
        /// Returns a list of all items flagged as secondary equipped.
        /// </summary>
        public List<UnitInventoryItem> GetSecondaryEquippedItems()
        {
            return GetAllItems().Where(i => i.IsSecondaryEquipped).ToList();
        }

        /// <summary>
        /// Returns a list of all items flagged as either primary or secondary equipped.
        /// </summary>
        /// <returns></returns>
        public List<UnitInventoryItem> GetAllEquippedItems()
        {
            return GetAllItems().Where(i => i.IsPrimaryEquipped || i.IsSecondaryEquipped).ToList();
        }

        /// <summary>
        /// Returns a list of all items that are NOT flagged as either primary or secondary equipped.
        /// </summary>
        public List<UnitInventoryItem> GetAllUnequippedItems()
        {
            return GetAllItems().Where(i => !i.IsPrimaryEquipped && !i.IsSecondaryEquipped).ToList();
        }
    }
}
