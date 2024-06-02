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
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ "Battle Style 1" }
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
                Name = 0,
                MovementType = 1
            };

            List<string> data = new List<string>() { };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new Class(config, data, DICTIONARY_BATTLE_STYLES));
        }

        [TestMethod]
        public void ClassConstructor_RequiredFields_WithInput_InvalidMovementType()
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

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new Class(config, data, DICTIONARY_BATTLE_STYLES));
        }

        [TestMethod]
        public void ClassConstructor_RequiredFields()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                MovementType = 1
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_MOVEMENT_TYPE
            };

            Class cls = new Class(config, data, DICTIONARY_BATTLE_STYLES);

            Assert.AreEqual<string>(INPUT_NAME, cls.Name);
            Assert.AreEqual<string>(INPUT_MOVEMENT_TYPE, cls.MovementType);
        }

        #region OptionalField_BattleStyle

        [TestMethod]
        public void ClassConstructor_OptionalField_BattleStyle_EmptyString()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                MovementType = 1,
                BattleStyle = 2
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_MOVEMENT_TYPE,
                string.Empty
            };

            Class cls = new Class(config, data, DICTIONARY_BATTLE_STYLES);

            Assert.IsNull(cls.BattleStyle);
        }

        [TestMethod]
        public void ClassConstructor_OptionalField_BattleStyle_NoMatch()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                MovementType = 1,
                BattleStyle = 2
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_MOVEMENT_TYPE,
                "Battle Style 2"
            };

            Assert.ThrowsException<UnmatchedBattleStyleException>(() => new Class(config, data, DICTIONARY_BATTLE_STYLES));
        }

        [TestMethod]
        public void ClassConstructor_OptionalField_BattleStyle()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                MovementType = 1,
                BattleStyle = 2
            };

            string battleStyle = "Battle Style 1";
            List<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_MOVEMENT_TYPE,
                battleStyle
            };

            Class cls = new Class(config, data, DICTIONARY_BATTLE_STYLES);

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
                MovementType = 1,
                Tags = new List<int> { 2, 3 }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_MOVEMENT_TYPE,
                string.Empty,
                string.Empty
            };

            Class cls = new Class(config, data, DICTIONARY_BATTLE_STYLES);

            CollectionAssert.AreEqual(new List<string>() { }, cls.Tags);
        }

        [TestMethod]
        public void ClassConstructor_OptionalField_Tags()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                MovementType = 1,
                Tags = new List<int> { 2, 3 }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_MOVEMENT_TYPE,
                "Tag 1,Tag 2",
                "Tag 3"
            };

            Class cls = new Class(config, data, DICTIONARY_BATTLE_STYLES);

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
                MovementType = 1,
                TextFields = new List<int> { 2, 3 }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_MOVEMENT_TYPE,
                string.Empty,
                string.Empty
            };

            Class cls = new Class(config, data, DICTIONARY_BATTLE_STYLES);

            CollectionAssert.AreEqual(new List<string>() { }, cls.TextFields);
        }

        [TestMethod]
        public void ClassConstructor_OptionalField_TextFields()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                MovementType = 1,
                TextFields = new List<int> { 2, 3 }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_MOVEMENT_TYPE,
                "Text Field 1",
                "Text Field 2"
            };

            Class cls = new Class(config, data, DICTIONARY_BATTLE_STYLES);

            CollectionAssert.AreEqual(new List<string>() { "Text Field 1", "Text Field 2" }, cls.TextFields);
        }

        #endregion OptionalField_TextFields

        #region FlagAsMatched

        [TestMethod]
        public void Class_FlagAsMatched()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                MovementType = 1
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_MOVEMENT_TYPE
            };

            Class cls = new Class(config, data, DICTIONARY_BATTLE_STYLES);

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
                MovementType = 1,
                BattleStyle = 2
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_MOVEMENT_TYPE,
                "Battle Style 1"
            };

            Class cls = new Class(config, data, DICTIONARY_BATTLE_STYLES);

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
            IDictionary<string, Class> dict = Class.BuildDictionary(null, DICTIONARY_BATTLE_STYLES);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void Class_BuildDictionary_WithInput_NullQuery()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Query = null,
                Name = 0,
                MovementType = 1
            };

            IDictionary<string, Class> dict = Class.BuildDictionary(config, DICTIONARY_BATTLE_STYLES);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void Class_BuildDictionary_WithInput_EmptyQuery()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ }
                    }
                },
                Name = 0,
                MovementType = 1
            };

            IDictionary<string, Class> dict = Class.BuildDictionary(config, DICTIONARY_BATTLE_STYLES);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void Class_BuildDictionary_WithInput_DuplicateName()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ INPUT_NAME, INPUT_MOVEMENT_TYPE },
                        new List<object>(){ INPUT_NAME, INPUT_MOVEMENT_TYPE }
                    }
                },
                Name = 0,
                MovementType = 1
            };

            Assert.ThrowsException<ClassProcessingException>(() => Class.BuildDictionary(config, DICTIONARY_BATTLE_STYLES));
        }

        [TestMethod]
        public void Class_BuildDictionary()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ INPUT_NAME, INPUT_MOVEMENT_TYPE }
                    }
                },
                Name = 0,
                MovementType = 1
            };

            IDictionary<string, Class> dict = Class.BuildDictionary(config, DICTIONARY_BATTLE_STYLES);
            Assert.AreEqual<int>(1, dict.Count);
        }

        #endregion BuildDictionary

        #region MatchNames

        [TestMethod]
        public void Class_MatchNames_UnmatchedName()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ "Class 1", INPUT_MOVEMENT_TYPE },
                        new List<object>(){ "Class 2", INPUT_MOVEMENT_TYPE },
                    }
                },
                Name = 0,
                MovementType = 1
            };

            IDictionary<string, Class> dict = Class.BuildDictionary(config, DICTIONARY_BATTLE_STYLES);
            IEnumerable<string> names = new List<string>() { "Class 3" };

            Assert.ThrowsException<UnmatchedClassException>(() => Class.MatchNames(dict, names));
        }

        [TestMethod]
        public void Class_MatchNames_SingleMatch()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ "Class 1", INPUT_MOVEMENT_TYPE },
                        new List<object>(){ "Class 2", INPUT_MOVEMENT_TYPE },
                    }
                },
                Name = 0,
                MovementType = 1
            };

            IDictionary<string, Class> dict = Class.BuildDictionary(config, DICTIONARY_BATTLE_STYLES);
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
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ "Class 1", INPUT_MOVEMENT_TYPE },
                        new List<object>(){ "Class 2", INPUT_MOVEMENT_TYPE },
                    }
                },
                Name = 0,
                MovementType = 1
            };

            IDictionary<string, Class> dict = Class.BuildDictionary(config, DICTIONARY_BATTLE_STYLES);
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
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ "Class 1", INPUT_MOVEMENT_TYPE },
                        new List<object>(){ "Class 2", INPUT_MOVEMENT_TYPE },
                    }
                },
                Name = 0,
                MovementType = 1
            };

            IDictionary<string, Class> dict = Class.BuildDictionary(config, DICTIONARY_BATTLE_STYLES);
            IEnumerable<string> names = new List<string>() { "Class 1", "Class 2" };

            List<Class> matches = Class.MatchNames(dict, names, true);
            Assert.AreEqual(2, matches.Count);
            Assert.IsFalse(matches[0].Matched);
            Assert.IsFalse(matches[1].Matched);
        }

        #endregion MatchNames
    }
}
