using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Items;
using RedditEmblemAPI.Models.Configuration.System.Skills;
using RedditEmblemAPI.Models.Configuration.System.Tags;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.Skills;

namespace UnitTests.Models.System
{
    [TestClass]
    public class ItemTests
    {
        #region Constants

        private const string INPUT_NAME = "Item Test";
        private const string INPUT_CATEGORY = "Category";
        private const string INPUT_UTILIZED_STATS = "Str";
        private const string INPUT_DEALS_DAMAGE = "Yes";
        private const string INPUT_USES = "0";
        private const string INPUT_STAT_MIGHT = "5";
        private const string INPUT_RANGE_MINIMUM = "1";
        private const string INPUT_RANGE_MAXIMUM = "2";

        #endregion Constants

        #region Setup

        private IDictionary<string, Skill> DICTIONARY_SKILL = new Dictionary<string, Skill>();
        private IDictionary<string, Tag> DICTIONARY_TAGS = new Dictionary<string, Tag>();
        private IDictionary<string, Engraving> DICTIONARY_ENGRAVINGS = new Dictionary<string, Engraving>();

        [TestInitialize]
        public void Setup()
        {
            Setup_Skills();
            Setup_Tags();
            Setup_Engravings(); //dependent on Tags
        }

        private void Setup_Skills()
        {
            SkillsConfig config = new SkillsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Skill 1" },
                            new List<object>(){ "Skill 2" }
                        }
                    }
                },
                Name = 0
            };

            this.DICTIONARY_SKILL = Skill.BuildDictionary(config);
        }

        private void Setup_Tags()
        {
            TagsConfig config = new TagsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Tag 1" },
                            new List<object>(){ "Tag 2" }
                        }
                    }
                },
                Name = 0
            };

            this.DICTIONARY_TAGS = Tag.BuildDictionary(config);
        }

        private void Setup_Engravings()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Engraving 1", "Tag 1" },
                            new List<object>(){ "Engraving 2", "Tag 1" }
                        }
                    }
                },
                Name = 0,
                Tags = new List<int> { 1 }
            };

            this.DICTIONARY_ENGRAVINGS = Engraving.BuildDictionary(config, DICTIONARY_TAGS);
        }

        #endregion Setup

        [TestMethod]
        public void ItemConstructor_RequiredFields_WithInputNull()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                }
            };

            List<string> data = new List<string>() { };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new Item(config, data, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS));
        }

        [TestMethod]
        public void ItemConstructor_RequiredFields()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                }
            };

            List<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM };

            Item item = new Item(config, data, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS);

            Assert.AreEqual<string>(INPUT_NAME, item.Name);
        }

        #region OptionalField_IsAlwaysUsable

        [TestMethod]
        public void ItemConstructor_OptionalField_IsAlwaysUsable_EmptyString()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                },
                IsAlwaysUsable = 8
            };

            List<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, string.Empty };

            Item item = new Item(config, data, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS);

            Assert.IsFalse(item.IsAlwaysUsable);
        }

        [TestMethod]
        public void ItemConstructor_OptionalField_IsAlwaysUsable_No()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                },
                IsAlwaysUsable = 8
            };

            List<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, "No" };

            Item item = new Item(config, data, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS);

            Assert.IsFalse(item.IsAlwaysUsable);
        }

        [TestMethod]
        public void ItemConstructor_OptionalField_IsAlwaysUsable_Yes()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                },
                IsAlwaysUsable = 8
            };

            List<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, "Yes" };

            Item item = new Item(config, data, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS);

            Assert.IsTrue(item.IsAlwaysUsable);
        }

        #endregion OptionalField_IsAlwaysUsable

        #region OptionalField_SpriteURL

        [TestMethod]
        public void ItemConstructor_OptionalField_SpriteURL_EmptyString()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                },
                SpriteURL = 8
            };

            List<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, string.Empty };

            Item item = new Item(config, data, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS);

            Assert.AreEqual<string>(string.Empty, item.SpriteURL);
        }

        [TestMethod]
        public void ItemConstructor_OptionalField_SpriteURL_InvalidURL()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                },
                SpriteURL = 8
            };

            List<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, "NotAURL" };

            Assert.ThrowsException<URLException>(() => new Item(config, data, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS));
        }

        [TestMethod]
        public void ItemConstructor_OptionalField_SpriteURL()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                },
                SpriteURL = 8
            };

            List<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, UnitTestConsts.IMAGE_URL };

            Item item = new Item(config, data, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS);

            Assert.AreEqual<string>(UnitTestConsts.IMAGE_URL, item.SpriteURL);
        }

        #endregion OptionalField_SpriteURL

        #region OptionalField_Tags

        [TestMethod]
        public void ItemConstructor_OptionalField_Tags_EmptyString()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                },
                Tags = new List<int> { 8, 9 }
            };

            List<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, string.Empty, string.Empty };

            Item item = new Item(config, data, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS);

            Assert.AreEqual<int>(0, item.Tags.Count);
        }

        [TestMethod]
        public void ItemConstructor_OptionalField_Tags_UnmatchedTag()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                },
                Tags = new List<int> { 8, 9 }
            };

            List<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, "Tag 3", string.Empty };

            Assert.ThrowsException<UnmatchedTagException>(() => new Item(config, data, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS));
        }

        [TestMethod]
        public void ItemConstructor_OptionalField_Tags_DuplicateTags()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                },
                Tags = new List<int> { 8, 9 }
            };

            List<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, "Tag 1", "Tag 1" };

            Item item = new Item(config, data, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS);

            Assert.AreEqual<int>(1, item.Tags.Count);
        }

        [TestMethod]
        public void ItemConstructor_OptionalField_Tags_MultipleSameField()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                },
                Tags = new List<int> { 8, 9 }
            };

            List<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, "Tag 1,Tag 2", string.Empty };

            Item item = new Item(config, data, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS);

            Assert.AreEqual<int>(2, item.Tags.Count);
        }

        [TestMethod]
        public void ItemConstructor_OptionalField_Tags_MultipleSeparateFields()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                },
                Tags = new List<int> { 8, 9 }
            };

            List<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, "Tag 1", "Tag 2" };

            Item item = new Item(config, data, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS);

            Assert.AreEqual<int>(2, item.Tags.Count);
        }

        #endregion OptionalField_Tags

        #region OptionalField_TextFields

        [TestMethod]
        public void ItemConstructor_OptionalField_TextFields_EmptyString()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                },
                TextFields = new List<int> { 8, 9 }
            };

            List<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, string.Empty, string.Empty };

            Item item = new Item(config, data, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS);

            CollectionAssert.AreEqual(new List<string>() { }, item.TextFields);
        }

        [TestMethod]
        public void ItemConstructor_OptionalField_TextFields()
        {
            string field1 = "Text Field 1";
            string field2 = "Text Field 2";

            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                },
                TextFields = new List<int> { 8, 9 }
            };

            List<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, field1, field2 };

            Item item = new Item(config, data, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS);

            CollectionAssert.AreEqual(new List<string>() { field1, field2 }, item.TextFields);
        }

        #endregion OptionalField_TextFields

        #region OptionalField_GraphicURL

        [TestMethod]
        public void ItemConstructor_OptionalField_GraphicURL_EmptyString()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                },
                GraphicURL = 8
            };

            List<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, string.Empty };

            Item item = new Item(config, data, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS);

            Assert.AreEqual<string>(string.Empty, item.GraphicURL);
        }

        [TestMethod]
        public void ItemConstructor_OptionalField_GraphicURL_InvalidURL()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                },
                GraphicURL = 8
            };

            List<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, "NotAURL" };

            Assert.ThrowsException<URLException>(() => new Item(config, data, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS));
        }

        [TestMethod]
        public void ItemConstructor_OptionalField_GraphicURL()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                },
                GraphicURL = 8
            };

            List<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, UnitTestConsts.IMAGE_URL };

            Item item = new Item(config, data, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS);

            Assert.AreEqual<string>(UnitTestConsts.IMAGE_URL, item.GraphicURL);
        }

        #endregion OptionalField_GraphicURL

        #region FlagAsMatched

        [TestMethod]
        public void Item_FlagAsMatched()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                }
            };

            List<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM };

            Item item = new Item(config, data, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS);

            Assert.IsFalse(item.Matched);
            item.FlagAsMatched();
            Assert.IsTrue(item.Matched);
        }

        [TestMethod]
        public void Item_FlagAsMatched_WithTags()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                },
                Tags = new List<int> { 8 }
            };

            List<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, "Tag 1, Tag 2" };

            Item item = new Item(config, data, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS);

            Assert.IsFalse(item.Matched);
            Assert.AreEqual<int>(2, item.Tags.Count);
            Assert.IsFalse(item.Tags[0].Matched);
            Assert.IsFalse(item.Tags[1].Matched);

            item.FlagAsMatched();

            Assert.IsTrue(item.Matched);
            Assert.IsTrue(item.Tags[0].Matched);
            Assert.IsTrue(item.Tags[1].Matched);
        }

        public void Item_FlagAsMatched_WithEngravings()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                },
                Engravings = new List<int> { 8 }
            };

            List<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, "Engraving 1, Engraving 2" };

            Item item = new Item(config, data, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS);

            Assert.IsFalse(item.Matched);
            Assert.AreEqual<int>(2, item.Engravings.Count);
            Assert.IsFalse(item.Engravings[0].Matched);
            Assert.IsFalse(item.Engravings[1].Matched);

            item.FlagAsMatched();

            Assert.IsTrue(item.Matched);
            Assert.IsTrue(item.Engravings[0].Matched);
            Assert.IsTrue(item.Engravings[1].Matched);
        }

        #endregion FlagAsMatched

        #region BuildDictionary

        [TestMethod]
        public void Item_BuildDictionary_WithQueryNull()
        {
            ItemsConfig config = new ItemsConfig();

            IDictionary<string, Item> dict = Item.BuildDictionary(config, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void Item_BuildDictionary_WithInput_Null()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ }
                        }
                    }
                },
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                }
            };

            IDictionary<string, Item> dict = Item.BuildDictionary(config, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void Item_BuildDictionary_WithInput_DuplicateName()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM },
                            new List<object>(){ INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM }
                        }
                    }
                },
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                }
            };

            Assert.ThrowsException<ItemProcessingException>(() => Item.BuildDictionary(config, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS));
        }

        [TestMethod]
        public void Item_BuildDictionary()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM }
                        }
                    }
                },
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                }
            };

            IDictionary<string, Item> dict = Item.BuildDictionary(config, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS);
            Assert.AreEqual<int>(1, dict.Count);
        }

        [TestMethod]
        public void Item_BuildDictionary_MultiQuery()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Item 1", INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM },
                            new List<object>(){ "Item 2", INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM }
                        }
                    },
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Item 3", INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM },
                            new List<object>(){ "Item 4", INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM }
                        }
                    }
                },
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                }
            };

            IDictionary<string, Item> dict = Item.BuildDictionary(config, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS);
            Assert.AreEqual<int>(4, dict.Count);
        }

        #endregion BuildDictionary

        #region MatchNames

        [TestMethod]
        public void Item_MatchNames_UnmatchedName()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Item 1", INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM },
                            new List<object>(){ "Item 2", INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM }
                        }
                    }
                },
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                }
            };

            IDictionary<string, Item> dict = Item.BuildDictionary(config, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS);
            IEnumerable<string> names = new List<string>() { "Item 3" };

            Assert.ThrowsException<UnmatchedItemException>(() => Item.MatchNames(dict, names));
        }

        [TestMethod]
        public void Item_MatchNames_SingleMatch()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Item 1", INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM },
                            new List<object>(){ "Item 2", INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM }
                        }
                    }
                },
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                }
            };

            IDictionary<string, Item> dict = Item.BuildDictionary(config, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS);
            IEnumerable<string> names = new List<string>() { "Item 1" };

            List<Item> matches = Item.MatchNames(dict, names);
            Assert.AreEqual(1, matches.Count);
            Assert.IsTrue(matches.First().Matched);
        }

        [TestMethod]
        public void Item_MatchNames_MultipleMatches()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Item 1", INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM },
                            new List<object>(){ "Item 2", INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM }
                        }
                    }
                },
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                }
            };

            IDictionary<string, Item> dict = Item.BuildDictionary(config, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS);
            IEnumerable<string> names = new List<string>() { "Item 1", "Item 2" };

            List<Item> matches = Item.MatchNames(dict, names);
            Assert.AreEqual(2, matches.Count);
            Assert.IsTrue(matches[0].Matched);
            Assert.IsTrue(matches[1].Matched);
        }

        [TestMethod]
        public void Item_MatchNames_MultipleMatches_SetMatchedStatus()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Item 1", INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM },
                            new List<object>(){ "Item 2", INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM }
                        }
                    }
                },
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>
                {
                    new NamedStatConfig_Displayed()
                    {
                        SourceName = "Mt",
                        Value = 5
                    }
                },
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                }
            };

            IDictionary<string, Item> dict = Item.BuildDictionary(config, DICTIONARY_SKILL, DICTIONARY_TAGS, DICTIONARY_ENGRAVINGS);
            IEnumerable<string> names = new List<string>() { "Item 1", "Item 2" };

            List<Item> matches = Item.MatchNames(dict, names, true);
            Assert.AreEqual(2, matches.Count);
            Assert.IsFalse(matches[0].Matched);
            Assert.IsFalse(matches[1].Matched);
        }

        #endregion MatchNames
    }
}