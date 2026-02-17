using NSubstitute;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.Convoy;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output;
using RedditEmblemAPI.Models.Output.Storage.Convoy;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Models.Storage
{
    public class ConvoyItemTests
    {
        #region Constants

        private const string INPUT_NAME = "Item 1";

        #endregion Constants

        #region SetUp

        private IDictionary<string, IItem> ITEMS;
        private IDictionary<string, IEngraving> ENGRAVINGS;
        private IDictionary<string, ITag> TAGS;


        [SetUp]
        public void SetUp()
        {
            SetUp_Tags();
            SetUp_Engravings();
            SetUp_Items();
        }

        private void SetUp_Tags()
        {
            string tag1Name = "Tag 1";
            ITag tag1 = Substitute.For<ITag>();
            tag1.Name.Returns(tag1Name);

            string tag2Name = "Tag 2";
            ITag tag2 = Substitute.For<ITag>();
            tag2.Name.Returns(tag2Name);

            this.TAGS = new Dictionary <string, ITag>();
            this.TAGS.Add(tag1Name, tag1);
            this.TAGS.Add(tag2Name, tag2);
        }

        private void SetUp_Engravings()
        {
            string engraving1Name = "Engraving 1";
            IEngraving engraving1 = Substitute.For<IEngraving>();
            engraving1.Name.Returns(engraving1Name);
            engraving1.Tags.Returns(new List<ITag>() { TAGS["Tag 1"] });

            string engraving2Name = "Engraving 2";
            IEngraving engraving2 = Substitute.For<IEngraving>();
            engraving2.Name.Returns(engraving2Name);
            engraving2.Tags.Returns(new List<ITag>() { TAGS["Tag 1"] });

            string engraving3Name = "Engraving 3";
            IEngraving engraving3 = Substitute.For<IEngraving>();
            engraving3.Name.Returns(engraving3Name);
            engraving3.Tags.Returns(new List<ITag>() { TAGS["Tag 2"] });

            this.ENGRAVINGS = new Dictionary<string, IEngraving>();
            this.ENGRAVINGS.Add(engraving1Name, engraving1);
            this.ENGRAVINGS.Add(engraving2Name, engraving2);
            this.ENGRAVINGS.Add(engraving3Name, engraving3);
        }

        private void SetUp_Items()
        {
            string item1Name = "Item 1";
            IItem item1 = Substitute.For<IItem>();
            item1.Name.Returns(item1Name);
            item1.Tags.Returns(new List<ITag>() { TAGS["Tag 1"] });
            item1.Engravings.Returns(new List<IEngraving>() { ENGRAVINGS["Engraving 1"] });

            string item2Name = "Item 2";
            IItem item2 = Substitute.For<IItem>();
            item2.Name.Returns(item2Name);
            item2.Tags.Returns(new List<ITag>() { TAGS["Tag 1"], TAGS["Tag 2"] });
            item2.Engravings.Returns(new List<IEngraving>() { ENGRAVINGS["Engraving 2"] });

            this.ITEMS = new Dictionary<string, IItem>();
            this.ITEMS.Add(item1Name, item1);
            this.ITEMS.Add(item2Name, item2);
        }

        #endregion SetUp

        [Test]
        public void Constructor_RequiredFields_IndexOutOfBounds()
        {
            ConvoyConfig config = new ConvoyConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>();

            Assert.Throws<RequiredValueNotProvidedException>(() => new ConvoyItem(config, data, ITEMS, ENGRAVINGS));
        }

        [Test]
        public void Constructor_RequiredFields_UnknownItem()
        {
            ConvoyConfig config = new ConvoyConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>()
            {
                "Item 3"
            };

            Assert.Throws<UnmatchedItemException>(() => new ConvoyItem(config, data, ITEMS, ENGRAVINGS));
        }

        [Test]
        public void Constructor_RequiredFields()
        {
            ConvoyConfig config = new ConvoyConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME
            };

            IConvoyItem item = new ConvoyItem(config, data, ITEMS, ENGRAVINGS);
            IItem expectedMatch = ITEMS[INPUT_NAME];

            Assert.That(item.FullName, Is.EqualTo(INPUT_NAME));
            Assert.That(item.Item, Is.EqualTo(expectedMatch));
            Assert.That(item.Tags, Is.EqualTo(expectedMatch.Tags));
            Assert.That(item.Uses, Is.Zero);

            item.Item.Received(1).FlagAsMatched();
        }

        [Test]
        public void Constructor_RequiredFields_UsesInItemName()
        {
            string itemName = "Item 2";
            string nameWithUses = $"{itemName} (30)";

            ConvoyConfig config = new ConvoyConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>()
            {
                nameWithUses
            };

            IConvoyItem item = new ConvoyItem(config, data, ITEMS, ENGRAVINGS);
            IItem expectedMatch = ITEMS[itemName];

            Assert.That(item.FullName, Is.EqualTo(nameWithUses));
            Assert.That(item.Item, Is.EqualTo(expectedMatch));
            Assert.That(item.Tags, Is.EqualTo(expectedMatch.Tags));
            Assert.That(item.Uses, Is.EqualTo(30));

            item.Item.Received(1).FlagAsMatched();
        }

        #region OptionalField_Uses

        [TestCase("-1")]
        public void Constructor_OptionalField_Uses_InvalidInputs(string input)
        {
            ConvoyConfig config = new ConvoyConfig()
            {
                Name = 0,
                Uses = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                input
            };

            Assert.Throws<PositiveIntegerException>(() => new ConvoyItem(config, data, ITEMS, ENGRAVINGS));
        }

        [TestCase("", 0)]
        [TestCase("15", 15)]
        public void Constructor_OptionalField_Uses_ValidInputs(string input, int expected)
        {
            ConvoyConfig config = new ConvoyConfig()
            {
                Name = 0,
                Uses = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                input
            };

            IConvoyItem item = new ConvoyItem(config, data, ITEMS, ENGRAVINGS);

            Assert.That(item.Uses, Is.EqualTo(expected));
        }

        #endregion OptionalField_Uses

        #region OptionalField_Stats

        [Test]
        public void Constructor_OptionalField_Stats_EmptyStatList()
        {
            IItem expectedMatch = ITEMS[INPUT_NAME];
            expectedMatch.Stats.Returns(new Dictionary<string, INamedStatValue>());

            ConvoyConfig config = new ConvoyConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME
            };

            IConvoyItem item = new ConvoyItem(config, data, ITEMS, ENGRAVINGS);

            Assert.That(item.Stats, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_Stats()
        {
            string stat1 = "Mt";
            string stat2 = "Hit";

            IDictionary<string, INamedStatValue> expectedMatchStats = new Dictionary<string, INamedStatValue>();
            expectedMatchStats.Add(stat1, new NamedStatValue(1));
            expectedMatchStats.Add(stat2, new NamedStatValue(2));

            IItem expectedMatch = ITEMS[INPUT_NAME];
            expectedMatch.Stats.Returns(expectedMatchStats);

            ConvoyConfig config = new ConvoyConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME
            };

            IConvoyItem item = new ConvoyItem(config, data, ITEMS, ENGRAVINGS);

            Assert.That(item.Stats.Count, Is.EqualTo(2));
            Assert.That(item.Stats.ContainsKey(stat1), Is.True);
            Assert.That(item.Stats.ContainsKey(stat2), Is.True);
            Assert.That(item.Stats[stat1].BaseValue, Is.EqualTo(1));
            Assert.That(item.Stats[stat2].BaseValue, Is.EqualTo(2));
        }

        #endregion OptionalField_Stats

        #region OptionalField_Owner

        [TestCase("")]
        [TestCase("IronPegasus")]
        public void Constructor_OptionalField_Owner_ValidInputs(string input)
        {
            ConvoyConfig config = new ConvoyConfig()
            {
                Name = 0,
                Owner = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                input
            };

            IConvoyItem item = new ConvoyItem(config, data, ITEMS, ENGRAVINGS);

            Assert.That(item.Owner, Is.EqualTo(input));
        }

        #endregion OptionalField_Owner

        #region OptionalField_Value

        [TestCase("-1")]
        public void Constructor_OptionalField_Value_InvalidInputs(string input)
        {
            ConvoyConfig config = new ConvoyConfig()
            {
                Name = 0,
                Value = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                input
            };

            Assert.Throws<PositiveIntegerException>(() => new ConvoyItem(config, data, ITEMS, ENGRAVINGS));
        }

        [TestCase("", -1)]
        [TestCase("100", 100)]
        public void Constructor_OptionalField_Value_ValidInputs(string input, int expected)
        {
            ConvoyConfig config = new ConvoyConfig()
            {
                Name = 0,
                Value = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                input
            };

            IConvoyItem item = new ConvoyItem(config, data, ITEMS, ENGRAVINGS);

            Assert.That(item.Value, Is.EqualTo(expected));
        }

        #endregion OptionalField_Value

        #region OptionalField_Quantity

        [TestCase("-1")]
        [TestCase("0")]
        public void Constructor_OptionalField_Quantity_InvalidInputs(string input)
        {
            ConvoyConfig config = new ConvoyConfig()
            {
                Name = 0,
                Quantity = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                input
            };

            Assert.Throws<NonZeroPositiveIntegerException>(() => new ConvoyItem(config, data, ITEMS, ENGRAVINGS));
        }

        [TestCase("10", 10)]
        public void Constructor_OptionalField_Quantity_ValidInputs(string input, int expected)
        {
            ConvoyConfig config = new ConvoyConfig()
            {
                Name = 0,
                Quantity = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                input
            };

            IConvoyItem item = new ConvoyItem(config, data, ITEMS, ENGRAVINGS);

            Assert.That(item.Quantity, Is.EqualTo(expected));
        }

        #endregion OptionalField_Quantity

        #region OptionalField_Engravings

        [Test]
        public void Constructor_OptionalField_Engravings()
        {
            ConvoyConfig config = new ConvoyConfig()
            {
                Name = 0,
                Engravings = new List<int>() { 1, 2 }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "Engraving 1",
                "Engraving 2"
            };

            IConvoyItem item = new ConvoyItem(config, data, ITEMS, ENGRAVINGS);

            //Engraving 1 is also on Item 1 - this will test the engraving lists Union()
            Assert.That(item.EngravingsList.Count, Is.EqualTo(2));
            Assert.That(item.EngravingsList.Contains(ENGRAVINGS["Engraving 1"]), Is.True);
            Assert.That(item.EngravingsList.Contains(ENGRAVINGS["Engraving 2"]), Is.True);
            item.EngravingsList.ForEach(e => e.Received(1).FlagAsMatched());

            //Both Engraving 1 and 2 have Tag 1 - this tests the tag lists Union()
            Assert.That(item.Tags.Count, Is.EqualTo(1));
            Assert.That(item.Tags.Contains(TAGS["Tag 1"]), Is.True);
        }

        [Test]
        public void Constructor_OptionalField_Engravings_WithStatMods()
        {
            string statName = "Mt";
            IDictionary<string, INamedStatValue> expectedMatchStats = new Dictionary<string, INamedStatValue>();
            expectedMatchStats.Add(statName, new NamedStatValue(1));

            IItem expectedMatch = ITEMS[INPUT_NAME];
            expectedMatch.Stats.Returns(expectedMatchStats);

            IDictionary<string, int> engravingStats = new Dictionary<string, int>();
            engravingStats.Add(statName, 5);

            string engravingName = "Engraving 1";
            IEngraving engraving = ENGRAVINGS[engravingName];
            engraving.ItemStatModifiers.Returns(engravingStats);

            ConvoyConfig config = new ConvoyConfig()
            {
                Name = 0,
                Engravings = new List<int>() { 1 }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                engravingName
            };

            IConvoyItem item = new ConvoyItem(config, data, ITEMS, ENGRAVINGS);

            Assert.That(item.EngravingsList.Count, Is.EqualTo(1));
            Assert.That(item.EngravingsList.Contains(ENGRAVINGS[engravingName]), Is.True);
            item.EngravingsList.ForEach(e => e.Received(1).FlagAsMatched());

            IUnitInventoryItemStat stat = item.MatchStatName(statName);

            Assert.That(stat.BaseValue, Is.EqualTo(1));
            Assert.That(stat.Modifiers.Count, Is.EqualTo(1));
            Assert.That(stat.Modifiers.ContainsKey(engravingName), Is.True);
            Assert.That(stat.FinalValue, Is.EqualTo(6));
        }

        #endregion OptionalField_Engravings

        #region MatchStatItem

        [Test]
        public void MatchStatItem_UnmatchedValue()
        {
            string statName = "Mt";

            IDictionary<string, INamedStatValue> stats = new Dictionary<string, INamedStatValue>();
            stats.Add(statName, new NamedStatValue(1));

            IItem expectedMatch = ITEMS[INPUT_NAME];
            expectedMatch.Stats.Returns(stats);

            ConvoyConfig config = new ConvoyConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME
            };

            IConvoyItem item = new ConvoyItem(config, data, ITEMS, ENGRAVINGS);

            Assert.Throws<UnmatchedStatException>(() => item.MatchStatName("Hit"));
        }

        [Test]
        public void MatchStatItem()
        {
            string statName = "Mt";
            int statValue = 15;

            IDictionary<string, INamedStatValue> stats = new Dictionary<string, INamedStatValue>();
            stats.Add(statName, new NamedStatValue(statValue));

            IItem expectedMatch = ITEMS[INPUT_NAME];
            expectedMatch.Stats.Returns(stats);

            ConvoyConfig config = new ConvoyConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME
            };

            IConvoyItem item = new ConvoyItem(config, data, ITEMS, ENGRAVINGS);
            IUnitInventoryItemStat match = item.MatchStatName(statName);

            Assert.That(match, Is.Not.Null);
            Assert.That(match.BaseValue, Is.EqualTo(statValue));
        }

        #endregion MatchStatItem

        #region BuildList

        [Test]
        public void BuildList_WithInput_Null()
        {
            List<IConvoyItem> list = ConvoyItem.BuildList(null, ITEMS, ENGRAVINGS);

            Assert.That(list, Is.Empty);
        }

        [Test]
        public void BuildList_WithInput_NullQuery()
        {
            ConvoyConfig config = new ConvoyConfig()
            {
                Query = null,
                Name = 0
            };

            List<IConvoyItem> list = ConvoyItem.BuildList(config, ITEMS, ENGRAVINGS);

            Assert.That(list, Is.Empty);
        }

        [Test]
        public void BuildList_WithInput_EmptyQuery()
        {
            ConvoyConfig config = new ConvoyConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ }
                    }
                },
                Name = 0
            };

            List<IConvoyItem> list = ConvoyItem.BuildList(config, ITEMS, ENGRAVINGS);

            Assert.That(list, Is.Empty);
        }

        [Test]
        public void BuildList_WithInput_Invalid()
        {
            ConvoyConfig config = new ConvoyConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ INPUT_NAME, "-1" }
                    }
                },
                Name = 0,
                Uses = 1
            };

            Assert.Throws<ConvoyItemProcessingException>(() => ConvoyItem.BuildList(config, ITEMS, ENGRAVINGS));
        }

        [Test]
        public void BuildList()
        {
            ConvoyConfig config = new ConvoyConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ INPUT_NAME }
                    }
                },
                Name = 0
            };

            List<IConvoyItem> list = ConvoyItem.BuildList(config, ITEMS, ENGRAVINGS);

            Assert.That(list.Count, Is.EqualTo(1));
        }

        [Test]
        public void BuildList_DuplicateItems()
        {
            ConvoyConfig config = new ConvoyConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ "Item 1" },
                        new List<object>(){ "Item 1" },
                        new List<object>(){ "Item 2" },
                        new List<object>(){ "Item 2" }
                    }
                },
                Name = 0
            };

            List<IConvoyItem> list = ConvoyItem.BuildList(config, ITEMS, ENGRAVINGS);

            Assert.That(list.Count, Is.EqualTo(4));
        }

        #endregion BuildList
    }
}
