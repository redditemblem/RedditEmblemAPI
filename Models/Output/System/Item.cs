using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Items;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Interfaces;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RedditEmblemAPI.Models.Output.System
{
    /// <summary>
    /// Object representing an item definition in the team's system. 
    /// </summary>
    public class Item : IMatchable
    {
        #region Attributes

        /// <summary>
        /// Flag indicating whether or not this item was found on a unit. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; private set; }

        /// <summary>
        /// The item's name. 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The sprite image URL for the item.
        /// </summary>
        public string SpriteURL { get; set; }

        /// <summary>
        /// The item's category. (ex. Sword)
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// The minimum weapon rank required to wield the item, if applicable.
        /// </summary>
        public string WeaponRank { get; set; }

        /// <summary>
        /// The unit stats that the item uses in calculations, if applicable.
        /// </summary>
        public List<string> UtilizedStats { get; set; }

        /// <summary>
        /// The unit stats that the item targets in calculations, if applicable.
        /// </summary>
        public List<string> TargetedStats { get; set; }

        /// <summary>
        /// Flag indicating whether or not this item is capable of attacking.
        /// </summary>
        public bool DealsDamage { get; set; }

        /// <summary>
        /// The maximum number of uses the item has. For items with single or infinite uses, this value is 0.
        /// </summary>
        public int MaxUses { get; set; }

        /// <summary>
        /// Collection of stat values for the item. (ex. Hit)
        /// </summary>
        [JsonIgnore]
        public IDictionary<string, NamedStatValue> Stats { get; set; }

        /// <summary>
        /// Collection of combat stat modifiers that will be applied to the owning unit when this item is equipped.
        /// </summary>
        [JsonIgnore]
        public IDictionary<string, int> EquippedCombatStatModifiers { get; set; }

        /// <summary>
        /// Collection of stat modifiers that will be applied to the owning unit when this item is equipped.
        /// </summary>
        [JsonIgnore]
        public IDictionary<string, int> EquippedStatModifiers { get; set; }

        /// <summary>
        /// Collection of skills active on the owning unit when this item is equipped.
        /// </summary>
        public List<UnitSkill> EquippedSkills { get; set; }

        /// <summary>
        /// Collection of combat stat modifiers that will be applied to the owning unit when this item is not equipped.
        /// </summary>
        [JsonIgnore]
        public IDictionary<string, int> InventoryCombatStatModifiers { get; set; }

        /// <summary>
        /// Collection of stat modifiers that will be applied to the owning unit when this item is not equipped.
        /// </summary>
        [JsonIgnore]
        public IDictionary<string, int> InventoryStatModifiers { get; set; }

        /// <summary>
        /// The item's range.
        /// </summary>
        public ItemRange Range { get; set; }

        /// <summary>
        /// The item's tags.
        /// </summary>
        [JsonIgnore]
        public List<Tag> Tags { get; set; }

        /// <summary>
        /// The item's engravings.
        /// </summary>
        [JsonIgnore]
        public List<Engraving> Engravings { get; set; }

        /// <summary>
        /// List of the item's text fields.
        /// </summary>
        public List<string> TextFields { get; set; }

        /// <summary>
        /// The graphic image URL for the item.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string GraphicURL { get; set; }

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        public Item(ItemsConfig config, IEnumerable<string> data, IReadOnlyDictionary<string, Skill> skills, IReadOnlyDictionary<string, Tag> tags, IReadOnlyDictionary<string, Engraving> engravings)
        {
            this.Matched = false;
            this.Name = DataParser.String(data, config.Name, "Name");
            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.Category = DataParser.String(data, config.Category, "Category");
            this.WeaponRank = DataParser.OptionalString(data, config.WeaponRank, "Weapon Rank");
            this.UtilizedStats = DataParser.List_StringCSV(data, config.UtilizedStats);
            this.TargetedStats = DataParser.List_StringCSV(data, config.TargetedStats);
            this.DealsDamage = DataParser.OptionalBoolean_YesNo(data, config.DealsDamage, "Deals Damage");
            this.MaxUses = DataParser.OptionalInt_Positive(data, config.Uses, "Uses");
            this.Range = new ItemRange(config.Range, data);
            this.TextFields = DataParser.List_Strings(data, config.TextFields);
            this.GraphicURL = DataParser.OptionalString_URL(data, config.GraphicURL, "Graphic URL");

            IEnumerable<string> itemTags = DataParser.List_StringCSV(data, config.Tags).Distinct();
            this.Tags = Tag.MatchNames(tags, itemTags, true);

            IEnumerable<string> itemEngravings = DataParser.List_Strings(data, config.Engravings).Distinct();
            this.Engravings = Engraving.MatchNames(engravings, itemEngravings, true);

            this.Stats = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(config.Stats, data, true);
            this.EquippedCombatStatModifiers = DataParser.NamedStatDictionary_OptionalInt_Any(config.EquippedCombatStatModifiers, data, false, "{0} (Equipped)");
            this.EquippedStatModifiers = DataParser.NamedStatDictionary_OptionalInt_Any(config.EquippedStatModifiers, data, false, "{0} (Equipped)");
            this.EquippedSkills = BuildEquippedSkills(data, config.EquippedSkills, skills);

            this.InventoryCombatStatModifiers = DataParser.NamedStatDictionary_OptionalInt_Any(config.InventoryCombatStatModifiers, data, false, "{0} (Inventory)");
            this.InventoryStatModifiers = DataParser.NamedStatDictionary_OptionalInt_Any(config.InventoryStatModifiers, data, false, "{0} (Inventory)");
        }

        #region Build Functions

        /// <summary>
        /// Builds and returns a list of the item's equipped skills.
        /// </summary>
        private List<UnitSkill> BuildEquippedSkills(IEnumerable<string> data, List<UnitSkillConfig> configs, IReadOnlyDictionary<string, Skill> skills)
        {
            //Note: I'm using UnitSkill here because that's what input structure the skill display is expecting
            List<UnitSkill> equippedSkills = new List<UnitSkill>();
            foreach (UnitSkillConfig config in configs)
            {
                string name = DataParser.OptionalString(data, config.Name, "Skill Name");
                if (string.IsNullOrEmpty(name)) continue;

                equippedSkills.Add(new UnitSkill(data, config, skills, true));
            }

            return equippedSkills;
        }

        #endregion Build Functions

        /// <summary>
        /// Sets the <c>Matched</c> flag for this <c>Item</c> to true. Additionally, calls <c>FlagAsMatched()</c> for all of its <c>IMatchable</c> child attributes.
        /// </summary>
        public void FlagAsMatched()
        {
            this.Matched = true;
            this.EquippedSkills.ForEach(s => s.SkillObj.FlagAsMatched());
            this.Tags.ForEach(t => t.FlagAsMatched());
            this.Engravings.ForEach(e => e.FlagAsMatched());
        }

        #region Static Functions

        /// <summary>
        /// Iterates through the data in <paramref name="config"/>'s <c>Query</c> and builds an <c>Item</c> from each valid row.
        /// </summary>
        /// <exception cref="ItemProcessingException"></exception>
        public static IReadOnlyDictionary<string, Item> BuildDictionary(ItemsConfig config, IReadOnlyDictionary<string, Skill> skills, IReadOnlyDictionary<string, Tag> tags, IReadOnlyDictionary<string, Engraving> engravings)
        {
            ConcurrentDictionary<string, Item> items = new ConcurrentDictionary<string, Item>();
            if (config == null || config.Queries == null)
                return items.ToFrozenDictionary();

            try
            {
                Parallel.ForEach(config.Queries.SelectMany(q => q.Data), row =>
                {
                    string name = string.Empty;
                    try
                    {
                        IEnumerable<string> item = row.Select(r => r.ToString());
                        name = DataParser.OptionalString(item, config.Name, "Name");
                        if (string.IsNullOrEmpty(name)) return;

                        if (!items.TryAdd(name, new Item(config, item, skills, tags, engravings)))
                            throw new NonUniqueObjectNameException("item");
                    }
                    catch (Exception ex)
                    {
                        throw new ItemProcessingException(name, ex);
                    }
                });
            }
            catch(AggregateException ex)
            {
                throw ex.InnerException;
            }
            
            return items.ToFrozenDictionary();
        }

        /// <summary>
        /// Matches each of the strings in <paramref name="names"/> to a <c>Item</c> in <paramref name="items"/> and returns the matches as a list.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned objects to true.</param>
        public static List<Item> MatchNames(IReadOnlyDictionary<string, Item> items, IEnumerable<string> names, bool skipMatchedStatusSet = false)
        {
            return names.Select(n => MatchName(items, n, skipMatchedStatusSet)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to a <c>Item</c> in <paramref name="items"/> and returns it.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned object to true.</param>
        /// <exception cref="UnmatchedItemException"></exception>
        public static Item MatchName(IReadOnlyDictionary<string, Item> items, string name, bool skipMatchedStatusSet = false)
        {
            Item match;
            if (!items.TryGetValue(name, out match))
                throw new UnmatchedItemException(name);

            if (!skipMatchedStatusSet) match.FlagAsMatched();

            return match;
        }

        #endregion
    }
}