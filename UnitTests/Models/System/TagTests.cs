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

            IDictionary<string, ITag> dict = Tag.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Tag 3" };

            Assert.Throws<UnmatchedTagException>(() => Tag.MatchNames(dict, names));
        }

        [Test]
        public void MatchNames_SingleMatch()
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

            IDictionary<string, ITag> dict = Tag.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Tag 1" };

            List<ITag> matches = Tag.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(1));
            Assert.That(matches.First().Matched, Is.True);
        }

        [Test]
        public void MatchNames_MultipleMatches()
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

            IDictionary<string, ITag> dict = Tag.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Tag 1", "Tag 2" };

            List<ITag> matches = Tag.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches[0].Matched, Is.True);
            Assert.That(matches[1].Matched, Is.True);
        }

        [Test]
        public void MatchNames_MultipleMatches_DoNotSetMatchedStatus()
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

            IDictionary<string, ITag> dict = Tag.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Tag 1", "Tag 2" };

            List<ITag> matches = Tag.MatchNames(dict, names, false);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches[0].Matched, Is.False);
            Assert.That(matches[1].Matched, Is.False);
        }

        #endregion MatchNames
    }
}