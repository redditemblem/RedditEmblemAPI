﻿using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.WeaponRankBonuses;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    [TestClass]
    public class WeaponRankBonusTests
    {
        #region Constants

        private const string INPUT_CATEGORY = "Sword";

        #endregion Constants

        [TestMethod]
        public void WeaponRankBonusConstructor_RequiredFields_WithInputNull()
        {
            WeaponRankBonusesConfig config = new WeaponRankBonusesConfig()
            {
                Category = 0
            };

            List<string> data = new List<string>() { };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new WeaponRankBonus(config, data));
        }

        [TestMethod]
        public void WeaponRankBonusConstructor_RequiredFields()
        {
            WeaponRankBonusesConfig config = new WeaponRankBonusesConfig()
            {
                Category = 0
            };

            List<string> data = new List<string>() { INPUT_CATEGORY };

            WeaponRankBonus bonus = new WeaponRankBonus(config, data);

            Assert.AreEqual<string>(INPUT_CATEGORY, bonus.Category);
        }

        #region OptionalField_Rank

        [TestMethod]
        public void WeaponRankBonusConstructor_OptionalField_Rank_EmptyString()
        {
            WeaponRankBonusesConfig config = new WeaponRankBonusesConfig()
            {
                Category = 0,
                Rank = 1
            };

            List<string> data = new List<string>() { INPUT_CATEGORY, string.Empty };

            WeaponRankBonus bonus = new WeaponRankBonus(config, data);

            Assert.AreEqual<string>(string.Empty, bonus.Rank);
        }

        [TestMethod]
        public void WeaponRankBonusConstructor_OptionalField_Rank()
        {
            WeaponRankBonusesConfig config = new WeaponRankBonusesConfig()
            {
                Category = 0,
                Rank = 1
            };

            List<string> data = new List<string>() { INPUT_CATEGORY, "E" };

            WeaponRankBonus bonus = new WeaponRankBonus(config, data);

            Assert.AreEqual<string>("E", bonus.Rank);
        }

        #endregion OptionalField_Rank

        #region OptionalField_CombatStatModifiers

        [TestMethod]
        public void WeaponRankBonusConstructor_OptionalField_CombatStatModifiers_EmptyString()
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

            List<string> data = new List<string>()
            {
                INPUT_CATEGORY,
                string.Empty,
                string.Empty
            };

            WeaponRankBonus bonus = new WeaponRankBonus(config, data);

            Assert.AreEqual<int>(0, bonus.CombatStatModifiers.Count);
        }

        [TestMethod]
        public void WeaponRankBonusConstructor_OptionalField_CombatStatModifiers()
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

            List<string> data = new List<string>()
            {
                INPUT_CATEGORY,
                "1",
                "-1"
            };

            WeaponRankBonus bonus = new WeaponRankBonus(config, data);

            Assert.AreEqual<int>(2, bonus.CombatStatModifiers.Count);
            Assert.AreEqual<int>(1, bonus.CombatStatModifiers[stat1]);
            Assert.AreEqual<int>(-1, bonus.CombatStatModifiers[stat2]);
        }

        #endregion OptionalField_CombatStatModifiers

        #region OptionalField_StatModifiers

        [TestMethod]
        public void WeaponRankBonusConstructor_OptionalField_StatModifiers_EmptyString()
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

            List<string> data = new List<string>()
            {
                INPUT_CATEGORY,
                string.Empty,
                string.Empty
            };

            WeaponRankBonus bonus = new WeaponRankBonus(config, data);

            Assert.AreEqual<int>(0, bonus.StatModifiers.Count);
        }

        [TestMethod]
        public void WeaponRankBonusConstructor_OptionalField_StatModifiers()
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

            List<string> data = new List<string>()
            {
                INPUT_CATEGORY,
                "1",
                "-1"
            };

            WeaponRankBonus bonus = new WeaponRankBonus(config, data);

            Assert.AreEqual<int>(2, bonus.StatModifiers.Count);
            Assert.AreEqual<int>(1, bonus.StatModifiers[stat1]);
            Assert.AreEqual<int>(-1, bonus.StatModifiers[stat2]);
        }

        #endregion OptionalField_StatModifiers

        #region BuildList

        [TestMethod]
        public void WeaponRankBonus_BuildList_WithInput_Null()
        {
            List<WeaponRankBonus> list = WeaponRankBonus.BuildList(null);
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void WeaponRankBonus_BuildList_WithInput_NullQuery()
        {
            WeaponRankBonusesConfig config = new WeaponRankBonusesConfig()
            {
                Query = null,
                Category = 0
            };

            List<WeaponRankBonus> list = WeaponRankBonus.BuildList(config);
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void WeaponRankBonus_BuildList_WithInput_EmptyQuery()
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

            List<WeaponRankBonus> list = WeaponRankBonus.BuildList(config);
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void WeaponRankBonus_BuildList_WithInput_CategoryOnly()
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

            List<WeaponRankBonus> list = WeaponRankBonus.BuildList(config);
            Assert.AreEqual(1, list.Count);
        }

        [TestMethod]
        public void WeaponRankBonus_BuildList_WithInput_SingleBonus()
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

            List<WeaponRankBonus> list = WeaponRankBonus.BuildList(config);
            Assert.AreEqual(1, list.Count);
        }

        [TestMethod]
        public void WeaponRankBonus_BuildList_WithInput_MultipleBonuses()
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

            List<WeaponRankBonus> list = WeaponRankBonus.BuildList(config);
            Assert.AreEqual(3, list.Count);
        }

        [TestMethod]
        public void WeaponRankBonus_BuildList_WithInput_DuplicateBonus()
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

            Assert.ThrowsException<WeaponRankBonusProcessingException>(() => WeaponRankBonus.BuildList(config));
        }

        [TestMethod]
        public void WeaponRankBonus_BuildList_WithInput_Invalid()
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

            Assert.ThrowsException<WeaponRankBonusProcessingException>(() => WeaponRankBonus.BuildList(config));
        }

        #endregion BuildList
    }
}
