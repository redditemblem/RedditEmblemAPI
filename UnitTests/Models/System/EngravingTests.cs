using NSubstitute;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Items;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    public class EngravingTests
    {
        #region Constants

        private const string INPUT_NAME = "Engraving Test";

        #endregion Constants

        #region Setup

        private IDictionary<string, ITag> TAGS;

        [SetUp]
        public void Setup()
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

        #endregion Setup

        [Test]
        public void Constructor_RequiredFields_WithInputNull()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>() { };

            Assert.Throws<RequiredValueNotProvidedException>(() => new Engraving(config, data, TAGS));
        }

        [Test]
        public void Constructor_RequiredFields()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME };

            IEngraving engraving = new Engraving(config, data, TAGS);

            Assert.That(engraving.Name, Is.EqualTo(INPUT_NAME));
        }

        #region OptionalField_SpriteURL

        [Test]
        public void Constructor_OptionalField_SpriteURL_EmptyString()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            IEngraving engraving = new Engraving(config, data, TAGS);

            Assert.That(engraving.SpriteURL, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_SpriteURL_InvalidURL()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "NotAURL"
            };

            Assert.Throws<URLException>(() => new Engraving(config, data, TAGS));
        }

        [Test]
        public void Constructor_OptionalField_SpriteURL()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL
            };

            IEngraving engraving = new Engraving(config, data, TAGS);

            Assert.That(engraving.SpriteURL, Is.EqualTo(UnitTestConsts.IMAGE_URL));
        }

        #endregion OptionalField_SpriteURL

        #region OptionalField_ItemStatModifiers

        [Test]
        public void Constructor_OptionalField_ItemStatModifiers_EmptyString()
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

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty,
                string.Empty
            };

            IEngraving engraving = new Engraving(config, data, TAGS);

            Assert.That(engraving.ItemStatModifiers, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_ItemStatModifiers()
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

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "1",
                "-1"
            };

            IEngraving engraving = new Engraving(config, data, TAGS);

            Assert.That(engraving.ItemStatModifiers.Count, Is.EqualTo(2));
            Assert.That(engraving.ItemStatModifiers[stat1], Is.EqualTo(1));
            Assert.That(engraving.ItemStatModifiers[stat2], Is.EqualTo(-1));
        }

        #endregion OptionalField_ItemStatModifiers

        #region OptionalField_ItemRangeOverrides

        [Test]
        public void Constructor_OptionalField_ItemRangeOverrides_EmptyStrings()
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

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty,
                string.Empty
            };

            IEngraving engraving = new Engraving(config, data, TAGS);

            Assert.That(engraving.ItemRangeOverrides.Minimum, Is.EqualTo(0));
            Assert.That(engraving.ItemRangeOverrides.Maximum, Is.EqualTo(0));
            Assert.That(engraving.ItemRangeOverrides.Shape, Is.EqualTo(ItemRangeShape.Standard));
            Assert.That(engraving.ItemRangeOverrides.CanOnlyUseBeforeMovement, Is.False);
        }

        [Test]
        public void Constructor_OptionalField_ItemRangeOverrides()
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

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "1",
                "2"
            };

            IEngraving engraving = new Engraving(config, data, TAGS);

            Assert.That(engraving.ItemRangeOverrides.Minimum, Is.EqualTo(1));
            Assert.That(engraving.ItemRangeOverrides.Maximum, Is.EqualTo(2));
            Assert.That(engraving.ItemRangeOverrides.Shape, Is.EqualTo(ItemRangeShape.Standard));
            Assert.That(engraving.ItemRangeOverrides.CanOnlyUseBeforeMovement, Is.False);
        }

        [Test]
        public void Constructor_OptionalField_ItemRangeOverrides_WithOptionalFields()
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

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "1",
                "2",
                "Cross",
                "Yes"
            };

            IEngraving engraving = new Engraving(config, data, TAGS);

            Assert.That(engraving.ItemRangeOverrides.Minimum, Is.EqualTo(1));
            Assert.That(engraving.ItemRangeOverrides.Maximum, Is.EqualTo(2));
            Assert.That(engraving.ItemRangeOverrides.Shape, Is.EqualTo(ItemRangeShape.Cross));
            Assert.That(engraving.ItemRangeOverrides.CanOnlyUseBeforeMovement, Is.True);
        }

        #endregion OptionalField_ItemRangeOverrides

        #region OptionalField_CombatStatModifiers

        [Test]
        public void Constructor_OptionalField_CombatStatModifiers_EmptyString()
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

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty,
                string.Empty
            };

            IEngraving engraving = new Engraving(config, data, TAGS);

            Assert.That(engraving.CombatStatModifiers, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_CombatStatModifiers()
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

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "1",
                "-1"
            };

            IEngraving engraving = new Engraving(config, data, TAGS);

            Assert.That(engraving.CombatStatModifiers.Count, Is.EqualTo(2));
            Assert.That(engraving.CombatStatModifiers[stat1], Is.EqualTo(1));
            Assert.That(engraving.CombatStatModifiers[stat2], Is.EqualTo(-1));
        }

        #endregion OptionalField_CombatStatModifiers

        #region OptionalField_StatModifiers

        [Test]
        public void Constructor_OptionalField_StatModifiers_EmptyString()
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

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty,
                string.Empty
            };

            IEngraving engraving = new Engraving(config, data, TAGS);

            Assert.That(engraving.StatModifiers, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_StatModifiers()
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

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "1",
                "-1"
            };

            IEngraving engraving = new Engraving(config, data, TAGS);

            Assert.That(engraving.StatModifiers.Count, Is.EqualTo(2));
            Assert.That(engraving.StatModifiers[stat1], Is.EqualTo(1));
            Assert.That(engraving.StatModifiers[stat2], Is.EqualTo(-1));
        }

        #endregion OptionalField_StatModifiers

        #region OptionalField_Tags

        [Test]
        public void Constructor_OptionalField_Tags_EmptyString()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                Tags = new List<int> { 1, 2 }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty,
                string.Empty
            };

            IEngraving engraving = new Engraving(config, data, TAGS);

            Assert.That(engraving.Tags, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_Tags_UnmatchedTag()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                Tags = new List<int> { 1 }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "Tag 3"
            };

            Assert.Throws<UnmatchedTagException>(() => new Engraving(config, data, TAGS));
        }

        [Test]
        public void Constructor_OptionalField_Tags_DuplicateTags()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                Tags = new List<int> { 1, 2 }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "Tag 1,Tag 1",
                "Tag 1"
            };

            IEngraving engraving = new Engraving(config, data, TAGS);

            Assert.That(engraving.Tags.Count, Is.EqualTo(1));
            engraving.Tags.First().DidNotReceive().FlagAsMatched();
        }

        [Test]
        public void Constructor_OptionalField_Tags_MultipleSameField()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                Tags = new List<int> { 1 }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "Tag 1,Tag 2"
            };

            IEngraving engraving = new Engraving(config, data, TAGS);

            Assert.That(engraving.Tags.Count, Is.EqualTo(2));
            engraving.Tags.ForEach(t => t.DidNotReceive().FlagAsMatched());
        }

        [Test]
        public void Constructor_OptionalField_Tags_MultipleSeparateFields()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                Tags = new List<int> { 1, 2 }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "Tag 1",
                "Tag 2"
            };

            IEngraving engraving = new Engraving(config, data, TAGS);

            Assert.That(engraving.Tags.Count, Is.EqualTo(2));
            engraving.Tags.ForEach(t => t.DidNotReceive().FlagAsMatched());
        }

        #endregion OptionalField_Tags

        #region OptionalField_TextFields

        [Test]
        public void Constructor_OptionalField_TextFields_EmptyString()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                TextFields = new List<int>() { 1, 2 }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty,
                string.Empty
            };

            IEngraving engraving = new Engraving(config, data, TAGS);

            Assert.That(engraving.TextFields, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_TextFields()
        {
            string textField1 = "Text Field 1";
            string textField2 = "Text Field 2";

            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                TextFields = new List<int>() { 1, 2 }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                textField1,
                textField2
            };

            IEngraving engraving = new Engraving(config, data, TAGS);

            IEnumerable<string> expected = new List<string>() { textField1, textField2 };
            Assert.That(engraving.TextFields, Is.EqualTo(expected));
        }

        #endregion OptionalField_TextFields

        #region FlagAsMatched

        [Test]
        public void FlagAsMatched_WithTags()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0,
                Tags = new List<int> { 1 }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "Tag 1"
            };

            IEngraving engraving = new Engraving(config, data, TAGS);

            Assert.That(engraving.Matched, Is.False);
            engraving.Tags.First().DidNotReceive().FlagAsMatched();

            engraving.FlagAsMatched();

            Assert.That(engraving.Matched, Is.True);
            engraving.Tags.First().Received().FlagAsMatched();
        }

        #endregion FlagAsMatched

        #region BuildDictionary

        [Test]
        public void BuildDictionary_WithInput_Null()
        {
            IDictionary<string, IEngraving> dict = Engraving.BuildDictionary(null, TAGS);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_NullQuery()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Queries = null,
                Name = 0
            };

            IDictionary<string, IEngraving> dict = Engraving.BuildDictionary(config, TAGS);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_EmptyQuery()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Queries = new List<IQuery>()
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

            IDictionary<string, IEngraving> dict = Engraving.BuildDictionary(config, TAGS);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_DuplicateName()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Queries = new List<IQuery>()
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

            Assert.Throws<EngravingProcessingException>(() => Engraving.BuildDictionary(config, TAGS));
        }

        [Test]
        public void BuildDictionary()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Queries = new List<IQuery>()
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

            IDictionary<string, IEngraving> dict = Engraving.BuildDictionary(config, TAGS);
            Assert.That(dict.Count, Is.EqualTo(1));
        }

        [Test]
        public void BuildDictionary_MultiQuery()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Queries = new List<IQuery>()
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

            IDictionary<string, IEngraving> dict = Engraving.BuildDictionary(config, TAGS);
            Assert.That(dict.Count, Is.EqualTo(4));
        }

        #endregion BuildDictionary

        #region MatchNames

        [Test]
        public void MatchNames_UnmatchedName()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Queries = new List<IQuery>()
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

            IDictionary<string, IEngraving> dict = Engraving.BuildDictionary(config, TAGS);
            IEnumerable<string> names = new List<string>() { "Engraving 3" };

            Assert.Throws<UnmatchedEngravingException>(() => Engraving.MatchNames(dict, names));
        }

        [Test]
        public void MatchNames_SingleMatch()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Queries = new List<IQuery>()
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

            IDictionary<string, IEngraving> dict = Engraving.BuildDictionary(config, TAGS);
            IEnumerable<string> names = new List<string>() { "Engraving 1" };

            List<IEngraving> matches = Engraving.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(1));
            Assert.That(matches.First().Matched, Is.True);
        }

        [Test]
        public void MatchNames_MultipleMatches()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Queries = new List<IQuery>()
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

            IDictionary<string, IEngraving> dict = Engraving.BuildDictionary(config, TAGS);
            IEnumerable<string> names = new List<string>() { "Engraving 1", "Engraving 2" };

            List<IEngraving> matches = Engraving.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches[0].Matched, Is.True);
            Assert.That(matches[1].Matched, Is.True);
        }

        [Test]
        public void MatchNames_MultipleMatches_DoNotSetMatchedStatus()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Queries = new List<IQuery>()
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

            IDictionary<string, IEngraving> dict = Engraving.BuildDictionary(config, TAGS);
            IEnumerable<string> names = new List<string>() { "Engraving 1", "Engraving 2" };

            List<IEngraving> matches = Engraving.MatchNames(dict, names, false);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches[0].Matched, Is.False);
            Assert.That(matches[1].Matched, Is.False);
        }

        #endregion MatchNames
    }
}
