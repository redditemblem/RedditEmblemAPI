using NSubstitute;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.Shop;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output;
using RedditEmblemAPI.Models.Output.Storage.Shop;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Models.Storage
{
    internal class ShopItemTests
    {
        #region Constants

        private const string INPUT_NAME = "Item 1";
        private const string INPUT_PRICE = "100";
        private const string INPUT_STOCK = "1";

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

            this.TAGS = new Dictionary<string, ITag>();
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
            ShopConfig config = new ShopConfig()
            {
                Name = 0,
                Price = 1,
                Stock = 2
            };

            IEnumerable<string> data = new List<string>();

            Assert.Throws<RequiredValueNotProvidedException>(() => new ShopItem(config, data, ITEMS, ENGRAVINGS));
        }

        [Test]
        public void Constructor_RequiredFields_UnknownItem()
        {
            ShopConfig config = new ShopConfig()
            {
                Name = 0,
                Price = 1,
                Stock = 2
            };

            IEnumerable<string> data = new List<string>()
            {
                "Item 3",
                INPUT_PRICE,
                INPUT_STOCK
            };

            Assert.Throws<UnmatchedItemException>(() => new ShopItem(config, data, ITEMS, ENGRAVINGS));
        }

        [Test]
        public void Constructor_RequiredFields()
        {
            ShopConfig config = new ShopConfig()
            {
                Name = 0,
                Price = 1,
                Stock = 2
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_PRICE,
                INPUT_STOCK
            };

            IShopItem item = new ShopItem(config, data, ITEMS, ENGRAVINGS);
            IItem expectedMatch = ITEMS[INPUT_NAME];

            Assert.That(item.FullName, Is.EqualTo(INPUT_NAME));
            Assert.That(item.Item, Is.EqualTo(expectedMatch));
            Assert.That(item.TagsList, Is.EqualTo(expectedMatch.Tags));
            Assert.That(item.Price, Is.EqualTo(100));
            Assert.That(item.Stock, Is.EqualTo(1));

            item.Item.Received(1).FlagAsMatched();
        }

        #region OptionalField_Stats

        [Test]
        public void Constructor_OptionalField_Stats_EmptyStatList()
        {
            IItem expectedMatch = ITEMS[INPUT_NAME];
            expectedMatch.Stats.Returns(new Dictionary<string, INamedStatValue>());

            ShopConfig config = new ShopConfig()
            {
                Name = 0,
                Price = 1,
                Stock = 2
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_PRICE,
                INPUT_STOCK
            };

            IShopItem item = new ShopItem(config, data, ITEMS, ENGRAVINGS);

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

            ShopConfig config = new ShopConfig()
            {
                Name = 0,
                Price = 1,
                Stock = 2
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_PRICE,
                INPUT_STOCK
            };

            IShopItem item = new ShopItem(config, data, ITEMS, ENGRAVINGS);

            Assert.That(item.Stats.Count, Is.EqualTo(2));
            Assert.That(item.Stats.ContainsKey(stat1), Is.True);
            Assert.That(item.Stats.ContainsKey(stat2), Is.True);
            Assert.That(item.Stats[stat1].BaseValue, Is.EqualTo(1));
            Assert.That(item.Stats[stat2].BaseValue, Is.EqualTo(2));
        }

        #endregion OptionalField_Stats

        #region OptionalField_SalePrice

        [Test]
        public void Constructor_OptionalField_SalePrice_EmptyString()
        {
            ShopConfig config = new ShopConfig()
            {
                Name = 0,
                Price = 1,
                Stock = 2,
                SalePrice = 3
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_PRICE,
                INPUT_STOCK,
                string.Empty
            };

            IShopItem item = new ShopItem(config, data, ITEMS, ENGRAVINGS);

            Assert.That(item.Price, Is.EqualTo(100));
            Assert.That(item.SalePrice, Is.EqualTo(item.Price));
        }

        [TestCase("-1")]
        public void Constructor_OptionalField_SalePrice_InvalidInputs(string input)
        {
            ShopConfig config = new ShopConfig()
            {
                Name = 0,
                Price = 1,
                Stock = 2,
                SalePrice = 3
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_PRICE,
                INPUT_STOCK,
                input
            };

            Assert.Throws<PositiveIntegerException>(() => new ShopItem(config, data, ITEMS, ENGRAVINGS));
        }

        [TestCase("0", 0)]
        [TestCase("1", 1)]
        public void Constructor_OptionalField_SalePrice_ValidInputs(string input, int expected)
        {
            ShopConfig config = new ShopConfig()
            {
                Name = 0,
                Price = 1,
                Stock = 2,
                SalePrice = 3
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_PRICE,
                INPUT_STOCK,
                input
            };

            IShopItem item = new ShopItem(config, data, ITEMS, ENGRAVINGS);

            Assert.That(item.Price, Is.EqualTo(100));
            Assert.That(item.SalePrice, Is.EqualTo(expected));
        }

        #endregion OptionalField_SalePrice

        #region OptionalField_IsNew

        [TestCase("", false)]
        [TestCase("No", false)]
        [TestCase("Yes", true)]
        public void Constructor_OptionalField_IsNew_ValidInputs(string input, bool expected)
        {
            ShopConfig config = new ShopConfig()
            {
                Name = 0,
                Price = 1,
                Stock = 2,
                IsNew = 3
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_PRICE,
                INPUT_STOCK,
                input
            };

            IShopItem item = new ShopItem(config, data, ITEMS, ENGRAVINGS);

            Assert.That(item.IsNew, Is.EqualTo(expected));
        }

        #endregion OptionalField_IsNew

        #region OptionalField_Engravings

        [Test]
        public void Constructor_OptionalField_Engravings()
        {
            ShopConfig config = new ShopConfig()
            {
                Name = 0,
                Price = 1,
                Stock = 2,
                Engravings = new List<int>() { 3, 4 }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_PRICE,
                INPUT_STOCK,
                "Engraving 1",
                "Engraving 2"
            };

            IShopItem item = new ShopItem(config, data, ITEMS, ENGRAVINGS);

            //Engraving 1 is also on Item 1 - this will test the engraving lists Union()
            Assert.That(item.EngravingsList.Count, Is.EqualTo(2));
            Assert.That(item.EngravingsList.Contains(ENGRAVINGS["Engraving 1"]), Is.True);
            Assert.That(item.EngravingsList.Contains(ENGRAVINGS["Engraving 2"]), Is.True);
            item.EngravingsList.ForEach(e => e.Received(1).FlagAsMatched());

            //Both Engraving 1 and 2 have Tag 1 - this tests the tag lists Union()
            Assert.That(item.TagsList.Count, Is.EqualTo(1));
            Assert.That(item.TagsList.Contains(TAGS["Tag 1"]), Is.True);
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

            ShopConfig config = new ShopConfig()
            {
                Name = 0,
                Price = 1,
                Stock = 2,
                Engravings = new List<int>() { 3 }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_PRICE,
                INPUT_STOCK,
                engravingName
            };

            IShopItem item = new ShopItem(config, data, ITEMS, ENGRAVINGS);

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

        #region BuildList

        [Test]
        public void BuildList_WithInput_Null()
        {
            List<IShopItem> list = ShopItem.BuildList(null, ITEMS, ENGRAVINGS);

            Assert.That(list, Is.Empty);
        }

        [Test]
        public void BuildList_WithInput_NullQuery()
        {
            ShopConfig config = new ShopConfig()
            {
                Query = null,
                Name = 0,
                Price = 1,
                Stock = 2
            };

            List<IShopItem> list = ShopItem.BuildList(config, ITEMS, ENGRAVINGS);

            Assert.That(list, Is.Empty);
        }

        [Test]
        public void BuildList_WithInput_EmptyQuery()
        {
            ShopConfig config = new ShopConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ }
                    }
                },
                Name = 0,
                Price = 1,
                Stock = 2
            };

            List<IShopItem> list = ShopItem.BuildList(config, ITEMS, ENGRAVINGS);

            Assert.That(list, Is.Empty);
        }

        [Test]
        public void BuildList_WithInput_Invalid()
        {
            ShopConfig config = new ShopConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ INPUT_NAME, "-1", INPUT_STOCK }
                    }
                },
                Name = 0,
                Price = 1,
                Stock = 2
            };

            Assert.Throws<ShopItemProcessingException>(() => ShopItem.BuildList(config, ITEMS, ENGRAVINGS));
        }

        [Test]
        public void BuildList()
        {
            ShopConfig config = new ShopConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ INPUT_NAME, INPUT_PRICE, INPUT_STOCK }
                    }
                },
                Name = 0,
                Price = 1,
                Stock = 2
            };

            List<IShopItem> list = ShopItem.BuildList(config, ITEMS, ENGRAVINGS);

            Assert.That(list.Count, Is.EqualTo(1));
        }

        [Test]
        public void BuildList_DuplicateItems()
        {
            ShopConfig config = new ShopConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ "Item 1", INPUT_PRICE, INPUT_STOCK },
                        new List<object>(){ "Item 1", INPUT_PRICE, INPUT_STOCK },
                        new List<object>(){ "Item 2", INPUT_PRICE, INPUT_STOCK },
                        new List<object>(){ "Item 2", INPUT_PRICE, INPUT_STOCK }
                    }
                },
                Name = 0,
                Price = 1,
                Stock = 2
            };

            List<IShopItem> list = ShopItem.BuildList(config, ITEMS, ENGRAVINGS);

            Assert.That(list.Count, Is.EqualTo(4));
        }

        #endregion BuildList
    }
}
