using NSubstitute;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Emblems;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    public class EmblemTests
    {
        #region Constants

        private const string INPUT_NAME = "Emblem Test";

        #endregion Constants

        [Test]
        public void Constructor_RequiredFields_WithInputNull()
        {
            EmblemsConfig config = new EmblemsConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>() { };

            Assert.Throws<RequiredValueNotProvidedException>(() => new Emblem(config, data));
        }

        [Test]
        public void Constructor_RequiredFields()
        {
            EmblemsConfig config = new EmblemsConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME };

            IEmblem emblem = new Emblem(config, data);

            Assert.That(emblem.Name, Is.EqualTo(INPUT_NAME));
        }

        #region OptionalField_SpriteURL

        [Test]
        public void Constructor_OptionalField_SpriteURL_EmptyString()
        {
            EmblemsConfig config = new EmblemsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            IEmblem emblem = new Emblem(config, data);

            Assert.That(emblem.SpriteURL, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_SpriteURL_InvalidURL()
        {
            EmblemsConfig config = new EmblemsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "NotAURL"
            };

            Assert.Throws<URLException>(() => new Emblem(config, data));
        }

        [Test]
        public void Constructor_OptionalField_SpriteURL()
        {
            EmblemsConfig config = new EmblemsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL
            };

            IEmblem emblem = new Emblem(config, data);

            Assert.That(emblem.SpriteURL, Is.EqualTo(UnitTestConsts.IMAGE_URL));
        }

        #endregion OptionalField_SpriteURL

        #region OptionalField_Tagline

        [Test]
        public void Constructor_OptionalField_Tagline_EmptyString()
        {
            EmblemsConfig config = new EmblemsConfig()
            {
                Name = 0,
                Tagline = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            IEmblem emblem = new Emblem(config, data);

            Assert.That(emblem.Tagline, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_Tagline()
        {
            string tagline = "This is my tagline";

            EmblemsConfig config = new EmblemsConfig()
            {
                Name = 0,
                Tagline = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                tagline
            };

            IEmblem emblem = new Emblem(config, data);

            Assert.That(emblem.Tagline, Is.EqualTo(tagline));
        }

        #endregion OptionalField_Tagline

        #region OptionalField_EngagedUnitAura

        [Test]
        public void Constructor_OptionalField_EngagedUnitAura_EmptyString()
        {
            EmblemsConfig config = new EmblemsConfig()
            {
                Name = 0,
                EngagedUnitAura = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            IEmblem emblem = new Emblem(config, data);

            Assert.That(emblem.EngagedUnitAura, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_EngagedUnitAura_InvalidHex()
        {
            EmblemsConfig config = new EmblemsConfig()
            {
                Name = 0,
                EngagedUnitAura = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "NotAHexCode"
            };

            Assert.Throws<HexException>(() => new Emblem(config, data));
        }

        [Test]
        public void Constructor_OptionalField_EngagedUnitAura()
        {
            string hexCode = "#F0F0F0";

            EmblemsConfig config = new EmblemsConfig()
            {
                Name = 0,
                EngagedUnitAura = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                hexCode
            };

            IEmblem emblem = new Emblem(config, data);

            Assert.That(emblem.EngagedUnitAura, Is.EqualTo(hexCode));
        }

        #endregion OptionalField_EngagedUnitAura

        #region OptionalField_TextFields

        [Test]
        public void Constructor_OptionalField_TextFields_EmptyString()
        {
            EmblemsConfig config = new EmblemsConfig()
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

            IEmblem emblem = new Emblem(config, data);

            Assert.That(emblem.TextFields, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_TextFields()
        {
            string textField1 = "Text Field 1";
            string textField2 = "Text Field 2";

            EmblemsConfig config = new EmblemsConfig()
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

            IEmblem emblem = new Emblem(config, data);

            IEnumerable<string> expected = new List<string>() { textField1, textField2 };
            Assert.That(emblem.TextFields, Is.EqualTo(expected));
        }

        #endregion OptionalField_TextFields

        #region FlagAsMatched

        [Test]
        public void FlagAsMatched()
        {
            EmblemsConfig config = new EmblemsConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME };

            IEmblem emblem = new Emblem(config, data);

            Assert.That(emblem.Matched, Is.False);

            emblem.FlagAsMatched();

            Assert.That(emblem.Matched, Is.True);
        }

        #endregion FlagAsMatched

        #region BuildDictionary

        [Test]
        public void BuildDictionary_WithInput_Null()
        {
            IDictionary<string, IEmblem> dict = Emblem.BuildDictionary(null);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_NullQuery()
        {
            EmblemsConfig config = new EmblemsConfig()
            {
                Queries = null,
                Name = 0
            };

            IDictionary<string, IEmblem> dict = Emblem.BuildDictionary(config);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_EmptyQuery()
        {
            EmblemsConfig config = new EmblemsConfig()
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

            IDictionary<string, IEmblem> dict = Emblem.BuildDictionary(config);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_DuplicateName()
        {
            EmblemsConfig config = new EmblemsConfig()
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

            Assert.Throws<EmblemProcessingException>(() => Emblem.BuildDictionary(config));
        }

        [Test]
        public void BuildDictionary()
        {
            EmblemsConfig config = new EmblemsConfig()
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

            IDictionary<string, IEmblem> dict = Emblem.BuildDictionary(config);
            Assert.That(dict.Count, Is.EqualTo(1));
        }

        [Test]
        public void BuildDictionary_MultiQuery()
        {
            EmblemsConfig config = new EmblemsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Emblem 1" },
                            new List<object>(){ "Emblem 2" }
                        }
                    },
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Emblem 3" },
                            new List<object>(){ "Emblem 4" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, IEmblem> dict = Emblem.BuildDictionary(config);
            Assert.That(dict.Count, Is.EqualTo(4));
        }

        #endregion BuildDictionary

        #region MatchNames

        [Test]
        public void MatchNames_UnmatchedName()
        {
            string emblem1Name = "Emblem 1";
            string emblem2Name = "Emblem 2";

            IEmblem emblem1 = Substitute.For<IEmblem>();
            emblem1.Name.Returns(emblem1Name);

            IDictionary<string, IEmblem> dict = new Dictionary<string, IEmblem>();
            dict.Add(emblem1Name, emblem1);

            IEnumerable<string> names = new List<string>() { emblem2Name };

            Assert.Throws<UnmatchedEmblemException>(() => Emblem.MatchNames(dict, names));
        }

        [Test]
        public void MatchNames_SingleMatch()
        {
            string emblem1Name = "Emblem 1";
            string emblem2Name = "Emblem 2";

            IEmblem emblem1 = Substitute.For<IEmblem>();
            emblem1.Name.Returns(emblem1Name);

            IEmblem emblem2 = Substitute.For<IEmblem>();
            emblem2.Name.Returns(emblem2Name);

            IDictionary<string, IEmblem> dict = new Dictionary<string, IEmblem>();
            dict.Add(emblem1Name, emblem1);
            dict.Add(emblem2Name, emblem2);

            IEnumerable<string> names = new List<string>() { emblem1Name };
            List<IEmblem> matches = Emblem.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(1));
            Assert.That(matches.Contains(emblem1), Is.True);
            matches.First().Received(1).FlagAsMatched();
        }

        [Test]
        public void MatchNames_MultipleMatches()
        {
            string emblem1Name = "Emblem 1";
            string emblem2Name = "Emblem 2";

            IEmblem emblem1 = Substitute.For<IEmblem>();
            emblem1.Name.Returns(emblem1Name);

            IEmblem emblem2 = Substitute.For<IEmblem>();
            emblem2.Name.Returns(emblem2Name);

            IDictionary<string, IEmblem> dict = new Dictionary<string, IEmblem>();
            dict.Add(emblem1Name, emblem1);
            dict.Add(emblem2Name, emblem2);

            IEnumerable<string> names = new List<string>() { emblem1Name, emblem2Name };
            List<IEmblem> matches = Emblem.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches.Contains(emblem1), Is.True);
            Assert.That(matches.Contains(emblem2), Is.True);

            matches[0].Received(1).FlagAsMatched();
            matches[1].Received(1).FlagAsMatched();
        }

        [Test]
        public void MatchNames_MultipleMatches_DoNotSetMatchedStatus()
        {
            string emblem1Name = "Emblem 1";
            string emblem2Name = "Emblem 2";

            IEmblem emblem1 = Substitute.For<IEmblem>();
            emblem1.Name.Returns(emblem1Name);

            IEmblem emblem2 = Substitute.For<IEmblem>();
            emblem2.Name.Returns(emblem2Name);

            IDictionary<string, IEmblem> dict = new Dictionary<string, IEmblem>();
            dict.Add(emblem1Name, emblem1);
            dict.Add(emblem2Name, emblem2);

            IEnumerable<string> names = new List<string>() { emblem1Name, emblem2Name };
            List<IEmblem> matches = Emblem.MatchNames(dict, names, false);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches.Contains(emblem1), Is.True);
            Assert.That(matches.Contains(emblem2), Is.True);

            matches[0].DidNotReceive().FlagAsMatched();
            matches[1].DidNotReceive().FlagAsMatched();
        }

        #endregion MatchNames

        #region MatchName

        [Test]
        public void MatchName_UnmatchedName()
        {
            string emblem1Name = "Emblem 1";

            IEmblem emblem1 = Substitute.For<IEmblem>();
            emblem1.Name.Returns(emblem1Name);

            IDictionary<string, IEmblem> dict = new Dictionary<string, IEmblem>();
            dict.Add(emblem1Name, emblem1);

            string name = "Emblem 2";

            Assert.Throws<UnmatchedEmblemException>(() => Emblem.MatchName(dict, name));
        }

        [Test]
        public void MatchName()
        {
            string emblem1Name = "Emblem 1";

            IEmblem emblem1 = Substitute.For<IEmblem>();
            emblem1.Name.Returns(emblem1Name);

            IDictionary<string, IEmblem> dict = new Dictionary<string, IEmblem>();
            dict.Add(emblem1Name, emblem1);

            IEmblem match = Emblem.MatchName(dict, emblem1Name);

            Assert.That(match, Is.Not.Null);
            Assert.That(match, Is.EqualTo(emblem1));
            match.Received(1).FlagAsMatched();
        }

        [Test]
        public void MatchName_DoNotSetMatchedStatus()
        {
            string emblem1Name = "Emblem 1";

            IEmblem emblem1 = Substitute.For<IEmblem>();
            emblem1.Name.Returns(emblem1Name);

            IDictionary<string, IEmblem> dict = new Dictionary<string, IEmblem>();
            dict.Add(emblem1Name, emblem1);

            IEmblem match = Emblem.MatchName(dict, emblem1Name, false);

            Assert.That(match, Is.Not.Null);
            Assert.That(match, Is.EqualTo(emblem1));
            match.DidNotReceive().FlagAsMatched();
        }

        #endregion MatchName
    }
}
