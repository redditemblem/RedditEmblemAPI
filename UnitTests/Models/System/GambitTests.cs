using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Battalions;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;

namespace UnitTests.Models.System
{
    public class GambitTests
    {
        #region Constants

        private const string INPUT_NAME = "Gambit Test";
        private const string INPUT_MAX_USES = "0";
        private const string INPUT_MINIMUM_RANGE = "0";
        private const string INPUT_MAXIMUM_RANGE = "0";

        private const string STAT_1_SOURCE_NAME = "Stat 1";
        private const string STAT_2_SOURCE_NAME = "Stat 2";

        #endregion Constants

        [Test]
        public void Constructor_RequiredFields_WithInputNull()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Name = 0,
                MaxUses = 1,
                Range = new GambitRangeConfig()
                {
                    Minimum = 2,
                    Maximum = 3
                },
                Stats = new List<NamedStatConfig>()
            };
            IEnumerable<string> data = new List<string>();

            Assert.Throws<RequiredValueNotProvidedException>(() => new Gambit(config, data));
        }

        [Test]
        public void Constructor_RequiredFields_WithInput_InvalidMaxUses()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Name = 0,
                MaxUses = 1,
                Range = new GambitRangeConfig()
                {
                    Minimum = 2,
                    Maximum = 3
                },
                Stats = new List<NamedStatConfig>()
            };
            IEnumerable<string> data = new List<string>() { INPUT_NAME, "-1" };

            Assert.Throws<PositiveIntegerException>(() => new Gambit(config, data));
        }

        [Test]
        public void Constructor_RequiredFields_WithInput_InvalidMinimumRange()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Name = 0,
                MaxUses = 1,
                Range = new GambitRangeConfig()
                {
                    Minimum = 2,
                    Maximum = 3
                },
                Stats = new List<NamedStatConfig>()
            };
            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, "-1" };

            Assert.Throws<PositiveIntegerException>(() => new Gambit(config, data));
        }

        [Test]
        public void Constructor_RequiredFields_WithInput_InvalidMaximumRange()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Name = 0,
                MaxUses = 1,
                Range = new GambitRangeConfig()
                {
                    Minimum = 2,
                    Maximum = 3
                },
                Stats = new List<NamedStatConfig>()
            };
            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, "-1" };

            Assert.Throws<PositiveIntegerException>(() => new Gambit(config, data));
        }

        [Test]
        public void Constructor_RequiredFields_WithInput_InvalidRangeSet()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Name = 0,
                MaxUses = 1,
                Range = new GambitRangeConfig()
                {
                    Minimum = 2,
                    Maximum = 3
                },
                Stats = new List<NamedStatConfig>()
            };
            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, "2", "1" };

            Assert.Throws<MinimumGreaterThanMaximumException>(() => new Gambit(config, data));
        }

        [Test]
        public void Constructor_RequiredFields_WithInput_InvalidStat()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Name = 0,
                MaxUses = 1,
                Range = new GambitRangeConfig()
                {
                    Minimum = 2,
                    Maximum = 3
                },
                Stats = new List<NamedStatConfig>()
                {
                    new NamedStatConfig(){ SourceName = STAT_1_SOURCE_NAME, Value = 4 }
                }
            };
            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "a" };

            Assert.Throws<AnyIntegerException>(() => new Gambit(config, data));
        }

        [Test]
        public void Constructor_RequiredFields()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Name = 0,
                MaxUses = 1,
                Range = new GambitRangeConfig()
                {
                    Minimum = 2,
                    Maximum = 3
                },
                Stats = new List<NamedStatConfig>()
                {
                    new NamedStatConfig(){ SourceName = STAT_1_SOURCE_NAME, Value = 4 },
                    new NamedStatConfig(){ SourceName = STAT_2_SOURCE_NAME, Value = 5 }
                }
            };
            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "1", "2" };

            IGambit gambit = new Gambit(config, data);

            Assert.That(gambit.Name, Is.EqualTo(INPUT_NAME));
            Assert.That(gambit.MaxUses, Is.EqualTo(0));
            Assert.That(gambit.Range.Minimum, Is.EqualTo(0));
            Assert.That(gambit.Range.Maximum, Is.EqualTo(0));
            Assert.That(gambit.Stats[STAT_1_SOURCE_NAME], Is.EqualTo(1));
            Assert.That(gambit.Stats[STAT_2_SOURCE_NAME], Is.EqualTo(2));
        }

        #region OptionalField_SpriteURL

        [Test]
        public void Constructor_OptionalField_SpriteURL_EmptyString()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Name = 0,
                MaxUses = 1,
                Range = new GambitRangeConfig()
                {
                    Minimum = 2,
                    Maximum = 3
                },
                Stats = new List<NamedStatConfig>(),
                SpriteURL = 4
            };
            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, string.Empty };

            IGambit gambit = new Gambit(config, data);

            Assert.That(gambit.SpriteURL, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_SpriteURL_InvalidURL()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Name = 0,
                MaxUses = 1,
                Range = new GambitRangeConfig()
                {
                    Minimum = 2,
                    Maximum = 3
                },
                Stats = new List<NamedStatConfig>(),
                SpriteURL = 4
            };
            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "NotAURL" };

            Assert.Throws<URLException>(() => new Gambit(config, data));
        }

        [Test]
        public void Constructor_OptionalField_SpriteURL()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Name = 0,
                MaxUses = 1,
                Range = new GambitRangeConfig()
                {
                    Minimum = 2,
                    Maximum = 3
                },
                Stats = new List<NamedStatConfig>(),
                SpriteURL = 4
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, UnitTestConsts.IMAGE_URL };

            IGambit gambit = new Gambit(config, data);

            Assert.That(gambit.SpriteURL, Is.EqualTo(UnitTestConsts.IMAGE_URL));
        }

        #endregion OptionalField_SpriteURL

        #region OptionalField_UtilizedStats

        [Test]
        public void Constructor_OptionalField_UtilizedStats_EmptyString()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Name = 0,
                MaxUses = 1,
                Range = new GambitRangeConfig()
                {
                    Minimum = 2,
                    Maximum = 3
                },
                Stats = new List<NamedStatConfig>(),
                UtilizedStats = 4
            };
            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, string.Empty };

            IGambit gambit = new Gambit(config, data);

            Assert.That(gambit.UtilizedStats, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_UtilizedStats_SingleValue()
        {
            string stat = "Str";

            GambitsConfig config = new GambitsConfig()
            {
                Name = 0,
                MaxUses = 1,
                Range = new GambitRangeConfig()
                {
                    Minimum = 2,
                    Maximum = 3
                },
                Stats = new List<NamedStatConfig>(),
                UtilizedStats = 4
            };
            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, stat };

            IGambit gambit = new Gambit(config, data);

            IEnumerable<string> expected = new List<string>() { stat };
            Assert.That(gambit.UtilizedStats, Is.EqualTo(expected));
        }

        [Test]
        public void Constructor_OptionalField_UtilizedStats_MultipleValues()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Name = 0,
                MaxUses = 1,
                Range = new GambitRangeConfig()
                {
                    Minimum = 2,
                    Maximum = 3
                },
                Stats = new List<NamedStatConfig>(),
                UtilizedStats = 4
            };
            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "Str,Mag,Skl" };

            IGambit gambit = new Gambit(config, data);

            IEnumerable<string> expected = new List<string>() { "Str", "Mag", "Skl" };
            Assert.That(gambit.UtilizedStats, Is.EqualTo(expected));
        }

        [Test]
        public void Constructor_OptionalField_UtilizedStats_ExtraComma()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Name = 0,
                MaxUses = 1,
                Range = new GambitRangeConfig()
                {
                    Minimum = 2,
                    Maximum = 3
                },
                Stats = new List<NamedStatConfig>(),
                UtilizedStats = 4
            };
            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "Str," };

            IGambit gambit = new Gambit(config, data);

            IEnumerable<string> expected = new List<string>() { "Str" };
            Assert.That(gambit.UtilizedStats, Is.EqualTo(expected));
        }

        #endregion OptionalField_UtilizedStats

        #region OptionalField_TextFields

        [Test]
        public void Constructor_OptionalField_TextFields_EmptyString()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Name = 0,
                MaxUses = 1,
                Range = new GambitRangeConfig()
                {
                    Minimum = 2,
                    Maximum = 3
                },
                Stats = new List<NamedStatConfig>(),
                TextFields = new List<int>() { 4, 5 }
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, string.Empty, string.Empty };

            IGambit gambit = new Gambit(config, data);

            Assert.That(gambit.TextFields, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_TextFields()
        {
            string textField1 = "Text Field 1";
            string textField2 = "Text Field 2";

            GambitsConfig config = new GambitsConfig()
            {
                Name = 0,
                MaxUses = 1,
                Range = new GambitRangeConfig()
                {
                    Minimum = 2,
                    Maximum = 3
                },
                Stats = new List<NamedStatConfig>(),
                TextFields = new List<int>() { 4, 5 }
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, textField1, textField2 };

            IGambit gambit = new Gambit(config, data);

            IEnumerable<string> expected = new List<string>() { textField1, textField2 };
            Assert.That(gambit.TextFields, Is.EqualTo(expected));
        }

        #endregion OptionalField_TextFields

        #region FlagAsMatched

        [Test]
        public void FlagAsMatched()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Name = 0,
                MaxUses = 1,
                Range = new GambitRangeConfig()
                {
                    Minimum = 2,
                    Maximum = 3
                },
                Stats = new List<NamedStatConfig>()
            };
            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE };

            IGambit gambit = new Gambit(config, data);

            Assert.That(gambit.Matched, Is.False);

            gambit.FlagAsMatched();

            Assert.That(gambit.Matched, Is.True);
        }

        #endregion FlagAsMatched

        #region BuildDictionary

        [Test]
        public void BuildDictionary_WithInput_Null()
        {
            IDictionary<string, IGambit> dict = Gambit.BuildDictionary(null);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_NullQuery()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Queries = null,
                Name = 0,
                MaxUses = 1,
                Range = new GambitRangeConfig()
                {
                    Minimum = 2,
                    Maximum = 3
                },
                Stats = new List<NamedStatConfig>()
            };

            IDictionary<string, IGambit> dict = Gambit.BuildDictionary(config);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_EmptyQuery()
        {
            GambitsConfig config = new GambitsConfig()
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
                MaxUses = 1,
                Range = new GambitRangeConfig()
                {
                    Minimum = 2,
                    Maximum = 3
                },
                Stats = new List<NamedStatConfig>()
            };

            IDictionary<string, IGambit> dict = Gambit.BuildDictionary(config);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_DuplicateName()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE },
                            new List<object>(){ INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE },
                        }
                    }
                },
                Name = 0,
                MaxUses = 1,
                Range = new GambitRangeConfig()
                {
                    Minimum = 2,
                    Maximum = 3
                },
                Stats = new List<NamedStatConfig>()
            };

            Assert.Throws<GambitProcessingException>(() => Gambit.BuildDictionary(config));
        }

        [Test]
        public void BuildDictionary()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE }
                        }
                    }
                },
                Name = 0,
                MaxUses = 1,
                Range = new GambitRangeConfig()
                {
                    Minimum = 2,
                    Maximum = 3
                },
                Stats = new List<NamedStatConfig>()
            };

            IDictionary<string, IGambit> dict = Gambit.BuildDictionary(config);
            Assert.That(dict.Count, Is.EqualTo(1));
        }

        [Test]
        public void BuildDictionary_MultiQuery()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Gambit 1", INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE },
                            new List<object>(){ "Gambit 2", INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE }
                        }
                    },
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Gambit 3", INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE },
                            new List<object>(){ "Gambit 4", INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE }
                        }
                    }
                },
                Name = 0,
                MaxUses = 1,
                Range = new GambitRangeConfig()
                {
                    Minimum = 2,
                    Maximum = 3
                },
                Stats = new List<NamedStatConfig>()
            };

            IDictionary<string, IGambit> dict = Gambit.BuildDictionary(config);
            Assert.That(dict.Count, Is.EqualTo(4));
        }

        # endregion BuildDictionary

        #region MatchNames

        [Test]
        public void MatchNames_UnmatchedName()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Gambit 1", INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE },
                            new List<object>(){ "Gambit 2", INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE }
                        }
                    }
                },
                Name = 0,
                MaxUses = 1,
                Range = new GambitRangeConfig()
                {
                    Minimum = 2,
                    Maximum = 3
                },
                Stats = new List<NamedStatConfig>()
            };

            IDictionary<string, IGambit> dict = Gambit.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Gambit 3" };

            Assert.Throws<UnmatchedGambitException>(() => Gambit.MatchNames(dict, names));
        }

        [Test]
        public void MatchNames_SingleMatch()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Gambit 1", INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE },
                            new List<object>(){ "Gambit 2", INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE }
                        }
                    }
                },
                Name = 0,
                MaxUses = 1,
                Range = new GambitRangeConfig()
                {
                    Minimum = 2,
                    Maximum = 3
                },
                Stats = new List<NamedStatConfig>()
            };

            IDictionary<string, IGambit> dict = Gambit.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Gambit 1" };

            List<IGambit> matches = Gambit.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(1));
            Assert.That(matches.First().Matched, Is.True);
        }

        [Test]
        public void MatchNames_MultipleMatches()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Gambit 1", INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE },
                            new List<object>(){ "Gambit 2", INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE }
                        }
                    }
                },
                Name = 0,
                MaxUses = 1,
                Range = new GambitRangeConfig()
                {
                    Minimum = 2,
                    Maximum = 3
                },
                Stats = new List<NamedStatConfig>()
            };

            IDictionary<string, IGambit> dict = Gambit.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Gambit 1", "Gambit 2" };

            List<IGambit> matches = Gambit.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches[0].Matched, Is.True);
            Assert.That(matches[1].Matched, Is.True);
        }

        [Test]
        public void MatchNames_MultipleMatches_DoNotSetMatchedStatus()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Gambit 1", INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE },
                            new List<object>(){ "Gambit 2", INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE }
                        }
                    }
                },
                Name = 0,
                MaxUses = 1,
                Range = new GambitRangeConfig()
                {
                    Minimum = 2,
                    Maximum = 3
                },
                Stats = new List<NamedStatConfig>()
            };

            IDictionary<string, IGambit> dict = Gambit.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Gambit 1", "Gambit 2" };

            List<IGambit> matches = Gambit.MatchNames(dict, names, false);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches[0].Matched, Is.False);
            Assert.That(matches[1].Matched, Is.False);
        }

        #endregion MatchNames
    }
}
