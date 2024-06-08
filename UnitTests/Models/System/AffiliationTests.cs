using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Affiliations;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    [TestClass]
    public class AffiliationTests
    {
        #region Constants

        private const string INPUT_NAME = "Affiliation Test";
        private const string INPUT_GROUPING = "1";

        #endregion Constants

        [TestMethod]
        public void AffiliationConstructor_RequiredFields_WithInputNull()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Name = 0,
                Grouping = 1
            };

            List<string> data = new List<string>() { };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new Affiliation(config, data));
        }

        [TestMethod]
        public void AffiliationConstructor_RequiredFields_WithInput_InvalidGrouping()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Name = 0,
                Grouping = 1
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                "0"
            };

            Assert.ThrowsException<NonZeroPositiveIntegerException>(() => new Affiliation(config, data));
        }

        [TestMethod]
        public void AffiliationConstructor_RequiredFields()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Name = 0,
                Grouping = 1
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GROUPING
            };

            Affiliation aff = new Affiliation(config, data);

            Assert.AreEqual<string>(INPUT_NAME, aff.Name);
            Assert.AreEqual<int>(1, aff.Grouping);
        }

        #region OptionalField_FlipUnitSprites

        public void AffiliationConstructor_OptionalField_FlipUnitSprites_EmptyString()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Name = 0,
                Grouping = 1,
                FlipUnitSprites = 2
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GROUPING,
                string.Empty
            };

            Affiliation aff = new Affiliation(config, data);

            Assert.IsFalse(aff.FlipUnitSprites);
        }

        [TestMethod]
        public void AffiliationConstructor_OptionalField_FlipUnitSprites_No()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Name = 0,
                Grouping = 1,
                FlipUnitSprites = 2
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GROUPING,
                "No"
            };

            Affiliation aff = new Affiliation(config, data);

            Assert.IsFalse(aff.FlipUnitSprites);
        }

        [TestMethod]
        public void AffiliationConstructor_OptionalField_FlipUnitSprites_Yes()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Name = 0,
                Grouping = 1,
                FlipUnitSprites = 2
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GROUPING,
                "Yes"
            };

            Affiliation aff = new Affiliation(config, data);

            Assert.IsTrue(aff.FlipUnitSprites);
        }

        #endregion OptionalField_FlipUnitSprites

        #region OptionalField_TextFields

        [TestMethod]
        public void AffiliationConstructor_OptionalField_TextFields_EmptyString()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Name = 0,
                Grouping = 1,
                TextFields = new List<int>() { 2, 3 }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GROUPING,
                string.Empty,
                string.Empty
            };

            Affiliation aff = new Affiliation(config, data);

            CollectionAssert.AreEqual(new List<string>() { }, aff.TextFields);
        }

        [TestMethod]
        public void AffiliationConstructor_OptionalField_TextFields()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Name = 0,
                Grouping = 1,
                TextFields = new List<int>() { 2, 3 }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GROUPING,
                "Text Field 1",
                "Text Field 2"
            };

            Affiliation aff = new Affiliation(config, data);

            CollectionAssert.AreEqual(new List<string>() { "Text Field 1", "Text Field 2" }, aff.TextFields);
        }

        #endregion OptionalField_TextFields

        #region FlagAsMatched

        [TestMethod]
        public void Affiliation_FlagAsMatched()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Name = 0,
                Grouping = 1
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GROUPING
            };

            Affiliation aff = new Affiliation(config, data);

            Assert.IsFalse(aff.Matched);
            aff.FlagAsMatched();
            Assert.IsTrue(aff.Matched);
        }

        #endregion FlagAsMatched

        #region BuildDictionary

        [TestMethod]
        public void Affiliation_BuildDictionary_WithInput_Null()
        {
            IDictionary<string, Affiliation> dict = Affiliation.BuildDictionary(null);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void Affiliation_BuildDictionary_WithInput_NullQuery()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Queries = null,
                Name = 0,
                Grouping = 1
            };

            IDictionary<string, Affiliation> dict = Affiliation.BuildDictionary(config);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void Affiliation_BuildDictionary_WithInput_EmptyQuery()
        {
            AffiliationsConfig config = new AffiliationsConfig()
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
                Grouping = 1
            };

            IDictionary<string, Affiliation> dict = Affiliation.BuildDictionary(config);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void Affiliation_BuildDictionary_WithInput_DuplicateName()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_NAME, INPUT_GROUPING },
                            new List<object>(){ INPUT_NAME, INPUT_GROUPING }
                        }
                    }
                },
                Name = 0,
                Grouping = 1
            };

            Assert.ThrowsException<AffiliationProcessingException>(() => Affiliation.BuildDictionary(config));
        }

        [TestMethod]
        public void Affiliation_BuildDictionary_WithInput_Invalid()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_NAME, "0" }
                        }
                    }
                },
                Name = 0,
                Grouping = 1
            };

            Assert.ThrowsException<AffiliationProcessingException>(() => Affiliation.BuildDictionary(config));
        }

        [TestMethod]
        public void Affiliation_BuildDictionary()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_NAME, "1" }
                        }
                    }
                },
                Name = 0,
                Grouping = 1
            };

            IDictionary<string, Affiliation> dict = Affiliation.BuildDictionary(config);
            Assert.AreEqual<int>(1, dict.Count);
        }

        [TestMethod]
        public void Affiliation_BuildDictionary_MultiQuery()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Affiliation 1", "1" },
                            new List<object>(){ "Affiliation 2", "1" }
                        }
                    },
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Affiliation 3", "1" },
                            new List<object>(){ "Affiliation 4", "1" }
                        }
                    }
                },
                Name = 0,
                Grouping = 1
            };

            IDictionary<string, Affiliation> dict = Affiliation.BuildDictionary(config);
            Assert.AreEqual<int>(4, dict.Count);
        }

        #endregion BuildDictionary

        #region MatchNames

        [TestMethod]
        public void Affiliation_MatchNames_UnmatchedName()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Affiliation 1", "1" },
                            new List<object>(){ "Affiliation 2", "1" }
                        }
                    }
                },
                Name = 0,
                Grouping = 1
            };

            IDictionary<string, Affiliation> dict = Affiliation.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Affiliation 3" };

            Assert.ThrowsException<UnmatchedAffiliationException>(() => Affiliation.MatchNames(dict, names));
        }

        [TestMethod]
        public void Affiliation_MatchNames_SingleMatch()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Affiliation 1", "1" },
                            new List<object>(){ "Affiliation 2", "1" }
                        }
                    }
                },
                Name = 0,
                Grouping = 1
            };

            IDictionary<string, Affiliation> dict = Affiliation.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Affiliation 1" };

            List<Affiliation> matches = Affiliation.MatchNames(dict, names);
            Assert.AreEqual(1, matches.Count);
            Assert.IsTrue(matches.First().Matched);
        }

        [TestMethod]
        public void Affiliation_MatchNames_MultipleMatches()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Affiliation 1", "1" },
                            new List<object>(){ "Affiliation 2", "1" }
                        }
                    }
                },
                Name = 0,
                Grouping = 1
            };

            IDictionary<string, Affiliation> dict = Affiliation.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Affiliation 1", "Affiliation 2" };

            List<Affiliation> matches = Affiliation.MatchNames(dict, names);
            Assert.AreEqual(2, matches.Count);
            Assert.IsTrue(matches[0].Matched);
            Assert.IsTrue(matches[1].Matched);
        }

        [TestMethod]
        public void Affiliation_MatchNames_MultipleMatches_SetMatchedStatus()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Affiliation 1", "1" },
                            new List<object>(){ "Affiliation 2", "1" }
                        }
                    }
                },
                Name = 0,
                Grouping = 1
            };

            IDictionary<string, Affiliation> dict = Affiliation.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Affiliation 1", "Affiliation 2" };

            List<Affiliation> matches = Affiliation.MatchNames(dict, names, true);
            Assert.AreEqual(2, matches.Count);
            Assert.IsFalse(matches[0].Matched);
            Assert.IsFalse(matches[1].Matched);
        }

        #endregion MatchNames
    }
}