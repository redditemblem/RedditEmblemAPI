using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Output.System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Units
{
    #region Interface

    /// <inheritdoc cref="UnitInventorySubsection"/>
    public interface IUnitInventorySubsection
    {
        /// <inheritdoc cref="UnitInventorySubsection.Items"/>
        List<IUnitInventoryItem> Items { get; }

        /// <inheritdoc cref="UnitInventorySubsection.EmptySlotCount"/>
        int EmptySlotCount { get; }

        /// <inheritdoc cref="UnitInventorySubsection.AddUnitInventoryItem(UnitInventoryItemConfig, IEnumerable{string}, IDictionary{string, IItem}, IDictionary{string, IEngraving})"/>
        void AddUnitInventoryItem(UnitInventoryItemConfig config, IEnumerable<string> data, IDictionary<string, IItem> items, IDictionary<string, IEngraving> engravings);

        /// <inheritdoc cref="UnitInventorySubsection.InsertUnitInventoryItem(IUnitInventoryItem)"/>
        void InsertUnitInventoryItem(IUnitInventoryItem item);
    }

    #endregion Interface

    public class UnitInventorySubsection : IUnitInventorySubsection
    {
        #region Attributes

        /// <summary>
        /// List of the items the unit is carrying.
        /// </summary>
        public List<IUnitInventoryItem> Items { get; private set; }

        /// <summary>
        /// Counter indicating the number of empty slots in the inventory.
        /// </summary>
        public int EmptySlotCount { get; private set; }

        #endregion Attributes

        /// <summary>
        /// Default constructor.
        /// </summary>
        public UnitInventorySubsection() 
        {
            this.Items = new List<IUnitInventoryItem>();
            this.EmptySlotCount = 0;
        }

        /// <summary>
        /// Uses the <paramref name="config"/> and <paramref name="data"/> to initialize a new <c>UnitInventoryItem</c> and adds it to <c>this.Items</c>. If the <paramref name="data"/> is empty, increments <c>this.EmptySlotCount</c> instead.
        /// </summary>
        public void AddUnitInventoryItem(UnitInventoryItemConfig config, IEnumerable<string> data, IDictionary<string, IItem> items, IDictionary<string, IEngraving> engravings)
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
        public void InsertUnitInventoryItem(IUnitInventoryItem item)
        {
            this.Items.Insert(0, item);
        }

        #region Static Functions

        public static List<IUnitInventorySubsection> BuildList(IEnumerable<InventorySubsectionConfig> configs, IEnumerable<string> data, IDictionary<string, IItem> items, IDictionary<string, IEngraving> engravings)
        {
            List<IUnitInventorySubsection> subsections = new List<IUnitInventorySubsection>();

            foreach (InventorySubsectionConfig config in configs)
            {
                IUnitInventorySubsection subsection = new UnitInventorySubsection();

                foreach (UnitInventoryItemConfig item in config.Slots)
                    subsection.AddUnitInventoryItem(item, data, items, engravings);

                subsections.Add(subsection);
            }

            return subsections;
        }

        #endregion Static Functions
    }
}
