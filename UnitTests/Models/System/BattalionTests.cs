using NSubstitute;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Battalions;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    internal class BattalionTests
    {
        #region Constants

        private const string INPUT_NAME = "Battalion Test";
        private const string INPUT_GAMBIT = "Gambit 1";
        private const string INPUT_MAX_ENDURANCE = "1";

        #endregion Constants

        #region Setup

        private IDictionary<string, IGambit> GAMBITS;

        [SetUp]
        public void SetUp()
        {
            IGambit gambit1 = Substitute.For<IGambit>();
            gambit1.Name.Returns(INPUT_GAMBIT);

            GAMBITS = new Dictionary<string, IGambit>();
            GAMBITS.Add(INPUT_GAMBIT, gambit1);
        }

        #endregion Setup

        [Test]
        public void Constructor_RequiredFields_WithInputNull()
        {
            BattalionsConfig config = new BattalionsConfig()
            {
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>()
            };

            IEnumerable<string> data = new List<string>();

            Assert.Throws<RequiredValueNotProvidedException>(() => new Battalion(config, data, GAMBITS));
        }

        [Test]
        public void Constructor_RequiredFields_WithInput_EmptyStringGambit()
        {
            BattalionsConfig config = new BattalionsConfig()
            {
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>()
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            Assert.Throws<RequiredValueNotProvidedException>(() => new Battalion(config, data, GAMBITS));
        }

        [Test]
        public void Constructor_RequiredFields_WithInput_UnmatchedStringGambit()
        {
            BattalionsConfig config = new BattalionsConfig()
            {
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>()
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "Gambit 3"
            };

            Assert.Throws<UnmatchedGambitException>(() => new Battalion(config, data, GAMBITS));
        }

        [Test]
        public void Constructor_RequiredFields_WithInput_EmptyMaxEndurance()
        {
            BattalionsConfig config = new BattalionsConfig()
            {
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>()
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GAMBIT,
                string.Empty
            };

            Assert.Throws<PositiveIntegerException>(() => new Battalion(config, data, GAMBITS));
        }

        [Test]
        public void Constructor_RequiredFields_WithInput_NegativeMaxEndurance()
        {
            BattalionsConfig config = new BattalionsConfig()
            {
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>()
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GAMBIT,
                "-1"
            };

            Assert.Throws<PositiveIntegerException>(() => new Battalion(config, data, GAMBITS));
        }

        [Test]
        public void Constructor_RequiredFields()
        {
            BattalionsConfig config = new BattalionsConfig()
            {
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>()
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GAMBIT,
                INPUT_MAX_ENDURANCE
            };

            IBattalion batt = new Battalion(config, data, GAMBITS);

            Assert.That(batt.Name, Is.EqualTo(INPUT_NAME));
            Assert.That(batt.Gambit, Is.EqualTo(GAMBITS[INPUT_GAMBIT]));
            Assert.That(batt.Gambit.Matched, Is.False);
            Assert.That(batt.MaxEndurance, Is.EqualTo(1));
        }

        #region OptionalField_SpriteURL

        [Test]
        public void Constructor_OptionalField_SpriteURL_EmptyString()
        {
            BattalionsConfig config = new BattalionsConfig()
            {
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>(),
                SpriteURL = 3
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GAMBIT,
                INPUT_MAX_ENDURANCE,
                string.Empty
            };

            IBattalion batt = new Battalion(config, data, GAMBITS);

            Assert.That(batt.SpriteURL, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_SpriteURL_InvalidURL()
        {
            BattalionsConfig config = new BattalionsConfig()
            {
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>(),
                SpriteURL = 3
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GAMBIT,
                INPUT_MAX_ENDURANCE,
                "NotAURL"
            };

            Assert.Throws<URLException>(() => new Battalion(config, data, GAMBITS));
        }

        [Test]
        public void Constructor_OptionalField_SpriteURL()
        {
            BattalionsConfig config = new BattalionsConfig()
            {
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>(),
                SpriteURL = 3
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GAMBIT,
                INPUT_MAX_ENDURANCE,
                UnitTestConsts.IMAGE_URL
            };

            IBattalion batt = new Battalion(config, data, GAMBITS);

            Assert.That(batt.SpriteURL, Is.EqualTo(UnitTestConsts.IMAGE_URL));
        }

        #endregion OptionalField_SpriteURL

        #region OptionalField_Rank

        [Test]
        public void Constructor_OptionalField_Rank_EmptyString()
        {
            BattalionsConfig config = new BattalionsConfig()
            {
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>(),
                Rank = 3
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GAMBIT,
                INPUT_MAX_ENDURANCE,
                string.Empty
            };

            IBattalion batt = new Battalion(config, data, GAMBITS);

            Assert.That(batt.Rank, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_Rank()
        {
            string rank = "E";

            BattalionsConfig config = new BattalionsConfig()
            {
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>(),
                Rank = 3
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GAMBIT,
                INPUT_MAX_ENDURANCE,
                rank
            };

            IBattalion batt = new Battalion(config, data, GAMBITS);

            Assert.That(batt.Rank, Is.EqualTo(rank));
        }

        #endregion OptionalField_Rank

        #region OptionalField_TextFields

        [Test]
        public void Constructor_OptionalField_TextFields_EmptyString()
        {
            BattalionsConfig config = new BattalionsConfig()
            {
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>(),
                TextFields = new List<int> { 3, 4 }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GAMBIT,
                INPUT_MAX_ENDURANCE,
                string.Empty,
                string.Empty
            };

            IBattalion batt = new Battalion(config, data, GAMBITS);

            Assert.That(batt.TextFields, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_TextFields()
        {
            string textField1 = "Text Field 1";
            string textField2 = "Text Field 2";

            BattalionsConfig config = new BattalionsConfig()
            {
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>(),
                TextFields = new List<int> { 3, 4 }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GAMBIT,
                INPUT_MAX_ENDURANCE,
                textField1,
                textField2
            };

            IBattalion batt = new Battalion(config, data, GAMBITS);

            List<string> expected = new List<string>() { textField1, textField2 };
            Assert.That(batt.TextFields, Is.EqualTo(expected));
        }

        #endregion OptionalField_TextFields

        #region OptionalField_Stats

        [Test]
        public void Constructor_OptionalField_Stats_EmptyStrings()
        {
            string stat1 = "Stat 1";
            string stat2 = "Stat 2";
            string stat3 = "Stat 3";

            BattalionsConfig config = new BattalionsConfig()
            {
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>()
                {
                    new NamedStatConfig{ SourceName = stat1, Value = 3 },
                    new NamedStatConfig{ SourceName = stat2, Value = 4 },
                    new NamedStatConfig{ SourceName = stat3, Value = 5 }
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GAMBIT,
                INPUT_MAX_ENDURANCE,
                string.Empty,
                string.Empty,
                string.Empty
            };

            IBattalion batt = new Battalion(config, data, GAMBITS);

            Assert.That(batt.Stats.Count, Is.EqualTo(3));
            Assert.That(batt.Stats[stat1], Is.EqualTo(0));
            Assert.That(batt.Stats[stat2], Is.EqualTo(0));
            Assert.That(batt.Stats[stat3], Is.EqualTo(0));
        }

        [Test]
        public void Constructor_OptionalField_Stats()
        {
            string stat1 = "Stat 1";
            string stat2 = "Stat 2";
            string stat3 = "Stat 3";

            BattalionsConfig config = new BattalionsConfig()
            {
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>()
                {
                    new NamedStatConfig{ SourceName = stat1, Value = 3 },
                    new NamedStatConfig{ SourceName = stat2, Value = 4 },
                    new NamedStatConfig{ SourceName = stat3, Value = 5 }
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GAMBIT,
                INPUT_MAX_ENDURANCE,
                "-1",
                "0",
                "1"
            };

            IBattalion batt = new Battalion(config, data, GAMBITS);

            Assert.That(batt.Stats.Count, Is.EqualTo(3));
            Assert.That(batt.Stats[stat1], Is.EqualTo(-1));
            Assert.That(batt.Stats[stat2], Is.EqualTo(0));
            Assert.That(batt.Stats[stat3], Is.EqualTo(1));
        }

        #endregion OptionalField_Stats

        #region OptionalField_StatModifiers

        [Test]
        public void Constructor_OptionalField_StatModifiers_EmptyStrings()
        {
            BattalionsConfig config = new BattalionsConfig()
            {
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>(),
                StatModifiers = new List<NamedStatConfig>()
                {
                    new NamedStatConfig{ SourceName = "Stat 1", Value = 3 },
                    new NamedStatConfig{ SourceName = "Stat 2", Value = 4 },
                    new NamedStatConfig{ SourceName = "Stat 3", Value = 5 }
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GAMBIT,
                INPUT_MAX_ENDURANCE,
                string.Empty,
                string.Empty,
                string.Empty
            };

            IBattalion batt = new Battalion(config, data, GAMBITS);

            Assert.That(batt.StatModifiers, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_StatModifiers()
        {
            string stat1 = "Stat 1";
            string stat2 = "Stat 2";
            string stat3 = "Stat 3";

            BattalionsConfig config = new BattalionsConfig()
            {
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>(),
                StatModifiers = new List<NamedStatConfig>()
                {
                    new NamedStatConfig{ SourceName = stat1, Value = 3 },
                    new NamedStatConfig{ SourceName = stat2, Value = 4 },
                    new NamedStatConfig{ SourceName = stat3, Value = 5 }
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GAMBIT,
                INPUT_MAX_ENDURANCE,
                "-1",
                "0",
                "1"
            };

            IBattalion batt = new Battalion(config, data, GAMBITS);

            Assert.That(batt.StatModifiers.Count, Is.EqualTo(2));
            Assert.That(batt.StatModifiers[stat1], Is.EqualTo(-1));
            Assert.That(batt.StatModifiers.ContainsKey(stat2), Is.False);
            Assert.That(batt.StatModifiers[stat3], Is.EqualTo(1));
        }

        #endregion OptionalField_StatModifiers

        #region MatchStatName

        [Test]
        public void MatchStatName_UnmatchedStatName()
        {
            string stat1 = "Stat 1";

            BattalionsConfig config = new BattalionsConfig()
            {
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>()
                {
                    new NamedStatConfig(){ SourceName = stat1, Value = 3 }
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GAMBIT,
                INPUT_MAX_ENDURANCE,
                "3"
            };

            IBattalion batt = new Battalion(config, data, GAMBITS);

            Assert.Throws<UnmatchedStatException>(() => batt.MatchStatName("Stat 2"));
        }

        [Test]
        public void MatchStatName()
        {
            string stat1 = "Stat 1";

            BattalionsConfig config = new BattalionsConfig()
            {
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>()
                {
                    new NamedStatConfig(){ SourceName = stat1, Value = 3 }
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GAMBIT,
                INPUT_MAX_ENDURANCE,
                "3"
            };

            IBattalion batt = new Battalion(config, data, GAMBITS);

            int value = batt.MatchStatName(stat1);
            Assert.That(value, Is.EqualTo(3));
        }

        #endregion MatchStatName

        #region FlagAsMatched

        [Test]
        public void FlagAsMatched()
        {
            BattalionsConfig config = new BattalionsConfig()
            {
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>()
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GAMBIT,
                INPUT_MAX_ENDURANCE
            };

            IBattalion batt = new Battalion(config, data, GAMBITS);

            Assert.That(batt.Matched, Is.False);
            batt.Gambit.DidNotReceive().FlagAsMatched();

            batt.FlagAsMatched();

            Assert.That(batt.Matched, Is.True);
            batt.Gambit.Received(1).FlagAsMatched();
        }

        #endregion FlagAsMatched

        #region BuildDictionary

        [Test]
        public void BuildDictionary_WithInput_Null()
        {
            IDictionary<string, IBattalion> dict = Battalion.BuildDictionary(null, GAMBITS);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_NullQuery()
        {
            BattalionsConfig config = new BattalionsConfig()
            {
                Queries = null,
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>()
            };

            IDictionary<string, IBattalion> dict = Battalion.BuildDictionary(config, GAMBITS);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_EmptyQuery()
        {
            BattalionsConfig config = new BattalionsConfig()
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
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>()
            };

            IDictionary<string, IBattalion> dict = Battalion.BuildDictionary(config, GAMBITS);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_DuplicateName()
        {
            BattalionsConfig config = new BattalionsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_NAME, INPUT_GAMBIT, INPUT_MAX_ENDURANCE },
                            new List<object>(){ INPUT_NAME, INPUT_GAMBIT, INPUT_MAX_ENDURANCE }
                        }
                    }
                },
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>()
            };

            Assert.Throws<BattalionProcessingException>(() => Battalion.BuildDictionary(config, GAMBITS));
        }

        [Test]
        public void BuildDictionary_WithInput_Invalid()
        {
            BattalionsConfig config = new BattalionsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_NAME, string.Empty, INPUT_MAX_ENDURANCE },
                            new List<object>(){ INPUT_NAME, INPUT_GAMBIT, INPUT_MAX_ENDURANCE }
                        }
                    }
                },
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>()
            };

            Assert.Throws<BattalionProcessingException>(() => Battalion.BuildDictionary(config, GAMBITS));
        }

        [Test]
        public void BuildDictionary()
        {
            BattalionsConfig config = new BattalionsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_NAME, INPUT_GAMBIT, INPUT_MAX_ENDURANCE }
                        }
                    }
                },
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>()
            };

            IDictionary<string, IBattalion> dict = Battalion.BuildDictionary(config, GAMBITS);
            Assert.That(dict.Count, Is.EqualTo(1));
        }

        [Test]
        public void BuildDictionary_MultiQuery()
        {
            BattalionsConfig config = new BattalionsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Battalion 1", INPUT_GAMBIT, INPUT_MAX_ENDURANCE },
                            new List<object>(){ "Battalion 2", INPUT_GAMBIT, INPUT_MAX_ENDURANCE }
                        }
                    },
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Battalion 3", INPUT_GAMBIT, INPUT_MAX_ENDURANCE },
                            new List<object>(){ "Battalion 4", INPUT_GAMBIT, INPUT_MAX_ENDURANCE }
                        }
                    }
                },
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>()
            };

            IDictionary<string, IBattalion> dict = Battalion.BuildDictionary(config, GAMBITS);
            Assert.That(dict.Count, Is.EqualTo(4));
        }

        #endregion BuildDictionary

        #region MatchNames

        [Test]
        public void MatchNames_UnmatchedName()
        {
            string batt1Name = "Battalion 1";
            string batt2Name = "Battalion 2";

            IBattalion batt1 = Substitute.For<IBattalion>();
            batt1.Name.Returns(batt1Name);

            IDictionary<string, IBattalion> dict = new Dictionary<string, IBattalion>();
            dict.Add(batt1Name, batt1);

            IEnumerable<string> names = new List<string>() { batt2Name };

            Assert.Throws<UnmatchedBattalionException>(() => Battalion.MatchNames(dict, names));
        }

        [Test]
        public void MatchNames_SingleMatch()
        {
            string batt1Name = "Battalion 1";
            string batt2Name = "Battalion 2";

            IBattalion batt1 = Substitute.For<IBattalion>();
            batt1.Name.Returns(batt1Name);

            IBattalion batt2 = Substitute.For<IBattalion>();
            batt2.Name.Returns(batt2Name);

            IDictionary<string, IBattalion> dict = new Dictionary<string, IBattalion>();
            dict.Add(batt1Name, batt1);
            dict.Add(batt2Name, batt2);

            IEnumerable<string> names = new List<string>() { batt1Name };
            List<IBattalion> matches = Battalion.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(1));
            Assert.That(matches.Contains(batt1), Is.True);
            matches.First().Received(1).FlagAsMatched();
        }

        [Test]
        public void MatchNames_MultipleMatches()
        {
            string batt1Name = "Battalion 1";
            string batt2Name = "Battalion 2";

            IBattalion batt1 = Substitute.For<IBattalion>();
            batt1.Name.Returns(batt1Name);

            IBattalion batt2 = Substitute.For<IBattalion>();
            batt2.Name.Returns(batt2Name);

            IDictionary<string, IBattalion> dict = new Dictionary<string, IBattalion>();
            dict.Add(batt1Name, batt1);
            dict.Add(batt2Name, batt2);

            IEnumerable<string> names = new List<string>() { batt1Name, batt2Name };
            List<IBattalion> matches = Battalion.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches.Contains(batt1), Is.True);
            Assert.That(matches.Contains(batt2), Is.True);

            matches[0].Received(1).FlagAsMatched();
            matches[1].Received(1).FlagAsMatched();
        }

        [Test]
        public void MatchNames_MultipleMatches_DoNotSetMatchedStatus()
        {
            string batt1Name = "Battalion 1";
            string batt2Name = "Battalion 2";

            IBattalion batt1 = Substitute.For<IBattalion>();
            batt1.Name.Returns(batt1Name);

            IBattalion batt2 = Substitute.For<IBattalion>();
            batt2.Name.Returns(batt2Name);

            IDictionary<string, IBattalion> dict = new Dictionary<string, IBattalion>();
            dict.Add(batt1Name, batt1);
            dict.Add(batt2Name, batt2);

            IEnumerable<string> names = new List<string>() { batt1Name, batt2Name };
            List<IBattalion> matches = Battalion.MatchNames(dict, names, false);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches.Contains(batt1), Is.True);
            Assert.That(matches.Contains(batt2), Is.True);

            matches[0].DidNotReceive().FlagAsMatched();
            matches[1].DidNotReceive().FlagAsMatched();
        }

        #endregion MatchNames

        #region MatchName

        [Test]
        public void MatchName_UnmatchedName()
        {
            string batt1Name = "Battalion 1";

            IBattalion batt1 = Substitute.For<IBattalion>();
            batt1.Name.Returns(batt1Name);

            IDictionary<string, IBattalion> dict = new Dictionary<string, IBattalion>();
            dict.Add(batt1Name, batt1);

            string name = "Battalion 2";

            Assert.Throws<UnmatchedBattalionException>(() => Battalion.MatchName(dict, name));
        }

        [Test]
        public void MatchName()
        {
            string batt1Name = "Battalion 1";

            IBattalion batt1 = Substitute.For<IBattalion>();
            batt1.Name.Returns(batt1Name);

            IDictionary<string, IBattalion> dict = new Dictionary<string, IBattalion>();
            dict.Add(batt1Name, batt1);

            IBattalion match = Battalion.MatchName(dict, batt1Name);

            Assert.That(match, Is.Not.Null);
            Assert.That(match, Is.EqualTo(batt1));
            match.Received(1).FlagAsMatched();
        }

        [Test]
        public void MatchName_DoNotSetMatchedStatus()
        {
            string batt1Name = "Battalion 1";

            IBattalion batt1 = Substitute.For<IBattalion>();
            batt1.Name.Returns(batt1Name);

            IDictionary<string, IBattalion> dict = new Dictionary<string, IBattalion>();
            dict.Add(batt1Name, batt1);

            IBattalion match = Battalion.MatchName(dict, batt1Name, false);

            Assert.That(match, Is.Not.Null);
            Assert.That(match, Is.EqualTo(batt1));
            match.DidNotReceive().FlagAsMatched();
        }

        #endregion MatchName
    }
}
