using NSubstitute;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Items;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.Skills;

namespace UnitTests.Models.System
{
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

        private IDictionary<string, ISkill> SKILLS;
        private IDictionary<string, ITag> TAGS;
        private IDictionary<string, IEngraving> ENGRAVINGS;

        [SetUp]
        public void Setup()
        {
            Setup_Skills();
            Setup_Tags();
            Setup_Engravings(); //dependent on Tags
        }

        private void Setup_Skills()
        {
            string skill1Name = "Skill 1";
            ISkill skill1 = Substitute.For<ISkill>();
            skill1.Name.Returns(skill1Name);

            this.SKILLS = new Dictionary<string, ISkill>();
            this.SKILLS.Add(skill1Name, skill1);
        }

        private void Setup_Tags()
        {
            string tag1Name = "Tag 1";
            ITag tag1 = Substitute.For<ITag>();
            tag1.Name.Returns(tag1Name);

            string tag2Name = "Tag 2";
            ITag tag2 = Substitute.For<ITag>();
            tag2.Name.Returns(tag2Name);

            this.TAGS = new Dictionary<string, ITag>();
            this.TAGS.Add(tag1Name, tag1);
            this.TAGS.Add(tag2Name, tag2);
        }

        private void Setup_Engravings()
        {
            string engraving1Name = "Engraving 1";
            IEngraving engraving1 = Substitute.For<IEngraving>();
            engraving1.Name.Returns(engraving1Name);
            engraving1.Tags = new List<ITag>() { this.TAGS["Tag 1"] };

            this.ENGRAVINGS = new Dictionary<string, IEngraving>();
            this.ENGRAVINGS.Add(engraving1Name, engraving1);
        }

        #endregion Setup

