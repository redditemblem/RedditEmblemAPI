using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.BattleStyles;
using RedditEmblemAPI.Models.Configuration.System.Classes;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    [TestClass]
    public class ClassTests
    {
        #region Constants

        private const string INPUT_NAME = "Class Test";
        private const string INPUT_MOVEMENT_TYPE = "Movement Type Test";

        #endregion Constants

        #region Setup

        private IDictionary<string, BattleStyle> DICTIONARY_BATTLE_STYLES = new Dictionary<string, BattleStyle>();

        [TestInitialize]
        public void Setup()
        {
            BattleStylesConfig config = new BattleStylesConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Battle Style 1" }
                        }
                    }
                },
                Name = 0
            };

            this.DICTIONARY_BATTLE_STYLES = BattleStyle.BuildDictionary(config);
        }

        #endregion Setup

        [TestMethod]
        public void ClassConstructor_RequiredFields_WithInputNull()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0
            };

            List<string> data = new List<string>() { };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new Class(config, data, true, DICTIONARY_BATTLE_STYLES));
        }

        [TestMethod]
        public void ClassConstructor_RequiredFields()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME
            };

            Class cls = new Class(config, data, true, DICTIONARY_BATTLE_STYLES);

            Assert.AreEqual<string>(INPUT_NAME, cls.Name);
        }

        #region OptionalField_MovementType

        [TestMethod]
        public void ClassConstructor_OptionalField_MovementType_EmptyString()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                MovementType = 1
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            //Movement type is conditionally required depending on whether or not the unit movement type field is in play
            Class cls = new Class(config, data, true, DICTIONARY_BATTLE_STYLES);
            Assert.AreEqual<string>(INPUT_NAME, cls.Name);
            Assert.AreEqual<string>(string.Empty, cls.MovementType);

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new Class(config, data, false, DICTIONARY_BATTLE_STYLES));
        }

        [TestMethod]
        public void ClassConstructor_OptionalField_MovementType()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                MovementType = 1
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                "Movement Type"
            };

            //Movement type is conditionally required depending on whether or not the unit movement type field is in play
            Class cls1 = new Class(config, data, true, DICTIONARY_BATTLE_STYLES);
            Assert.AreEqual<string>(INPUT_NAME, cls1.Name);
            Assert.AreEqual<string>("Movement Type", cls1.MovementType);

            Class cls2 = new Class(config, data, false, DICTIONARY_BATTLE_STYLES);
            Assert.AreEqual<string>(INPUT_NAME, cls2.Name);
            Assert.AreEqual<string>("Movement Type", cls2.MovementType);
        }

        #endregion OptionalField_MovementType

        #region OptionalField_BattleStyle

        [TestMethod]
        public void ClassConstructor_OptionalField_BattleStyle_EmptyString()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                BattleStyle = 1
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            Class cls = new Class(config, data, true, DICTIONARY_BATTLE_STYLES);

            Assert.IsNull(cls.BattleStyle);
        }

        [TestMethod]
        public void ClassConstructor_OptionalField_BattleStyle_NoMatch()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                BattleStyle = 1
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                "Battle Style 2"
            };

            Assert.ThrowsException<UnmatchedBattleStyleException>(() => new Class(config, data, true, DICTIONARY_BATTLE_STYLES));
        }

        [TestMethod]
        public void ClassConstructor_OptionalField_BattleStyle()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                BattleStyle = 1
            };

            string battleStyle = "Battle Style 1";
            List<string> data = new List<string>()
            {
                INPUT_NAME,
                battleStyle
            };

            Class cls = new Class(config, data, true, DICTIONARY_BATTLE_STYLES);

            Assert.IsNotNull(cls.BattleStyle);
            Assert.AreEqual<string>(battleStyle, cls.BattleStyle.Name);
            Assert.IsFalse(cls.BattleStyle.Matched);
        }

        #endregion OptionalField_BattleStyle

        #region OptionalField_Tags

        [TestMethod]
        public void ClassConstructor_OptionalField_Tags_EmptyString()
        {
            ClassesConfig config = new ClassesConfig()
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

            Class cls = new Class(config, data, true, DICTIONARY_BATTLE_STYLES);

            CollectionAssert.AreEqual(new List<string>() { }, cls.Tags);
        }

        [TestMethod]
        public void ClassConstructor_OptionalField_Tags()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                Tags = new List<int> { 1, 2 }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                "Tag 1,Tag 2",
                "Tag 3"
            };

            Class cls = new Class(config, data, true, DICTIONARY_BATTLE_STYLES);

            CollectionAssert.AreEqual(new List<string>() { "Tag 1", "Tag 2", "Tag 3"}, cls.Tags);
        }

        #endregion OptionalField_Tags

        #region OptionalField_TextFields

        [TestMethod]
        public void ClassConstructor_OptionalField_TextFields_EmptyString()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                TextFields = new List<int> { 1, 2 }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty,
                string.Empty
            };

            Class cls = new Class(config, data, true, DICTIONARY_BATTLE_STYLES);

            CollectionAssert.AreEqual(new List<string>() { }, cls.TextFields);
        }

        [TestMethod]
        public void ClassConstructor_OptionalField_TextFields()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                TextFields = new List<int> { 1, 2 }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                "Text Field 1",
                "Text Field 2"
            };

            Class cls = new Class(config, data, true, DICTIONARY_BATTLE_STYLES);

            CollectionAssert.AreEqual(new List<string>() { "Text Field 1", "Text Field 2" }, cls.TextFields);
        }

        #endregion OptionalField_TextFields

        #region FlagAsMatched

        [TestMethod]
        public void Class_FlagAsMatched()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME
            };

            Class cls = new Class(config, data, true, DICTIONARY_BATTLE_STYLES);

            Assert.IsFalse(cls.Matched);
            cls.FlagAsMatched();
            Assert.IsTrue(cls.Matched);
        }

        [TestMethod]
        public void Class_FlagAsMatched_WithBattleStyle()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                BattleStyle = 1
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                "Battle Style 1"
            };

            Class cls = new Class(config, data, true, DICTIONARY_BATTLE_STYLES);

            Assert.IsFalse(cls.Matched);
            Assert.IsFalse(cls.BattleStyle.Matched);
            cls.FlagAsMatched();
            Assert.IsTrue(cls.Matched);
            Assert.IsTrue(cls.BattleStyle.Matched);
        }

        #endregion FlagAsMatched

        #region BuildDictionary

        [TestMethod]
        public void Class_BuildDictionary_WithInput_Null()
        {
            IDictionary<string, Class> dict = Class.BuildDictionary(null, true, DICTIONARY_BATTLE_STYLES);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void Class_BuildDictionary_WithInput_NullQuery()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Queries = null,
                Name = 0
            };

            IDictionary<string, Class> dict = Class.BuildDictionary(config, true, DICTIONARY_BATTLE_STYLES);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void Class_BuildDictionary_WithInput_EmptyQuery()
        {
            ClassesConfig config = new ClassesConfig()
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

            IDictionary<string, Class> dict = Class.BuildDictionary(config, true, DICTIONARY_BATTLE_STYLES);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void Class_BuildDictionary_WithInput_DuplicateName()
        {
            ClassesConfig config = new ClassesConfig()
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

            Assert.ThrowsException<ClassProcessingException>(() => Class.BuildDictionary(config, true, DICTIONARY_BATTLE_STYLES));
        }

        [TestMethod]
        public void Class_BuildDictionary()
        {
            ClassesConfig config = new ClassesConfig()
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

            IDictionary<string, Class> dict = Class.BuildDictionary(config, true, DICTIONARY_BATTLE_STYLES);
            Assert.AreEqual<int>(1, dict.Count);
        }

        [TestMethod]
        public void Class_BuildDictionary_MultiQuery()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Class 1" },
                            new List<object>(){ "Class 2" }
                        }
                    },
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Class 3" },
                            new List<object>(){ "Class 4" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, Class> dict = Class.BuildDictionary(config, true, DICTIONARY_BATTLE_STYLES);
            Assert.AreEqual<int>(4, dict.Count);
        }

        #endregion BuildDictionary

        #region MatchNames

        [TestMethod]
        public void Class_MatchNames_UnmatchedName()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Class 1" },
                            new List<object>(){ "Class 2" },
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, Class> dict = Class.BuildDictionary(config, true, DICTIONARY_BATTLE_STYLES);
            IEnumerable<string> names = new List<string>() { "Class 3" };

            Assert.ThrowsException<UnmatchedClassException>(() => Class.MatchNames(dict, names));
        }

        [TestMethod]
        public void Class_MatchNames_SingleMatch()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Class 1" },
                            new List<object>(){ "Class 2" },
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, Class> dict = Class.BuildDictionary(config, true, DICTIONARY_BATTLE_STYLES);
            IEnumerable<string> names = new List<string>() { "Class 1" };

            List<Class> matches = Class.MatchNames(dict, names);
            Assert.AreEqual(1, matches.Count);
            Assert.IsTrue(matches.First().Matched);
        }

        [TestMethod]
        public void Class_MatchNames_MultipleMatches()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Class 1" },
                            new List<object>(){ "Class 2" },
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, Class> dict = Class.BuildDictionary(config, true, DICTIONARY_BATTLE_STYLES);
            IEnumerable<string> names = new List<string>() { "Class 1", "Class 2" };

            List<Class> matches = Class.MatchNames(dict, names);
            Assert.AreEqual(2, matches.Count);
            Assert.IsTrue(matches[0].Matched);
            Assert.IsTrue(matches[1].Matched);
        }

        [TestMethod]
        public void Class_MatchNames_MultipleMatches_SetMatchedStatus()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Class 1" },
                            new List<object>(){ "Class 2" },
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, Class> dict = Class.BuildDictionary(config, true, DICTIONARY_BATTLE_STYLES);
            IEnumerable<string> names = new List<string>() { "Class 1", "Class 2" };

            List<Class> matches = Class.MatchNames(dict, names, true);
            Assert.AreEqual(2, matches.Count);
            Assert.IsFalse(matches[0].Matched);
            Assert.IsFalse(matches[1].Matched);
        }

        #endregion MatchNames
    }
}
