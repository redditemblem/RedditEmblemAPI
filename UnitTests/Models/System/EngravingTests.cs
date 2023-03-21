using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Items;
using RedditEmblemAPI.Models.Exceptions.Processing;
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

        [TestMethod]
        public void EngravingConstructor_RequiredFields_WithInputNull()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0
            };

            List<string> data = new List<string>() { };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new Engraving(config, data));
        }

        [TestMethod]
        public void EngravingConstructor_RequiredFields()
        {
            EngravingsConfig config = new EngravingsConfig()
            {
                Name = 0
            };

            List<string> data = new List<string>() { INPUT_NAME };

            Engraving engraving = new Engraving(config, data);

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

            Engraving engraving = new Engraving(config, data);

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

            Assert.ThrowsException<URLException>(() => new Engraving(config, data));
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

            Engraving engraving = new Engraving(config, data);

            Assert.AreEqual<string>(UnitTestConsts.IMAGE_URL, engraving.SpriteURL);
        }

        #endregion OptionalField_SpriteURL

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

            Engraving engraving = new Engraving(config, data);

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

            Engraving engraving = new Engraving(config, data);

            CollectionAssert.AreEqual(new List<string>() { field1, field2 }, engraving.TextFields);
        }

        #endregion OptionalField_TextFields

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

            Engraving engraving = new Engraving(config, data);

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

            Engraving engraving = new Engraving(config, data);

            Assert.AreEqual<int>(2, engraving.StatModifiers.Count);
            Assert.AreEqual<int>(1, engraving.StatModifiers[stat1]);
            Assert.AreEqual<int>(-1, engraving.StatModifiers[stat2]);
        }

        #endregion OptionalField_StatModifiers

        #region BuildDictionary

        [TestMethod]
        public void Engraving_BuildDictionary_WithInput_Null()
        {
            EngravingsConfig config = new EngravingsConfig()
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

            IDictionary<string, Engraving> dict = Engraving.BuildDictionary(config);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void Engraving_BuildDictionary_WithInput_DuplicateName()
        {
            EngravingsConfig config = new EngravingsConfig()
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

            Assert.ThrowsException<EngravingProcessingException>(() => Engraving.BuildDictionary(config));
        }

        [TestMethod]
        public void Engraving_BuildDictionary()
        {
            EngravingsConfig config = new EngravingsConfig()
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

            IDictionary<string, Engraving> dict = Engraving.BuildDictionary(config);
            Assert.AreEqual<int>(1, dict.Count);
        }

        # endregion BuildDictionary
    }
}
