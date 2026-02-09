using NSubstitute;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Classes;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    public class ClassTests
    {
        #region Constants

        private const string INPUT_NAME = "Class Test";
        private const string INPUT_MOVEMENT_TYPE = "Movement Type Test";
        private const string INPUT_BATTLE_STYLE = "Battle Style 1";

        #endregion Constants

        #region Setup

        private IDictionary<string, IBattleStyle> BATTLE_STYLES;

        [SetUp]
        public void Setup()
        {
            IBattleStyle battleStyle1 = Substitute.For<IBattleStyle>();
            battleStyle1.Name.Returns(INPUT_BATTLE_STYLE);
            
            this.BATTLE_STYLES = new Dictionary<string, IBattleStyle>();
            this.BATTLE_STYLES.Add(INPUT_BATTLE_STYLE, battleStyle1);
        }

        #endregion Setup

        [Test]
        public void Constructor_RequiredFields_WithInputNull()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>();

            Assert.Throws<RequiredValueNotProvidedException>(() => new Class(config, data, true, BATTLE_STYLES));
        }

        [Test]
        public void Constructor_RequiredFields()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME
            };

            IClass cls = new Class(config, data, true, BATTLE_STYLES);

            Assert.That(cls.Name, Is.EqualTo(INPUT_NAME));
        }

        #region OptionalField_MovementType

        [Test]
        public void Constructor_OptionalField_MovementType_EmptyString()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                MovementType = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            //Movement type is conditionally required depending on whether or not the unit movement type field is in play
            bool isUnitMovementTypeConfigured = true;
            IClass cls = new Class(config, data, isUnitMovementTypeConfigured, BATTLE_STYLES);

            Assert.That(cls.Name, Is.EqualTo(INPUT_NAME));
            Assert.That(cls.MovementType, Is.Empty);

            isUnitMovementTypeConfigured = false;

            Assert.Throws<RequiredValueNotProvidedException>(() => new Class(config, data, isUnitMovementTypeConfigured, BATTLE_STYLES));
        }

        [Test]
        public void Constructor_OptionalField_MovementType()
        {
            string movementType = "Movement Type";

            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                MovementType = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                movementType
            };

            //Movement type is conditionally required depending on whether or not the unit movement type field is in play
            bool isUnitMovementTypeConfigured = true;
            IClass cls1 = new Class(config, data, isUnitMovementTypeConfigured, BATTLE_STYLES);

            Assert.That(cls1.Name, Is.EqualTo(INPUT_NAME));
            Assert.That(cls1.MovementType, Is.EqualTo(movementType));

            isUnitMovementTypeConfigured = false;
            IClass cls2 = new Class(config, data, isUnitMovementTypeConfigured, BATTLE_STYLES);

            Assert.That(cls2.Name, Is.EqualTo(INPUT_NAME));
            Assert.That(cls2.MovementType, Is.EqualTo(movementType));
        }

        #endregion OptionalField_MovementType

        #region OptionalField_BattleStyle

        [Test]
        public void Constructor_OptionalField_BattleStyle_EmptyString()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                BattleStyle = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            IClass cls = new Class(config, data, true, BATTLE_STYLES);

            Assert.That(cls.BattleStyle, Is.Null);
        }

        [Test]
        public void Constructor_OptionalField_BattleStyle_NoMatch()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                BattleStyle = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "Battle Style 2"
            };

            Assert.Throws<UnmatchedBattleStyleException>(() => new Class(config, data, true, BATTLE_STYLES));
        }

        [Test]
        public void Constructor_OptionalField_BattleStyle()
        {
            string battleStyle = "Battle Style 1";

            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                BattleStyle = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                battleStyle
            };

            IClass cls = new Class(config, data, true, BATTLE_STYLES);

            Assert.That(cls.BattleStyle, Is.Not.Null);
            Assert.That(cls.BattleStyle.Name, Is.EqualTo(battleStyle));
            Assert.That(cls.BattleStyle.Matched, Is.False);
        }

        #endregion OptionalField_BattleStyle

        #region OptionalField_Tags

        [Test]
        public void Constructor_OptionalField_Tags_EmptyString()
        {
            ClassesConfig config = new ClassesConfig()
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

            IClass cls = new Class(config, data, true, BATTLE_STYLES);

            Assert.That(cls.Tags, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_Tags()
        {
            string tag1 = "Tag 1";
            string tag2 = "Tag 2";
            string tag3 = "Tag 3";

            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                Tags = new List<int> { 1, 2 }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                tag1 + "," + tag2,
                tag3
            };

            IClass cls = new Class(config, data, true, BATTLE_STYLES);

            List<string> expected = new List<string>() { tag1, tag2, tag3 };
            Assert.That(cls.Tags, Is.EqualTo(expected));
        }

        #endregion OptionalField_Tags

        #region OptionalField_TextFields

        [Test]
        public void Constructor_OptionalField_TextFields_EmptyString()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                TextFields = new List<int> { 1, 2 }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty,
                string.Empty
            };

            IClass cls = new Class(config, data, true, BATTLE_STYLES);

            Assert.That(cls.TextFields, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_TextFields()
        {
            string textField1 = "Text Field 1";
            string textField2 = "Text Field 2";

            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                TextFields = new List<int> { 1, 2 }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                textField1,
                textField2
            };

            IClass cls = new Class(config, data, true, BATTLE_STYLES);

            List<string> expected = new List<string>() { textField1, textField2 };
            Assert.That(cls.TextFields, Is.EqualTo(expected));
        }

        #endregion OptionalField_TextFields

        #region FlagAsMatched

        [Test]
        public void FlagAsMatched()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME
            };

            IClass cls = new Class(config, data, true, BATTLE_STYLES);

            Assert.That(cls.Matched, Is.False);

            cls.FlagAsMatched();

            Assert.That(cls.Matched, Is.True);
        }

        [Test]
        public void FlagAsMatched_WithBattleStyle()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                BattleStyle = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_BATTLE_STYLE
            };

            IClass cls = new Class(config, data, true, BATTLE_STYLES);

            Assert.That(cls.Matched, Is.False);
            Assert.That(cls.BattleStyle.Matched, Is.False);

            cls.FlagAsMatched();

            Assert.That(cls.Matched, Is.True);
            cls.BattleStyle.Received().FlagAsMatched();
        }

        #endregion FlagAsMatched

        #region BuildDictionary

        [Test]
        public void BuildDictionary_WithInput_Null()
        {
            IDictionary<string, IClass> dict = Class.BuildDictionary(null, true, BATTLE_STYLES);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_NullQuery()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Queries = null,
                Name = 0
            };

            IDictionary<string, IClass> dict = Class.BuildDictionary(config, true, BATTLE_STYLES);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_EmptyQuery()
        {
            ClassesConfig config = new ClassesConfig()
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

            IDictionary<string, IClass> dict = Class.BuildDictionary(config, true, BATTLE_STYLES);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_DuplicateName()
        {
            ClassesConfig config = new ClassesConfig()
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

            Assert.Throws<ClassProcessingException>(() => Class.BuildDictionary(config, true, BATTLE_STYLES));
        }

        [Test]
        public void BuildDictionary()
        {
            ClassesConfig config = new ClassesConfig()
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

            IDictionary<string, IClass> dict = Class.BuildDictionary(config, true, BATTLE_STYLES);
            Assert.That(dict.Count, Is.EqualTo(1));
        }

        [Test]
        public void BuildDictionary_MultiQuery()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Queries = new List<IQuery>()
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

            IDictionary<string, IClass> dict = Class.BuildDictionary(config, true, BATTLE_STYLES);
            Assert.That(dict.Count, Is.EqualTo(4));
        }

        #endregion BuildDictionary

        #region MatchNames

        [Test]
        public void MatchNames_UnmatchedName()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Queries = new List<IQuery>()
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

            IDictionary<string, IClass> dict = Class.BuildDictionary(config, true, BATTLE_STYLES);
            IEnumerable<string> names = new List<string>() { "Class 3" };

            Assert.Throws<UnmatchedClassException>(() => Class.MatchNames(dict, names));
        }

        [Test]
        public void MatchNames_SingleMatch()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Queries = new List<IQuery>()
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

            IDictionary<string, IClass> dict = Class.BuildDictionary(config, true, BATTLE_STYLES);
            IEnumerable<string> names = new List<string>() { "Class 1" };

            List<IClass> matches = Class.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(1));
            Assert.That(matches.First().Matched, Is.True);
        }

        [Test]
        public void MatchNames_MultipleMatches()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Queries = new List<IQuery>()
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

            IDictionary<string, IClass> dict = Class.BuildDictionary(config, true, BATTLE_STYLES);
            IEnumerable<string> names = new List<string>() { "Class 1", "Class 2" };

            List<IClass> matches = Class.MatchNames(dict, names);
            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches[0].Matched, Is.True);
            Assert.That(matches[1].Matched, Is.True);
        }

        [Test]
        public void MatchNames_MultipleMatches_DoNotSetMatchedStatus()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Queries = new List<IQuery>()
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

            IDictionary<string, IClass> dict = Class.BuildDictionary(config, true, BATTLE_STYLES);
            IEnumerable<string> names = new List<string>() { "Class 1", "Class 2" };

            List<IClass> matches = Class.MatchNames(dict, names, false);
            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches[0].Matched, Is.False);
            Assert.That(matches[1].Matched, Is.False);
        }

        #endregion MatchNames
    }
}
