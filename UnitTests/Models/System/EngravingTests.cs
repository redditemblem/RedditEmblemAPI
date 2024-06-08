using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Items;
using RedditEmblemAPI.Models.Configuration.System.Tags;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    [TestClass]
    public class EngravingTests
    {
        #region Constants

        private const string INPUT_NAME = "Engraving Test";

        #endregion Constants

        #region Setup

        private IDictionary<string, Tag> DICTIONARY_TAGS = new Dictionary<string, Tag>();

        [TestInitialize]
        public void Setup()
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

        #endregion Setup

        [TestMethod]
        public void EngravingConstructor_RequiredFields_WithInputNull()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0
            };

            List<string> data = new List<string>() { };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new Engraving(config, data, DICTIONARY_TAGS));
        }

        [TestMethod]
        public void EngravingConstructor_RequiredFields()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0
            };

            List<string> data = new List<string>() { INPUT_NAME };

            Engraving engraving = new Engraving(config, data, DICTIONARY_TAGS);

            Assert.AreEqual<string>(INPUT_NAME, engraving.Name);
        }

        #region OptionalField_SpriteURL

        [TestMethod]
        public void EngravingConstructor_OptionalField_SpriteURL_EmptyString()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            Engraving engraving = new Engraving(config, data, DICTIONARY_TAGS);

            Assert.AreEqual<string>(string.Empty, engraving.SpriteURL);
        }

        [TestMethod]
        public void EngravingConstructor_OptionalField_SpriteURL_InvalidURL()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                "NotAURL"
            };

            Assert.ThrowsException<URLException>(() => new Engraving(config, data, DICTIONARY_TAGS));
        }

        [TestMethod]
        public void EngravingConstructor_OptionalField_SpriteURL()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL
            };

            Engraving engraving = new Engraving(config, data, DICTIONARY_TAGS);

            Assert.AreEqual<string>(UnitTestConsts.IMAGE_URL, engraving.SpriteURL);
        }

        #endregion OptionalField_SpriteURL

        #region OptionalField_ItemStatModifiers

        [TestMethod]
        public void EngravingConstructor_OptionalField_ItemStatModifiers_EmptyString()
        {
            string stat1 = "Stat 1";
            string stat2 = "Stat 2";

            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                ItemStatModifiers = new List<NamedStatConfig>()
                {
                    new NamedStatConfig { SourceName = stat1, Value = 1 },
                    new NamedStatConfig { SourceName = stat2, Value = 2 }
                }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty,
                string.Empty
            };

            Engraving engraving = new Engraving(config, data, DICTIONARY_TAGS);

            Assert.AreEqual<int>(0, engraving.ItemStatModifiers.Count);
        }

        [TestMethod]
        public void EngravingConstructor_OptionalField_ItemStatModifiers()
        {
            string stat1 = "Stat 1";
            string stat2 = "Stat 2";

            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                ItemStatModifiers = new List<NamedStatConfig>()
                {
                    new NamedStatConfig { SourceName = stat1, Value = 1 },
                    new NamedStatConfig { SourceName = stat2, Value = 2 }
                }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                "1",
                "-1"
            };

            Engraving engraving = new Engraving(config, data, DICTIONARY_TAGS);

            Assert.AreEqual<int>(2, engraving.ItemStatModifiers.Count);
            Assert.AreEqual<int>(1, engraving.ItemStatModifiers[stat1]);
            Assert.AreEqual<int>(-1, engraving.ItemStatModifiers[stat2]);
        }

        #endregion OptionalField_ItemStatModifiers

        #region OptionalField_ItemRangeOverrides

        [TestMethod]
        public void EngravingConstructor_OptionalField_ItemRangeOverrides_EmptyStrings()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                ItemRangeOverrides = new ItemRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty,
                string.Empty
            };

            Engraving engraving = new Engraving(config, data, DICTIONARY_TAGS);

            Assert.AreEqual<int>(0, engraving.ItemRangeOverrides.Minimum);
            Assert.AreEqual<int>(0, engraving.ItemRangeOverrides.Maximum);
            Assert.AreEqual<ItemRangeShape>(ItemRangeShape.Standard, engraving.ItemRangeOverrides.Shape);
            Assert.IsFalse(engraving.ItemRangeOverrides.CanOnlyUseBeforeMovement);
        }

        [TestMethod]
        public void EngravingConstructor_OptionalField_ItemRangeOverrides()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                ItemRangeOverrides = new ItemRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                "1",
                "2"
            };

            Engraving engraving = new Engraving(config, data, DICTIONARY_TAGS);

            Assert.AreEqual<int>(1, engraving.ItemRangeOverrides.Minimum);
            Assert.AreEqual<int>(2, engraving.ItemRangeOverrides.Maximum);
            Assert.AreEqual<ItemRangeShape>(ItemRangeShape.Standard, engraving.ItemRangeOverrides.Shape);
            Assert.IsFalse(engraving.ItemRangeOverrides.CanOnlyUseBeforeMovement);
        }

        [TestMethod]
        public void EngravingConstructor_OptionalField_ItemRangeOverrides_WithOptionalFields()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                ItemRangeOverrides = new ItemRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2,
                    Shape = 3,
                    CanOnlyUseBeforeMovement = 4
                }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                "1",
                "2",
                "Cross",
                "Yes"
            };

            Engraving engraving = new Engraving(config, data, DICTIONARY_TAGS);

            Assert.AreEqual<int>(1, engraving.ItemRangeOverrides.Minimum);
            Assert.AreEqual<int>(2, engraving.ItemRangeOverrides.Maximum);
            Assert.AreEqual<ItemRangeShape>(ItemRangeShape.Cross, engraving.ItemRangeOverrides.Shape);
            Assert.IsTrue(engraving.ItemRangeOverrides.CanOnlyUseBeforeMovement);
        }

        #endregion OptionalField_ItemRangeOverrides

        #region OptionalField_CombatStatModifiers

        [TestMethod]
        public void EngravingConstructor_OptionalField_CombatStatModifiers_EmptyString()
        {
            string stat1 = "Stat 1";
            string stat2 = "Stat 2";

            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                CombatStatModifiers = new List<NamedStatConfig>()
                {
                    new NamedStatConfig { SourceName = stat1, Value = 1 },
                    new NamedStatConfig { SourceName = stat2, Value = 2 }
                }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty,
                string.Empty
            };

            Engraving engraving = new Engraving(config, data, DICTIONARY_TAGS);

            Assert.AreEqual<int>(0, engraving.CombatStatModifiers.Count);
        }

        [TestMethod]
        public void EngravingConstructor_OptionalField_CombatStatModifiers()
        {
            string stat1 = "Stat 1";
            string stat2 = "Stat 2";

            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                CombatStatModifiers = new List<NamedStatConfig>()
                {
                    new NamedStatConfig { SourceName = stat1, Value = 1 },
                    new NamedStatConfig { SourceName = stat2, Value = 2 }
                }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                "1",
                "-1"
            };

            Engraving engraving = new Engraving(config, data, DICTIONARY_TAGS);

            Assert.AreEqual<int>(2, engraving.CombatStatModifiers.Count);
            Assert.AreEqual<int>(1, engraving.CombatStatModifiers[stat1]);
            Assert.AreEqual<int>(-1, engraving.CombatStatModifiers[stat2]);
        }

        #endregion OptionalField_CombatStatModifiers

        #region OptionalField_StatModifiers

        [TestMethod]
        public void EngravingConstructor_OptionalField_StatModifiers_EmptyString()
        {
            string stat1 = "Stat 1";
            string stat2 = "Stat 2";

            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                StatModifiers = new List<NamedStatConfig>()
                {
                    new NamedStatConfig { SourceName = stat1, Value = 1 },
                    new NamedStatConfig { SourceName = stat2, Value = 2 }
                }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty,
                string.Empty
            };

            Engraving engraving = new Engraving(config, data, DICTIONARY_TAGS);

            Assert.AreEqual<int>(0, engraving.StatModifiers.Count);
        }

        [TestMethod]
        public void EngravingConstructor_OptionalField_StatModifiers()
        {
            string stat1 = "Stat 1";
            string stat2 = "Stat 2";

            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                StatModifiers = new List<NamedStatConfig>()
                {
                    new NamedStatConfig { SourceName = stat1, Value = 1 },
                    new NamedStatConfig { SourceName = stat2, Value = 2 }
                }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                "1",
                "-1"
            };

            Engraving engraving = new Engraving(config, data, DICTIONARY_TAGS);

            Assert.AreEqual<int>(2, engraving.StatModifiers.Count);
            Assert.AreEqual<int>(1, engraving.StatModifiers[stat1]);
            Assert.AreEqual<int>(-1, engraving.StatModifiers[stat2]);
        }

        #endregion OptionalField_StatModifiers

        #region OptionalField_Tags

        [TestMethod]
        public void EngravingConstructor_OptionalField_Tags_EmptyString()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                Tags = new List<int> { 1, 2 }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty,
                string.Empty
            };

            Engraving engraving = new Engraving(config, data, DICTIONARY_TAGS);

            Assert.AreEqual<int>(0, engraving.Tags.Count);
        }

        [TestMethod]
        public void EngravingConstructor_OptionalField_Tags_UnmatchedTag()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                Tags = new List<int> { 1 }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                "Tag 3"
            };

            Assert.ThrowsException<UnmatchedTagException>(() => new Engraving(config, data, DICTIONARY_TAGS));
        }

        [TestMethod]
        public void EngravingConstructor_OptionalField_Tags_DuplicateTags()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                Tags = new List<int> { 1, 2 }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                "Tag 1,Tag 1",
                "Tag 1"
            };

            Engraving engraving = new Engraving(config, data, DICTIONARY_TAGS);

            Assert.AreEqual<int>(1, engraving.Tags.Count);
        }

        [TestMethod]
        public void EngravingConstructor_OptionalField_Tags_MultipleSameField()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                Tags = new List<int> { 1 }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                "Tag 1,Tag 2"
            };

            Engraving engraving = new Engraving(config, data, DICTIONARY_TAGS);

            Assert.AreEqual<int>(2, engraving.Tags.Count);
        }

        [TestMethod]
        public void EngravingConstructor_OptionalField_Tags_MultipleSeparateFields()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                Tags = new List<int> { 1, 2 }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                "Tag 1",
                "Tag 2"
            };

            Engraving engraving = new Engraving(config, data, DICTIONARY_TAGS);

            Assert.AreEqual<int>(2, engraving.Tags.Count);
        }

        #endregion OptionalField_Tags

        #region OptionalField_TextFields

        [TestMethod]
        public void EngravingConstructor_OptionalField_TextFields_EmptyString()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                TextFields = new List<int>() { 1, 2 }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty,
                string.Empty
            };

            Engraving engraving = new Engraving(config, data, DICTIONARY_TAGS);

            CollectionAssert.AreEqual(new List<string>() { }, engraving.TextFields);
        }

        [TestMethod]
        public void EngravingConstructor_OptionalField_TextFields()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                TextFields = new List<int>() { 1, 2 }
            };

            string field1 = "Text Field 1";
            string field2 = "Text Field 2";

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                field1,
                field2
            };

            Engraving engraving = new Engraving(config, data, DICTIONARY_TAGS);

            CollectionAssert.AreEqual(new List<string>() { field1, field2 }, engraving.TextFields);
        }

        #endregion OptionalField_TextFields

        #region FlagAsMatched

        [TestMethod]
        public void Engraving_FlagAsMatched()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                Tags = new List<int> { 1 }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                "Tag 1"
            };

            Engraving engraving = new Engraving(config, data, DICTIONARY_TAGS);

            Assert.IsFalse(engraving.Matched);
            Assert.IsFalse(engraving.Tags.First().Matched);

            engraving.FlagAsMatched();

            Assert.IsTrue(engraving.Matched);
            Assert.IsTrue(engraving.Tags.First().Matched);
        }

        #endregion FlagAsMatched

        #region BuildDictionary

        [TestMethod]
        public void Engraving_BuildDictionary_WithInput_Null()
        {
            IDictionary<string, Engraving> dict = Engraving.BuildDictionary(null, DICTIONARY_TAGS);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void Engraving_BuildDictionary_WithInput_NullQuery()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Queries = null,
                Name = 0
            };

            IDictionary<string, Engraving> dict = Engraving.BuildDictionary(config, DICTIONARY_TAGS);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void Engraving_BuildDictionary_WithInput_EmptyQuery()
        {
            EngravingsConfig config = new EngravingsConfig()
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
                Name = 0
            };

            IDictionary<string, Engraving> dict = Engraving.BuildDictionary(config, DICTIONARY_TAGS);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void Engraving_BuildDictionary_WithInput_DuplicateName()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_NAME },
                            new List<object>(){ INPUT_NAME }
                        }
                    }
                },
                Name = 0
            };

            Assert.ThrowsException<EngravingProcessingException>(() => Engraving.BuildDictionary(config, DICTIONARY_TAGS));
        }

        [TestMethod]
        public void Engraving_BuildDictionary()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_NAME }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, Engraving> dict = Engraving.BuildDictionary(config, DICTIONARY_TAGS);
            Assert.AreEqual<int>(1, dict.Count);
        }

        [TestMethod]
        public void Engraving_BuildDictionary_MultiQuery()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Engraving 1" },
                            new List<object>(){ "Engraving 2" }
                        }
                    },
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Engraving 3" },
                            new List<object>(){ "Engraving 4" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, Engraving> dict = Engraving.BuildDictionary(config, DICTIONARY_TAGS);
            Assert.AreEqual<int>(4, dict.Count);
        }

        #endregion BuildDictionary

        #region MatchNames

        [TestMethod]
        public void Engraving_MatchNames_UnmatchedName()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Engraving 1" },
                            new List<object>(){ "Engraving 2" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, Engraving> dict = Engraving.BuildDictionary(config, DICTIONARY_TAGS);
            IEnumerable<string> names = new List<string>() { "Engraving 3" };

            Assert.ThrowsException<UnmatchedEngravingException>(() => Engraving.MatchNames(dict, names));
        }

        [TestMethod]
        public void Engraving_MatchNames_SingleMatch()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Engraving 1" },
                            new List<object>(){ "Engraving 2" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, Engraving> dict = Engraving.BuildDictionary(config, DICTIONARY_TAGS);
            IEnumerable<string> names = new List<string>() { "Engraving 1" };

            List<Engraving> matches = Engraving.MatchNames(dict, names);
            Assert.AreEqual(1, matches.Count);
            Assert.IsTrue(matches.First().Matched);
        }

        [TestMethod]
        public void Engraving_MatchNames_MultipleMatches()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Engraving 1" },
                            new List<object>(){ "Engraving 2" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, Engraving> dict = Engraving.BuildDictionary(config, DICTIONARY_TAGS);
            IEnumerable<string> names = new List<string>() { "Engraving 1", "Engraving 2" };

            List<Engraving> matches = Engraving.MatchNames(dict, names);
            Assert.AreEqual(2, matches.Count);
            Assert.IsTrue(matches[0].Matched);
            Assert.IsTrue(matches[1].Matched);
        }

        [TestMethod]
        public void Engraving_MatchNames_MultipleMatches_SetMatchedStatus()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Engraving 1" },
                            new List<object>(){ "Engraving 2" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, Engraving> dict = Engraving.BuildDictionary(config, DICTIONARY_TAGS);
            IEnumerable<string> names = new List<string>() { "Engraving 1", "Engraving 2" };

            List<Engraving> matches = Engraving.MatchNames(dict, names, true);
            Assert.AreEqual(2, matches.Count);
            Assert.IsFalse(matches[0].Matched);
            Assert.IsFalse(matches[1].Matched);
        }

        #endregion MatchNames
    }
}
