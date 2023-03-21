using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Emblems;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    [TestClass]
    public class EmblemTests
    {
        #region Constants

        private const string INPUT_NAME = "Emblem Test";

        #endregion Constants

        [TestMethod]
        public void EmblemConstructor_RequiredFields_WithInputNull()
        {
            EmblemsConfig config = new EmblemsConfig()
            {
                Name = 0
            };

            List<string> data = new List<string>() { };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new Emblem(config, data));
        }

        [TestMethod]
        public void EmblemConstructor_RequiredFields()
        {
            EmblemsConfig config = new EmblemsConfig()
            {
                Name = 0
            };

            List<string> data = new List<string>() { INPUT_NAME };

            Emblem emblem = new Emblem(config, data);

            Assert.AreEqual<string>(INPUT_NAME, emblem.Name);
        }

        #region OptionalField_SpriteURL

        [TestMethod]
        public void EmblemConstructor_OptionalField_SpriteURL_EmptyString()
        {
            EmblemsConfig config = new EmblemsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            Emblem emblem = new Emblem(config, data);

            Assert.AreEqual<string>(string.Empty, emblem.SpriteURL);
        }

        [TestMethod]
        public void EmblemConstructor_OptionalField_SpriteURL_InvalidURL()
        {
            EmblemsConfig config = new EmblemsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                "NotAURL"
            };

            Assert.ThrowsException<URLException>(() => new Emblem(config, data));
        }

        [TestMethod]
        public void EmblemConstructor_OptionalField_SpriteURL()
        {
            EmblemsConfig config = new EmblemsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL
            };

            Emblem emblem = new Emblem(config, data);

            Assert.AreEqual<string>(UnitTestConsts.IMAGE_URL, emblem.SpriteURL);
        }

        #endregion OptionalField_SpriteURL

        #region OptionalField_Tagline

        [TestMethod]
        public void EmblemConstructor_OptionalField_Tagline_EmptyString()
        {
            EmblemsConfig config = new EmblemsConfig()
            {
                Name = 0,
                Tagline = 1
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            Emblem emblem = new Emblem(config, data);

            Assert.AreEqual<string>(string.Empty, emblem.Tagline);
        }

        [TestMethod]
        public void EmblemConstructor_OptionalField_Tagline()
        {
            EmblemsConfig config = new EmblemsConfig()
            {
                Name = 0,
                Tagline = 1
            };

            string tagline = "This is my tagline";

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                tagline
            };

            Emblem emblem = new Emblem(config, data);

            Assert.AreEqual<string>(tagline, emblem.Tagline);
        }

        #endregion OptionalField_Tagline

        #region OptionalField_EngagedUnitAura

        [TestMethod]
        public void EmblemConstructor_OptionalField_EngagedUnitAura_EmptyString()
        {
            EmblemsConfig config = new EmblemsConfig()
            {
                Name = 0,
                EngagedUnitAura = 1
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            Emblem emblem = new Emblem(config, data);

            Assert.AreEqual<string>(string.Empty, emblem.EngagedUnitAura);
        }

        [TestMethod]
        public void EmblemConstructor_OptionalField_EngagedUnitAura_InvalidHex()
        {
            EmblemsConfig config = new EmblemsConfig()
            {
                Name = 0,
                EngagedUnitAura = 1
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                "NotAHexCode"
            };

            Assert.ThrowsException<HexException>(() => new Emblem(config, data));
        }

        [TestMethod]
        public void EmblemConstructor_OptionalField_EngagedUnitAura()
        {
            EmblemsConfig config = new EmblemsConfig()
            {
                Name = 0,
                EngagedUnitAura = 1
            };

            string hexCode = "#F0F0F0";

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                hexCode
            };

            Emblem emblem = new Emblem(config, data);

            Assert.AreEqual<string>(hexCode, emblem.EngagedUnitAura);
        }

        #endregion OptionalField_EngagedUnitAura

        #region OptionalField_TextFields

        [TestMethod]
        public void EmblemConstructor_OptionalField_TextFields_EmptyString()
        {
            EmblemsConfig config = new EmblemsConfig()
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

            Emblem emblem = new Emblem(config, data);

            CollectionAssert.AreEqual(new List<string>() { }, emblem.TextFields);
        }

        [TestMethod]
        public void EmblemConstructor_OptionalField_TextFields()
        {
            EmblemsConfig config = new EmblemsConfig()
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

            Emblem emblem = new Emblem(config, data);

            CollectionAssert.AreEqual(new List<string>() { field1, field2 }, emblem.TextFields);
        }

        #endregion OptionalField_TextFields

        #region BuildDictionary

        [TestMethod]
        public void Emblem_BuildDictionary_WithInput_Null()
        {
            EmblemsConfig config = new EmblemsConfig()
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

            IDictionary<string, Emblem> dict = Emblem.BuildDictionary(config);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void Emblem_BuildDictionary_WithInput_DuplicateName()
        {
            EmblemsConfig config = new EmblemsConfig()
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

            Assert.ThrowsException<EmblemProcessingException>(() => Emblem.BuildDictionary(config));
        }

        [TestMethod]
        public void Emblem_BuildDictionary()
        {
            EmblemsConfig config = new EmblemsConfig()
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

            IDictionary<string, Emblem> dict = Emblem.BuildDictionary(config);
            Assert.AreEqual<int>(1, dict.Count);
        }

        # endregion BuildDictionary
    }
}
