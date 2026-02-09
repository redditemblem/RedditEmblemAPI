using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.WeaponRankBonuses;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    public class WeaponRankBonusTests
    {
        #region Constants

        private const string INPUT_CATEGORY = "Sword";

        #endregion Constants

        [Test]
        public void Constructor_RequiredFields_WithInputNull()
        {
            WeaponRankBonusesConfig config = new WeaponRankBonusesConfig()
            {
                Category = 0
            };

            IEnumerable<string> data = new List<string>() { };

            Assert.Throws<RequiredValueNotProvidedException>(() => new WeaponRankBonus(config, data));
        }

        [Test]
        public void Constructor_RequiredFields()
        {
            WeaponRankBonusesConfig config = new WeaponRankBonusesConfig()
            {
                Category = 0
            };

            IEnumerable<string> data = new List<string>() { INPUT_CATEGORY };

            IWeaponRankBonus bonus = new WeaponRankBonus(config, data);

            Assert.That(bonus.Category, Is.EqualTo(INPUT_CATEGORY));
        }

        #region OptionalField_Rank

        [Test]
        public void Constructor_OptionalField_Rank_EmptyString()
        {
            WeaponRankBonusesConfig config = new WeaponRankBonusesConfig()
            {
                Category = 0,
                Rank = 1
            };

            IEnumerable<string> data = new List<string>() { INPUT_CATEGORY, string.Empty };

            IWeaponRankBonus bonus = new WeaponRankBonus(config, data);

            Assert.That(bonus.Rank, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_Rank()
        {
            string rank = "E";

            WeaponRankBonusesConfig config = new WeaponRankBonusesConfig()
            {
                Category = 0,
                Rank = 1
            };

            IEnumerable<string> data = new List<string>() { INPUT_CATEGORY, rank };

            IWeaponRankBonus bonus = new WeaponRankBonus(config, data);

            Assert.That(bonus.Rank, Is.EqualTo(rank));
        }

        #endregion OptionalField_Rank

        #region OptionalField_CombatStatModifiers

        [Test]
        public void Constructor_OptionalField_CombatStatModifiers_EmptyString()
        {
            string stat1 = "Stat 1";
            string stat2 = "Stat 2";

            WeaponRankBonusesConfig config = new WeaponRankBonusesConfig()
            {
                Category = 0,
                CombatStatModifiers = new List<NamedStatConfig>()
                {
                    new NamedStatConfig { SourceName = stat1, Value = 1 },
                    new NamedStatConfig { SourceName = stat2, Value = 2 }
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_CATEGORY,
                string.Empty,
                string.Empty
            };

            IWeaponRankBonus bonus = new WeaponRankBonus(config, data);

            Assert.That(bonus.CombatStatModifiers, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_CombatStatModifiers()
        {
            string stat1 = "Stat 1";
            string stat2 = "Stat 2";

            WeaponRankBonusesConfig config = new WeaponRankBonusesConfig()
            {
                Category = 0,
                CombatStatModifiers = new List<NamedStatConfig>()
                {
                    new NamedStatConfig { SourceName = stat1, Value = 1 },
                    new NamedStatConfig { SourceName = stat2, Value = 2 }
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_CATEGORY,
                "1",
                "-1"
            };

            IWeaponRankBonus bonus = new WeaponRankBonus(config, data);

            Assert.That(bonus.CombatStatModifiers.Count, Is.EqualTo(2));
            Assert.That(bonus.CombatStatModifiers[stat1], Is.EqualTo(1));
            Assert.That(bonus.CombatStatModifiers[stat2], Is.EqualTo(-1));
        }

        #endregion OptionalField_CombatStatModifiers

        #region OptionalField_StatModifiers

        [Test]
        public void Constructor_OptionalField_StatModifiers_EmptyString()
        {
            string stat1 = "Stat 1";
            string stat2 = "Stat 2";

            WeaponRankBonusesConfig config = new WeaponRankBonusesConfig()
            {
                Category = 0,
                StatModifiers = new List<NamedStatConfig>()
                {
                    new NamedStatConfig { SourceName = stat1, Value = 1 },
                    new NamedStatConfig { SourceName = stat2, Value = 2 }
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_CATEGORY,
                string.Empty,
                string.Empty
            };

            IWeaponRankBonus bonus = new WeaponRankBonus(config, data);

            Assert.That(bonus.StatModifiers, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_StatModifiers()
        {
            string stat1 = "Stat 1";
            string stat2 = "Stat 2";

            WeaponRankBonusesConfig config = new WeaponRankBonusesConfig()
            {
                Category = 0,
                StatModifiers = new List<NamedStatConfig>()
                {
                    new NamedStatConfig { SourceName = stat1, Value = 1 },
                    new NamedStatConfig { SourceName = stat2, Value = 2 }
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_CATEGORY,
                "1",
                "-1"
            };

            IWeaponRankBonus bonus = new WeaponRankBonus(config, data);

            Assert.That(bonus.StatModifiers.Count, Is.EqualTo(2));
            Assert.That(bonus.StatModifiers[stat1], Is.EqualTo(1));
            Assert.That(bonus.StatModifiers[stat2], Is.EqualTo(-1));
        }

        #endregion OptionalField_StatModifiers

        #region BuildList

        [Test]
        public void BuildList_WithInput_Null()
        {
            List<IWeaponRankBonus> list = WeaponRankBonus.BuildList(null);
            Assert.That(list, Is.Empty);
        }

        [Test]
        public void BuildList_WithInput_NullQuery()
        {
            WeaponRankBonusesConfig config = new WeaponRankBonusesConfig()
            {
                Query = null,
                Category = 0
            };

            List<IWeaponRankBonus> list = WeaponRankBonus.BuildList(config);
            Assert.That(list, Is.Empty);
        }

        [Test]
        public void BuildList_WithInput_EmptyQuery()
        {
            WeaponRankBonusesConfig config = new WeaponRankBonusesConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ }
                    }
                },
                Category = 0
            };

            List<IWeaponRankBonus> list = WeaponRankBonus.BuildList(config);
            Assert.That(list, Is.Empty);
        }

        [Test]
        public void BuildList_WithInput_CategoryOnly()
        {
            WeaponRankBonusesConfig config = new WeaponRankBonusesConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ INPUT_CATEGORY }
                    }
                },
                Category = 0
            };

            List<IWeaponRankBonus> list = WeaponRankBonus.BuildList(config);
            Assert.That(list.Count, Is.EqualTo(1));
        }

        [Test]
        public void BuildList_WithInput_SingleBonus()
        {
            WeaponRankBonusesConfig config = new WeaponRankBonusesConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ INPUT_CATEGORY, "A" }
                    }
                },
                Category = 0,
                Rank = 1
            };

            List<IWeaponRankBonus> list = WeaponRankBonus.BuildList(config);
            Assert.That(list.Count, Is.EqualTo(1));
        }

        [Test]
        public void BuildList_WithInput_MultipleBonuses()
        {
            WeaponRankBonusesConfig config = new WeaponRankBonusesConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ INPUT_CATEGORY, "E" },
                        new List<object>(){ INPUT_CATEGORY, "D" },
                        new List<object>(){ INPUT_CATEGORY, "C" }
                    }
                },
                Category = 0,
                Rank = 1
            };

            List<IWeaponRankBonus> list = WeaponRankBonus.BuildList(config);
            Assert.That(list.Count, Is.EqualTo(3));
        }

        [Test]
        public void BuildList_WithInput_DuplicateBonus()
        {
            WeaponRankBonusesConfig config = new WeaponRankBonusesConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ INPUT_CATEGORY, "A" },
                        new List<object>(){ INPUT_CATEGORY, "A" }
                    }
                },
                Category = 0,
                Rank = 1
            };

            Assert.Throws<WeaponRankBonusProcessingException>(() => WeaponRankBonus.BuildList(config));
        }

        [Test]
        public void BuildList_WithInput_Invalid()
        {
            WeaponRankBonusesConfig config = new WeaponRankBonusesConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ INPUT_CATEGORY, "A" },
                        new List<object>(){ INPUT_CATEGORY, "A" }
                    }
                },
                Category = 0,
                CombatStatModifiers = new List<NamedStatConfig>
                {
                    new NamedStatConfig(){ SourceName = "Atk", Value = 1 }
                }
            };

            Assert.Throws<WeaponRankBonusProcessingException>(() => WeaponRankBonus.BuildList(config));
        }

        #endregion BuildList
    }
}
