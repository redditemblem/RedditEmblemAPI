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

            this.GAMBITS = new Dictionary<string, IGambit>();
            this.GAMBITS.Add(INPUT_GAMBIT, gambit1);
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

            Assert.Throws<RequiredValueNotProvidedException>(() => new Battalion(config, data, this.GAMBITS));
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

            Assert.Throws<RequiredValueNotProvidedException>(() => new Battalion(config, data, this.GAMBITS));
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

            Assert.Throws<UnmatchedGambitException>(() => new Battalion(config, data, this.GAMBITS));
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

            Assert.Throws<PositiveIntegerException>(() => new Battalion(config, data, this.GAMBITS));
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

            Assert.Throws<PositiveIntegerException>(() => new Battalion(config, data, this.GAMBITS));
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

            IBattalion batt = new Battalion(config, data, this.GAMBITS);

            Assert.That(batt.Name, Is.EqualTo(INPUT_NAME));
            Assert.That(batt.GambitObj, Is.EqualTo(this.GAMBITS[INPUT_GAMBIT]));
            Assert.That(batt.GambitObj.Matched, Is.False);
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

            IBattalion batt = new Battalion(config, data, this.GAMBITS);

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

            Assert.Throws<URLException>(() => new Battalion(config, data, this.GAMBITS));
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

            IBattalion batt = new Battalion(config, data, this.GAMBITS);

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

            IBattalion batt = new Battalion(config, data, this.GAMBITS);

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

            IBattalion batt = new Battalion(config, data, this.GAMBITS);

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

            IBattalion batt = new Battalion(config, data, this.GAMBITS);

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

            IBattalion batt = new Battalion(config, data, this.GAMBITS);

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

            IBattalion batt = new Battalion(config, data, this.GAMBITS);

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

            IBattalion batt = new Battalion(config, data, this.GAMBITS);

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

            IBattalion batt = new Battalion(config, data, this.GAMBITS);

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

            IBattalion batt = new Battalion(config, data, this.GAMBITS);

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

            IBattalion batt = new Battalion(config, data, this.GAMBITS);

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

            IBattalion batt = new Battalion(config, data, this.GAMBITS);

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

            IBattalion batt = new Battalion(config, data, this.GAMBITS);

            Assert.That(batt.Matched, Is.False);
            Assert.That(batt.GambitObj.Matched, Is.False);

            batt.FlagAsMatched();

            Assert.That(batt.Matched, Is.True);
            batt.GambitObj.Received().FlagAsMatched();
        }

        #endregion FlagAsMatched

        #region BuildDictionary

        [Test]
        public void BuildDictionary_WithInput_Null()
        {
            IDictionary<string, IBattalion> dict = Battalion.BuildDictionary(null, this.GAMBITS);
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

            IDictionary<string, IBattalion> dict = Battalion.BuildDictionary(config, this.GAMBITS);
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

            IDictionary<string, IBattalion> dict = Battalion.BuildDictionary(config, this.GAMBITS);
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

            Assert.Throws<BattalionProcessingException>(() => Battalion.BuildDictionary(config, this.GAMBITS));
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

            Assert.Throws<BattalionProcessingException>(() => Battalion.BuildDictionary(config, this.GAMBITS));
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

            IDictionary<string, IBattalion> dict = Battalion.BuildDictionary(config, this.GAMBITS);
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

            IDictionary<string, IBattalion> dict = Battalion.BuildDictionary(config, this.GAMBITS);
            Assert.That(dict.Count, Is.EqualTo(4));
        }

        #endregion BuildDictionary

        #region MatchNames

        [Test]
        public void MatchNames_UnmatchedName()
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
                    }
                },
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>()
            };

            IDictionary<string, IBattalion> dict = Battalion.BuildDictionary(config, this.GAMBITS);
            IEnumerable<string> names = new List<string>() { "Fake Battalion" };

            Assert.Throws<UnmatchedBattalionException>(() => Battalion.MatchNames(dict, names));
        }

        [Test]
        public void MatchNames_SingleMatch()
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
                    }
                },
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>()
            };

            IDictionary<string, IBattalion> dict = Battalion.BuildDictionary(config, this.GAMBITS);
            IEnumerable<string> names = new List<string>() { "Battalion 1" };

            List<IBattalion> matches = Battalion.MatchNames(dict, names);
            Assert.That(matches.Count, Is.EqualTo(1));
            Assert.That(matches.First().Matched, Is.True);
        }

        [Test]
        public void MatchNames_MultipleMatches()
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
                    }
                },
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>()
            };

            IDictionary<string, IBattalion> dict = Battalion.BuildDictionary(config, this.GAMBITS);
            IEnumerable<string> names = new List<string>() { "Battalion 1", "Battalion 2" };

            List<IBattalion> matches = Battalion.MatchNames(dict, names);
            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches[0].Matched, Is.True);
            Assert.That(matches[1].Matched, Is.True);
        }

        [Test]
        public void MatchNames_MultipleMatches_DoNotSetMatchedStatus()
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
                    }
                },
                Name = 0,
                Gambit = 1,
                MaxEndurance = 2,
                Stats = new List<NamedStatConfig>()
            };

            IDictionary<string, IBattalion> dict = Battalion.BuildDictionary(config, this.GAMBITS);
            IEnumerable<string> names = new List<string>() { "Battalion 1", "Battalion 2" };

            List<IBattalion> matches = Battalion.MatchNames(dict, names, false);
            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches[0].Matched, Is.False);
            Assert.That(matches[1].Matched, Is.False);
        }

        #endregion MatchNames
    }
}
