using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Units
{
    #region Interface
    
    /// <inheritdoc cref="Unit"/>
    public partial interface IUnit
    {
        /// <inheritdoc cref="Unit.WeaponRanks"/>
        IDictionary<string, string> WeaponRanks { get; }

        /// <inheritdoc cref="Unit.Inventory"/>
        IUnitInventory Inventory { get; }
    }
    
    #endregion Interface

    // Partial class for holding inventory attributes/functions.
    public partial class Unit : IUnit
    {
        #region Attributes

        /// <summary>
        /// Collection of the unit's weapon ranks.
        /// </summary>
        public IDictionary<string, string> WeaponRanks { get; private set; }

        /// <summary>
        /// Container for information about the unit's inventory.
        /// </summary>
        public IUnitInventory Inventory { get; private set; }

        #endregion Attributes

        /// <summary>
        /// Partial constructor. Builds weapon ranks and inventory.
        /// </summary>
        /// <remarks>
        /// Depends on the following being built beforehand:
        /// <list type="bullet">
        /// <item>Stats</item>
        /// <item>Emblem (Engage)</item>
        /// </list>
        /// </remarks>
        public void Constructor_Unit_Inventory(UnitsConfig config, IEnumerable<string> data, SystemInfo system)
        {
            this.WeaponRanks = BuildWeaponRanks(data, config.WeaponRanks, system.Constants.WeaponRanks.Any());
            this.Inventory = BuildUnitInventory(data, config.Inventory, system);
        }

        #region Build Functions

        /// <summary>
        /// Builds and returns the unit's dictionary of weapon rank types/letters.
        /// </summary>
        /// <param name="validateWeaponRankLetters">Flag indicating if weapon rank types should have an accompanying letter.</param>
        /// <exception cref="WeaponRankMissingLetterException"></exception>
        /// <exception cref="NonUniqueObjectNameException"></exception>
        private IDictionary<string, string> BuildWeaponRanks(IEnumerable<string> data, List<UnitWeaponRanksConfig> config, bool validateWeaponRankLetters)
        {
            IDictionary<string, string> weaponRanks = new Dictionary<string, string>();
            foreach (UnitWeaponRanksConfig rank in config)
            {
                string rankType;
                if (!string.IsNullOrEmpty(rank.SourceName)) rankType = rank.SourceName;
                else rankType = DataParser.OptionalString(data, rank.Type, "Weapon Rank Type");

                string rankLetter = DataParser.OptionalString(data, rank.Rank, "Weapon Rank Letter");

                if (!string.IsNullOrEmpty(rankType))
                {
                    if (validateWeaponRankLetters && string.IsNullOrEmpty(rankLetter))
                    {
                        //If we're using fixed weapon rank sources (i.e. 3H-style), just skip blank ranks.
                        if (!string.IsNullOrEmpty(rank.SourceName)) continue;
                        else throw new WeaponRankMissingLetterException(rankType);
                    }

                    if (!weaponRanks.TryAdd(rankType, rankLetter))
                        throw new NonUniqueObjectNameException("weapon rank", rankType);
                }
            }

            return weaponRanks;
        }

        /// <summary>
        /// Builds and returns the unit's inventory container.
        /// </summary>
        /// <remarks>
        /// Depends on the following being built beforehand:
        /// <list type="bullet">
        /// <item>Stats</item>
        /// <item>WeaponRanks</item>
        /// <item>Emblem</item>
        /// </list>
        /// </remarks>
        private IUnitInventory BuildUnitInventory(IEnumerable<string> data, InventoryConfig config, SystemInfo system)
        {
            IUnitInventory inventory = new UnitInventory(config, system, data, this.Emblem);

            foreach (IUnitInventoryItem item in inventory.GetAllItems())
            {
                //Check if the item can be equipped
                string unitRank;
                if (item.Item.IsAlwaysUsable)
                {
                    item.CanEquip = true;
                }
                else if (this.WeaponRanks.TryGetValue(item.Item.Category, out unitRank))
                {
                    if (string.IsNullOrEmpty(unitRank)
                     || string.IsNullOrEmpty(item.Item.WeaponRank)
                     || system.Constants.WeaponRanks.IndexOf(unitRank) >= system.Constants.WeaponRanks.IndexOf(item.Item.WeaponRank))
                        item.CanEquip = true;
                }
                else if (string.IsNullOrEmpty(item.Item.WeaponRank) && !item.Item.UtilizedStats.Any())
                {
                    item.CanEquip = true;
                }

            }

            IUnitInventoryItem primaryEquipped = inventory.GetPrimaryEquippedItem();
            if (primaryEquipped != null)
            {
                //Check if we need to apply weapon rank bonuses for the primary equipped item
                if (this.WeaponRanks.ContainsKey(primaryEquipped.Item.Category))
                {
                    string unitRank;
                    this.WeaponRanks.TryGetValue(primaryEquipped.Item.Category, out unitRank);

                    IWeaponRankBonus rankBonus = system.WeaponRankBonuses.FirstOrDefault(b => b.Category == primaryEquipped.Item.Category && b.Rank == unitRank);
                    if (rankBonus != null)
                    {
                        string modifierName = $"{primaryEquipped.Item.Category} {unitRank} Rank Bonus";
                        this.Stats.ApplyCombatStatModifiers(rankBonus.CombatStatModifiers, modifierName);
                        this.Stats.ApplyGeneralStatModifiers(rankBonus.StatModifiers, modifierName);
                    }
                }
            }

            //Apply equipped stat modifiers
            IEnumerable<IUnitInventoryItem> equippedItems = inventory.GetAllEquippedItems();
            if (this.Emblem != null)
            {
                IUnitInventoryItem emblemEquipped = this.Emblem.EngageWeapons.SingleOrDefault(i => i.IsPrimaryEquipped);
                if (emblemEquipped != null) equippedItems = equippedItems.Append(emblemEquipped);
            }

            foreach (IUnitInventoryItem equipped in equippedItems)
            {
                string modifierName = $"{equipped.Item.Name} (Eqp)";
                this.Stats.ApplyCombatStatModifiers(equipped.Item.EquippedCombatStatModifiers, modifierName);
                this.Stats.ApplyGeneralStatModifiers(equipped.Item.EquippedStatModifiers, modifierName);

                //If the equipped item has an engraving, apply those modifiers too.
                foreach (IEngraving engraving in equipped.EngravingsList)
                {
                    string engravingModifierName = $"{equipped.Item.Name} (Eqp) {engraving.Name}";
                    this.Stats.ApplyCombatStatModifiers(engraving.CombatStatModifiers, engravingModifierName);
                    this.Stats.ApplyGeneralStatModifiers(engraving.StatModifiers, engravingModifierName);
                }
            }

            //Apply inventory stat modifiers for all nonequipped items
            foreach (IUnitInventoryItem inv in inventory.GetAllUnequippedItems())
            {
                string modifierName = $"{inv.Item.Name} (Inv)";
                this.Stats.ApplyCombatStatModifiers(inv.Item.InventoryCombatStatModifiers, modifierName);
                this.Stats.ApplyGeneralStatModifiers(inv.Item.InventoryStatModifiers, modifierName);
            }

            return inventory;
        }

        #endregion Build Functions
    }
}
