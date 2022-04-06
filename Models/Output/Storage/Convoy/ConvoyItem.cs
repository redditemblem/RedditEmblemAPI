using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Convoy;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RedditEmblemAPI.Models.Output.Storage.Convoy
{
    public class ConvoyItem
    {
        /// <summary>
        /// The full name of the item pulled from raw convoy data.
        /// </summary>
        [JsonIgnore]
        public string FullName { get; set; }

        /// <summary>
        /// Only for JSON serialization. The name of the item.
        /// </summary>
        [JsonProperty]
        private string Name { get { return this.Item.Name; } }

        [JsonIgnore]
        public Item Item { get; set; }

        /// <summary>
        /// The player that owns this item.
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// The number of uses the item currently has remaining.
        /// </summary>
        public int Uses { get; set; }

        /// <summary>
        /// The number of this item present in the convoy.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The sell value of this item.
        /// </summary>
        public int Value { get; set; }


        private static Regex usesRegex = new Regex(@"\([0-9]+\)"); //match item uses (ex. "(5)")

        /// <summary>
        /// Constructor. Builds the <c>ConvoyItem</c> and matches it to an <c>Item</c> definition from <paramref name="items"/>.
        /// </summary>
        /// <exception cref="UnmatchedItemException"></exception>
        public ConvoyItem(ConvoyConfig config, IList<string> data, IDictionary<string, Item> items)
        {
            this.FullName = DataParser.String(data, config.Name, "Name");
            this.Uses = 0;

            string name = this.FullName;

            //Search for uses syntax
            Match usesMatch = usesRegex.Match(name);
            if (usesMatch.Success)
            {
                //Convert item use synatax to int
                string u = usesMatch.Value.ToString();
                u = u.Substring(1, u.Length - 2);
                this.Uses = int.Parse(u);
                name = usesRegex.Replace(name, string.Empty);
            }

            name = name.Trim();

            Item match;
            if(!items.TryGetValue(name, out match))
                throw new UnmatchedItemException(name);
            this.Item = match;
            match.Matched = true;

            this.Owner = DataParser.OptionalString(data, config.Owner, "Owner");
            this.Value = DataParser.OptionalInt_Positive(data, config.Value, "Value", -1);
            this.Quantity = DataParser.OptionalInt_NonZeroPositive(data, config.Quantity, "Quantity");
        }
    }
}
