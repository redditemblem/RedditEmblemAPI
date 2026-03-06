using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Output.System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Units
{
    #region Interface

    /// <inheritdoc cref="UnitInventory"/>
    public interface IUnitInventory
    {
        /// <inheritdoc cref="UnitInventory.Subsections"/>
        List<IUnitInventorySubsection> Subsections { get; }

        /// <inheritdoc cref="UnitInventory.GetAllItems()"/>
        IList<IUnitInventoryItem> GetAllItems();

        /// <inheritdoc cref="UnitInventory.GetPrimaryEquippedItem()"/>
        IUnitInventoryItem GetPrimaryEquippedItem();

        /// <inheritdoc cref="UnitInventory.GetSecondaryEquippedItems()"/>
        IList<IUnitInventoryItem> GetSecondaryEquippedItems();

        /// <inheritdoc cref="UnitInventory.GetAllEquippedItems()"/>
        IList<IUnitInventoryItem> GetAllEquippedItems();

        /// <inheritdoc cref="UnitInventory.GetAllUnequippedItems()"/>
        IList<IUnitInventoryItem> GetAllUnequippedItems();
    }

    #endregion Interface

    public class UnitInventory : IUnitInventory
    {
        #region Attributes

        /// <summary>
        /// List of unit's inventory subsections.
        /// </summary>
        public List<IUnitInventorySubsection> Subsections { get; private set; }

        #endregion Attributes

        /// <summary>
        /// Constructor. Builds the Items list and flags equipped items.
        /// </summary>
        /// <exception cref="UnmatchedEquippedItemException"></exception>
        public UnitInventory(InventoryConfig config, SystemInfo system, IEnumerable<string> data, IUnitEmblem emblem)
        {
            this.Subsections = UnitInventorySubsection.BuildList(config.Subsections, data, system.Items, system.Engravings);

            IList<IUnitInventoryItem> items = GetAllItems();

            //Find the all equipped items and flag them
            string equippedItemName = DataParser.OptionalString(data, config.PrimaryEquippedItem, "Equipped Item");
            if (!string.IsNullOrEmpty(equippedItemName))
            {
                IUnitInventoryItem equipped = items.FirstOrDefault(i => i.FullName == equippedItemName);
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

                IUnitInventoryItem equipped = items.FirstOrDefault(i => i.FullName == secondaryEquippedItemName);
                if (equipped == null)
                    throw new UnmatchedEquippedItemException(secondaryEquippedItemName);
                equipped.IsSecondaryEquipped = true;
            }
        }

        /// <summary>
        /// Returns a flat list of all <c>UnitInventoryItems</c>, ignoring which <c>this.Subsections</c> they belong to.
        /// </summary>
        public IList<IUnitInventoryItem> GetAllItems()
        {
            return this.Subsections.SelectMany(s => s.Items).ToList();
        }

        /// <summary>
        /// Returns the item flagged as the primary equipped item, if any.
        /// </summary>
        public IUnitInventoryItem GetPrimaryEquippedItem()
        {
            return GetAllItems().SingleOrDefault(i => i.IsPrimaryEquipped);
        }

        /// <summary>
        /// Returns a list of all items flagged as secondary equipped.
        /// </summary>
        public IList<IUnitInventoryItem> GetSecondaryEquippedItems()
        {
            return GetAllItems().Where(i => i.IsSecondaryEquipped).ToList();
        }

        /// <summary>
        /// Returns a list of all items flagged as either primary or secondary equipped.
        /// </summary>
        /// <returns></returns>
        public IList<IUnitInventoryItem> GetAllEquippedItems()
        {
            return GetAllItems().Where(i => i.IsPrimaryEquipped || i.IsSecondaryEquipped).ToList();
        }

        /// <summary>
        /// Returns a list of all items that are NOT flagged as either primary or secondary equipped.
        /// </summary>
        public IList<IUnitInventoryItem> GetAllUnequippedItems()
        {
            return GetAllItems().Where(i => !i.IsPrimaryEquipped && !i.IsSecondaryEquipped).ToList();
        }
    }
}
