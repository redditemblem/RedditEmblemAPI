using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.BattleStyles;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    [TestClass]
    public class BattleStyleTests
    {
        #region Constants

        private const string INPUT_NAME = "Battle Style Test";

        #endregion Constants

        [TestMethod]
        public void BattleStyleConstructor_RequiredFields_WithInputNull()
        {
            BattleStylesConfig config = new BattleStylesConfig()
            {
                Name = 0
            };

            List<string> data = new List<string>() { };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new BattleStyle(config, data));
        }

        [TestMethod]
        public void BattleStyleConstructor_RequiredFields()
        {
            BattleStylesConfig config = new BattleStylesConfig()
            {
                Name = 0
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME
            };

            BattleStyle style = new BattleStyle(config, data);

            Assert.AreEqual<string>(INPUT_NAME, style.Name);
        }

        #region OptionalField_SpriteURL

        public void BattleStyleConstructor_OptionalField_SpriteURL_EmptyString()
        {
            BattleStylesConfig config = new BattleStylesConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            BattleStyle style = new BattleStyle(config, data);

            Assert.AreEqual<string>(string.Empty, style.SpriteURL);
        }

        [TestMethod]
        public void BattleStyleConstructor_OptionalField_SpriteURL()
        {
            BattleStylesConfig config = new BattleStylesConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL
            };

            BattleStyle style = new BattleStyle(config, data);

            Assert.AreEqual<string>(UnitTestConsts.IMAGE_URL, style.SpriteURL);
        }

        #endregion OptionalField_SpriteURL

        #region OptionalField_TextFields

        [TestMethod]
        public void BattleStyleConstructor_OptionalField_TextFields_EmptyString()
        {
            BattleStylesConfig config = new BattleStylesConfig()
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

            BattleStyle style = new BattleStyle(config, data);

            CollectionAssert.AreEqual(new List<string>() { }, style.TextFields);
        }

        [TestMethod]
        public void BattleStyleConstructor_OptionalField_TextFields()
        {
            BattleStylesConfig config = new BattleStylesConfig()
            {
                Name = 0,
                TextFields = new List<int>() { 1, 2 }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                "Text Field 1",
                "Text Field 2"
            };

            BattleStyle style = new BattleStyle(config, data);

            CollectionAssert.AreEqual(new List<string>() { "Text Field 1", "Text Field 2" }, style.TextFields);
        }

        #endregion OptionalField_TextFields

        #region FlagAsMatched

        [TestMethod]
        public void BattleStyle_FlagAsMatched()
        {
            BattleStylesConfig config = new BattleStylesConfig()
            {
                Name = 0
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME
            };

            BattleStyle style = new BattleStyle(config, data);

            Assert.IsFalse(style.Matched);
            style.FlagAsMatched();
            Assert.IsTrue(style.Matched);
        }

        #endregion FlagAsMatched

        #region BuildDictionary

        [TestMethod]
        public void BattleStyle_BuildDictionary_WithInput_Null()
        {
            IDictionary<string, BattleStyle> dict = BattleStyle.BuildDictionary(null);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void BattleStyle_BuildDictionary_WithInput_NullQuery()
        {
            BattleStylesConfig config = new BattleStylesConfig()
            {
                Queries = null,
                Name = 0
            };

            IDictionary<string, BattleStyle> dict = BattleStyle.BuildDictionary(config);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void BattleStyle_BuildDictionary_WithInput_EmptyQuery()
        {
            BattleStylesConfig config = new BattleStylesConfig()
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

            IDictionary<string, BattleStyle> dict = BattleStyle.BuildDictionary(config);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void BattleStyle_BuildDictionary_WithInput_DuplicateName()
        {
            BattleStylesConfig config = new BattleStylesConfig()
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

            Assert.ThrowsException<BattleStyleProcessingException>(() => BattleStyle.BuildDictionary(config));
        }

        [TestMethod]
        public void BattleStyle_BuildDictionary_WithInput_Invalid()
        {
            BattleStylesConfig config = new BattleStylesConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_NAME, "NotURL" }
                        }
                    }
                },
                Name = 0,
                SpriteURL = 1
            };

            Assert.ThrowsException<BattleStyleProcessingException>(() => BattleStyle.BuildDictionary(config));
        }

        [TestMethod]
        public void BattleStyle_BuildDictionary()
        {
            BattleStylesConfig config = new BattleStylesConfig()
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

            IDictionary<string, BattleStyle> dict = BattleStyle.BuildDictionary(config);
            Assert.AreEqual<int>(1, dict.Count);
        }

        [TestMethod]
        public void BattleStyle_BuildDictionary_MultiQuery()
        {
            BattleStylesConfig config = new BattleStylesConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Battle Style 1" },
                            new List<object>(){ "Battle Style 2" }
                        }
                    },
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Battle Style 3" },
                            new List<object>(){ "Battle Style 4" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, BattleStyle> dict = BattleStyle.BuildDictionary(config);
            Assert.AreEqual<int>(4, dict.Count);
        }

        #endregion BuildDictionary

        #region MatchNames

        [TestMethod]
        public void BattleStyle_MatchNames_UnmatchedName()
        {
            BattleStylesConfig config = new BattleStylesConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Battle Style 1" },
                            new List<object>(){ "Battle Style 2" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, BattleStyle> dict = BattleStyle.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Battle Style 3" };

            Assert.ThrowsException<UnmatchedBattleStyleException>(() => BattleStyle.MatchNames(dict, names));
        }

        [TestMethod]
        public void BattleStyle_MatchNames_SingleMatch()
        {
            BattleStylesConfig config = new BattleStylesConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Battle Style 1" },
                            new List<object>(){ "Battle Style 2" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, BattleStyle> dict = BattleStyle.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Battle Style 1" };

            List<BattleStyle> matches = BattleStyle.MatchNames(dict, names);
            Assert.AreEqual(1, matches.Count);
            Assert.IsTrue(matches.First().Matched);
        }

        [TestMethod]
        public void BattleStyle_MatchNames_MultipleMatches()
        {
            BattleStylesConfig config = new BattleStylesConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Battle Style 1" },
                            new List<object>(){ "Battle Style 2" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, BattleStyle> dict = BattleStyle.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Battle Style 1", "Battle Style 2" };

            List<BattleStyle> matches = BattleStyle.MatchNames(dict, names);
            Assert.AreEqual(2, matches.Count);
            Assert.IsTrue(matches[0].Matched);
            Assert.IsTrue(matches[1].Matched);
        }

        [TestMethod]
        public void BattleStyle_MatchNames_MultipleMatches_SetMatchedStatus()
        {
            BattleStylesConfig config = new BattleStylesConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Battle Style 1" },
                            new List<object>(){ "Battle Style 2" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, BattleStyle> dict = BattleStyle.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Battle Style 1", "Battle Style 2" };

            List<BattleStyle> matches = BattleStyle.MatchNames(dict, names, true);
            Assert.AreEqual(2, matches.Count);
            Assert.IsFalse(matches[0].Matched);
            Assert.IsFalse(matches[1].Matched);
        }

        #endregion MatchNames
    }
}