        [Test]
        public void Constructor_RequiredFields_WithInputNull()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>(),
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                }
            };

            IEnumerable<string> data = new List<string>() { };

            Assert.Throws<RequiredValueNotProvidedException>(() => new Item(config, data, SKILLS, TAGS, ENGRAVINGS));
        }

        [Test]
        public void Constructor_RequiredFields()
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

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_STAT_MIGHT, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM };

            IItem item = new Item(config, data, SKILLS, TAGS, ENGRAVINGS);

            IEnumerable<string> expectedUtilizedStats = new List<string>() { INPUT_UTILIZED_STATS };

            Assert.That(item.Name, Is.EqualTo(INPUT_NAME));
            Assert.That(item.Category, Is.EqualTo(INPUT_CATEGORY));
            Assert.That(item.UtilizedStats, Is.EqualTo(expectedUtilizedStats));
            Assert.That(item.DealsDamage, Is.True);
            Assert.That(item.MaxUses, Is.EqualTo(0));
            Assert.That(item.Stats.Count, Is.EqualTo(1));
            Assert.That(item.Stats["Mt"].Value, Is.EqualTo(5));
            Assert.That(item.Range.Minimum, Is.EqualTo(1));
            Assert.That(item.Range.Maximum, Is.EqualTo(2));
        }

        #region OptionalField_IsAlwaysUsable

        [Test]
        public void Constructor_OptionalField_IsAlwaysUsable_EmptyString()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>(),
                Range = new ItemRangeConfig()
                {
                    Minimum = 5,
                    Maximum = 6
                },
                IsAlwaysUsable = 7
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, string.Empty };

            IItem item = new Item(config, data, SKILLS, TAGS, ENGRAVINGS);

            Assert.That(item.IsAlwaysUsable, Is.False);
        }

        [Test]
        public void Constructor_OptionalField_IsAlwaysUsable_No()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>(),
                Range = new ItemRangeConfig()
                {
                    Minimum = 5,
                    Maximum = 6
                },
                IsAlwaysUsable = 7
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, "No" };

            IItem item = new Item(config, data, SKILLS, TAGS, ENGRAVINGS);

            Assert.That(item.IsAlwaysUsable, Is.False);
        }

        [Test]
        public void Constructor_OptionalField_IsAlwaysUsable_Yes()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>(),
                Range = new ItemRangeConfig()
                {
                    Minimum = 5,
                    Maximum = 6
                },
                IsAlwaysUsable = 7
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, "Yes" };

            IItem item = new Item(config, data, SKILLS, TAGS, ENGRAVINGS);

            Assert.That(item.IsAlwaysUsable, Is.True);
        }

        #endregion OptionalField_IsAlwaysUsable

        #region OptionalField_SpriteURL

        [Test]
        public void Constructor_OptionalField_SpriteURL_EmptyString()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>(),
                Range = new ItemRangeConfig()
                {
                    Minimum = 5,
                    Maximum = 6
                },
                SpriteURL = 7
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, string.Empty };

            IItem item = new Item(config, data, SKILLS, TAGS, ENGRAVINGS);

            Assert.That(item.SpriteURL, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_SpriteURL_InvalidURL()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>(),
                Range = new ItemRangeConfig()
                {
                    Minimum = 5,
                    Maximum = 6
                },
                SpriteURL = 7
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, "NotAURL" };

            Assert.Throws<URLException>(() => new Item(config, data, SKILLS, TAGS, ENGRAVINGS));
        }

        [Test]
        public void Constructor_OptionalField_SpriteURL()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>(),
                Range = new ItemRangeConfig()
                {
                    Minimum = 5,
                    Maximum = 6
                },
                SpriteURL = 7
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, UnitTestConsts.IMAGE_URL };

            IItem item = new Item(config, data, SKILLS, TAGS, ENGRAVINGS);

            Assert.That(item.SpriteURL, Is.EqualTo(UnitTestConsts.IMAGE_URL));
        }

        #endregion OptionalField_SpriteURL

        #region OptionalField_Tags

        [Test]
        public void Constructor_OptionalField_Tags_EmptyString()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>(),
                Range = new ItemRangeConfig()
                {
                    Minimum = 5,
                    Maximum = 6
                },
                Tags = new List<int> { 7, 8 }
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, string.Empty, string.Empty };

            IItem item = new Item(config, data, SKILLS, TAGS, ENGRAVINGS);

            Assert.That(item.Tags, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_Tags_UnmatchedTag()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>(),
                Range = new ItemRangeConfig()
                {
                    Minimum = 5,
                    Maximum = 6
                },
                Tags = new List<int> { 7, 8 }
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, "Tag 3", string.Empty };

            Assert.Throws<UnmatchedTagException>(() => new Item(config, data, SKILLS, TAGS, ENGRAVINGS));
        }

        [Test]
        public void Constructor_OptionalField_Tags_DuplicateTags()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>(),
                Range = new ItemRangeConfig()
                {
                    Minimum = 5,
                    Maximum = 6
                },
                Tags = new List<int> { 7, 8 }
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, "Tag 1", "Tag 1" };

            IItem item = new Item(config, data, SKILLS, TAGS, ENGRAVINGS);

            Assert.That(item.Tags.Count, Is.EqualTo(1));
            item.Tags.First().DidNotReceive().FlagAsMatched();
        }

        [Test]
        public void Constructor_OptionalField_Tags_MultipleSameField()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>(),
                Range = new ItemRangeConfig()
                {
                    Minimum = 5,
                    Maximum = 6
                },
                Tags = new List<int> { 7, 8 }
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, "Tag 1,Tag 2", string.Empty };

            IItem item = new Item(config, data, SKILLS, TAGS, ENGRAVINGS);

            Assert.That(item.Tags.Count, Is.EqualTo(2));
            item.Tags.ForEach(t => t.DidNotReceive().FlagAsMatched());
        }

        [Test]
        public void Constructor_OptionalField_Tags_MultipleSeparateFields()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>(),
                Range = new ItemRangeConfig()
                {
                    Minimum = 5,
                    Maximum = 6
                },
                Tags = new List<int> { 7, 8 }
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, "Tag 1", "Tag 2" };

            IItem item = new Item(config, data, SKILLS, TAGS, ENGRAVINGS);

            Assert.That(item.Tags.Count, Is.EqualTo(2));
            item.Tags.ForEach(t => t.DidNotReceive().FlagAsMatched());
        }

        #endregion OptionalField_Tags

        #region OptionalField_TextFields

        [Test]
        public void Constructor_OptionalField_TextFields_EmptyString()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>(),
                Range = new ItemRangeConfig()
                {
                    Minimum = 5,
                    Maximum = 6
                },
                TextFields = new List<int> { 7, 8 }
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, string.Empty, string.Empty };

            IItem item = new Item(config, data, SKILLS, TAGS, ENGRAVINGS);

            Assert.That(item.TextFields, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_TextFields()
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
                Stats = new List<NamedStatConfig_Displayed>(),
                Range = new ItemRangeConfig()
                {
                    Minimum = 5,
                    Maximum = 6
                },
                TextFields = new List<int> { 7, 8 }
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, field1, field2 };

            IItem item = new Item(config, data, SKILLS, TAGS, ENGRAVINGS);

            IEnumerable<string> expected = new List<string>() { field1, field2 };
            Assert.That(item.TextFields, Is.EqualTo(expected));
        }

        #endregion OptionalField_TextFields

        #region OptionalField_GraphicURL

        [Test]
        public void Constructor_OptionalField_GraphicURL_EmptyString()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>(),
                Range = new ItemRangeConfig()
                {
                    Minimum = 5,
                    Maximum = 6
                },
                GraphicURL = 7
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, string.Empty };

            IItem item = new Item(config, data, SKILLS, TAGS, ENGRAVINGS);

            Assert.That(item.GraphicURL, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_GraphicURL_InvalidURL()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>(),
                Range = new ItemRangeConfig()
                {
                    Minimum = 5,
                    Maximum = 6
                },
                GraphicURL = 7
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, "NotAURL" };

            Assert.Throws<URLException>(() => new Item(config, data, SKILLS, TAGS, ENGRAVINGS));
        }

        [Test]
        public void Constructor_OptionalField_GraphicURL()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>(),
                Range = new ItemRangeConfig()
                {
                    Minimum = 5,
                    Maximum = 6
                },
                GraphicURL = 7
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, UnitTestConsts.IMAGE_URL };

            IItem item = new Item(config, data, SKILLS, TAGS, ENGRAVINGS);

            Assert.That(item.GraphicURL, Is.EqualTo(UnitTestConsts.IMAGE_URL));
        }

        #endregion OptionalField_GraphicURL

        #region FlagAsMatched

        [Test]
        public void FlagAsMatched()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>(),
                Range = new ItemRangeConfig()
                {
                    Minimum = 5,
                    Maximum = 6
                }
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM };

            IItem item = new Item(config, data, SKILLS, TAGS, ENGRAVINGS);

            Assert.That(item.Matched, Is.False);

            item.FlagAsMatched();

            Assert.That(item.Matched, Is.True);
        }

        [Test]
        public void FlagAsMatched_WithSkills()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>(),
                Range = new ItemRangeConfig()
                {
                    Minimum = 5,
                    Maximum = 6
                },
                EquippedSkills = new List<UnitSkillConfig>()
                { 
                    new UnitSkillConfig(){ Name = 7 }
                }
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, "Skill 1" };

            IItem item = new Item(config, data, SKILLS, TAGS, ENGRAVINGS);

            Assert.That(item.Matched, Is.False);
            Assert.That(item.EquippedSkills.Count, Is.EqualTo(1));
            item.EquippedSkills.ForEach(s => s.Skill.DidNotReceive().FlagAsMatched());

            item.FlagAsMatched();

            Assert.That(item.Matched, Is.True);
            item.EquippedSkills.ForEach(s => s.Skill.Received(1).FlagAsMatched());
        }

        [Test]
        public void FlagAsMatched_WithTags()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>(),
                Range = new ItemRangeConfig()
                {
                    Minimum = 5,
                    Maximum = 6
                },
                Tags = new List<int> { 7 }
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, "Tag 1, Tag 2" };

            IItem item = new Item(config, data, SKILLS, TAGS, ENGRAVINGS);

            Assert.That(item.Matched, Is.False);
            Assert.That(item.Tags.Count, Is.EqualTo(2));
            item.Tags.ForEach(t => t.DidNotReceive().FlagAsMatched());

            item.FlagAsMatched();

            Assert.That(item.Matched, Is.True);
            item.Tags.ForEach(t => t.Received(1).FlagAsMatched());
        }

        public void FlagAsMatched_WithEngravings()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>(),
                Range = new ItemRangeConfig()
                {
                    Minimum = 6,
                    Maximum = 7
                },
                Engravings = new List<int> { 8 }
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM, "Engraving 1, Engraving 2" };

            IItem item = new Item(config, data, SKILLS, TAGS, ENGRAVINGS);

            Assert.That(item.Matched, Is.False);
            Assert.That(item.Engravings.Count, Is.EqualTo(2));
            item.Engravings.ForEach(e => e.DidNotReceive().FlagAsMatched());

            item.FlagAsMatched();

            Assert.That(item.Matched, Is.True);
            item.Engravings.ForEach(e => e.Received(1).FlagAsMatched());
        }

        #endregion FlagAsMatched

        #region BuildDictionary


        [Test]
        public void BuildDictionary_WithInput_Null()
        {
            IDictionary<string, IItem> dict = Item.BuildDictionary(null, SKILLS, TAGS, ENGRAVINGS);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_NullQuery()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Queries = null
            };

            IDictionary<string, IItem> dict = Item.BuildDictionary(config, SKILLS, TAGS, ENGRAVINGS);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_EmptyQuery()
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
                Stats = new List<NamedStatConfig_Displayed>(),
                Range = new ItemRangeConfig()
                {
                    Minimum = 5,
                    Maximum = 6
                }
            };

            IDictionary<string, IItem> dict = Item.BuildDictionary(config, SKILLS, TAGS, ENGRAVINGS);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_DuplicateName()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM },
                            new List<object>(){ INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM }
                        }
                    }
                },
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>(),
                Range = new ItemRangeConfig()
                {
                    Minimum = 5,
                    Maximum = 6
                }
            };

            Assert.Throws<ItemProcessingException>(() => Item.BuildDictionary(config, SKILLS, TAGS, ENGRAVINGS));
        }

        [Test]
        public void BuildDictionary()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_NAME, INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM }
                        }
                    }
                },
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>(),
                Range = new ItemRangeConfig()
                {
                    Minimum = 5,
                    Maximum = 6
                }
            };

            IDictionary<string, IItem> dict = Item.BuildDictionary(config, SKILLS, TAGS, ENGRAVINGS);
            Assert.That(dict.Count, Is.EqualTo(1));
        }

        [Test]
        public void BuildDictionary_MultiQuery()
        {
            ItemsConfig config = new ItemsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Item 1", INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM },
                            new List<object>(){ "Item 2", INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM }
                        }
                    },
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Item 3", INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM },
                            new List<object>(){ "Item 4", INPUT_CATEGORY, INPUT_UTILIZED_STATS, INPUT_DEALS_DAMAGE, INPUT_USES, INPUT_RANGE_MINIMUM, INPUT_RANGE_MAXIMUM }
                        }
                    }
                },
                Name = 0,
                Category = 1,
                UtilizedStats = new List<int> { 2 },
                DealsDamage = 3,
                Uses = 4,
                Stats = new List<NamedStatConfig_Displayed>(),
                Range = new ItemRangeConfig()
                {
                    Minimum = 5,
                    Maximum = 6
                }
            };

            IDictionary<string, IItem> dict = Item.BuildDictionary(config, SKILLS, TAGS, ENGRAVINGS);
            Assert.That(dict.Count, Is.EqualTo(4));
        }

        #endregion BuildDictionary

        #region MatchNames

        [Test]
        public void MatchNames_UnmatchedName()
        {
            string item1Name = "Item 1";
            string item2Name = "Item 2";

            IItem item1 = Substitute.For<IItem>();
            item1.Name.Returns(item1Name);

            IDictionary<string, IItem> dict = new Dictionary<string, IItem>();
            dict.Add(item1Name, item1);

            IEnumerable<string> names = new List<string>() { item2Name };

            Assert.Throws<UnmatchedItemException>(() => Item.MatchNames(dict, names));
        }

        [Test]
        public void MatchNames_SingleMatch()
        {
            string item1Name = "Item 1";
            string item2Name = "Item 2";

            IItem item1 = Substitute.For<IItem>();
            item1.Name.Returns(item1Name);

            IItem item2 = Substitute.For<IItem>();
            item2.Name.Returns(item2Name);

            IDictionary<string, IItem> dict = new Dictionary<string, IItem>();
            dict.Add(item1Name, item1);
            dict.Add(item2Name, item2);

            IEnumerable<string> names = new List<string>() { item1Name };
            List<IItem> matches = Item.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(1));
            Assert.That(matches.Contains(item1), Is.True);
            matches.First().Received(1).FlagAsMatched();
        }

        [Test]
        public void MatchNames_MultipleMatches()
        {
            string item1Name = "Item 1";
            string item2Name = "Item 2";

            IItem item1 = Substitute.For<IItem>();
            item1.Name.Returns(item1Name);

            IItem item2 = Substitute.For<IItem>();
            item2.Name.Returns(item2Name);

            IDictionary<string, IItem> dict = new Dictionary<string, IItem>();
            dict.Add(item1Name, item1);
            dict.Add(item2Name, item2);

            IEnumerable<string> names = new List<string>() { item1Name, item2Name };
            List<IItem> matches = Item.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches.Contains(item1), Is.True);
            Assert.That(matches.Contains(item2), Is.True);

            matches[0].Received(1).FlagAsMatched();
            matches[1].Received(1).FlagAsMatched();
        }

        [Test]
        public void MatchNames_MultipleMatches_DoNotSetMatchedStatus()
        {
            string item1Name = "Item 1";
            string item2Name = "Item 2";

            IItem item1 = Substitute.For<IItem>();
            item1.Name.Returns(item1Name);

            IItem item2 = Substitute.For<IItem>();
            item2.Name.Returns(item2Name);

            IDictionary<string, IItem> dict = new Dictionary<string, IItem>();
            dict.Add(item1Name, item1);
            dict.Add(item2Name, item2);

            IEnumerable<string> names = new List<string>() { item1Name, item2Name };
            List<IItem> matches = Item.MatchNames(dict, names, false);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches.Contains(item1), Is.True);
            Assert.That(matches.Contains(item2), Is.True);

            matches[0].DidNotReceive().FlagAsMatched();
            matches[1].DidNotReceive().FlagAsMatched();
        }

        #endregion MatchNames

        #region MatchName

        [Test]
        public void MatchName_UnmatchedName()
        {
            string item1Name = "Item 1";

            IItem item1 = Substitute.For<IItem>();
            item1.Name.Returns(item1Name);

            IDictionary<string, IItem> dict = new Dictionary<string, IItem>();
            dict.Add(item1Name, item1);

            string name = "Item 2";

            Assert.Throws<UnmatchedItemException>(() => Item.MatchName(dict, name));
        }

        [Test]
        public void MatchName()
        {
            string item1Name = "Item 1";

            IItem item1 = Substitute.For<IItem>();
            item1.Name.Returns(item1Name);

            IDictionary<string, IItem> dict = new Dictionary<string, IItem>();
            dict.Add(item1Name, item1);

            IItem match = Item.MatchName(dict, item1Name);

            Assert.That(match, Is.Not.Null);
            Assert.That(match, Is.EqualTo(item1));
            match.Received(1).FlagAsMatched();
        }

        [Test]
        public void MatchName_DoNotSetMatchedStatus()
        {
            string item1Name = "Item 1";

            IItem item1 = Substitute.For<IItem>();
            item1.Name.Returns(item1Name);

            IDictionary<string, IItem> dict = new Dictionary<string, IItem>();
            dict.Add(item1Name, item1);

            IItem match = Item.MatchName(dict, item1Name, false);

            Assert.That(match, Is.Not.Null);
            Assert.That(match, Is.EqualTo(item1));
            match.DidNotReceive().FlagAsMatched();
        }

        #endregion MatchName
    }
}