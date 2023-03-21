using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Tags;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    [TestClass]
    public class TagTests
    {
        #region Constants

        private const string INPUT_NAME = "Tag Test";

        #endregion Constants

        [TestMethod]
        public void TagConstructor_RequiredFields_WithInputNull()
        {
            TagsConfig config = new TagsConfig()
            {
                Name = 0
            };

            List<string> data = new List<string>() { };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new Tag(config, data));
        }

        [TestMethod]
        public void TagConstructor_RequiredFields()
        {
            TagsConfig config = new TagsConfig()
            {
                Name = 0
            };

            List<string> data = new List<string>() { INPUT_NAME };

            Tag tag = new Tag(config, data);

            Assert.AreEqual<string>(INPUT_NAME, tag.Name);
        }

        #region OptionalField_SpriteURL

        [TestMethod]
        public void TagConstructor_OptionalField_SpriteURL_EmptyString()
        {
            TagsConfig config = new TagsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            List<string> data = new List<string>() { INPUT_NAME, string.Empty };

            Tag tag = new Tag(config, data);

            Assert.AreEqual<string>(string.Empty, tag.SpriteURL);
        }

        [TestMethod]
        public void TagConstructor_OptionalField_SpriteURL_InvalidURL()
        {
            TagsConfig config = new TagsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            List<string> data = new List<string>() { INPUT_NAME, "NotAURL" };

            Assert.ThrowsException<URLException>(() => new Tag(config, data));
        }

        [TestMethod]
        public void TagConstructor_OptionalField_SpriteURL()
        {
            TagsConfig config = new TagsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            List<string> data = new List<string>() { INPUT_NAME, UnitTestConsts.IMAGE_URL };

            Tag tag = new Tag(config, data);

            Assert.AreEqual<string>(UnitTestConsts.IMAGE_URL, tag.SpriteURL);
        }

        #endregion OptionalField_SpriteURL

        #region OptionalField_ShowOnUnit

        [TestMethod]
        public void TagConstructor_OptionalField_ShowOnUnit_EmptyString()
        {
            TagsConfig config = new TagsConfig()
            {
                Name = 0,
                ShowOnUnit = 1
            };

            List<string> data = new List<string>() { INPUT_NAME, string.Empty };

            Tag tag = new Tag(config, data);

            Assert.IsFalse(tag.ShowOnUnit);
        }

        [TestMethod]
        public void TagConstructor_OptionalField_ShowOnUnit_No()
        {
            TagsConfig config = new TagsConfig()
            {
                Name = 0,
                ShowOnUnit = 1
            };

            List<string> data = new List<string>() { INPUT_NAME, "No" };

            Tag tag = new Tag(config, data);

            Assert.IsFalse(tag.ShowOnUnit);
        }

        [TestMethod]
        public void TagConstructor_OptionalField_ShowOnUnit_Yes_NoSpriteURL()
        {
            TagsConfig config = new TagsConfig()
            {
                Name = 0,
                ShowOnUnit = 1
            };

            List<string> data = new List<string>() { INPUT_NAME, "Yes" };

            Tag tag = new Tag(config, data);

            Assert.IsFalse(tag.ShowOnUnit);
        }

        [TestMethod]
        public void TagConstructor_OptionalField_ShowOnUnit_Yes_WithSpriteURL()
        {
            TagsConfig config = new TagsConfig()
            {
                Name = 0,
                SpriteURL = 1,
                ShowOnUnit = 2
            };

            List<string> data = new List<string>() { INPUT_NAME, UnitTestConsts.IMAGE_URL, "Yes" };

            Tag tag = new Tag(config, data);

            Assert.IsTrue(tag.ShowOnUnit);
        }

        #endregion OptionalField_ShowOnUnit

        #region OptionalField_UnitAura

        [TestMethod]
        public void TagConstructor_OptionalField_UnitAura_EmptyString()
        {
            TagsConfig config = new TagsConfig()
            {
                Name = 0,
                UnitAura = 1
            };

            List<string> data = new List<string>() { INPUT_NAME, string.Empty };

            Tag tag = new Tag(config, data);

            Assert.AreEqual<string>(string.Empty, tag.UnitAura);
        }

        [TestMethod]
        public void TagConstructor_OptionalField_UnitAura_InvalidHex()
        {
            TagsConfig config = new TagsConfig()
            {
                Name = 0,
                UnitAura = 1
            };

            List<string> data = new List<string>() { INPUT_NAME, "NotAHex" };

            Assert.ThrowsException<HexException>(() => new Tag(config, data));
        }

        [TestMethod]
        public void TagConstructor_OptionalField_UnitAura()
        {
            TagsConfig config = new TagsConfig()
            {
                Name = 0,
                UnitAura = 1
            };

            string hexcode = "#F0F0F0";

            List<string> data = new List<string>() { INPUT_NAME, hexcode };

            Tag tag = new Tag(config, data);

            Assert.AreEqual<string>(hexcode, tag.UnitAura);
        }

        #endregion OptionalField_UnitAura

        #region BuildDictionary

        [TestMethod]
        public void Tag_BuildDictionary_WithInput_Null()
        {
            TagsConfig config = new TagsConfig()
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

            IDictionary<string, Tag> dict = Tag.BuildDictionary(config);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void Tag_BuildDictionary_WithInput_DuplicateName()
        {
            TagsConfig config = new TagsConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ INPUT_NAME },
                        new List<object>(){ INPUT_NAME }
                    }
                },
                Name = 0
            };

            Assert.ThrowsException<TagProcessingException>(() => Tag.BuildDictionary(config));
        }

        [TestMethod]
        public void Tag_BuildDictionary()
        {
            TagsConfig config = new TagsConfig()
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

            IDictionary<string, Tag> dict = Tag.BuildDictionary(config);
            Assert.AreEqual<int>(1, dict.Count);
        }

        #endregion BuildDictionary
    }
}