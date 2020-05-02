using Newtonsoft.Json;
using RedditEmblemAPI.Models.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RedditEmblemAPI.Models.Output
{
    public class UnitHeldItem
    {        
        /// <summary>
        /// The full name of the item pulled from raw Unit data.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// The name of the item.
        /// </summary>
        [JsonProperty]
        private string Name { get { return this.Item.Name; } }

        /// <summary>
        /// The <c>Item</c> object.
        /// </summary>
        [JsonIgnore]
        public Item Item { get; set; }

        /// <summary>
        /// Flag indicating if this item can be equipped by the unit.
        /// </summary>
        public bool CanEquip { get; set; }

        /// <summary>
        /// Flag indicating if this is the unit's currently equipped item.
        /// </summary>
        public bool IsEquipped { get; set; }

        /// <summary>
        /// Flag indicating if this item will be dropped upon unit defeat.
        /// </summary>
        public bool IsDroppable { get; set; }

        /// <summary>
        /// The number of uses the item currently has remaining.
        /// </summary>
        public int Uses { get; set; }

        private static Regex usesRegex = new Regex(@"\([0-9]+\)"); //match item uses (ex. "(5)")
        private static Regex dropRegex = new Regex(@"\(D\)");      //match item droppable (ex. "(D)")

        public UnitHeldItem(string fullName, IDictionary<string, Item> items)
        {
            this.FullName = fullName;
            this.IsDroppable = false;
            this.CanEquip = false;
            this.IsEquipped = false;
            this.Uses = 0;

            string Name = this.FullName;

            //Search for droppable syntax
            Match dropMatch = dropRegex.Match(Name);
            if (dropMatch.Success)
            {
                this.IsDroppable = true;
                Name = dropRegex.Replace(Name, string.Empty);
            }

            //Search for uses syntax
            Match usesMatch = usesRegex.Match(Name);
            if (usesMatch.Success)
            {
                //Convert item use synatax to int
                string u = usesMatch.Value.ToString();
                u = u.Substring(1, u.Length - 2);
                this.Uses = int.Parse(u);
                Name = usesRegex.Replace(Name, string.Empty);
            }

            Name = Name.Trim();

            Item match;
            if (!items.TryGetValue(Name, out match))
                throw new UnmatchedItemException(Name);

            this.Item = match;
            match.Matched = true;
        }
    }
}
