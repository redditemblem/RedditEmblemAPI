using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Shop;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.Match;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Storage.Shop
{
    #region Interface

    /// <inheritdoc cref="ShopItem"/>
    public interface IShopItem
    {
        /// <inheritdoc cref="ShopItem.FullName"/>
        string FullName { get; set; }

        /// <inheritdoc cref="ShopItem.Item"/>
        IItem Item { get; set; }

        /// <inheritdoc cref="ShopItem.Stats"/>
        IDictionary<string, IUnitInventoryItemStat> Stats { get; set; }

        /// <inheritdoc cref="ShopItem.Price"/>
        int Price { get; set; }

        /// <inheritdoc cref="ShopItem.SalePrice"/>
        int SalePrice { get; set; }

        /// <inheritdoc cref="ShopItem.Stock"/>
        int Stock { get; set; }

        /// <inheritdoc cref="ShopItem.IsNew"/>
        bool IsNew { get; set; }

        /// <inheritdoc cref="ShopItem.Tags"/>
        List<ITag> Tags { get; set; }

        /// <inheritdoc cref="ShopItem.Engravings"/>
        List<IEngraving> EngravingsList { get; set; }

        /// <inheritdoc cref="ShopItem.MatchStatName(string)"/>
        IUnitInventoryItemStat MatchStatName(string name);
    }

    #endregion Interface

    public class ShopItem : IShopItem
    {
        #region Attributes

        /// <summary>
        /// The full name of the item pulled from raw <c>ShopData</c> data.
        /// </summary>
        [JsonIgnore]
        public string FullName { get; set; }

        /// <summary>
        /// The <c>IItem</c> object.
        /// </summary>
        [JsonProperty("name")]
        [JsonConverter(typeof(MatchableNameConverter))]
        public IItem Item { get; set; }

        /// <summary>
        /// Dictionary of the item's stats. Copied over from <c>Item</c> on match.
        /// </summary>
        public IDictionary<string, IUnitInventoryItemStat> Stats { get; set; }

        /// <summary>
        /// The purchase price of the item.
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// If the item is on sale, the sale price. Else, the value of <c>Price</c>.
        /// </summary>
        public int SalePrice { get; set; }

        /// <summary>
        /// The number of items available for sale. A value of 99 indicates infinite stock.
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// Flag indicating if the item is a new addition to the shop.
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// List of the item's tags.
        /// </summary>
        [JsonProperty(ItemConverterType = typeof(MatchableNameConverter))]
        public List<ITag> Tags { get; set; }

        /// <summary>
        /// List of engravings applied to the item.
        /// </summary>
        [JsonIgnore]
        public List<IEngraving> EngravingsList { get; set; }

        #region JSON Serialization Only

        /// <summary>
        /// Only for JSON serialization. List of the engravings on the item.
        /// </summary>
        [JsonProperty]
        private IEnumerable<string> Engravings { get { return this.EngravingsList.Select(e => e.Name).Union(this.Item.Engravings.Select(e => e.Name)).Distinct(); } }

        #endregion JSON Serialization Only

        #endregion

        /// <summary>
        /// Constructor. Builds the <c>ShopItem</c> and matches it to an <c>Item</c> definition from <paramref name="items"/>.
        /// </summary>
        /// <exception cref="UnmatchedItemException"></exception>
        public ShopItem(ShopConfig config, IEnumerable<string> data, IDictionary<string, IItem> items, IDictionary<string, IEngraving> engravings)
        {
            this.FullName = DataParser.String(data, config.Name, "Name");
            this.Item = System.Item.MatchName(items, this.FullName);

            //Copy data from parent item
            this.Stats = new Dictionary<string, IUnitInventoryItemStat>();
            foreach (KeyValuePair<string, INamedStatValue> stat in this.Item.Stats)
                this.Stats.Add(stat.Key, new UnitInventoryItemStat(stat.Value));
            this.Tags = this.Item.Tags.ToList();

            this.Price = DataParser.Int_Positive(data, config.Price, "Price");
            this.SalePrice = DataParser.OptionalInt_Positive(data, config.SalePrice, "Sale Price", this.Price);
            this.Stock = DataParser.Int_Positive(data, config.Stock, "Stock");
            this.IsNew = DataParser.OptionalBoolean_YesNo(data, config.IsNew, "Is New");

            List<string> itemEngravings = DataParser.List_Strings(data, config.Engravings);
            this.EngravingsList = Engraving.MatchNames(engravings, itemEngravings);

            ApplyEngravings();
        }

        private void ApplyEngravings()
        {
            foreach (IEngraving engraving in this.EngravingsList.Union(this.Item.Engravings))
            {
                //Apply any modifiers to the item's stats
                foreach (KeyValuePair<string, int> mod in engraving.ItemStatModifiers)
                {
                    IUnitInventoryItemStat stat = MatchStatName(mod.Key);
                    stat.Modifiers.TryAdd(engraving.Name, mod.Value);
                }

                //Apply any tags
                this.Tags = this.Tags.Union(engraving.Tags).ToList();
            }
        }

        public IUnitInventoryItemStat MatchStatName(string name)
        {
            IUnitInventoryItemStat stat;
            if (!this.Stats.TryGetValue(name, out stat))
                throw new UnmatchedStatException(name);

            return stat;
        }

        #region Static Functions

        /// <summary>
        /// Iterates through <paramref name="config"/>'s queried data and builds an <c>IShopItem</c> from each valid row.
        /// </summary>
        /// <exception cref="ShopItemProcessingException"></exception>
        public static List<IShopItem> BuildList(ShopConfig config, IDictionary<string, IItem> items, IDictionary<string, IEngraving> engravings)
        {
            List<IShopItem> shopItems = new List<IShopItem>();
            if (config?.Query is null) return shopItems;

            foreach (IList<object> row in config.Query.Data)
            {
                string name = string.Empty;
                try
                {
                    IEnumerable<string> shopItem = row.Select(r => r.ToString());
                    name = DataParser.OptionalString(shopItem, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    shopItems.Add(new ShopItem(config, shopItem, items, engravings));
                }
                catch (Exception ex)
                {
                    throw new ShopItemProcessingException(name, ex);
                }
            }

            return shopItems;
        }

        #endregion Static Functions
    }
}
