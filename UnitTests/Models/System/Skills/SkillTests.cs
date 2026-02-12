using NSubstitute;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Skills;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.EquippedItem;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.PairUp;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.Radius;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.TerrainType;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.UnitStats;

namespace UnitTests.Models.System.Skills
{
    public class SkillTests
    {
        #region Constants

        private const string INPUT_NAME = "Skill Test";

        #endregion Constants

        [Test]
        public void Constructor_RequiredFields_WithInputNull()
        {
            SkillsConfig config = new SkillsConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>() { };

            Assert.Throws<RequiredValueNotProvidedException>(() => new Skill(config, data));
        }

        [Test]
        public void Constructor_RequiredFields()
        {
            SkillsConfig config = new SkillsConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME
            };

            ISkill skill = new Skill(config, data);

            Assert.That(skill.Name, Is.EqualTo(INPUT_NAME));
        }

        #region OptionalField_SpriteURL

        [Test]
        public void Constructor_OptionalField_SpriteURL_EmptyString()
        {
            SkillsConfig config = new SkillsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            ISkill skill = new Skill(config, data);

            Assert.That(skill.SpriteURL, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_SpriteURL_InvalidURL()
        {
            SkillsConfig config = new SkillsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "NotAURL"
            };

            Assert.Throws<URLException>(() => new Skill(config, data));
        }

        [Test]
        public void Constructor_OptionalField_SpriteURL()
        {
            SkillsConfig config = new SkillsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL
            };

            ISkill skill = new Skill(config, data);

            Assert.That(skill.SpriteURL, Is.EqualTo(UnitTestConsts.IMAGE_URL));
        }

        #endregion OptionalField_SpriteURL

        #region OptionalField_TextFields

        [Test]
        public void Constructor_OptionalField_TextFields_EmptyString()
        {
            SkillsConfig config = new SkillsConfig()
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

            ISkill skill = new Skill(config, data);

            List<string> expected = new List<string>();
            Assert.That(skill.TextFields, Is.EqualTo(expected));
        }

        [Test]
        public void Constructor_OptionalField_TextFields()
        {
            string textField1 = "Text Field 1";
            string textField2 = "Text Field 2";

            SkillsConfig config = new SkillsConfig()
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

            ISkill skill = new Skill(config, data);

            List<string> expected = new List<string>() { textField1, textField2 };
            Assert.That(skill.TextFields, Is.EqualTo(expected));
        }

        #endregion OptionalField_TextFields

        #region OptionalField_Effects

