using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Battalions;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;

namespace UnitTests.Models.System
{
    [TestClass]
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

        [TestMethod]
        public void GambitConstructor_RequiredFields_WithInputNull()
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
            List<string> data = new List<string>() { };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new Gambit(config, data));
        }

        [TestMethod]
        public void GambitConstructor_RequiredFields_WithInput_InvalidMaxUses()
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
            List<string> data = new List<string>() { INPUT_NAME, "-1" };

            Assert.ThrowsException<PositiveIntegerException>(() => new Gambit(config, data));
        }

        [TestMethod]
        public void GambitConstructor_RequiredFields_WithInput_InvalidMinimumRange()
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
            List<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, "-1" };

            Assert.ThrowsException<PositiveIntegerException>(() => new Gambit(config, data));
        }

        [TestMethod]
        public void GambitConstructor_RequiredFields_WithInput_InvalidMaximumRange()
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
            List<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, "-1" };

            Assert.ThrowsException<PositiveIntegerException>(() => new Gambit(config, data));
        }

        [TestMethod]
        public void GambitConstructor_RequiredFields_WithInput_InvalidRangeSet()
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
            List<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, "2", "1" };

            Assert.ThrowsException<MinimumGreaterThanMaximumException>(() => new Gambit(config, data));
        }

        [TestMethod]
        public void GambitConstructor_RequiredFields_WithInput_InvalidStat()
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
            List<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "a" };

            Assert.ThrowsException<AnyIntegerException>(() => new Gambit(config, data));
        }

        [TestMethod]
        public void GambitConstructor_RequiredFields()
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
            List<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "1", "2" };

            Gambit gambit = new Gambit(config, data);

            Assert.AreEqual<string>(INPUT_NAME, gambit.Name);
            Assert.AreEqual<int>(0, gambit.MaxUses);
            Assert.AreEqual<int>(0, gambit.Range.Minimum);
            Assert.AreEqual<int>(0, gambit.Range.Maximum);
            Assert.AreEqual<int>(1, gambit.Stats[STAT_1_SOURCE_NAME]);
            Assert.AreEqual<int>(2, gambit.Stats[STAT_2_SOURCE_NAME]);
        }

        #region OptionalField_SpriteURL

        [TestMethod]
        public void GambitConstructor_OptionalField_SpriteURL_EmptyString()
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
                },
                SpriteURL = 6
            };
            List<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "1", "2", string.Empty };

            Gambit gambit = new Gambit(config, data);

            Assert.AreEqual<string>(string.Empty, gambit.SpriteURL);
        }

        [TestMethod]
        public void GambitConstructor_OptionalField_SpriteURL_InvalidURL()
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
                },
                SpriteURL = 6
            };
            List<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "1", "2", "NotAURL" };

            Assert.ThrowsException<URLException>(() => new Gambit(config, data));
        }

        [TestMethod]
        public void GambitConstructor_OptionalField_SpriteURL()
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
                },
                SpriteURL = 6
            };

            List<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "1", "2", UnitTestConsts.IMAGE_URL };

            Gambit gambit = new Gambit(config, data);

            Assert.AreEqual<string>(UnitTestConsts.IMAGE_URL, gambit.SpriteURL);
        }

        #endregion OptionalField_SpriteURL

        #region OptionalField_UtilizedStats

        [TestMethod]
        public void GambitConstructor_OptionalField_UtilizedStats_EmptyString()
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
                },
                UtilizedStats = 6
            };
            List<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "1", "2", string.Empty };

            Gambit gambit = new Gambit(config, data);

            CollectionAssert.AreEqual(new List<string>() { }, gambit.UtilizedStats);
        }

        [TestMethod]
        public void GambitConstructor_OptionalField_UtilizedStats_SingleValue()
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
                },
                UtilizedStats = 6
            };
            List<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "1", "2", "Str" };

            Gambit gambit = new Gambit(config, data);

            CollectionAssert.AreEqual(new List<string>() { "Str" }, gambit.UtilizedStats);
        }

        [TestMethod]
        public void GambitConstructor_OptionalField_UtilizedStats_MultipleValues()
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
                },
                UtilizedStats = 6
            };
            List<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "1", "2", "Str,Mag,Skl" };

            Gambit gambit = new Gambit(config, data);

            CollectionAssert.AreEqual(new List<string>() { "Str", "Mag", "Skl" }, gambit.UtilizedStats);
        }

        [TestMethod]
        public void GambitConstructor_OptionalField_UtilizedStats_ExtraComma()
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
                },
                UtilizedStats = 6
            };
            List<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "1", "2", "Str," };

            Gambit gambit = new Gambit(config, data);

            CollectionAssert.AreEqual(new List<string>() { "Str" }, gambit.UtilizedStats);
        }

        #endregion OptionalField_UtilizedStats

        #region OptionalField_TextFields

        [TestMethod]
        public void GambitConstructor_OptionalField_TextFields_EmptyString()
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
                },
                TextFields = new List<int>() { 6, 7 }
            };

            List<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "1", "2", string.Empty, string.Empty };

            Gambit gambit = new Gambit(config, data);

            CollectionAssert.AreEqual(new List<string>() { }, gambit.TextFields);
        }

        [TestMethod]
        public void GambitConstructor_OptionalField_TextFields()
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
                },
                TextFields = new List<int>() { 6, 7 }
            };

            string field1 = "Text Field 1";
            string field2 = "Text Field 2";

            List<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "1", "2", field1, field2 };


            Gambit gambit = new Gambit(config, data);

            CollectionAssert.AreEqual(new List<string>() { field1, field2 }, gambit.TextFields);
        }

        #endregion OptionalField_TextFields

        #region FlagAsMatched

        [TestMethod]
        public void Gambit_FlagAsMatched()
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
            List<string> data = new List<string>() { INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "1", "2" };

            Gambit gambit = new Gambit(config, data);

            Assert.IsFalse(gambit.Matched);
            gambit.FlagAsMatched();
            Assert.IsTrue(gambit.Matched);
        }

        #endregion FlagAsMatched

        #region BuildDictionary

        [TestMethod]
        public void Gambit_BuildDictionary_WithInput_Null()
        {
            IDictionary<string, Gambit> dict = Gambit.BuildDictionary(null);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void Gambit_BuildDictionary_WithInput_NullQuery()
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
                {
                    new NamedStatConfig(){ SourceName = STAT_1_SOURCE_NAME, Value = 4 },
                    new NamedStatConfig(){ SourceName = STAT_2_SOURCE_NAME, Value = 5 }
                }
            };

            IDictionary<string, Gambit> dict = Gambit.BuildDictionary(config);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void Gambit_BuildDictionary_WithInput_EmptyQuery()
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
                {
                    new NamedStatConfig(){ SourceName = STAT_1_SOURCE_NAME, Value = 4 },
                    new NamedStatConfig(){ SourceName = STAT_2_SOURCE_NAME, Value = 5 }
                }
            };

            IDictionary<string, Gambit> dict = Gambit.BuildDictionary(config);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void Gambit_BuildDictionary_WithInput_DuplicateName()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "1", "1" },
                            new List<object>(){ INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "1", "1" },
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
                {
                    new NamedStatConfig(){ SourceName = STAT_1_SOURCE_NAME, Value = 4 },
                    new NamedStatConfig(){ SourceName = STAT_2_SOURCE_NAME, Value = 5 }
                }
            };

            Assert.ThrowsException<GambitProcessingException>(() => Gambit.BuildDictionary(config));
        }

        [TestMethod]
        public void Gambit_BuildDictionary()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_NAME, INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "1", "1" }
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
                {
                    new NamedStatConfig(){ SourceName = STAT_1_SOURCE_NAME, Value = 4 },
                    new NamedStatConfig(){ SourceName = STAT_2_SOURCE_NAME, Value = 5 }
                }
            };

            IDictionary<string, Gambit> dict = Gambit.BuildDictionary(config);
            Assert.AreEqual<int>(1, dict.Count);
        }

        [TestMethod]
        public void Gambit_BuildDictionary_MultiQuery()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Gambit 1", INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "1", "1" },
                            new List<object>(){ "Gambit 2", INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "1", "1" }
                        }
                    },
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Gambit 3", INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "1", "1" },
                            new List<object>(){ "Gambit 4", INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "1", "1" }
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
                {
                    new NamedStatConfig(){ SourceName = STAT_1_SOURCE_NAME, Value = 4 },
                    new NamedStatConfig(){ SourceName = STAT_2_SOURCE_NAME, Value = 5 }
                }
            };

            IDictionary<string, Gambit> dict = Gambit.BuildDictionary(config);
            Assert.AreEqual<int>(4, dict.Count);
        }

        # endregion BuildDictionary

        #region MatchNames

        [TestMethod]
        public void Gambit_MatchNames_UnmatchedName()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Gambit 1", INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "1", "2" },
                            new List<object>(){ "Gambit 2", INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "1", "2" }
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
                {
                    new NamedStatConfig(){ SourceName = STAT_1_SOURCE_NAME, Value = 4 },
                    new NamedStatConfig(){ SourceName = STAT_2_SOURCE_NAME, Value = 5 }
                }
            };

            IDictionary<string, Gambit> dict = Gambit.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Gambit 3" };

            Assert.ThrowsException<UnmatchedGambitException>(() => Gambit.MatchNames(dict, names));
        }

        [TestMethod]
        public void Gambit_MatchNames_SingleMatch()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Gambit 1", INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "1", "2" },
                            new List<object>(){ "Gambit 2", INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "1", "2" }
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
                {
                    new NamedStatConfig(){ SourceName = STAT_1_SOURCE_NAME, Value = 4 },
                    new NamedStatConfig(){ SourceName = STAT_2_SOURCE_NAME, Value = 5 }
                }
            };

            IDictionary<string, Gambit> dict = Gambit.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Gambit 1" };

            List<Gambit> matches = Gambit.MatchNames(dict, names);
            Assert.AreEqual(1, matches.Count);
            Assert.IsTrue(matches.First().Matched);
        }

        [TestMethod]
        public void Gambit_MatchNames_MultipleMatches()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Gambit 1", INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "1", "2" },
                            new List<object>(){ "Gambit 2", INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "1", "2" }
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
                {
                    new NamedStatConfig(){ SourceName = STAT_1_SOURCE_NAME, Value = 4 },
                    new NamedStatConfig(){ SourceName = STAT_2_SOURCE_NAME, Value = 5 }
                }
            };

            IDictionary<string, Gambit> dict = Gambit.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Gambit 1", "Gambit 2" };

            List<Gambit> matches = Gambit.MatchNames(dict, names);
            Assert.AreEqual(2, matches.Count);
            Assert.IsTrue(matches[0].Matched);
            Assert.IsTrue(matches[1].Matched);
        }

        [TestMethod]
        public void Gambit_MatchNames_MultipleMatches_SetMatchedStatus()
        {
            GambitsConfig config = new GambitsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Gambit 1", INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "1", "2" },
                            new List<object>(){ "Gambit 2", INPUT_MAX_USES, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "1", "2" }
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
                {
                    new NamedStatConfig(){ SourceName = STAT_1_SOURCE_NAME, Value = 4 },
                    new NamedStatConfig(){ SourceName = STAT_2_SOURCE_NAME, Value = 5 }
                }
            };

            IDictionary<string, Gambit> dict = Gambit.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Gambit 1", "Gambit 2" };

            List<Gambit> matches = Gambit.MatchNames(dict, names, true);
            Assert.AreEqual(2, matches.Count);
            Assert.IsFalse(matches[0].Matched);
            Assert.IsFalse(matches[1].Matched);
        }

        #endregion MatchNames
    }
}
