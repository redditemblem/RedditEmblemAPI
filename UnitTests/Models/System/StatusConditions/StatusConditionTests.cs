using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Statuses;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.StatusConditions;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;

namespace UnitTests.Models.System.StatusConditions
{
    public  class StatusConditionTests
    {
        #region Constants

        private const string INPUT_NAME = "Status Condition Test";

        #endregion Constants

        [Test]
        public void Constructor_RequiredFields_WithInputNull()
        {
            StatusConditionConfig config = new StatusConditionConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>();

            Assert.Throws<RequiredValueNotProvidedException>(() => new StatusCondition(config, data));
        }

        [Test]
        public void Constructor_RequiredFields()
        {
            StatusConditionConfig config = new StatusConditionConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME
            };

            IStatusCondition status = new StatusCondition(config, data);

            Assert.That(status.Name, Is.EqualTo(INPUT_NAME));
        }

        #region OptionalField_SpriteURL

        [Test]
        public void Constructor_OptionalField_SpriteURL_EmptyString()
        {
            StatusConditionConfig config = new StatusConditionConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            IStatusCondition status = new StatusCondition(config, data);

            Assert.That(status.SpriteURL, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_SpriteURL_InvalidURL()
        {
            StatusConditionConfig config = new StatusConditionConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "NotAURL"
            };

            Assert.Throws<URLException>(() => new StatusCondition(config, data));
        }

        [Test]
        public void Constructor_OptionalField_SpriteURL()
        {
            StatusConditionConfig config = new StatusConditionConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL
            };

            IStatusCondition status = new StatusCondition(config, data);

            Assert.That(status.SpriteURL, Is.EqualTo(UnitTestConsts.IMAGE_URL));
        }

        #endregion OptionalField_SpriteURL

        #region OptionalField_Type

        [Test]
        public void Constructor_OptionalField_Type_UnmatchedType()
        {
            StatusConditionConfig config = new StatusConditionConfig()
            {
                Name = 0,
                Type = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "fake"
            };

            Assert.Throws<UnmatchedStatusConditionTypeException>(() => new StatusCondition(config, data));
        }

        [TestCase("", StatusConditionType.Unassigned)]
        [TestCase("Positive", StatusConditionType.Positive)]
        [TestCase("Negative", StatusConditionType.Negative)]
        [TestCase("Neutral", StatusConditionType.Neutral)]
        public void Constructor_OptionalField_Type(string type, StatusConditionType expectedType)
        {
            StatusConditionConfig config = new StatusConditionConfig()
            {
                Name = 0,
                Type = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                type
            };

            IStatusCondition status = new StatusCondition(config, data);

            Assert.That(status.Type, Is.EqualTo(expectedType));
        }

        #endregion OptionalField_Type

        #region OptionalField_Turns

        [TestCase("", 0)]
        [TestCase("1", 1)]
        public void Constructor_OptionalField_Turns(string input, int expected)
        {
            StatusConditionConfig config = new StatusConditionConfig()
            {
                Name = 0,
                Turns = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                input
            };

            IStatusCondition status = new StatusCondition(config, data);

            Assert.That(status.Turns, Is.EqualTo(expected));
        }

        [TestCase("-1")]
        [TestCase("0")]
        public void Constructor_OptionalField_Turns_InvalidInputs(string turns)
        {
            StatusConditionConfig config = new StatusConditionConfig()
            {
                Name = 0,
                Turns = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                turns
            };

            Assert.Throws<NonZeroPositiveIntegerException>(() => new StatusCondition(config, data));
        }

        #endregion OptionalField_Turns

        #region OptionalField_TextFields

        [Test]
        public void Constructor_OptionalField_TextFields_EmptyString()
        {
            StatusConditionConfig config = new StatusConditionConfig()
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

            IStatusCondition status = new StatusCondition(config, data);

            List<string> expected = new List<string>();
            Assert.That(status.TextFields, Is.EqualTo(expected));
        }

