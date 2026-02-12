using NSubstitute;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.BattleStyles;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    public class BattleStyleTests
    {
        #region Constants

        private const string INPUT_NAME = "Battle Style Test";

        #endregion Constants

        [Test]
        public void Constructor_RequiredFields_WithInputNull()
        {
            BattleStylesConfig config = new BattleStylesConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>() { };

            Assert.Throws<RequiredValueNotProvidedException>(() => new BattleStyle(config, data));
        }

        [Test]
        public void Constructor_RequiredFields()
        {
            BattleStylesConfig config = new BattleStylesConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME
            };

            IBattleStyle style = new BattleStyle(config, data);

            Assert.That(style.Name, Is.EqualTo(INPUT_NAME));
        }

        #region OptionalField_SpriteURL

        public void Constructor_OptionalField_SpriteURL_EmptyString()
        {
            BattleStylesConfig config = new BattleStylesConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            IBattleStyle style = new BattleStyle(config, data);

            Assert.That(style.SpriteURL, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_SpriteURL()
        {
            BattleStylesConfig config = new BattleStylesConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL
            };

            IBattleStyle style = new BattleStyle(config, data);

            Assert.That(style.SpriteURL, Is.EqualTo(UnitTestConsts.IMAGE_URL));
        }

        #endregion OptionalField_SpriteURL

        #region OptionalField_TextFields

        [Test]
        public void Constructor_OptionalField_TextFields_EmptyString()
        {
            BattleStylesConfig config = new BattleStylesConfig()
            {
                Name = 0,
                TextFields = new List<int>() { 1, 2 }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty,
                string.Empty
            };

            IBattleStyle style = new BattleStyle(config, data);

            Assert.That(style.TextFields, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_TextFields()
        {
            string textField1 = "Text Field 1";
            string textField2 = "Text Field 2";

            BattleStylesConfig config = new BattleStylesConfig()
            {
                Name = 0,
                TextFields = new List<int>() { 1, 2 }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                textField1,
                textField2
            };

            IBattleStyle style = new BattleStyle(config, data);

            IEnumerable<string> expected = new List<string>() { textField1, textField2 };
            Assert.That(style.TextFields, Is.EqualTo(expected));
        }

        #endregion OptionalField_TextFields

        #region FlagAsMatched

        [Test]
        public void FlagAsMatched()
        {
            BattleStylesConfig config = new BattleStylesConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME
            };

            IBattleStyle style = new BattleStyle(config, data);

            Assert.That(style.Matched, Is.False);

            style.FlagAsMatched();

            Assert.That(style.Matched, Is.True);
        }

        #endregion FlagAsMatched

        #region BuildDictionary

        [Test]
        public void BuildDictionary_WithInput_Null()
        {
            IDictionary<string, IBattleStyle> dict = BattleStyle.BuildDictionary(null);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_NullQuery()
        {
            BattleStylesConfig config = new BattleStylesConfig()
            {
                Queries = null,
                Name = 0
            };

            IDictionary<string, IBattleStyle> dict = BattleStyle.BuildDictionary(config);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_EmptyQuery()
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

            IDictionary<string, IBattleStyle> dict = BattleStyle.BuildDictionary(config);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_DuplicateName()
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

            Assert.Throws<BattleStyleProcessingException>(() => BattleStyle.BuildDictionary(config));
        }

        [Test]
        public void BuildDictionary_WithInput_Invalid()
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

            Assert.Throws<BattleStyleProcessingException>(() => BattleStyle.BuildDictionary(config));
        }

        [Test]
        public void BuildDictionary()
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

            IDictionary<string, IBattleStyle> dict = BattleStyle.BuildDictionary(config);
            Assert.That(dict.Count, Is.EqualTo(1));
        }

        [Test]
        public void BuildDictionary_MultiQuery()
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

            IDictionary<string, IBattleStyle> dict = BattleStyle.BuildDictionary(config);
            Assert.That(dict.Count, Is.EqualTo(4));
        }

        #endregion BuildDictionary

        #region MatchNames

        [Test]
        public void MatchNames_UnmatchedName()
        {
            string style1Name = "Battle Style 1";
            string style2Name = "Battle Style 2";

            IBattleStyle style1 = Substitute.For<IBattleStyle>();
            style1.Name.Returns(style1Name);

            IDictionary<string, IBattleStyle> dict = new Dictionary<string, IBattleStyle>();
            dict.Add(style1Name, style1);

            IEnumerable<string> names = new List<string>() { style2Name };

            Assert.Throws<UnmatchedBattleStyleException>(() => BattleStyle.MatchNames(dict, names));
        }

        [Test]
        public void MatchNames_SingleMatch()
        {
            string style1Name = "Battle Style 1";
            string style2Name = "Battle Style 2";

            IBattleStyle style1 = Substitute.For<IBattleStyle>();
            style1.Name.Returns(style1Name);

            IBattleStyle style2 = Substitute.For<IBattleStyle>();
            style2.Name.Returns(style2Name);

            IDictionary<string, IBattleStyle> dict = new Dictionary<string, IBattleStyle>();
            dict.Add(style1Name, style1);
            dict.Add(style2Name, style2);

            IEnumerable<string> names = new List<string>() { style1Name };
            List<IBattleStyle> matches = BattleStyle.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(1));
            Assert.That(matches.Contains(style1), Is.True);
            matches.First().Received(1).FlagAsMatched();
        }

        [Test]
        public void MatchNames_MultipleMatches()
        {
            string style1Name = "Battle Style 1";
            string style2Name = "Battle Style 2";

            IBattleStyle style1 = Substitute.For<IBattleStyle>();
            style1.Name.Returns(style1Name);

            IBattleStyle style2 = Substitute.For<IBattleStyle>();
            style2.Name.Returns(style2Name);

            IDictionary<string, IBattleStyle> dict = new Dictionary<string, IBattleStyle>();
            dict.Add(style1Name, style1);
            dict.Add(style2Name, style2);

            IEnumerable<string> names = new List<string>() { style1Name, style2Name };
            List<IBattleStyle> matches = BattleStyle.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches.Contains(style1), Is.True);
            Assert.That(matches.Contains(style2), Is.True);

            matches[0].Received(1).FlagAsMatched();
            matches[1].Received(1).FlagAsMatched();
        }

        [Test]
        public void MatchNames_MultipleMatches_DoNotSetMatchedStatus()
        {
            string style1Name = "Battle Style 1";
            string style2Name = "Battle Style 2";

            IBattleStyle style1 = Substitute.For<IBattleStyle>();
            style1.Name.Returns(style1Name);

            IBattleStyle style2 = Substitute.For<IBattleStyle>();
            style2.Name.Returns(style2Name);

            IDictionary<string, IBattleStyle> dict = new Dictionary<string, IBattleStyle>();
            dict.Add(style1Name, style1);
            dict.Add(style2Name, style2);

            IEnumerable<string> names = new List<string>() { style1Name, style2Name };
            List<IBattleStyle> matches = BattleStyle.MatchNames(dict, names, false);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches.Contains(style1), Is.True);
            Assert.That(matches.Contains(style2), Is.True);

            matches[0].DidNotReceive().FlagAsMatched();
            matches[1].DidNotReceive().FlagAsMatched();
        }

        #endregion MatchNames

        #region MatchName

        [Test]
        public void MatchName_UnmatchedName()
        {
            string style1Name = "Battle Style 1";

            IBattleStyle style1 = Substitute.For<IBattleStyle>();
            style1.Name.Returns(style1Name);

            IDictionary<string, IBattleStyle> dict = new Dictionary<string, IBattleStyle>();
            dict.Add(style1Name, style1);

            string name = "Battle Style 2";

            Assert.Throws<UnmatchedBattleStyleException>(() => BattleStyle.MatchName(dict, name));
        }

        [Test]
        public void MatchName()
        {
            string style1Name = "Battle Style 1";

            IBattleStyle style1 = Substitute.For<IBattleStyle>();
            style1.Name.Returns(style1Name);

            IDictionary<string, IBattleStyle> dict = new Dictionary<string, IBattleStyle>();
            dict.Add(style1Name, style1);

            IBattleStyle match = BattleStyle.MatchName(dict, style1Name);

            Assert.That(match, Is.Not.Null);
            Assert.That(match, Is.EqualTo(style1));
            match.Received(1).FlagAsMatched();
        }

        [Test]
        public void MatchName_DoNotSetMatchedStatus()
        {
            string style1Name = "Battle Style 1";

            IBattleStyle style1 = Substitute.For<IBattleStyle>();
            style1.Name.Returns(style1Name);

            IDictionary<string, IBattleStyle> dict = new Dictionary<string, IBattleStyle>();
            dict.Add(style1Name, style1);

            IBattleStyle match = BattleStyle.MatchName(dict, style1Name, false);

            Assert.That(match, Is.Not.Null);
            Assert.That(match, Is.EqualTo(style1));
            match.DidNotReceive().FlagAsMatched();
        }

        #endregion MatchName
    }
}