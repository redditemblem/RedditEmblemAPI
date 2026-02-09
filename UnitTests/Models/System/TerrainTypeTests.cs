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

            this.AFFILIATIONS = new Dictionary<string, IAffiliation>();
            this.AFFILIATIONS.Add(aff1Name, aff1);
        }

        #endregion Setup

        [Test]
        public void Constructor_RequiredFields_WithInputNull()
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
                            }
                        }
                    }
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "1"
            };

            ITerrainType terrain = new TerrainType(config, data, AFFILIATIONS);

            Assert.That(terrain.Name, Is.EqualTo(INPUT_NAME));
            Assert.That(terrain.StatGroups.Count, Is.EqualTo(1));
            Assert.That(terrain.StatGroups.First().MovementCosts[infantry], Is.EqualTo(1));
        }

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
                Queries = new List<IQuery>()
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
                Queries = new List<IQuery>()
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
                Queries = new List<IQuery>()
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
                Queries = new List<IQuery>()
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
                Queries = new List<IQuery>()
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
                Queries = new List<IQuery>()
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