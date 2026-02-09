using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Affiliations;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    public class AffiliationTests
    {
        #region Constants

        private const string INPUT_NAME = "Affiliation Test";
        private const string INPUT_GROUPING = "1";

        #endregion Constants

        [Test]
        public void Constructor_RequiredFields_WithInputNull()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Name = 0,
                Grouping = 1
            };

            IEnumerable<string> data = new List<string>();

            Assert.Throws<RequiredValueNotProvidedException>(() => new Affiliation(config, data));
        }

        [Test]
        public void Constructor_RequiredFields_WithInput_InvalidGrouping()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Name = 0,
                Grouping = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "0"
            };

            Assert.Throws<NonZeroPositiveIntegerException>(() => new Affiliation(config, data));
        }

        [Test]
        public void Constructor_RequiredFields()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Name = 0,
                Grouping = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GROUPING
            };

            IAffiliation aff = new Affiliation(config, data);

            Assert.That(aff.Name, Is.EqualTo(INPUT_NAME));
            Assert.That(aff.Grouping, Is.EqualTo(1));
        }

        #region OptionalField_SpriteURL

        [Test]
        public void Constructor_OptionalField_SpriteURL_EmptyString()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Name = 0,
                Grouping = 1,
                SpriteURL = 2
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GROUPING,
                string.Empty
            };

            IAffiliation aff = new Affiliation(config, data);

            Assert.That(aff.SpriteURL, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_SpriteURL_InvalidURL()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Name = 0,
                Grouping = 1,
                SpriteURL = 2
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GROUPING,
                "NotAURL"
            };

            Assert.Throws<URLException>(() => new Affiliation(config, data));
        }

        [Test]
        public void Constructor_OptionalField_SpriteURL()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Name = 0,
                Grouping = 1,
                SpriteURL = 2
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GROUPING,
                UnitTestConsts.IMAGE_URL
            };

            IAffiliation aff = new Affiliation(config, data);

            Assert.That(aff.SpriteURL, Is.EqualTo(UnitTestConsts.IMAGE_URL));
        }

        #endregion OptionalField_SpriteURL

        #region OptionalField_FlipUnitSprites

        [Test]
        public void Constructor_OptionalField_FlipUnitSprites_EmptyString()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Name = 0,
                Grouping = 1,
                FlipUnitSprites = 2
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GROUPING,
                string.Empty
            };

            IAffiliation aff = new Affiliation(config, data);

            Assert.That(aff.FlipUnitSprites, Is.False);
        }

        [Test]
        public void Constructor_OptionalField_FlipUnitSprites_No()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Name = 0,
                Grouping = 1,
                FlipUnitSprites = 2
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GROUPING,
                "No"
            };

            IAffiliation aff = new Affiliation(config, data);

            Assert.That(aff.FlipUnitSprites, Is.False);
        }

        [Test]
        public void Constructor_OptionalField_FlipUnitSprites_Yes()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Name = 0,
                Grouping = 1,
                FlipUnitSprites = 2
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GROUPING,
                "Yes"
            };

            IAffiliation aff = new Affiliation(config, data);

            Assert.That(aff.FlipUnitSprites, Is.True);
        }

        #endregion OptionalField_FlipUnitSprites

        #region OptionalField_TextFields

        [Test]
        public void Constructor_OptionalField_TextFields_EmptyString()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Name = 0,
                Grouping = 1,
                TextFields = new List<int>() { 2, 3 }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GROUPING,
                string.Empty,
                string.Empty
            };

            IAffiliation aff = new Affiliation(config, data);

            List<string> expected = new List<string>();
            Assert.That(aff.TextFields, Is.EqualTo(expected));
        }

        [Test]
        public void Constructor_OptionalField_TextFields()
        {
            string textField1 = "Text Field 1";
            string textField2 = "Text Field 2";

            AffiliationsConfig config = new AffiliationsConfig()
            {
                Name = 0,
                Grouping = 1,
                TextFields = new List<int>() { 2, 3 }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GROUPING,
                textField1,
                textField2
            };

            IAffiliation aff = new Affiliation(config, data);

            List<string> expected = new List<string>() { textField1, textField2 };
            Assert.That(aff.TextFields, Is.EqualTo(expected));
        }

        #endregion OptionalField_TextFields

        #region FlagAsMatched

        [Test]
        public void FlagAsMatched()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Name = 0,
                Grouping = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_GROUPING
            };

            IAffiliation aff = new Affiliation(config, data);

            Assert.That(aff.Matched, Is.False);

            aff.FlagAsMatched();

            Assert.That(aff.Matched, Is.True);
        }

        #endregion FlagAsMatched

        #region BuildDictionary

        [Test]
        public void BuildDictionary_WithInput_Null()
        {
            IDictionary<string, IAffiliation> dict = Affiliation.BuildDictionary(null);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_NullQuery()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Queries = null,
                Name = 0,
                Grouping = 1
            };

            IDictionary<string, IAffiliation> dict = Affiliation.BuildDictionary(config);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_EmptyQuery()
        {
            AffiliationsConfig config = new AffiliationsConfig()
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
                Grouping = 1
            };

            IDictionary<string, IAffiliation> dict = Affiliation.BuildDictionary(config);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_DuplicateName()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Queries = new List<IQuery>()
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

            Assert.Throws<AffiliationProcessingException>(() => Affiliation.BuildDictionary(config));
        }

        [Test]
        public void BuildDictionary_WithInput_Invalid()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Queries = new List<IQuery>()
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

            Assert.Throws<AffiliationProcessingException>(() => Affiliation.BuildDictionary(config));
        }

        [Test]
        public void BuildDictionary()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Queries = new List<IQuery>()
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

            IDictionary<string, IAffiliation> dict = Affiliation.BuildDictionary(config);
            Assert.That(dict.Count, Is.EqualTo(1));
        }

        [Test]
        public void BuildDictionary_MultiQuery()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Queries = new List<IQuery>()
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

            IDictionary<string, IAffiliation> dict = Affiliation.BuildDictionary(config);
            Assert.That(dict.Count, Is.EqualTo(4));
        }

        #endregion BuildDictionary

        #region MatchNames

        [Test]
        public void MatchNames_UnmatchedName()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Queries = new List<IQuery>()
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

            IDictionary<string, IAffiliation> dict = Affiliation.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Affiliation 3" };

            Assert.Throws<UnmatchedAffiliationException>(() => Affiliation.MatchNames(dict, names));
        }

        [Test]
        public void MatchNames_SingleMatch()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Queries = new List<IQuery>()
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

            IDictionary<string, IAffiliation> dict = Affiliation.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Affiliation 1" };

            List<IAffiliation> matches = Affiliation.MatchNames(dict, names);
            Assert.That(matches.Count, Is.EqualTo(1));
            Assert.That(matches.First().Matched, Is.True);
        }

        [Test]
        public void MatchNames_MultipleMatches()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Queries = new List<IQuery>()
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

            IDictionary<string, IAffiliation> dict = Affiliation.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Affiliation 1", "Affiliation 2" };

            List<IAffiliation> matches = Affiliation.MatchNames(dict, names);
            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches[0].Matched, Is.True);
            Assert.That(matches[1].Matched, Is.True);
        }

        [Test]
        public void MatchNames_MultipleMatches_DoNotSetMatchedStatus()
        {
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Queries = new List<IQuery>()
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

            IDictionary<string, IAffiliation> dict = Affiliation.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Affiliation 1", "Affiliation 2" };

            List<IAffiliation> matches = Affiliation.MatchNames(dict, names, false);
            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches[0].Matched, Is.False);
            Assert.That(matches[1].Matched, Is.False);
        }

        #endregion MatchNames
    }
}