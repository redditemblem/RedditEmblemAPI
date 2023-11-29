using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Shop;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Storage.Shop
{
    public class ShopItem
    {
        #region Attributes

        /// <summary>
        /// The full name of the item pulled from raw <c>ShopData</c> data.
        /// </summary>
        [JsonIgnore]
        public string FullName { get; set; }

        /// <summary>
        /// The <c>Item</c> object.
        /// </summary>
        [JsonIgnore]
        public Item Item { get; set; }

        /// <summary>
        /// Dictionary of the item's stats. Copied over from <c>Item</c> on match.
        /// </summary>
        public IDictionary<string, UnitInventoryItemStat> Stats { get; set; }

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
        [JsonIgnore]
        public List<Tag> TagsList { get; set; }

        /// <summary>
        /// List of engravings applied to the item.
        /// </summary>
        [JsonIgnore]
        public List<Engraving> EngravingsList { get; set; }

        #region JSON Serialization Only

        /// <summary>
        /// Only for JSON serialization. The name of the item.
        /// </summary>
        [JsonProperty]
        private string Name { get { return this.Item.Name; } }

        /// <summary>
        /// For JSON serialization only. Names of the item's tags.
        /// </summary>
        [JsonProperty]
        private IEnumerable<string> Tags { get { return this.TagsList.Select(t => t.Name); } }

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
        public ShopItem(ShopConfig config, IEnumerable<string> data, IDictionary<string, Item> items, IDictionary<string, Engraving> engravings)
        {
            this.FullName = DataParser.String(data, config.Name, "Name");
            this.Item = Item.MatchName(items, this.FullName);

            //Copy data from parent item
            this.Stats = new Dictionary<string, UnitInventoryItemStat>();
            foreach (KeyValuePair<string, decimal> stat in this.Item.Stats)
                this.Stats.Add(stat.Key, new UnitInventoryItemStat(stat.Value));
            this.TagsList = this.Item.Tags.ToList();

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
            foreach (Engraving engraving in this.EngravingsList.Union(this.Item.Engravings))
            {
                //Apply any modifiers to the item's stats
                foreach (KeyValuePair<string, int> mod in engraving.ItemStatModifiers)
                {
                    UnitInventoryItemStat stat = MatchStatName(mod.Key);
                    stat.Modifiers.TryAdd(engraving.Name, mod.Value);
                }

                //Apply any tags
                this.TagsList = this.TagsList.Union(engraving.Tags).ToList();
            }
        }

        public UnitInventoryItemStat MatchStatName(string name)
        {
            UnitInventoryItemStat stat;
            if (!this.Stats.TryGetValue(name, out stat))
                throw new UnmatchedStatException(name);

            return stat;
        }

        #region Static Functions

        /// <summary>
        /// Iterates through the data in <paramref name="config"/>'s <c>Query</c> and builds a <c>ShopItem</c> from each valid row.
        /// </summary>
        /// <exception cref="ShopItemProcessingException"></exception>
        public static List<ShopItem> BuildList(ShopConfig config, IDictionary<string, Item> items, IDictionary<string, Engraving> engravings)
        {
            List<ShopItem> shopItems = new List<ShopItem>();
            if (config == null || config.Query == null)
                return shopItems;

            foreach (List<object> row in config.Query.Data)
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
