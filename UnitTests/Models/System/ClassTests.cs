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
            cls.BattleStyle.DidNotReceive().FlagAsMatched();

            cls.FlagAsMatched();

            Assert.That(cls.Matched, Is.True);
            cls.BattleStyle.Received(1).FlagAsMatched();
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

            IDictionary<string, IClass> dict = Class.BuildDictionary(config, true, BATTLE_STYLES);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_DuplicateName()
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

            Assert.Throws<ClassProcessingException>(() => Class.BuildDictionary(config, true, BATTLE_STYLES));
        }

        [Test]
        public void BuildDictionary()
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

            IDictionary<string, IClass> dict = Class.BuildDictionary(config, true, BATTLE_STYLES);
            Assert.That(dict.Count, Is.EqualTo(1));
        }

        [Test]
        public void BuildDictionary_MultiQuery()
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

            IDictionary<string, IClass> dict = Class.BuildDictionary(config, true, BATTLE_STYLES);
            Assert.That(dict.Count, Is.EqualTo(4));
        }

        #endregion BuildDictionary

        #region MatchNames

        [Test]
        public void MatchNames_UnmatchedName()
        {
            string class1Name = "Class 1";
            string class2Name = "Class 2";

            IClass class1 = Substitute.For<IClass>();
            class1.Name.Returns(class1Name);

            IDictionary<string, IClass> dict = new Dictionary<string, IClass>();
            dict.Add(class1Name, class1);

            IEnumerable<string> names = new List<string>() { class2Name };

            Assert.Throws<UnmatchedClassException>(() => Class.MatchNames(dict, names));
        }

        [Test]
        public void MatchNames_SingleMatch()
        {
            string class1Name = "Class 1";
            string class2Name = "Class 2";

            IClass class1 = Substitute.For<IClass>();
            class1.Name.Returns(class1Name);

            IClass class2 = Substitute.For<IClass>();
            class2.Name.Returns(class2Name);

            IDictionary<string, IClass> dict = new Dictionary<string, IClass>();
            dict.Add(class1Name, class1);
            dict.Add(class2Name, class2);

            IEnumerable<string> names = new List<string>() { class1Name };
            List<IClass> matches = Class.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(1));
            Assert.That(matches.Contains(class1), Is.True);
            matches.First().Received(1).FlagAsMatched();
        }

        [Test]
        public void MatchNames_MultipleMatches()
        {
            string class1Name = "Class 1";
            string class2Name = "Class 2";

            IClass class1 = Substitute.For<IClass>();
            class1.Name.Returns(class1Name);

            IClass class2 = Substitute.For<IClass>();
            class2.Name.Returns(class2Name);

            IDictionary<string, IClass> dict = new Dictionary<string, IClass>();
            dict.Add(class1Name, class1);
            dict.Add(class2Name, class2);

            IEnumerable<string> names = new List<string>() { class1Name, class2Name };
            List<IClass> matches = Class.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches.Contains(class1), Is.True);
            Assert.That(matches.Contains(class2), Is.True);

            matches[0].Received(1).FlagAsMatched();
            matches[1].Received(1).FlagAsMatched();
        }

        [Test]
        public void MatchNames_MultipleMatches_DoNotSetMatchedStatus()
        {
            string class1Name = "Class 1";
            string class2Name = "Class 2";

            IClass class1 = Substitute.For<IClass>();
            class1.Name.Returns(class1Name);

            IClass class2 = Substitute.For<IClass>();
            class2.Name.Returns(class2Name);

            IDictionary<string, IClass> dict = new Dictionary<string, IClass>();
            dict.Add(class1Name, class1);
            dict.Add(class2Name, class2);

            IEnumerable<string> names = new List<string>() { class1Name, class2Name };
            List<IClass> matches = Class.MatchNames(dict, names, false);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches.Contains(class1), Is.True);
            Assert.That(matches.Contains(class2), Is.True);

            matches[0].DidNotReceive().FlagAsMatched();
            matches[1].DidNotReceive().FlagAsMatched();
        }

        #endregion MatchNames

        #region MatchName

        [Test]
        public void MatchName_UnmatchedName()
        {
            string class1Name = "Class 1";

            IClass class1 = Substitute.For<IClass>();
            class1.Name.Returns(class1Name);

            IDictionary<string, IClass> dict = new Dictionary<string, IClass>();
            dict.Add(class1Name, class1);

            string name = "Class 2";

            Assert.Throws<UnmatchedClassException>(() => Class.MatchName(dict, name));
        }

        [Test]
        public void MatchName()
        {
            string class1Name = "Class 1";

            IClass class1 = Substitute.For<IClass>();
            class1.Name.Returns(class1Name);

            IDictionary<string, IClass> dict = new Dictionary<string, IClass>();
            dict.Add(class1Name, class1);

            IClass match = Class.MatchName(dict, class1Name);

            Assert.That(match, Is.Not.Null);
            Assert.That(match, Is.EqualTo(class1));
            match.Received(1).FlagAsMatched();
        }

        [Test]
        public void MatchName_DoNotSetMatchedStatus()
        {
            string class1Name = "Class 1";

            IClass class1 = Substitute.For<IClass>();
            class1.Name.Returns(class1Name);

            IDictionary<string, IClass> dict = new Dictionary<string, IClass>();
            dict.Add(class1Name, class1);

            IClass match = Class.MatchName(dict, class1Name, false);

            Assert.That(match, Is.Not.Null);
            Assert.That(match, Is.EqualTo(class1));
            match.DidNotReceive().FlagAsMatched();
        }

        #endregion MatchName
    }
}
