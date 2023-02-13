using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Affiliations;
using RedditEmblemAPI.Models.Exceptions.Processing;
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

            Assert.AreEqual<bool>(false, aff.FlipUnitSprites);
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

            Assert.AreEqual<bool>(false, aff.FlipUnitSprites);
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

            Assert.AreEqual<bool>(true, aff.FlipUnitSprites);
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

        #region BuildDictionary

        [TestMethod]
        public void Affiliation_BuildDictionary_WithInput_Null()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ }
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
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ INPUT_NAME, INPUT_GROUPING },
                        new List<object>(){ INPUT_NAME, INPUT_GROUPING }
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
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ INPUT_NAME, "0" }
                    }
                },
                Name = 0,
                Grouping = 1
            };

            Assert.ThrowsException<AffiliationProcessingException>(() => Affiliation.BuildDictionary(config));
        }

        # endregion BuildDictionary
    }
}