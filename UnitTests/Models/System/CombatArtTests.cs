using NSubstitute;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.CombatArts;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    public class CombatArtTests
    {
        #region Constants

        private const string INPUT_NAME = "Combat Art Test";
        private const string INPUT_MINIMUM_RANGE = "1";
        private const string INPUT_MAXIMUM_RANGE = "2";
        private const string INPUT_DURABILITY_COST = "3";

        private const string STAT_1_SOURCE_NAME = "Stat 1";
        private const string STAT_2_SOURCE_NAME = "Stat 2";

        #endregion Constants

        #region Setup

        private IDictionary<string, ITag> TAGS;


        [SetUp]
        public void Setup()
        {
            string tag1Name = "Tag 1";
            ITag tag1 = Substitute.For<ITag>();
            tag1.Name.Returns(tag1Name);

            string tag2Name = "Tag 2";
            ITag tag2 = Substitute.For<ITag>();
            tag2.Name.Returns(tag2Name);

            this.TAGS = new Dictionary<string, ITag>();
            this.TAGS.Add(tag1Name, tag1);
            this.TAGS.Add(tag2Name, tag2);
        }

        #endregion Setup

        [Test]
        public void Constructor_RequiredFields_WithInputNull()
        {
            CombatArtsConfig config = new CombatArtsConfig()
            {
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>()
                {
                    new NamedStatConfig(){ SourceName = STAT_1_SOURCE_NAME, Value = 3 }
                },
                DurabilityCost = 5
            };

            IEnumerable<string> data = new List<string>() { };

            Assert.Throws<RequiredValueNotProvidedException>(() => new CombatArt(config, data, TAGS));
        }

        [Test]
        public void Constructor_RequiredFields()
        {
            CombatArtsConfig config = new CombatArtsConfig()
            {
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>()
                {
                    new NamedStatConfig(){ SourceName = STAT_1_SOURCE_NAME, Value = 3 },
                    new NamedStatConfig(){ SourceName = STAT_2_SOURCE_NAME, Value = 4 }
                },
                DurabilityCost = 5
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, "-1", "1", INPUT_DURABILITY_COST };

            ICombatArt art = new CombatArt(config, data, TAGS);

            Assert.That(art.Name, Is.EqualTo(INPUT_NAME));
            Assert.That(art.Range.Minimum, Is.EqualTo(1));
            Assert.That(art.Range.Maximum, Is.EqualTo(2));
            Assert.That(art.Stats[STAT_1_SOURCE_NAME], Is.EqualTo(-1));
            Assert.That(art.Stats[STAT_2_SOURCE_NAME], Is.EqualTo(1));
            Assert.That(art.DurabilityCost, Is.EqualTo(3));
        }

        #region OptionalField_SpriteURL

        [Test]
        public void Constructor_OptionalField_SpriteURL_EmptyString()
        {
            CombatArtsConfig config = new CombatArtsConfig()
            {
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 3,
                SpriteURL = 4
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST, string.Empty };

            ICombatArt art = new CombatArt(config, data, TAGS);

            Assert.That(art.SpriteURL, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_SpriteURL_InvalidURL()
        {
            CombatArtsConfig config = new CombatArtsConfig()
            {
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 3,
                SpriteURL = 4
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST, "NotAURL" };

            Assert.Throws<URLException>(() => new CombatArt(config, data, TAGS));
        }

        [Test]
        public void Constructor_OptionalField_SpriteURL()
        {
            CombatArtsConfig config = new CombatArtsConfig()
            {
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 3,
                SpriteURL = 4
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST, UnitTestConsts.IMAGE_URL };

            ICombatArt art = new CombatArt(config, data, TAGS);

            Assert.That(art.SpriteURL, Is.EqualTo(UnitTestConsts.IMAGE_URL));
        }

        #endregion OptionalField_SpriteURL

        #region OptionalField_WeaponRank

        [Test]
        public void Constructor_OptionalField_WeaponRank_EmptyString()
        {
            CombatArtsConfig config = new CombatArtsConfig()
            {
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 3,
                WeaponRank = 4
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST, string.Empty };

            ICombatArt art = new CombatArt(config, data, TAGS);

            Assert.That(art.WeaponRank, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_WeaponRank()
        {
            string weaponRank = "S";

            CombatArtsConfig config = new CombatArtsConfig()
            {
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 3,
                WeaponRank = 4
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST, weaponRank };

            ICombatArt art = new CombatArt(config, data, TAGS);

            Assert.That(art.WeaponRank, Is.EqualTo(weaponRank));
        }

        #endregion OptionalField_WeaponRank

        #region OptionalField_Category

        [Test]
        public void Constructor_OptionalField_Category_EmptyString()
        {
            CombatArtsConfig config = new CombatArtsConfig()
            {
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 3,
                Category = 4
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST, string.Empty };

            ICombatArt art = new CombatArt(config, data, TAGS);

            Assert.That(art.Category, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_Category()
        {
            string category = "Sword";

            CombatArtsConfig config = new CombatArtsConfig()
            {
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 3,
                Category = 4
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST, category };

            ICombatArt art = new CombatArt(config, data, TAGS);

            Assert.That(art.Category, Is.EqualTo(category));
        }

        #endregion OptionalField_Category

        #region OptionalField_UtilizedStats

        [Test]
        public void Constructor_OptionalField_UtilizedStats_WithInputEmpty()
        {
            CombatArtsConfig config = new CombatArtsConfig()
            {
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 3,
                UtilizedStats = new List<int>() { 4, 5 }
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST, string.Empty, string.Empty };

            ICombatArt art = new CombatArt(config, data, TAGS);

            Assert.That(art.UtilizedStats, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_UtilizedStats_WithSingleValueInputs()
        {
            string stat1 = "Str";
            string stat2 = "Mag";

            CombatArtsConfig config = new CombatArtsConfig()
            {
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 3,
                UtilizedStats = new List<int>() { 4, 5 }
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST, stat1, stat2 };

            ICombatArt art = new CombatArt(config, data, TAGS);

            List<string> expected = new List<string>() { stat1, stat2 };
            Assert.That(art.UtilizedStats, Is.EqualTo(expected));
        }

        [Test]
        public void Constructor_OptionalField_UtilizedStats_WithCSVInputs()
        {
            string stat1 = "Str";
            string stat2 = "Mag";
            string stat3 = "Skl";
            string stat4 = "Lck";

            CombatArtsConfig config = new CombatArtsConfig()
            {
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 3,
                UtilizedStats = new List<int>() { 4, 5 }
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST, stat1 + "," + stat2, stat3 + "," + stat4 };

            ICombatArt art = new CombatArt(config, data, TAGS);

            List<string> expected = new List<string>() { stat1, stat2, stat3, stat4 };
            Assert.That(art.UtilizedStats, Is.EqualTo(expected));
        }

        [Test]
        public void Constructor_OptionalField_UtilizedStats_WithDuplicateInputs()
        {
            string stat1 = "Str";
            string stat2 = "Mag";

            CombatArtsConfig config = new CombatArtsConfig()
            {
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 3,
                UtilizedStats = new List<int>() { 4, 5 }
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST, stat1 + "," + stat2, stat1 };

            ICombatArt art = new CombatArt(config, data, TAGS);

            //Uncertain if this is correct, but I'm leaving it as is for now. Should duplicates be removed?
            List<string> expected = new List<string>() { stat1, stat2, stat1 };
            Assert.That(art.UtilizedStats, Is.EqualTo(expected));
        }

        #endregion OptionalField_UtilizedStats

        #region OptionalField_Tags

        [Test]
        public void Constructor_OptionalField_Tags_WithInputEmpty()
        {
            CombatArtsConfig config = new CombatArtsConfig()
            {
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 3,
                Tags = new List<int>() { 4, 5 }
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST, string.Empty, string.Empty };

            ICombatArt art = new CombatArt(config, data, TAGS);

            Assert.That(art.TagsList, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_Tags_UnmatchedTag()
        {
            CombatArtsConfig config = new CombatArtsConfig()
            {
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 3,
                Tags = new List<int>() { 4 }
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST, "Tag 3" };

            Assert.Throws<UnmatchedTagException>(() => new CombatArt(config, data, TAGS));
        }

        [Test]
        public void Constructor_OptionalField_Tags_DuplicateTags()
        {
            string tag1 = "Tag 1";

            CombatArtsConfig config = new CombatArtsConfig()
            {
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 3,
                Tags = new List<int>() { 4, 5 }
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST, tag1, tag1 };

            ICombatArt art = new CombatArt(config, data, TAGS);

            Assert.That(art.TagsList.Count, Is.EqualTo(1));
            art.TagsList.First().DidNotReceive().FlagAsMatched();
        }

        [Test]
        public void Constructor_OptionalField_Tags_MultipleSameField()
        {
            CombatArtsConfig config = new CombatArtsConfig()
            {
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 3,
                Tags = new List<int>() { 4 }
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST, "Tag 1,Tag 2" };

            ICombatArt art = new CombatArt(config, data, TAGS);

            Assert.That(art.TagsList.Count, Is.EqualTo(2));
            art.TagsList.ForEach(t => t.DidNotReceive().FlagAsMatched());
        }

        [Test]
        public void Constructor_OptionalField_Tags_MultipleSeparateFields()
        {
            CombatArtsConfig config = new CombatArtsConfig()
            {
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 3,
                Tags = new List<int>() { 4, 5 }
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST, "Tag 1", "Tag 2" };

            ICombatArt art = new CombatArt(config, data, TAGS);

            Assert.That(art.TagsList.Count, Is.EqualTo(2));
            art.TagsList.ForEach(t => t.DidNotReceive().FlagAsMatched());
        }

        #endregion OptionalField_Tags

        #region OptionalField_TextFields

        [Test]
        public void Constructor_OptionalField_TextFields_EmptyString()
        {
            CombatArtsConfig config = new CombatArtsConfig()
            {
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 5,
                TextFields = new List<int>() { 6, 7 }
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST, string.Empty, string.Empty };

            ICombatArt art = new CombatArt(config, data, TAGS);

            Assert.That(art.TextFields, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_TextFields()
        {
            string textField1 = "Text Field 1";
            string textField2 = "Text Field 2";

            CombatArtsConfig config = new CombatArtsConfig()
            {
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 3,
                TextFields = new List<int>() { 4, 5 }
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST, textField1, textField2 };

            ICombatArt art = new CombatArt(config, data, TAGS);

            List<string> expected = new List<string>() { textField1, textField2 };
            Assert.That(art.TextFields, Is.EqualTo(expected));
        }

        #endregion OptionalField_TextFields

        #region FlagAsMatched

        [Test]
        public void FlagAsMatched()
        {
            CombatArtsConfig config = new CombatArtsConfig()
            {
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 3
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST };

            ICombatArt art = new CombatArt(config, data, TAGS);

            Assert.That(art.Matched, Is.False);

            art.FlagAsMatched();

            Assert.That(art.Matched, Is.True);
        }

        [Test]
        public void FlagAsMatched_WithTags()
        {
            CombatArtsConfig config = new CombatArtsConfig()
            {
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 3,
                Tags = new List<int>() { 4, 5 }
            };

            IEnumerable<string> data = new List<string>() { INPUT_NAME, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST, "Tag 1", "Tag 2" };

            ICombatArt art = new CombatArt(config, data, TAGS);

            Assert.That(art.Matched, Is.False);
            Assert.That(art.TagsList.Count, Is.EqualTo(2));
            Assert.That(art.TagsList[0].Matched, Is.False);
            Assert.That(art.TagsList[1].Matched, Is.False);

            art.FlagAsMatched();

            Assert.That(art.Matched, Is.True);
            art.TagsList.ForEach(t => t.Received().FlagAsMatched());
        }

        #endregion FlagAsMatched

        #region BuildDictionary

        [Test]
        public void BuildDictionary_WithInput_Null()
        {
            IDictionary<string, ICombatArt> dict = CombatArt.BuildDictionary(null, TAGS);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_NullQuery()
        {
            CombatArtsConfig config = new CombatArtsConfig()
            {
                Queries = null
            };

            IDictionary<string, ICombatArt> dict = CombatArt.BuildDictionary(config, TAGS);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_EmptyQuery()
        {
            CombatArtsConfig config = new CombatArtsConfig()
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
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 3
            };

            IDictionary<string, ICombatArt> dict = CombatArt.BuildDictionary(config, TAGS);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_DuplicateName()
        {
            CombatArtsConfig config = new CombatArtsConfig()
            {
                Queries = new List<IQuery>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>() { INPUT_NAME, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST },
                            new List<object>() { INPUT_NAME, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST }
                        }
                    }
                },
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 3
            };

            Assert.Throws<CombatArtProcessingException>(() => CombatArt.BuildDictionary(config, TAGS));
        }

        [Test]
        public void BuildDictionary()
        {
            CombatArtsConfig config = new CombatArtsConfig()
            {
                Queries = new List<IQuery>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>() { INPUT_NAME, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST }
                        }
                    }
                },
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 3
            };

            IDictionary<string, ICombatArt> dict = CombatArt.BuildDictionary(config, TAGS);
            Assert.That(dict.Count, Is.EqualTo(1));
        }

        [Test]
        public void BuildDictionary_MultiQuery()
        {
            CombatArtsConfig config = new CombatArtsConfig()
            {
                Queries = new List<IQuery>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>() { "Combat Art 1", INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST },
                            new List<object>() { "Combat Art 2", INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST }
                        }
                    },
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>() { "Combat Art 3", INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST },
                            new List<object>() { "Combat Art 4", INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST }
                        }
                    }
                },
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 3
            };

            IDictionary<string, ICombatArt> dict = CombatArt.BuildDictionary(config, TAGS);
            Assert.That(dict.Count, Is.EqualTo(4));
        }

        #endregion BuildDictionary

        #region MatchNames

        [Test]
        public void MatchNames_UnmatchedName()
        {
            CombatArtsConfig config = new CombatArtsConfig()
            {
                Queries = new List<IQuery>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>() { "Combat Art 1", INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST },
                            new List<object>() { "Combat Art 2", INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST }
                        }
                    }
                },
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 3
            };

            IDictionary<string, ICombatArt> dict = CombatArt.BuildDictionary(config, TAGS);
            IEnumerable<string> names = new List<string>() { "Combat Art 3" };

            Assert.Throws<UnmatchedCombatArtException>(() => CombatArt.MatchNames(dict, names));
        }

        [Test]
        public void MatchNames_SingleMatch()
        {
            CombatArtsConfig config = new CombatArtsConfig()
            {
                Queries = new List<IQuery>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>() { "Combat Art 1", INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST },
                            new List<object>() { "Combat Art 2", INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST }
                        }
                    }
                },
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 3
            };

            IDictionary<string, ICombatArt> dict = CombatArt.BuildDictionary(config, TAGS);
            IEnumerable<string> names = new List<string>() { "Combat Art 1" };

            List<ICombatArt> matches = CombatArt.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(1));
            Assert.That(matches.First().Matched, Is.True);
        }

        [Test]
        public void MatchNames_MultipleMatches()
        {
            string combatArt1 = "Combat Art 1";
            string combatArt2 = "Combat Art 2";

            CombatArtsConfig config = new CombatArtsConfig()
            {
                Queries = new List<IQuery>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>() { combatArt1, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST },
                            new List<object>() { combatArt2, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST }
                        }
                    }
                },
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 3
            };

            IDictionary<string, ICombatArt> dict = CombatArt.BuildDictionary(config, TAGS);
            IEnumerable<string> names = new List<string>() { combatArt1, combatArt2 };

            List<ICombatArt> matches = CombatArt.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches[0].Matched, Is.True);
            Assert.That(matches[1].Matched, Is.True);
        }

        [Test]
        public void MatchNames_MultipleMatches_DoNotSetMatchedStatus()
        {
            string combatArt1 = "Combat Art 1";
            string combatArt2 = "Combat Art 2";

            CombatArtsConfig config = new CombatArtsConfig()
            {
                Queries = new List<IQuery>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>() { combatArt1, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST },
                            new List<object>() { combatArt2, INPUT_MINIMUM_RANGE, INPUT_MAXIMUM_RANGE, INPUT_DURABILITY_COST }
                        }
                    }
                },
                Name = 0,
                Range = new CombatArtRangeConfig()
                {
                    Minimum = 1,
                    Maximum = 2
                },
                Stats = new List<NamedStatConfig>(),
                DurabilityCost = 3
            };

            IDictionary<string, ICombatArt> dict = CombatArt.BuildDictionary(config, TAGS);
            IEnumerable<string> names = new List<string>() { "Combat Art 1", "Combat Art 2" };

            List<ICombatArt> matches = CombatArt.MatchNames(dict, names, false);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches[0].Matched, Is.False);
            Assert.That(matches[1].Matched, Is.False);
        }

        #endregion MatchNames
    }
}
