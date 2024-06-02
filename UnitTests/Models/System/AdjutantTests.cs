using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Adjutants;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    [TestClass]
    public class AdjutantTests
    {
        #region Constants

        private const string INPUT_NAME = "Adjutant Test";

        #endregion Constants

        [TestMethod]
        public void AdjutantConstructor_RequiredFields_WithInputNull()
        {
            AdjutantsConfig config = new AdjutantsConfig()
            {
                Name = 0
            };

            List<string> data = new List<string>() { };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new Adjutant(config, data));
        }

        [TestMethod]
        public void AdjutantConstructor_RequiredFields()
        {
            AdjutantsConfig config = new AdjutantsConfig()
            {
                Name = 0
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME
            };

            Adjutant adj = new Adjutant(config, data);

            Assert.AreEqual<string>(INPUT_NAME, adj.Name);
        }

        #region OptionalField_SpriteURL

        public void AdjutantConstructor_OptionalField_SpriteURL_EmptyString()
        {
            AdjutantsConfig config = new AdjutantsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            Adjutant adj = new Adjutant(config, data);

            Assert.AreEqual<string>(string.Empty, adj.SpriteURL);
        }

        [TestMethod]
        public void AdjutantConstructor_OptionalField_SpriteURL()
        {
            AdjutantsConfig config = new AdjutantsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL
            };

            Adjutant adj = new Adjutant(config, data);

            Assert.AreEqual<string>(UnitTestConsts.IMAGE_URL, adj.SpriteURL);
        }

        #endregion OptionalField_SpriteURL

        #region OptionalField_CombatStatModifiers

        [TestMethod]
        public void AdjutantConstructor_OptionalField_CombatStatModifiers_EmptyString()
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

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty,
                string.Empty
            };

            Adjutant adj = new Adjutant(config, data);

            Assert.AreEqual<int>(0, adj.CombatStatModifiers.Count);
        }

        [TestMethod]
        public void AdjutantConstructor_OptionalField_CombatStatModifiers()
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

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                "1",
                "-1"
            };

            Adjutant adj = new Adjutant(config, data);

            Assert.AreEqual<int>(2, adj.CombatStatModifiers.Count);
            Assert.AreEqual<int>(1, adj.CombatStatModifiers[stat1]);
            Assert.AreEqual<int>(-1, adj.CombatStatModifiers[stat2]);
        }

        #endregion OptionalField_CombatStatModifiers

        #region OptionalField_StatModifiers

        [TestMethod]
        public void AdjutantConstructor_OptionalField_StatModifiers_EmptyString()
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

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty,
                string.Empty
            };

            Adjutant adj = new Adjutant(config, data);

            Assert.AreEqual<int>(0, adj.StatModifiers.Count);
        }

        [TestMethod]
        public void AdjutantConstructor_OptionalField_StatModifiers()
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

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                "1",
                "-1"
            };

            Adjutant adj = new Adjutant(config, data);

            Assert.AreEqual<int>(2, adj.StatModifiers.Count);
            Assert.AreEqual<int>(1, adj.StatModifiers[stat1]);
            Assert.AreEqual<int>(-1, adj.StatModifiers[stat2]);
        }

        #endregion OptionalField_StatModifiers

        #region OptionalField_TextFields

        [TestMethod]
        public void AdjutantConstructor_OptionalField_TextFields_EmptyString()
        {
            AdjutantsConfig config = new AdjutantsConfig()
            {
                Name = 0,
                TextFields = new List<int>() { 1, 2 }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty,
                string.Empty
            };

            Adjutant adj = new Adjutant(config, data);

            CollectionAssert.AreEqual(new List<string>() { }, adj.TextFields);
        }

        [TestMethod]
        public void AdjutantConstructor_OptionalField_TextFields()
        {
            AdjutantsConfig config = new AdjutantsConfig()
            {
                Name = 0,
                TextFields = new List<int>() { 1, 2 }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                "Text Field 1",
                "Text Field 2"
            };

            Adjutant adj = new Adjutant(config, data);

            CollectionAssert.AreEqual(new List<string>() { "Text Field 1", "Text Field 2" }, adj.TextFields);
        }

        #endregion OptionalField_TextFields

        #region FlagAsMatched

        [TestMethod]
        public void Adjutant_FlagAsMatched()
        {
            AdjutantsConfig config = new AdjutantsConfig()
            {
                Name = 0
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME
            };

            Adjutant adj = new Adjutant(config, data);

            Assert.IsFalse(adj.Matched);
            adj.FlagAsMatched();
            Assert.IsTrue(adj.Matched);
        }

        #endregion FlagAsMatched

        #region BuildDictionary

        [TestMethod]
        public void Adjutant_BuildDictionary_WithInput_Null()
        {
            IDictionary<string, Adjutant> dict = Adjutant.BuildDictionary(null);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void Adjutant_BuildDictionary_WithInput_NullQuery()
        {
            AdjutantsConfig config = new AdjutantsConfig()
            {
                Query = null,
                Name = 0
            };

            IDictionary<string, Adjutant> dict = Adjutant.BuildDictionary(config);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void Adjutant_BuildDictionary_WithInput_EmptyQuery()
        {
            AdjutantsConfig config = new AdjutantsConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ }
                    }
                },
                Name = 0
            };

            IDictionary<string, Adjutant> dict = Adjutant.BuildDictionary(config);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void Adjutant_BuildDictionary_WithInput_DuplicateName()
        {
            AdjutantsConfig config = new AdjutantsConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ INPUT_NAME },
                        new List<object>(){ INPUT_NAME }
                    }
                },
                Name = 0
            };

            Assert.ThrowsException<AdjutantProcessingException>(() => Adjutant.BuildDictionary(config));
        }

        [TestMethod]
        public void Adjutant_BuildDictionary_WithInput_Invalid()
        {
            AdjutantsConfig config = new AdjutantsConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ INPUT_NAME, "NotURL" }
                    }
                },
                Name = 0,
                SpriteURL = 1
            };

            Assert.ThrowsException<AdjutantProcessingException>(() => Adjutant.BuildDictionary(config));
        }

        [TestMethod]
        public void Adjutant_BuildDictionary()
        {
            AdjutantsConfig config = new AdjutantsConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ INPUT_NAME }
                    }
                },
                Name = 0
            };

            IDictionary<string, Adjutant> dict = Adjutant.BuildDictionary(config);
            Assert.AreEqual<int>(1, dict.Count);
        }

        #endregion BuildDictionary

        #region MatchNames

        [TestMethod]
        public void Adjutant_MatchNames_UnmatchedName()
        {
            AdjutantsConfig config = new AdjutantsConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ "Adjutant 1" },
                        new List<object>(){ "Adjutant 2" }
                    }
                },
                Name = 0
            };

            IDictionary<string, Adjutant> dict = Adjutant.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Adjutant 3" };

            Assert.ThrowsException<UnmatchedAdjutantException>(() => Adjutant.MatchNames(dict, names));
        }

        [TestMethod]
        public void Adjutant_MatchNames_SingleMatch()
        {
            AdjutantsConfig config = new AdjutantsConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ "Adjutant 1" },
                        new List<object>(){ "Adjutant 2" }
                    }
                },
                Name = 0
            };

            IDictionary<string, Adjutant> dict = Adjutant.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Adjutant 1" };

            List<Adjutant> matches = Adjutant.MatchNames(dict, names);
            Assert.AreEqual(1, matches.Count);
            Assert.IsTrue(matches.First().Matched);
        }

        [TestMethod]
        public void Adjutant_MatchNames_MultipleMatches()
        {
            AdjutantsConfig config = new AdjutantsConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ "Adjutant 1" },
                        new List<object>(){ "Adjutant 2" }
                    }
                },
                Name = 0
            };

            IDictionary<string, Adjutant> dict = Adjutant.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Adjutant 1", "Adjutant 2" };

            List<Adjutant> matches = Adjutant.MatchNames(dict, names);
            Assert.AreEqual(2, matches.Count);
            Assert.IsTrue(matches[0].Matched);
            Assert.IsTrue(matches[1].Matched);
        }

        [TestMethod]
        public void Adjutant_MatchNames_MultipleMatches_SetMatchedStatus()
        {
            AdjutantsConfig config = new AdjutantsConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ "Adjutant 1" },
                        new List<object>(){ "Adjutant 2" }
                    }
                },
                Name = 0
            };

            IDictionary<string, Adjutant> dict = Adjutant.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Adjutant 1", "Adjutant 2" };

            List<Adjutant> matches = Adjutant.MatchNames(dict, names, true);
            Assert.AreEqual(2, matches.Count);
            Assert.IsFalse(matches[0].Matched);
            Assert.IsFalse(matches[1].Matched);
        }

        #endregion MatchNames
    }
}