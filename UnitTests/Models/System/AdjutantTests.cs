using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Adjutants;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    public class AdjutantTests
    {
        #region Constants

        private const string INPUT_NAME = "Adjutant Test";

        #endregion Constants

        [Test]
        public void Constructor_RequiredFields_WithInputNull()
        {
            AdjutantsConfig config = new AdjutantsConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>() { };

            Assert.Throws<RequiredValueNotProvidedException>(() => new Adjutant(config, data));
        }

        [Test]
        public void Constructor_RequiredFields()
        {
            AdjutantsConfig config = new AdjutantsConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME
            };

            IAdjutant adj = new Adjutant(config, data);

            Assert.That(adj.Name, Is.EqualTo(INPUT_NAME));
        }

        #region OptionalField_SpriteURL

        [Test]
        public void Constructor_OptionalField_SpriteURL_EmptyString()
        {
            AdjutantsConfig config = new AdjutantsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            IAdjutant adj = new Adjutant(config, data);

            Assert.That(adj.SpriteURL, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_SpriteURL()
        {
            AdjutantsConfig config = new AdjutantsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL
            };

            IAdjutant adj = new Adjutant(config, data);

            Assert.That(adj.SpriteURL, Is.EqualTo(UnitTestConsts.IMAGE_URL));
        }

        #endregion OptionalField_SpriteURL

        #region OptionalField_CombatStatModifiers

        [Test]
        public void Constructor_OptionalField_CombatStatModifiers_EmptyString()
        {
            string stat1 = "Stat 1";
            string stat2 = "Stat 2";

            AdjutantsConfig config = new AdjutantsConfig()
            {
                Name = 0,
                CombatStatModifiers = new List<NamedStatConfig>()
                {
                    new NamedStatConfig { SourceName = stat1, Value = 1 },
                    new NamedStatConfig { SourceName = stat2, Value = 2 }
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty,
                string.Empty
            };

            IAdjutant adj = new Adjutant(config, data);

            IDictionary<string, int> expected = new Dictionary<string, int>();
            Assert.That(adj.CombatStatModifiers, Is.EqualTo(expected));
        }

        [Test]
        public void Constructor_OptionalField_CombatStatModifiers()
        {
            string stat1 = "Stat 1";
            string stat2 = "Stat 2";

            AdjutantsConfig config = new AdjutantsConfig()
            {
                Name = 0,
                CombatStatModifiers = new List<NamedStatConfig>()
                {
                    new NamedStatConfig { SourceName = stat1, Value = 1 },
                    new NamedStatConfig { SourceName = stat2, Value = 2 }
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "1",
                "-1"
            };

            IAdjutant adj = new Adjutant(config, data);

            IDictionary<string, int> expected = new Dictionary<string, int>()
            {
                { stat1, 1 },
                { stat2, -1 }
            };
            Assert.That(adj.CombatStatModifiers, Is.EqualTo(expected));
        }

        #endregion OptionalField_CombatStatModifiers

        #region OptionalField_StatModifiers

        [Test]
        public void Constructor_OptionalField_StatModifiers_EmptyString()
        {
            string stat1 = "Stat 1";
            string stat2 = "Stat 2";

            AdjutantsConfig config = new AdjutantsConfig()
            {
                Name = 0,
                StatModifiers = new List<NamedStatConfig>()
                {
                    new NamedStatConfig { SourceName = stat1, Value = 1 },
                    new NamedStatConfig { SourceName = stat2, Value = 2 }
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty,
                string.Empty
            };

            IAdjutant adj = new Adjutant(config, data);

            IDictionary<string, int> expected = new Dictionary<string, int>();
            Assert.That(adj.StatModifiers, Is.EqualTo(expected));
        }

        [Test]
        public void Constructor_OptionalField_StatModifiers()
        {
            string stat1 = "Stat 1";
            string stat2 = "Stat 2";

            AdjutantsConfig config = new AdjutantsConfig()
            {
                Name = 0,
                StatModifiers = new List<NamedStatConfig>()
                {
                    new NamedStatConfig { SourceName = stat1, Value = 1 },
                    new NamedStatConfig { SourceName = stat2, Value = 2 }
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "1",
                "-1"
            };

            IAdjutant adj = new Adjutant(config, data);

            IDictionary<string, int> expected = new Dictionary<string, int>()
            {
                { stat1, 1 },
                { stat2, -1 }
            };
            Assert.That(adj.StatModifiers, Is.EqualTo(expected));
        }

        #endregion OptionalField_StatModifiers

        #region OptionalField_TextFields

        [Test]
        public void Constructor_OptionalField_TextFields_EmptyString()
        {
            AdjutantsConfig config = new AdjutantsConfig()
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

            IAdjutant adj = new Adjutant(config, data);

            List<string> expected = new List<string>();
            Assert.That(adj.TextFields, Is.EqualTo(expected));
        }

        [Test]
        public void Constructor_OptionalField_TextFields()
        {
            string textField1 = "Text Field 1";
            string textField2 = "Text Field 2";

            AdjutantsConfig config = new AdjutantsConfig()
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

            IAdjutant adj = new Adjutant(config, data);

            List<string> expected = new List<string>() { textField1, textField2 };
            Assert.That(adj.TextFields, Is.EqualTo(expected));
        }

        #endregion OptionalField_TextFields

        #region FlagAsMatched

        [Test]
        public void FlagAsMatched()
        {
            AdjutantsConfig config = new AdjutantsConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME
            };

            IAdjutant adj = new Adjutant(config, data);

            Assert.That(adj.Matched, Is.False);

            adj.FlagAsMatched();

            Assert.That(adj.Matched, Is.True);
        }

        #endregion FlagAsMatched

        #region BuildDictionary

        [Test]
        public void BuildDictionary_WithInput_Null()
        {
            IDictionary<string, IAdjutant> dict = Adjutant.BuildDictionary(null);

            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_NullQuery()
        {
            AdjutantsConfig config = new AdjutantsConfig()
            {
                Queries = null,
                Name = 0
            };

            IDictionary<string, IAdjutant> dict = Adjutant.BuildDictionary(config);

            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_EmptyQuery()
        {
            AdjutantsConfig config = new AdjutantsConfig()
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

            IDictionary<string, IAdjutant> dict = Adjutant.BuildDictionary(config);

            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_DuplicateName()
        {
            AdjutantsConfig config = new AdjutantsConfig()
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

            Assert.Throws<AdjutantProcessingException>(() => Adjutant.BuildDictionary(config));
        }

        [Test]
        public void BuildDictionary_WithInput_Invalid()
        {
            AdjutantsConfig config = new AdjutantsConfig()
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

            Assert.Throws<AdjutantProcessingException>(() => Adjutant.BuildDictionary(config));
        }

        [Test]
        public void BuildDictionary()
        {
            AdjutantsConfig config = new AdjutantsConfig()
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

            IDictionary<string, IAdjutant> dict = Adjutant.BuildDictionary(config);

            Assert.That(dict.Count, Is.EqualTo(1));
        }

        [Test]
        public void BuildDictionary_MultiQuery()
        {
            AdjutantsConfig config = new AdjutantsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Adjutant Test 1" },
                            new List<object>(){ "Adjutant Test 2" }
                        }
                    },
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Adjutant Test 3" },
                            new List<object>(){ "Adjutant Test 4" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, IAdjutant> dict = Adjutant.BuildDictionary(config);

            Assert.That(dict.Count, Is.EqualTo(4));
        }

        #endregion BuildDictionary

        #region MatchNames

        [Test]
        public void MatchNames_UnmatchedName()
        {
            AdjutantsConfig config = new AdjutantsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Adjutant 1" },
                            new List<object>(){ "Adjutant 2" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, IAdjutant> dict = Adjutant.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Adjutant 3" };

            Assert.Throws<UnmatchedAdjutantException>(() => Adjutant.MatchNames(dict, names));
        }

        [Test]
        public void MatchNames_SingleMatch()
        {
            AdjutantsConfig config = new AdjutantsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Adjutant 1" },
                            new List<object>(){ "Adjutant 2" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, IAdjutant> dict = Adjutant.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Adjutant 1" };

            List<IAdjutant> matches = Adjutant.MatchNames(dict, names);
            Assert.That(matches.Count, Is.EqualTo(1));
            Assert.That(matches.First().Matched, Is.True);
        }

        [Test]
        public void MatchNames_MultipleMatches()
        {
            AdjutantsConfig config = new AdjutantsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Adjutant 1" },
                            new List<object>(){ "Adjutant 2" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, IAdjutant> dict = Adjutant.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Adjutant 1", "Adjutant 2" };

            List<IAdjutant> matches = Adjutant.MatchNames(dict, names);
            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches[0].Matched, Is.True);
            Assert.That(matches[1].Matched, Is.True);
        }

        [Test]
        public void MatchNames_MultipleMatches_DoNotSetMatchedStatus()
        {
            AdjutantsConfig config = new AdjutantsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Adjutant 1" },
                            new List<object>(){ "Adjutant 2" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, IAdjutant> dict = Adjutant.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Adjutant 1", "Adjutant 2" };

            List<IAdjutant> matches = Adjutant.MatchNames(dict, names, false);
            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches[0].Matched, Is.False);
            Assert.That(matches[1].Matched, Is.False);
        }

        #endregion MatchNames
    }
}