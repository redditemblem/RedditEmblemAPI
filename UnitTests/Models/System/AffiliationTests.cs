using NSubstitute;
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
        public void Constructor_RequiredFields_IndexOutOfBounds()
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

            IDictionary<string, IAffiliation> dict = Affiliation.BuildDictionary(config);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_DuplicateName()
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

            Assert.Throws<AffiliationProcessingException>(() => Affiliation.BuildDictionary(config));
        }

        [Test]
        public void BuildDictionary_WithInput_Invalid()
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

            Assert.Throws<AffiliationProcessingException>(() => Affiliation.BuildDictionary(config));
        }

        [Test]
        public void BuildDictionary()
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

            IDictionary<string, IAffiliation> dict = Affiliation.BuildDictionary(config);
            Assert.That(dict.Count, Is.EqualTo(1));
        }

        [Test]
        public void BuildDictionary_MultiQuery()
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

            IDictionary<string, IAffiliation> dict = Affiliation.BuildDictionary(config);
            Assert.That(dict.Count, Is.EqualTo(4));
        }

        #endregion BuildDictionary

        #region MatchNames

        [Test]
        public void MatchNames_UnmatchedName()
        {
            string aff1Name = "Affiliation 1";
            string aff2Name = "Affiliation 2";

            IAffiliation aff1 = Substitute.For<IAffiliation>();
            aff1.Name.Returns(aff1Name);

            IDictionary<string, IAffiliation> dict = new Dictionary<string, IAffiliation>();
            dict.Add(aff1Name, aff1);

            IEnumerable<string> names = new List<string>() { aff2Name };

            Assert.Throws<UnmatchedAffiliationException>(() => Affiliation.MatchNames(dict, names));
        }

        [Test]
        public void MatchNames_SingleMatch()
        {
            string aff1Name = "Affiliation 1";
            string aff2Name = "Affiliation 2";

            IAffiliation aff1 = Substitute.For<IAffiliation>();
            aff1.Name.Returns(aff1Name);

            IAffiliation aff2 = Substitute.For<IAffiliation>();
            aff2.Name.Returns(aff2Name);

            IDictionary<string, IAffiliation> dict = new Dictionary<string, IAffiliation>();
            dict.Add(aff1Name, aff1);
            dict.Add(aff2Name, aff2);

            IEnumerable<string> names = new List<string>() { aff1Name };
            List<IAffiliation> matches = Affiliation.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(1));
            Assert.That(matches.Contains(aff1), Is.True);
            matches.First().Received(1).FlagAsMatched();
        }

        [Test]
        public void MatchNames_MultipleMatches()
        {
            string aff1Name = "Affiliation 1";
            string aff2Name = "Affiliation 2";

            IAffiliation aff1 = Substitute.For<IAffiliation>();
            aff1.Name.Returns(aff1Name);

            IAffiliation aff2 = Substitute.For<IAffiliation>();
            aff2.Name.Returns(aff2Name);

            IDictionary<string, IAffiliation> dict = new Dictionary<string, IAffiliation>();
            dict.Add(aff1Name, aff1);
            dict.Add(aff2Name, aff2);

            IEnumerable<string> names = new List<string>() { aff1Name, aff2Name };
            List<IAffiliation> matches = Affiliation.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches.Contains(aff1), Is.True);
            Assert.That(matches.Contains(aff2), Is.True);

            matches[0].Received(1).FlagAsMatched();
            matches[1].Received(1).FlagAsMatched();
        }

        [Test]
        public void MatchNames_MultipleMatches_DoNotSetMatchedStatus()
        {
            string aff1Name = "Affiliation 1";
            string aff2Name = "Affiliation 2";

            IAffiliation aff1 = Substitute.For<IAffiliation>();
            aff1.Name.Returns(aff1Name);

            IAffiliation aff2 = Substitute.For<IAffiliation>();
            aff2.Name.Returns(aff2Name);

            IDictionary<string, IAffiliation> dict = new Dictionary<string, IAffiliation>();
            dict.Add(aff1Name, aff1);
            dict.Add(aff2Name, aff2);

            IEnumerable<string> names = new List<string>() { aff1Name, aff2Name };
            List<IAffiliation> matches = Affiliation.MatchNames(dict, names, false);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches.Contains(aff1), Is.True);
            Assert.That(matches.Contains(aff2), Is.True);

            matches[0].DidNotReceive().FlagAsMatched();
            matches[1].DidNotReceive().FlagAsMatched();
        }

        #endregion MatchNames

        #region MatchName

        [Test]
        public void MatchName_UnmatchedName()
        {
            string aff1Name = "Affiliation 1";

            IAffiliation aff1 = Substitute.For<IAffiliation>();
            aff1.Name.Returns(aff1Name);

            IDictionary<string, IAffiliation> dict = new Dictionary<string, IAffiliation>();
            dict.Add(aff1Name, aff1);

            string name = "Affiliation 2";

            Assert.Throws<UnmatchedAffiliationException>(() => Affiliation.MatchName(dict, name));
        }

        [Test]
        public void MatchName()
        {
            string aff1Name = "Affiliation 1";

            IAffiliation aff1 = Substitute.For<IAffiliation>();
            aff1.Name.Returns(aff1Name);

            IDictionary<string, IAffiliation> dict = new Dictionary<string, IAffiliation>();
            dict.Add(aff1Name, aff1);

            IAffiliation match = Affiliation.MatchName(dict, aff1Name);

            Assert.That(match, Is.Not.Null);
            Assert.That(match, Is.EqualTo(aff1));
            match.Received(1).FlagAsMatched();
        }

        [Test]
        public void MatchName_DoNotSetMatchedStatus()
        {
            string aff1Name = "Affiliation 1";

            IAffiliation aff1 = Substitute.For<IAffiliation>();
            aff1.Name.Returns(aff1Name);

            IDictionary<string, IAffiliation> dict = new Dictionary<string, IAffiliation>();
            dict.Add(aff1Name, aff1);

            IAffiliation match = Affiliation.MatchName(dict, aff1Name, false);

            Assert.That(match, Is.Not.Null);
            Assert.That(match, Is.EqualTo(aff1));
            match.DidNotReceive().FlagAsMatched();
        }

        #endregion MatchName
    }
}