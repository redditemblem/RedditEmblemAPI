using NSubstitute;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Tags;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    public class TagTests
    {
        #region Constants

        private const string INPUT_NAME = "Tag Test";

        #endregion Constants

        [Test]
        public void Constructor_RequiredFields_WithInputNull()
        {
            TagsConfig config = new TagsConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>();

            Assert.Throws<RequiredValueNotProvidedException>(() => new Tag(config, data));
        }

        [Test]
        public void Constructor_RequiredFields()
        {
            TagsConfig config = new TagsConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME };

            ITag tag = new Tag(config, data);

            Assert.That(tag.Name, Is.EqualTo(INPUT_NAME));
        }

        #region OptionalField_SpriteURL

        [Test]
        public void Constructor_OptionalField_SpriteURL_EmptyString()
        {
            TagsConfig config = new TagsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, string.Empty };

            ITag tag = new Tag(config, data);

            Assert.That(tag.SpriteURL, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_SpriteURL_InvalidURL()
        {
            TagsConfig config = new TagsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, "NotAURL" };

            Assert.Throws<URLException>(() => new Tag(config, data));
        }

        [Test]
        public void Constructor_OptionalField_SpriteURL()
        {
            TagsConfig config = new TagsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, UnitTestConsts.IMAGE_URL };

            ITag tag = new Tag(config, data);

            Assert.That(tag.SpriteURL, Is.EqualTo(UnitTestConsts.IMAGE_URL));
        }

        #endregion OptionalField_SpriteURL

        #region OptionalField_ShowOnUnit

        [Test]
        public void Constructor_OptionalField_ShowOnUnit_EmptyString()
        {
            TagsConfig config = new TagsConfig()
            {
                Name = 0,
                ShowOnUnit = 1
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, string.Empty };

            ITag tag = new Tag(config, data);

            Assert.That(tag.ShowOnUnit, Is.False);
        }

        [Test]
        public void Constructor_OptionalField_ShowOnUnit_No()
        {
            TagsConfig config = new TagsConfig()
            {
                Name = 0,
                ShowOnUnit = 1
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, "No" };

            ITag tag = new Tag(config, data);

            Assert.That(tag.ShowOnUnit, Is.False);
        }

        [Test]
        public void Constructor_OptionalField_ShowOnUnit_Yes_MissingSpriteURL()
        {
            TagsConfig config = new TagsConfig()
            {
                Name = 0,
                ShowOnUnit = 1
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, "Yes" };

            ITag tag = new Tag(config, data);

            Assert.That(tag.ShowOnUnit, Is.False);
        }

        [Test]
        public void Constructor_OptionalField_ShowOnUnit_Yes_WithSpriteURL()
        {
            TagsConfig config = new TagsConfig()
            {
                Name = 0,
                SpriteURL = 1,
                ShowOnUnit = 2
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, UnitTestConsts.IMAGE_URL, "Yes" };

            ITag tag = new Tag(config, data);

            Assert.That(tag.ShowOnUnit, Is.True);
        }

        #endregion OptionalField_ShowOnUnit

        #region OptionalField_UnitAura

        [Test]
        public void Constructor_OptionalField_UnitAura_EmptyString()
        {
            TagsConfig config = new TagsConfig()
            {
                Name = 0,
                UnitAura = 1
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, string.Empty };

            ITag tag = new Tag(config, data);

            Assert.That(tag.UnitAura, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_UnitAura_InvalidHex()
        {
            TagsConfig config = new TagsConfig()
            {
                Name = 0,
                UnitAura = 1
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, "NotAHex" };

            Assert.Throws<HexException>(() => new Tag(config, data));
        }

        [Test]
        public void Constructor_OptionalField_UnitAura()
        {
            string hexcode = "#F0F0F0";

            TagsConfig config = new TagsConfig()
            {
                Name = 0,
                UnitAura = 1
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, hexcode };

            ITag tag = new Tag(config, data);

            Assert.That(tag.UnitAura, Is.EqualTo(hexcode));
        }

        #endregion OptionalField_UnitAura

        #region FlagAsMatched

        [Test]
        public void FlagAsMatched()
        {
            TagsConfig config = new TagsConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME };

            ITag tag = new Tag(config, data);

            Assert.That(tag.Matched, Is.False);

            tag.FlagAsMatched();

            Assert.That(tag.Matched, Is.True);
        }

        #endregion FlagAsMatched

        #region BuildDictionary

        [Test]
        public void BuildDictionary_WithQueryNull()
        {
            TagsConfig config = new TagsConfig();

            IDictionary<string, ITag> dict = Tag.BuildDictionary(config);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_Null()
        {
            TagsConfig config = new TagsConfig()
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

            IDictionary<string, ITag> dict = Tag.BuildDictionary(config);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_DuplicateName()
        {
            TagsConfig config = new TagsConfig()
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

            Assert.Throws<TagProcessingException>(() => Tag.BuildDictionary(config));
        }

        [Test]
        public void BuildDictionary()
        {
            TagsConfig config = new TagsConfig()
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

            IDictionary<string, ITag> dict = Tag.BuildDictionary(config);
            Assert.That(dict.Count, Is.EqualTo(1));
        }

        [Test]
        public void BuildDictionary_MultiQuery()
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
                    },
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Tag 3" },
                            new List<object>(){ "Tag 4" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, ITag> dict = Tag.BuildDictionary(config);
            Assert.That(dict.Count, Is.EqualTo(4));
        }

        #endregion BuildDictionary

        #region MatchNames

        [Test]
        public void MatchNames_UnmatchedName()
        {
            string tag1Name = "Tag 1";
            string tag2Name = "Tag 2";

            ITag tag1 = Substitute.For<ITag>();
            tag1.Name.Returns(tag1Name);

            IDictionary<string, ITag> dict = new Dictionary<string, ITag>();
            dict.Add(tag1Name, tag1);

            IEnumerable<string> names = new List<string>() { tag2Name };

            Assert.Throws<UnmatchedTagException>(() => Tag.MatchNames(dict, names));
        }

        [Test]
        public void MatchNames_SingleMatch()
        {
            string tag1Name = "Tag 1";
            string tag2Name = "Tag 2";

            ITag tag1 = Substitute.For<ITag>();
            tag1.Name.Returns(tag1Name);

            ITag tag2 = Substitute.For<ITag>();
            tag2.Name.Returns(tag2Name);

            IDictionary<string, ITag> dict = new Dictionary<string, ITag>();
            dict.Add(tag1Name, tag1);
            dict.Add(tag2Name, tag2);

            IEnumerable<string> names = new List<string>() { tag1Name };
            List<ITag> matches = Tag.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(1));
            Assert.That(matches.Contains(tag1), Is.True);
            matches.First().Received(1).FlagAsMatched();
        }

        [Test]
        public void MatchNames_MultipleMatches()
        {
            string tag1Name = "Tag 1";
            string tag2Name = "Tag 2";

            ITag tag1 = Substitute.For<ITag>();
            tag1.Name.Returns(tag1Name);

            ITag tag2 = Substitute.For<ITag>();
            tag2.Name.Returns(tag2Name);

            IDictionary<string, ITag> dict = new Dictionary<string, ITag>();
            dict.Add(tag1Name, tag1);
            dict.Add(tag2Name, tag2);

            IEnumerable<string> names = new List<string>() { tag1Name, tag2Name };
            List<ITag> matches = Tag.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches.Contains(tag1), Is.True);
            Assert.That(matches.Contains(tag2), Is.True);

            matches[0].Received(1).FlagAsMatched();
            matches[1].Received(1).FlagAsMatched();
        }

        [Test]
        public void MatchNames_MultipleMatches_DoNotSetMatchedStatus()
        {
            string tag1Name = "Tag 1";
            string tag2Name = "Tag 2";

            ITag tag1 = Substitute.For<ITag>();
            tag1.Name.Returns(tag1Name);

            ITag tag2 = Substitute.For<ITag>();
            tag2.Name.Returns(tag2Name);

            IDictionary<string, ITag> dict = new Dictionary<string, ITag>();
            dict.Add(tag1Name, tag1);
            dict.Add(tag2Name, tag2);

            IEnumerable<string> names = new List<string>() { tag1Name, tag2Name };
            List<ITag> matches = Tag.MatchNames(dict, names, false);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches.Contains(tag1), Is.True);
            Assert.That(matches.Contains(tag2), Is.True);

            matches[0].DidNotReceive().FlagAsMatched();
            matches[1].DidNotReceive().FlagAsMatched();
        }

        #endregion MatchNames

        #region MatchName

        [Test]
        public void MatchName_UnmatchedName()
        {
            string tag1Name = "Tag 1";

            ITag tag1 = Substitute.For<ITag>();
            tag1.Name.Returns(tag1Name);

            IDictionary<string, ITag> dict = new Dictionary<string, ITag>();
            dict.Add(tag1Name, tag1);

            string name = "Tag 2";

            Assert.Throws<UnmatchedTagException>(() => Tag.MatchName(dict, name));
        }

        [Test]
        public void MatchName()
        {
            string tag1Name = "Tag 1";

            ITag tag1 = Substitute.For<ITag>();
            tag1.Name.Returns(tag1Name);

            IDictionary<string, ITag> dict = new Dictionary<string, ITag>();
            dict.Add(tag1Name, tag1);

            ITag match = Tag.MatchName(dict, tag1Name);

            Assert.That(match, Is.Not.Null);
            Assert.That(match, Is.EqualTo(tag1));
            match.Received(1).FlagAsMatched();
        }

        [Test]
        public void MatchName_DoNotSetMatchedStatus()
        {
            string tag1Name = "Tag 1";

            ITag tag1 = Substitute.For<ITag>();
            tag1.Name.Returns(tag1Name);

            IDictionary<string, ITag> dict = new Dictionary<string, ITag>();
            dict.Add(tag1Name, tag1);

            ITag match = Tag.MatchName(dict, tag1Name, false);

            Assert.That(match, Is.Not.Null);
            Assert.That(match, Is.EqualTo(tag1));
            match.DidNotReceive().FlagAsMatched();
        }

        #endregion MatchName
    }
}