        [Test]
        public void Constructor_OptionalField_TextFields()
        {
            string textField1 = "Text Field 1";
            string textField2 = "Text Field 2";

            StatusConditionConfig config = new StatusConditionConfig()
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

            IStatusCondition status = new StatusCondition(config, data);

            List<string> expected = new List<string>() { textField1, textField2 };
            Assert.That(status.TextFields, Is.EqualTo(expected));
        }

        #endregion OptionalField_TextFields

        #region OptionalField_Effects

        [Test]
        public void Constructor_OptionalField_Effects_EmptyList()
        {
            StatusConditionConfig config = new StatusConditionConfig()
            {
                Name = 0,
                Effects = new List<StatusConditionEffectConfig>()
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME
            };

            IStatusCondition status = new StatusCondition(config, data);

            Assert.That(status.Effects, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_Effects_EmptyStringEffectType()
        {
            StatusConditionConfig config = new StatusConditionConfig()
            {
                Name = 0,
                Effects = new List<StatusConditionEffectConfig>()
                {
                    new StatusConditionEffectConfig()
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

            IStatusCondition status = new StatusCondition(config, data);

            Assert.That(status.Effects, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_Effects_UnmatchedEffectType()
        {
            StatusConditionConfig config = new StatusConditionConfig()
            {
                Name = 0,
                Effects = new List<StatusConditionEffectConfig>()
                {
                    new StatusConditionEffectConfig()
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

            Assert.Throws<UnmatchedStatusConditionEffectException>(() => new StatusCondition(config, data));
        }

        [TestCase("OverrideMovement", "1", "", "", typeof(OverrideMovementEffect))]
        [TestCase("OverrideMovementType", "Mounted", "", "", typeof(OverrideMovementTypeEffect_Status))]
        [TestCase("TerrainTypeMovementCostSet", "1", "1", "No", typeof(TerrainTypeMovementCostSetEffect_Status))]
        [TestCase("DoesNotBlockEnemyAffiliations", "", "", "", typeof(DoesNotBlockEnemyAffiliationsEffect))]
        [TestCase("PreventAllItemUse", "", "", "", typeof(PreventAllItemUseEffect))]
        [TestCase("PreventUtilStatItemUse", "Str", "", "", typeof(PreventUtilStatItemUseEffect))]
        [TestCase("PreventCategoryItemUse", "Sword", "", "", typeof(PreventCategoryItemUseEffect))]
        [TestCase("CombatStatModifier", "Hit", "1", "", typeof(CombatStatModifierEffect))]
        [TestCase("CombatStatModifierWithAdditionalStatMultiplier", "Hit", "1", "Shields", typeof(CombatStatModifierWithAdditionalStatMultiplierEffect))]
        [TestCase("StatModifier", "Str", "1", "", typeof(StatModifierEffect))]
        [TestCase("StatModifierWithAdditionalStatMultiplier", "Str", "1", "Shields", typeof(StatModifierWithAdditionalStatMultiplierEffect))]
        [TestCase("AddTag", "Flying", "", "", typeof(AddTagEffect))]
        [TestCase("RemoveTag", "Armor", "", "", typeof(RemoveTagEffect))]
        public void Constructor_OptionalField_Effects(string effectName, string effectParam1, string effectParam2, string effectParam3, Type expectedType)
        {
            StatusConditionConfig config = new StatusConditionConfig()
            {
                Name = 0,
                Effects = new List<StatusConditionEffectConfig>()
                {
                    new StatusConditionEffectConfig()
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

            IStatusCondition status = new StatusCondition(config, data);

            Assert.That(status, Is.Not.Null);
            Assert.That(status.Effects.Count, Is.EqualTo(1));

            var effect = status.Effects.First();

            Assert.That(effect, Is.Not.Null);
            Assert.That(effect.GetType(), Is.EqualTo(expectedType));
        }

        #endregion OptionalField_Effects

        #region FlagAsMatched

        [Test]
        public void FlagAsMatched()
        {
            StatusConditionConfig config = new StatusConditionConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME
            };

            IStatusCondition status = new StatusCondition(config, data);

            Assert.That(status.Matched, Is.False);

            status.FlagAsMatched();

            Assert.That(status.Matched, Is.True);
        }

        #endregion FlagAsMatched

        #region BuildDictionary

        [Test]
        public void BuildDictionary_WithInput_Null()
        {
            IDictionary<string, IStatusCondition> dict = StatusCondition.BuildDictionary(null);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_NullQuery()
        {
            StatusConditionConfig config = new StatusConditionConfig()
            {
                Queries = null,
                Name = 0
            };

            IDictionary<string, IStatusCondition> dict = StatusCondition.BuildDictionary(config);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_EmptyQuery()
        {
            StatusConditionConfig config = new StatusConditionConfig()
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
                Name = 0
            };

            IDictionary<string, IStatusCondition> dict = StatusCondition.BuildDictionary(config);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_DuplicateName()
        {
            StatusConditionConfig config = new StatusConditionConfig()
            {
                Queries = new List<IQuery>()
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

            Assert.Throws<StatusConditionProcessingException>(() => StatusCondition.BuildDictionary(config));
        }

        [Test]
        public void BuildDictionary_WithInput_Invalid()
        {
            StatusConditionConfig config = new StatusConditionConfig()
            {
                Queries = new List<IQuery>()
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

            Assert.Throws<StatusConditionProcessingException>(() => StatusCondition.BuildDictionary(config));
        }

        [Test]
        public void BuildDictionary()
        {
            StatusConditionConfig config = new StatusConditionConfig()
            {
                Queries = new List<IQuery>()
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

            IDictionary<string, IStatusCondition> dict = StatusCondition.BuildDictionary(config);
            Assert.That(dict.Count, Is.EqualTo(1));
        }

        [Test]
        public void BuildDictionary_MultiQuery()
        {
            StatusConditionConfig config = new StatusConditionConfig()
            {
                Queries = new List<IQuery>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Status Condition 1" },
                            new List<object>(){ "Status Condition 2" }
                        }
                    },
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Status Condition 3" },
                            new List<object>(){ "Status Condition 4" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, IStatusCondition> dict = StatusCondition.BuildDictionary(config);
            Assert.That(dict.Count, Is.EqualTo(4));
        }

        #endregion BuildDictionary

        #region MatchNames

        [Test]
        public void MatchNames_UnmatchedName()
        {
            StatusConditionConfig config = new StatusConditionConfig()
            {
                Queries = new List<IQuery>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Status Condition 1" },
                            new List<object>(){ "Status Condition 2" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, IStatusCondition> dict = StatusCondition.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Status Condition 3" };

            Assert.Throws<UnmatchedStatusConditionException>(() => StatusCondition.MatchNames(dict, names));
        }

        [Test]
        public void MatchNames_SingleMatch()
        {
            StatusConditionConfig config = new StatusConditionConfig()
            {
                Queries = new List<IQuery>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Status Condition 1" },
                            new List<object>(){ "Status Condition 2" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, IStatusCondition> dict = StatusCondition.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Status Condition 1" };

            List<IStatusCondition> matches = StatusCondition.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(1));
            Assert.That(matches.First().Matched, Is.True);
        }

        [Test]
        public void MatchNames_MultipleMatches()
        {
            StatusConditionConfig config = new StatusConditionConfig()
            {
                Queries = new List<IQuery>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Status Condition 1" },
                            new List<object>(){ "Status Condition 2" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, IStatusCondition> dict = StatusCondition.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Status Condition 1", "Status Condition 2" };

            List<IStatusCondition> matches = StatusCondition.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches[0].Matched, Is.True);
            Assert.That(matches[1].Matched, Is.True);
        }

        [Test]
        public void MatchNames_MultipleMatches_DoNotSetMatchedStatus()
        {
            StatusConditionConfig config = new StatusConditionConfig()
            {
                Queries = new List<IQuery>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Status Condition 1" },
                            new List<object>(){ "Status Condition 2" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, IStatusCondition> dict = StatusCondition.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Status Condition 1", "Status Condition 2" };

            List<IStatusCondition> matches = StatusCondition.MatchNames(dict, names, false);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches[0].Matched, Is.False);
            Assert.That(matches[1].Matched, Is.False);
        }

        #endregion MatchNames
    }
}
