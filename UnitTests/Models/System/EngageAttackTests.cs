using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Emblems;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    [TestClass]
    public class EngageAttackTests
    {
        #region Constants

        private const string INPUT_NAME = "Engage Attack Test";

        #endregion Constants

        [TestMethod]
        public void EngageAttackConstructor_RequiredFields_WithInputNull()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
            {
                Name = 0
            };

            List<string> data = new List<string>() { };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new EngageAttack(config, data));
        }

        [TestMethod]
        public void EngageAttackConstructor_RequiredFields()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
            {
                Name = 0
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME
            };

            EngageAttack attack = new EngageAttack(config, data);

            Assert.AreEqual<string>(INPUT_NAME, attack.Name);
        }

        #region OptionalField_SpriteURL

        public void EngageAttackConstructor_OptionalField_SpriteURL_EmptyString()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            EngageAttack attack = new EngageAttack(config, data);

            Assert.AreEqual<string>(string.Empty, attack.SpriteURL);
        }

        [TestMethod]
        public void EngageAttackConstructor_OptionalField_SpriteURL()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL
            };

            EngageAttack attack = new EngageAttack(config, data);

            Assert.AreEqual<string>(UnitTestConsts.IMAGE_URL, attack.SpriteURL);
        }

        #endregion OptionalField_SpriteURL

        #region OptionalField_TextFields

        [TestMethod]
        public void EngageAttackConstructor_OptionalField_TextFields_EmptyString()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
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

            EngageAttack attack = new EngageAttack(config, data);

            CollectionAssert.AreEqual(new List<string>() { }, attack.TextFields);
        }

        [TestMethod]
        public void EngageAttackConstructor_OptionalField_TextFields()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
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

            EngageAttack attack = new EngageAttack(config, data);

            CollectionAssert.AreEqual(new List<string>() { "Text Field 1", "Text Field 2" }, attack.TextFields);
        }

        #endregion OptionalField_TextFields

        #region FlagAsMatched

        [TestMethod]
        public void EngageAttack_FlagAsMatched()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
            {
                Name = 0
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME
            };

            EngageAttack attack = new EngageAttack(config, data);

            Assert.IsFalse(attack.Matched);
            attack.FlagAsMatched();
            Assert.IsTrue(attack.Matched);
        }

        #endregion FlagAsMatched

        #region BuildDictionary

        [TestMethod]
        public void EngageAttack_BuildDictionary_WithInput_Null()
        {
            IDictionary<string, EngageAttack> dict = EngageAttack.BuildDictionary(null);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void EngageAttack_BuildDictionary_WithInput_NullQuery()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
            {
                Query = null,
                Name = 0
            };

            IDictionary<string, EngageAttack> dict = EngageAttack.BuildDictionary(config);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void EngageAttack_BuildDictionary_WithInput_EmptyQuery()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
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

            IDictionary<string, EngageAttack> dict = EngageAttack.BuildDictionary(config);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void EngageAttack_BuildDictionary_WithInput_DuplicateName()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
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

            Assert.ThrowsException<EngageAttackProcessingException>(() => EngageAttack.BuildDictionary(config));
        }

        [TestMethod]
        public void EngageAttack_BuildDictionary_WithInput_Invalid()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ INPUT_NAME, "NotURL" }
                    }
                },
                Name = 0,
                SpriteURL = 1
            };

            Assert.ThrowsException<EngageAttackProcessingException>(() => EngageAttack.BuildDictionary(config));
        }

        [TestMethod]
        public void EngageAttack_BuildDictionary()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
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

            IDictionary<string, EngageAttack> dict = EngageAttack.BuildDictionary(config);
            Assert.AreEqual<int>(1, dict.Count);
        }

        #endregion BuildDictionary

        #region MatchNames

        [TestMethod]
        public void EngageAttack_MatchNames_UnmatchedName()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ "Engage Attack 1" },
                        new List<object>(){ "Engage Attack 2" }
                    }
                },
                Name = 0
            };

            IDictionary<string, EngageAttack> dict = EngageAttack.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Engage Attack 3" };

            Assert.ThrowsException<UnmatchedEngageAttackException>(() => EngageAttack.MatchNames(dict, names));
        }

        [TestMethod]
        public void EngageAttack_MatchNames_SingleMatch()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ "Engage Attack 1" },
                        new List<object>(){ "Engage Attack 2" }
                    }
                },
                Name = 0
            };

            IDictionary<string, EngageAttack> dict = EngageAttack.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Engage Attack 1" };

            List<EngageAttack> matches = EngageAttack.MatchNames(dict, names);
            Assert.AreEqual(1, matches.Count);
            Assert.IsTrue(matches.First().Matched);
        }

        [TestMethod]
        public void EngageAttack_MatchNames_MultipleMatches()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ "Engage Attack 1" },
                        new List<object>(){ "Engage Attack 2" }
                    }
                },
                Name = 0
            };

            IDictionary<string, EngageAttack> dict = EngageAttack.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Engage Attack 1", "Engage Attack 2" };

            List<EngageAttack> matches = EngageAttack.MatchNames(dict, names);
            Assert.AreEqual(2, matches.Count);
            Assert.IsTrue(matches[0].Matched);
            Assert.IsTrue(matches[1].Matched);
        }

        [TestMethod]
        public void EngageAttack_MatchNames_MultipleMatches_SetMatchedStatus()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ "Engage Attack 1" },
                        new List<object>(){ "Engage Attack 2" }
                    }
                },
                Name = 0
            };

            IDictionary<string, EngageAttack> dict = EngageAttack.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Engage Attack 1", "Engage Attack 2" };

            List<EngageAttack> matches = EngageAttack.MatchNames(dict, names, true);
            Assert.AreEqual(2, matches.Count);
            Assert.IsFalse(matches[0].Matched);
            Assert.IsFalse(matches[1].Matched);
        }

        #endregion MatchNames
    }
}