        [Test]
        public void Constructor_OptionalField_Effects_EmptyList()
        {
            SkillsConfig config = new SkillsConfig()
            {
                Name = 0,
                Effects = new List<SkillEffectConfig>()
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME
            };

            ISkill skill = new Skill(config, data);

            Assert.That(skill.Effects, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_Effects_EmptyStringEffectType()
        {
            SkillsConfig config = new SkillsConfig()
            {
                Name = 0,
                Effects = new List<SkillEffectConfig>()
                {
                    new SkillEffectConfig()
                    {
                        Type = 1,
                        Parameters = new List<int>(){ 2, 3, 4 }
                    }
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            ISkill skill = new Skill(config, data);

            Assert.That(skill.Effects, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_Effects_UnmatchedEffectType()
        {
            SkillsConfig config = new SkillsConfig()
            {
                Name = 0,
                Effects = new List<SkillEffectConfig>()
                {
                    new SkillEffectConfig()
                    {
                        Type = 1,
                        Parameters = new List<int>(){ 2, 3, 4 }
                    }
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "FakeEffect"
            };

            Assert.Throws<UnmatchedSkillEffectException>(() => new Skill(config, data));
        }

        //Unit Stat Effects
        [TestCase("CombatStatModifier", "Hit", "1", "", typeof(CombatStatModifierEffect))]
        [TestCase("HPBelowCombatStatModifier", "10", "Hit", "1", typeof(HPBelowCombatStatModifierEffect))]
        [TestCase("HPAboveCombatStatModifier", "10", "Hit", "1", typeof(HPAboveCombatStatModifierEffect))]
        [TestCase("StatModifier", "Str", "1", "", typeof(StatModifierEffect))]
        [TestCase("HPBelowStatModifier", "10", "Str", "1", typeof(HPBelowStatModifierEffect))]
        [TestCase("HPAboveStatModifier", "10", "Str", "1", typeof(HPAboveStatModifierEffect))]
        [TestCase("HPDifferenceCombatStatModifier", "0.5", "Hit", "", typeof(HPDifferenceCombatStatModifierEffect))]
        [TestCase("HPDifferenceStatModifier", "0.5", "Str", "", typeof(HPDifferenceStatModifierEffect))]
        [TestCase("ReplaceCombatStatFormulaVariable", "Hit", "{UnitStat[Str]}", "{UnitStat[Mag]}", typeof(ReplaceCombatStatFormulaVariableEffect))]
        //Equipped Item Effects
        [TestCase("EquippedCategoryCombatStatModifier", "Sword", "Hit", "1", typeof(EquippedCategoryCombatStatModifierEffect))]
        [TestCase("EquippedCategoryStatModifier", "Sword", "Str", "1", typeof(EquippedCategoryStatModifierEffect))]
        //Item Effects
        [TestCase("ItemMaxUsesMultiplier", "Sword", "1.5", "", typeof(ItemMaxUsesMultiplierEffect))]
        //Terrain Type Effects
        [TestCase("TerrainTypeCombatStatModifier", "1", "Hit", "1", typeof(TerrainTypeCombatStatModifierEffect))]
        [TestCase("TerrainTypeStatModifier", "1", "Str", "1", typeof(TerrainTypeStatModifierEffect))]
        [TestCase("TerrainTypeCombatStatBonusCombatStatModifier", "Avo", "Hit", "1", typeof(TerrainTypeCombatStatBonusCombatStatModifierEffect))]
        [TestCase("TerrainTypeStatBonusCombatStatModifier", "Def", "Str", "1", typeof(TerrainTypeStatBonusCombatStatModifierEffect))]
        [TestCase("TerrainTypeCombatStatBonusStatModifier", "Avo", "Hit", "1", typeof(TerrainTypeCombatStatBonusStatModifierEffect))]
        [TestCase("TerrainTypeStatBonusStatModifier", "Def", "Str", "1", typeof(TerrainTypeStatBonusStatModifierEffect))]
        //Unit Movement Range Effects
        //Movement Costs
        [TestCase("OverrideMovementType", "Mounted", "", "", typeof(OverrideMovementTypeEffect_Skill))]
        [TestCase("TerrainTypeMovementCostModifier", "1", "1", "", typeof(TerrainTypeMovementCostModifierEffect))]
        [TestCase("TerrainTypeMovementCostSet", "1", "1", "No", typeof(TerrainTypeMovementCostSetEffect_Skill))]
        [TestCase("WarpMovementCostModifier", "1", "1", "", typeof(WarpMovementCostModifierEffect))]
        [TestCase("WarpMovementCostSet", "1", "1", "", typeof(WarpMovementCostSetEffect))]
        [TestCase("RadiusAllyMovementCostSet", "1", "1", "", typeof(RadiusAllyMovementCostSetEffect))]
        //Affiliations
        [TestCase("IgnoreUnitAffiliations", "", "", "", typeof(IgnoreUnitAffiliationsEffect))]
        [TestCase("HPBelowIgnoreUnitAffiliations", "10", "", "", typeof(HPBelowIgnoreUnitAffiliationsEffect))]
        [TestCase("HPAboveIgnoreUnitAffiliations", "10", "", "", typeof(HPAboveIgnoreUnitAffiliationsEffect))]
        [TestCase("ObstructTileRadius", "2", "", "", typeof(ObstructTileRadiusEffect))]
        [TestCase("HPBelowObstructTileRadius", "2", "10", "", typeof(HPBelowObstructTileRadiusEffect))]
        [TestCase("HPAboveObstructTileRadius", "2", "10", "", typeof(HPAboveObstructTileRadiusEffect))]
        //Teleportation
        [TestCase("AllyRadiusTeleport", "1", "1", "", typeof(AllyRadiusTeleportEffect))]
        [TestCase("HPBelowAllyRadiusTeleport", "1", "1", "10", typeof(HPBelowAllyRadiusTeleportEffect))]
        [TestCase("HPAboveAllyRadiusTeleport", "1", "1", "10", typeof(HPAboveAllyRadiusTeleportEffect))]
        [TestCase("HPAboveObstructTileRadius", "1", "1", "10", typeof(HPAboveObstructTileRadiusEffect))]
        [TestCase("AllyHPBelowAllyRadiusTeleport", "1", "1", "10", typeof(AllyHPBelowAllyRadiusTeleportEffect))]
        [TestCase("AllyHPAboveAllyRadiusTeleport", "1", "1", "10", typeof(AllyHPAboveAllyRadiusTeleportEffect))]
        [TestCase("EnemyRadiusTeleport", "1", "1", "", typeof(EnemyRadiusTeleportEffect))]
        [TestCase("HPBelowEnemyRadiusTeleport", "1", "1", "10", typeof(HPBelowEnemyRadiusTeleportEffect))]
        [TestCase("HPAboveEnemyRadiusTeleport", "1", "1", "10", typeof(HPAboveEnemyRadiusTeleportEffect))]
        //Unit Item Range Effects
        [TestCase("ItemAllowMeleeRange", "Sword", "", "", typeof(ItemAllowMeleeRangeEffect))]
        [TestCase("ItemMinRangeSet", "Sword", "1", "", typeof(ItemMinRangeSetEffect))]
        [TestCase("ItemMinRangeModifier", "Sword", "-1", "", typeof(ItemMinRangeModifierEffect))]
        [TestCase("ItemMaxRangeSet", "Sword", "2", "", typeof(ItemMaxRangeSetEffect))]
        [TestCase("ItemMaxRangeModifier", "Sword", "1", "", typeof(ItemMaxRangeModifierEffect))]
        [TestCase("ObstructItemRanges", "1", "", "", typeof(ObstructItemRangesEffect))]
        //Unit Radius Stat Effects
        //Normal
        [TestCase("AllyRadiusCombatStatModifier", "1", "Hit", "1", typeof(AllyRadiusCombatStatModifierEffect))]
        [TestCase("AllyRadiusStatModifier", "1", "Str", "1", typeof(AllyRadiusStatModifierEffect))]
        [TestCase("EnemyRadiusCombatStatModifier", "1", "Hit", "1", typeof(EnemyRadiusCombatStatModifierEffect))]
        [TestCase("EnemyRadiusStatModifier", "1", "Str", "1", typeof(EnemyRadiusStatModifierEffect))]
        //Inverted
        [TestCase("AllyRadiusSelfCombatStatModifier", "1", "Hit", "1", typeof(AllyRadiusSelfCombatStatModifierEffect))]
        [TestCase("AllyRadiusSelfStatModifier", "1", "Str", "1", typeof(AllyRadiusSelfStatModifierEffect))]
        [TestCase("NoAllyRadiusSelfCombatStatModifier", "1", "Hit", "1", typeof(NoAllyRadiusSelfCombatStatModifierEffect))]
        [TestCase("NoAllyRadiusSelfStatModifier", "1", "Str", "1", typeof(NoAllyRadiusSelfStatModifierEffect))]
        //Pair Up Effects
        [TestCase("InPairUpFrontCombatStatModifier", "Hit", "1", "", typeof(InPairUpFrontCombatStatModifierEffect))]
        [TestCase("InPairUpFrontStatModifier", "Str", "1", "", typeof(InPairUpFrontStatModifierEffect))]
        [TestCase("InPairUpBackAllyCombatStatModifier", "Hit", "1", "", typeof(InPairUpBackAllyCombatStatModifierEffect))]
        [TestCase("InPairUpBackAllyStatModifier", "Str", "1", "", typeof(InPairUpBackAllyStatModifierEffect))]
        public void Constructor_OptionalField_Effects(string effectName, string effectParam1, string effectParam2, string effectParam3, Type expectedType)
        {
            SkillsConfig config = new SkillsConfig()
            {
                Name = 0,
                Effects = new List<SkillEffectConfig>()
                {
                    new SkillEffectConfig()
                    {
                        Type = 1,
                        Parameters = new List<int>(){ 2, 3, 4 }
                    }
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                effectName,
                effectParam1,
                effectParam2,
                effectParam3
            };

            ISkill skill = new Skill(config, data);

            Assert.That(skill, Is.Not.Null);
            Assert.That(skill.Effects.Count, Is.EqualTo(1));

            var effect = skill.Effects.First();

            Assert.That(effect, Is.Not.Null);
            Assert.That(effect.GetType(), Is.EqualTo(expectedType));
        }

        #endregion OptionalField_Effects

        #region FlagAsMatched

        [Test]
        public void FlagAsMatched()
        {
            SkillsConfig config = new SkillsConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME
            };

            ISkill skill = new Skill(config, data);

            Assert.That(skill.Matched, Is.False);

            skill.FlagAsMatched();

            Assert.That(skill.Matched, Is.True);
        }

        #endregion FlagAsMatched

        #region BuildDictionary

        [Test]
        public void BuildDictionary_WithInput_Null()
        {
            IDictionary<string, ISkill> dict = Skill.BuildDictionary(null);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_NullQuery()
        {
            SkillsConfig config = new SkillsConfig()
            {
                Queries = null,
                Name = 0
            };

            IDictionary<string, ISkill> dict = Skill.BuildDictionary(config);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_EmptyQuery()
        {
            SkillsConfig config = new SkillsConfig()
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

            IDictionary<string, ISkill> dict = Skill.BuildDictionary(config);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_DuplicateName()
        {
            SkillsConfig config = new SkillsConfig()
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

            Assert.Throws<SkillProcessingException>(() => Skill.BuildDictionary(config));
        }

        [Test]
        public void BuildDictionary_WithInput_Invalid()
        {
            SkillsConfig config = new SkillsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_NAME, "NotAURL" }
                        }
                    }
                },
                Name = 0,
                SpriteURL = 1
            };

            Assert.Throws<SkillProcessingException>(() => Skill.BuildDictionary(config));
        }

        [Test]
        public void BuildDictionary()
        {
            SkillsConfig config = new SkillsConfig()
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

            IDictionary<string, ISkill> dict = Skill.BuildDictionary(config);
            Assert.That(dict.Count, Is.EqualTo(1));
        }

        [Test]
        public void BuildDictionary_MultiQuery()
        {
            SkillsConfig config = new SkillsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Skill 1" },
                            new List<object>(){ "Skill 2" }
                        }
                    },
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Skill 3" },
                            new List<object>(){ "Skill 4" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, ISkill> dict = Skill.BuildDictionary(config);
            Assert.That(dict.Count, Is.EqualTo(4));
        }

        #endregion BuildDictionary

        #region MatchNames

        [Test]
        public void MatchNames_UnmatchedName()
        {
            string skill1Name = "Skill 1";
            string skill2Name = "Skill 2";

            ISkill skill1 = Substitute.For<ISkill>();
            skill1.Name.Returns(skill1Name);

            IDictionary<string, ISkill> dict = new Dictionary<string, ISkill>();
            dict.Add(skill1Name, skill1);

            IEnumerable<string> names = new List<string>() { skill2Name };

            Assert.Throws<UnmatchedSkillException>(() => Skill.MatchNames(dict, names));
        }

        [Test]
        public void MatchNames_SingleMatch()
        {
            string skill1Name = "Skill 1";
            string skill2Name = "Skill 2";

            ISkill skill1 = Substitute.For<ISkill>();
            skill1.Name.Returns(skill1Name);

            ISkill skill2 = Substitute.For<ISkill>();
            skill2.Name.Returns(skill2Name);

            IDictionary<string, ISkill> dict = new Dictionary<string, ISkill>();
            dict.Add(skill1Name, skill1);
            dict.Add(skill2Name, skill2);

            IEnumerable<string> names = new List<string>() { skill1Name };
            List<ISkill> matches = Skill.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(1));
            Assert.That(matches.Contains(skill1), Is.True);
            matches.First().Received(1).FlagAsMatched();
        }

        [Test]
        public void MatchNames_MultipleMatches()
        {
            string skill1Name = "Skill 1";
            string skill2Name = "Skill 2";

            ISkill skill1 = Substitute.For<ISkill>();
            skill1.Name.Returns(skill1Name);

            ISkill skill2 = Substitute.For<ISkill>();
            skill2.Name.Returns(skill2Name);

            IDictionary<string, ISkill> dict = new Dictionary<string, ISkill>();
            dict.Add(skill1Name, skill1);
            dict.Add(skill2Name, skill2);

            IEnumerable<string> names = new List<string>() { skill1Name, skill2Name };
            List<ISkill> matches = Skill.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches.Contains(skill1), Is.True);
            Assert.That(matches.Contains(skill2), Is.True);

            matches[0].Received(1).FlagAsMatched();
            matches[1].Received(1).FlagAsMatched();
        }

        [Test]
        public void MatchNames_MultipleMatches_DoNotSetMatchedStatus()
        {
            string skill1Name = "Skill 1";
            string skill2Name = "Skill 2";

            ISkill skill1 = Substitute.For<ISkill>();
            skill1.Name.Returns(skill1Name);

            ISkill skill2 = Substitute.For<ISkill>();
            skill2.Name.Returns(skill2Name);

            IDictionary<string, ISkill> dict = new Dictionary<string, ISkill>();
            dict.Add(skill1Name, skill1);
            dict.Add(skill2Name, skill2);

            IEnumerable<string> names = new List<string>() { skill1Name, skill2Name };
            List<ISkill> matches = Skill.MatchNames(dict, names, false);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches.Contains(skill1), Is.True);
            Assert.That(matches.Contains(skill2), Is.True);

            matches[0].DidNotReceive().FlagAsMatched();
            matches[1].DidNotReceive().FlagAsMatched();
        }

        #endregion MatchNames

        #region MatchName

        [Test]
        public void MatchName_UnmatchedName()
        {
            string skill1Name = "Skill 1";

            ISkill skill1 = Substitute.For<ISkill>();
            skill1.Name.Returns(skill1Name);

            IDictionary<string, ISkill> dict = new Dictionary<string, ISkill>();
            dict.Add(skill1Name, skill1);

            string name = "Skill 2";

            Assert.Throws<UnmatchedSkillException>(() => Skill.MatchName(dict, name));
        }

        [Test]
        public void MatchName()
        {
            string skill1Name = "Skill 1";

            ISkill skill1 = Substitute.For<ISkill>();
            skill1.Name.Returns(skill1Name);

            IDictionary<string, ISkill> dict = new Dictionary<string, ISkill>();
            dict.Add(skill1Name, skill1);

            ISkill match = Skill.MatchName(dict, skill1Name);

            Assert.That(match, Is.Not.Null);
            Assert.That(match, Is.EqualTo(skill1));
            match.Received(1).FlagAsMatched();
        }

        [Test]
        public void MatchName_DoNotSetMatchedStatus()
        {
            string skill1Name = "Skill 1";

            ISkill skill1 = Substitute.For<ISkill>();
            skill1.Name.Returns(skill1Name);

            IDictionary<string, ISkill> dict = new Dictionary<string, ISkill>();
            dict.Add(skill1Name, skill1);

            ISkill match = Skill.MatchName(dict, skill1Name, false);

            Assert.That(match, Is.Not.Null);
            Assert.That(match, Is.EqualTo(skill1));
            match.DidNotReceive().FlagAsMatched();
        }

        #endregion MatchName
    }
}
