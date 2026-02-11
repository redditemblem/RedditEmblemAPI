using NSubstitute;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.TerrainTypes;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    public class TerrainTypeTests
    {
        #region Constants

        private const string INPUT_NAME = "Terrain Type Test";

        #endregion Constants

        #region Setup

        private IDictionary<string, IAffiliation> AFFILIATIONS;

        [SetUp]
        public void SetUp()
        {
            string aff1Name = "Affiliation 1";
            IAffiliation aff1 = Substitute.For<IAffiliation>();
            aff1.Name.Returns(aff1Name);
            aff1.Grouping = 1;

            string aff2Name = "Affiliation 2";
            IAffiliation aff2 = Substitute.For<IAffiliation>();
            aff2.Name.Returns(aff2Name);
            aff2.Grouping = 2;

            string aff3Name = "Affiliation 3";
            IAffiliation aff3 = Substitute.For<IAffiliation>();
            aff3.Name.Returns(aff3Name);
            aff3.Grouping = 3;

            this.AFFILIATIONS = new Dictionary<string, IAffiliation>();
            this.AFFILIATIONS.Add(aff1Name, aff1);
            this.AFFILIATIONS.Add(aff2Name, aff2);
            this.AFFILIATIONS.Add(aff3Name, aff3);
        }

        #endregion Setup

        [Test]
        public void Constructor_RequiredFields_IndexOutOfBounds()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>()
            };

            IEnumerable<string> data = new List<string>();

            Assert.Throws<RequiredValueNotProvidedException>(() => new TerrainType(config, data, AFFILIATIONS));
        }

        [Test]
        public void Constructor_RequiredFields()
        {
            string infantry = "Infantry";
            string mounted = "Mounted";
            string flying = "Flying";

            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>()
                {
                    new TerrainTypeStatsConfig
                    {
                        MovementCosts = new List<NamedStatConfig>()
                        {
                            new NamedStatConfig()
                            {
                                SourceName = infantry,
                                Value = 1
                            },
                            new NamedStatConfig()
                            {
                                SourceName = mounted,
                                Value = 2
                            },
                            new NamedStatConfig()
                            {
                                SourceName = flying,
                                Value = 3
                            }
                        }
                    }
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "1",
                "2",
                "3"
            };

            ITerrainType terrain = new TerrainType(config, data, AFFILIATIONS);

            Assert.That(terrain.Name, Is.EqualTo(INPUT_NAME));
            Assert.That(terrain.StatGroups.Count, Is.EqualTo(1));

            ITerrainTypeStats stats = terrain.StatGroups.First();

            Assert.That(stats.MovementCosts.Count, Is.EqualTo(3));
            Assert.That(stats.MovementCosts.ContainsKey(infantry), Is.True);
            Assert.That(stats.MovementCosts[infantry], Is.EqualTo(1));
            Assert.That(stats.MovementCosts.ContainsKey(mounted), Is.True);
            Assert.That(stats.MovementCosts[mounted], Is.EqualTo(2));
            Assert.That(stats.MovementCosts.ContainsKey(flying), Is.True);
            Assert.That(stats.MovementCosts[flying], Is.EqualTo(3));
        }

        [Test]
        public void Constructor_RequiredFields_MultipleStatGroups()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>()
                {
                    new TerrainTypeStatsConfig
                    {
                        MovementCosts = new List<NamedStatConfig>()
                    },
                    new TerrainTypeStatsConfig
                    {
                        AffiliationGroupings = 1,
                        MovementCosts = new List<NamedStatConfig>()
                    },
                    new TerrainTypeStatsConfig
                    {
                        AffiliationGroupings = 2,
                        MovementCosts = new List<NamedStatConfig>()
                    }
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "1",
                "2,3"
            };

            ITerrainType terrain = new TerrainType(config, data, AFFILIATIONS);

            Assert.That(terrain.Name, Is.EqualTo(INPUT_NAME));
            Assert.That(terrain.StatGroups.Count, Is.EqualTo(3));

            ITerrainTypeStats stats = terrain.StatGroups[0];
            Assert.That(stats.AffiliationGroupings, Is.Empty);

            stats = terrain.StatGroups[1];
            IEnumerable<int> expectedAffGroupings = new List<int>() { 1 };
            Assert.That(stats.AffiliationGroupings, Is.EqualTo(expectedAffGroupings));

            stats = terrain.StatGroups[2];
            expectedAffGroupings = new List<int>() { 2, 3 };
            Assert.That(stats.AffiliationGroupings, Is.EqualTo(expectedAffGroupings));
        }

        [Test]
        public void Constructor_RequiredFields_MultipleStatGroups_WithDuplicateAffiliationGroupings()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>()
                {
                    new TerrainTypeStatsConfig
                    {
                        AffiliationGroupings = 1,
                        MovementCosts = new List<NamedStatConfig>()
                    },
                    new TerrainTypeStatsConfig
                    {
                        AffiliationGroupings = 2,
                        MovementCosts = new List<NamedStatConfig>()
                    }
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "1",
                "1,2"
            };

            Assert.Throws<DuplicateTerrainTypeStatsException>(() => new TerrainType(config, data, AFFILIATIONS));
        }

        [Test]
        public void Constructor_RequiredFields_MultipleStatGroups_EmptyStringAffiliationGrouping()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>()
                {
                    new TerrainTypeStatsConfig
                    {
                        MovementCosts = new List<NamedStatConfig>()
                    },
                    new TerrainTypeStatsConfig
                    {
                        AffiliationGroupings = 1,
                        MovementCosts = new List<NamedStatConfig>()
                    }
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            ITerrainType terrain = new TerrainType(config, data, AFFILIATIONS);

            Assert.That(terrain.StatGroups.Count, Is.EqualTo(1));
            Assert.That(terrain.StatGroups[0].AffiliationGroupings, Is.Empty);
        }

        #region OptionalField_CannotStopOn

        [Test]
        public void Constructor_OptionalField_CannotStopOn_EmptyString()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>(),
                CannotStopOn = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            ITerrainType terrain = new TerrainType(config, data, AFFILIATIONS);

            Assert.That(terrain.CannotStopOn, Is.False);
        }

        [Test]
        public void Constructor_OptionalField_CannotStopOn_No()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>(),
                CannotStopOn = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "No"
            };

            ITerrainType terrain = new TerrainType(config, data, AFFILIATIONS);

            Assert.That(terrain.CannotStopOn, Is.False);
        }

        [Test]
        public void Constructor_OptionalField_CannotStopOn_Yes()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>(),
                CannotStopOn = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "Yes"
            };

            ITerrainType terrain = new TerrainType(config, data, AFFILIATIONS);

            Assert.That(terrain.CannotStopOn, Is.True);
        }

        #endregion OptionalField_CannotStopOn

        #region OptionalField_BlocksItems

        [Test]
        public void Constructor_OptionalField_BlocksItems_EmptyString()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>(),
                BlocksItems = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            ITerrainType terrain = new TerrainType(config, data, AFFILIATIONS);

            Assert.That(terrain.BlocksItems, Is.False);
        }

        [Test]
        public void Constructor_OptionalField_BlocksItems_No()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>(),
                BlocksItems = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "No"
            };

            ITerrainType terrain = new TerrainType(config, data, AFFILIATIONS);

            Assert.That(terrain.BlocksItems, Is.False);
        }

        [Test]
        public void Constructor_OptionalField_BlocksItems_Yes()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>(),
                BlocksItems = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "Yes"
            };

            ITerrainType terrain = new TerrainType(config, data, AFFILIATIONS);

            Assert.That(terrain.BlocksItems, Is.True);
        }

        #endregion OptionalField_BlocksItems

        #region OptionalField_RestrictAffiliations

        [Test]
        public void Constructor_OptionalField_RestrictAffiliations_EmptyString()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>(),
                RestrictAffiliations = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            ITerrainType terrain = new TerrainType(config, data, AFFILIATIONS);

            Assert.That(terrain.RestrictAffiliations, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_RestrictAffiliations()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>(),
                RestrictAffiliations = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "1,2,3"
            };

            ITerrainType terrain = new TerrainType(config, data, AFFILIATIONS);

            List<int> expected = new List<int>() { 1, 2, 3 };
            Assert.That(terrain.RestrictAffiliations, Is.EqualTo(expected));
        }

        #endregion OptionalField_RestrictAffiliations

        #region OptionalField_Groupings

        [Test]
        public void Constructor_OptionalField_Groupings_EmptyString()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>(),
                Groupings = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            ITerrainType terrain = new TerrainType(config, data, AFFILIATIONS);

            Assert.That(terrain.Groupings, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_Groupings()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>(),
                Groupings = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "1,2,3"
            };

            ITerrainType terrain = new TerrainType(config, data, AFFILIATIONS);

            List<int> expected = new List<int>() { 1, 2, 3 };
            Assert.That(terrain.Groupings, Is.EqualTo(expected));
        }

        #endregion OptionalField_Groupings

        #region OptionalField_TextFields

        [Test]
        public void Constructor_OptionalField_TextFields_EmptyString()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>(),
                TextFields = new List<int>() { 1, 2 }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty,
                string.Empty
            };

            ITerrainType terrain = new TerrainType(config, data, AFFILIATIONS);

            Assert.That(terrain.TextFields, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_TextFields()
        {
            string textField1 = "Text Field 1";
            string textField2 = "Text Field 2";

            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>(),
                TextFields = new List<int>() { 1, 2 }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                textField1,
                textField2
            };

            ITerrainType terrain = new TerrainType(config, data, AFFILIATIONS);

            IEnumerable<string> expected = new List<string>() { textField1, textField2 };
            Assert.That(terrain.TextFields, Is.EqualTo(expected));
        }

        #endregion OptionalField_TextFields

        #region OptionalField_WarpType_WarpCost

        [Test]
        public void Constructor_OptionalField_WarpType_WarpCost_EmptyStrings()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>(),
                WarpType = 1,
                WarpCost = 2
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty,
                string.Empty
            };

            ITerrainType terrain = new TerrainType(config, data, AFFILIATIONS);

            Assert.That(terrain.WarpType, Is.EqualTo(WarpType.None));
            Assert.That(terrain.WarpCost, Is.EqualTo(-1));
        }

        [Test]
        public void Constructor_OptionalField_WarpType_WarpCost_UnmatchedWarpType()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>(),
                WarpType = 1,
                WarpCost = 2
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "FakeWarpType",
                string.Empty
            };

            Assert.Throws<UnmatchedWarpTypeException>(() => new TerrainType(config, data, AFFILIATIONS));
        }

        [TestCase("")]
        [TestCase("test")]
        [TestCase("-1")]
        public void Constructor_OptionalField_WarpType_WarpCost_InvalidCosts(string input)
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>(),
                WarpType = 1,
                WarpCost = 2
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "Entrance",
                input
            };

            Assert.Throws<PositiveIntegerException>(() => new TerrainType(config, data, AFFILIATIONS));
        }

        [TestCase("Entrance", "0", WarpType.Entrance, 0)]
        [TestCase("Exit", "", WarpType.Exit, -1)]
        [TestCase("Exit", "1", WarpType.Exit, -1)] //cost is ignored for exit warps
        [TestCase("Dual", "2", WarpType.Dual, 2)]
        public void Constructor_OptionalField_WarpType_WarpCost_ValidInputs(string input1, string input2, WarpType expected1, int expected2)
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>(),
                WarpType = 1,
                WarpCost = 2
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                input1,
                input2
            };

            ITerrainType terrain = new TerrainType(config, data, AFFILIATIONS);

            Assert.That(terrain.WarpType, Is.EqualTo(expected1));
            Assert.That(terrain.WarpCost, Is.EqualTo(expected2));
        }

        #endregion OptionalField_WarpType

        #region GetTerrainTypeStatsByAffiliation

        [Test]
        public void GetTerrainTypeStatsByAffiliation()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>()
                {
                    new TerrainTypeStatsConfig
                    {
                        MovementCosts = new List<NamedStatConfig>()
                    },
                    new TerrainTypeStatsConfig
                    {
                        AffiliationGroupings = 1,
                        MovementCosts = new List<NamedStatConfig>()
                    },
                    new TerrainTypeStatsConfig
                    {
                        AffiliationGroupings = 2,
                        MovementCosts = new List<NamedStatConfig>()
                    }
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "1",
                "2"
            };

            ITerrainType terrain = new TerrainType(config, data, AFFILIATIONS);

            Assert.That(terrain.Name, Is.EqualTo(INPUT_NAME));
            Assert.That(terrain.StatGroups.Count, Is.EqualTo(3));

            //Assert that we have a stats object matching affiliation group 1
            IAffiliation affiliation = AFFILIATIONS["Affiliation 1"];
            ITerrainTypeStats result = terrain.GetTerrainTypeStatsByAffiliation(affiliation);

            Assert.That(affiliation.Grouping, Is.EqualTo(1)); //mock is unchanged
            Assert.That(result.AffiliationGroupings.Contains(affiliation.Grouping), Is.True);

            //Assert that we have a stats object matching affiliation group 2
            affiliation = AFFILIATIONS["Affiliation 2"];
            result = terrain.GetTerrainTypeStatsByAffiliation(affiliation);

            Assert.That(affiliation.Grouping, Is.EqualTo(2)); //mock is unchanged
            Assert.That(result.AffiliationGroupings.Contains(affiliation.Grouping), Is.True);

            //Assert that we get back the default stats object (no affiliation groups) back when we pass in an unmatched group ID
            int group = 3;
            result = terrain.GetTerrainTypeStatsByAffiliation(group);

            Assert.That(result.AffiliationGroupings, Is.Empty);
        }

        #endregion GetTerrainTypeStatsByAffiliation

        #region FlagAsMatched

        [Test]
        public void FlagAsMatched()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>()
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME
            };

            ITerrainType terrain = new TerrainType(config, data, AFFILIATIONS);

            Assert.That(terrain.Matched, Is.False);

            terrain.FlagAsMatched();

            Assert.That(terrain.Matched, Is.True);
        }

        #endregion FlagAsMatched

        #region BuildDictionary

        [Test]
        public void BuildDictionary_WithInput_Null()
        {
            IDictionary<string, ITerrainType> dict = TerrainType.BuildDictionary(null, AFFILIATIONS);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_NullQuery()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Queries = null,
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>()
            };

            IDictionary<string, ITerrainType> dict = TerrainType.BuildDictionary(config, AFFILIATIONS);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_EmptyQuery()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
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
                StatGroups = new List<TerrainTypeStatsConfig>()
            };

            IDictionary<string, ITerrainType> dict = TerrainType.BuildDictionary(config, AFFILIATIONS);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_DuplicateName()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
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
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>()
            };

            Assert.Throws<TerrainTypeProcessingException>(() => TerrainType.BuildDictionary(config, AFFILIATIONS));
        }

        [Test]
        public void BuildDictionary_WithInput_Invalid()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_NAME, "a" }
                        }
                    }
                },
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>(),
                RestrictAffiliations = 1
            };

            Assert.Throws<TerrainTypeProcessingException>(() => TerrainType.BuildDictionary(config, AFFILIATIONS));
        }

        [Test]
        public void BuildDictionary()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
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
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>()
            };

            IDictionary<string, ITerrainType> dict = TerrainType.BuildDictionary(config, AFFILIATIONS);
            Assert.That(dict.Count, Is.EqualTo(1));
        }

        [Test]
        public void BuildDictionary_MultiQuery()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Terrain Type 1" },
                            new List<object>(){ "Terrain Type 2" }
                        }
                    },
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Terrain Type 3" },
                            new List<object>(){ "Terrain Type 4" }
                        }
                    }
                },
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>()
            };

            IDictionary<string, ITerrainType> dict = TerrainType.BuildDictionary(config, AFFILIATIONS);
            Assert.That(dict.Count, Is.EqualTo(4));
        }

        #endregion BuildDictionary

        #region MatchNames

        [Test]
        public void MatchNames_UnmatchedName()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Terrain Type 1" },
                            new List<object>(){ "Terrain Type 2" }
                        }
                    }
                },
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>()
            };

            IDictionary<string, ITerrainType> dict = TerrainType.BuildDictionary(config, AFFILIATIONS);
            IEnumerable<string> names = new List<string>() { "Terrain Type 3" };
            Coordinate coord = new Coordinate();

            Assert.Throws<UnmatchedTileTerrainTypeException>(() => TerrainType.MatchNames(dict, names, coord));
        }

        [Test]
        public void MatchNames_SingleMatch()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Terrain Type 1" },
                            new List<object>(){ "Terrain Type 2" }
                        }
                    }
                },
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>()
            };

            IDictionary<string, ITerrainType> dict = TerrainType.BuildDictionary(config, AFFILIATIONS);
            IEnumerable<string> names = new List<string>() { "Terrain Type 1" };
            Coordinate coord = new Coordinate();

            List<ITerrainType> matches = TerrainType.MatchNames(dict, names, coord);

            Assert.That(matches.Count, Is.EqualTo(1));
            Assert.That(matches.First().Matched, Is.True);
        }

        [Test]
        public void MatchNames_MultipleMatches()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Terrain Type 1" },
                            new List<object>(){ "Terrain Type 2" }
                        }
                    }
                },
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>()
            };

            IDictionary<string, ITerrainType> dict = TerrainType.BuildDictionary(config, AFFILIATIONS);
            IEnumerable<string> names = new List<string>() { "Terrain Type 1", "Terrain Type 2" };
            Coordinate coord = new Coordinate();

            List<ITerrainType> matches = TerrainType.MatchNames(dict, names, coord);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches[0].Matched, Is.True);
            Assert.That(matches[1].Matched, Is.True);
        }

        [Test]
        public void MatchNames_MultipleMatches_DoNotSetMatchedStatus()
        {
            TerrainTypesConfig config = new TerrainTypesConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Terrain Type 1" },
                            new List<object>(){ "Terrain Type 2" }
                        }
                    }
                },
                Name = 0,
                StatGroups = new List<TerrainTypeStatsConfig>()
            };

            IDictionary<string, ITerrainType> dict = TerrainType.BuildDictionary(config, AFFILIATIONS);
            IEnumerable<string> names = new List<string>() { "Terrain Type 1", "Terrain Type 2" };
            Coordinate coord = new Coordinate();

            List<ITerrainType> matches = TerrainType.MatchNames(dict, names, coord, false);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches[0].Matched, Is.False);
            Assert.That(matches[1].Matched, Is.False);
        }

        #endregion MatchNames
    }
}