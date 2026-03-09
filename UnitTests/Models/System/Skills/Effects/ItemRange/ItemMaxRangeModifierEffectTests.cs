using NSubstitute;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Models.Output.System.Skills.Effects;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Models.System.Skills.Effects.ItemRange
{
    public class ItemMaxRangeModifierEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            IEnumerable<string> parameters = new List<string>();

            Assert.Throws<SkillEffectMissingParameterException>(() => new ItemMaxRangeModifierEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new ItemMaxRangeModifierEffect(parameters));
        }

        [Test]
        public void Constructor_2EmptyStrings()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new ItemMaxRangeModifierEffect(parameters));
        }

        [Test]
        public void Constructor_3EmptyStrings()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, string.Empty, string.Empty };

            Assert.Throws<NonZeroPositiveIntegerException>(() => new ItemMaxRangeModifierEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyCategory()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, "1", string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new ItemMaxRangeModifierEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyCategory_WithDealsDamageFilterType()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, "1", "All" };

            Assert.Throws<RequiredValueNotProvidedException>(() => new ItemMaxRangeModifierEffect(parameters));
        }

        [Test]
        public void Constructor_DealsDamageFilterType_UnknownValue()
        {
            IEnumerable<string> parameters = new List<string>() { "Category", "1", "Mismatch" };

            Assert.Throws<UnmatchedDealsDamageFilterTypeException>(() => new ItemMaxRangeModifierEffect(parameters));
        }

        [TestCase("", DealsDamageFilterType.All)]
        [TestCase("Attack", DealsDamageFilterType.Attack)]
        [TestCase("Utility", DealsDamageFilterType.Utility)]
        public void Constructor(string dealsDamageFilter, DealsDamageFilterType expected)
        {
            string category = "Category";
            string values = "2";

            IEnumerable<string> parameters = new List<string>()
            {
                category,
                values, 
                dealsDamageFilter
            };

            ItemMaxRangeModifierEffect effect = new ItemMaxRangeModifierEffect(parameters);

            Assert.That(effect.Categories, Is.EqualTo(new List<string>() { category }));
            Assert.That(effect.Value, Is.EqualTo(2));
            Assert.That(effect.DealsDamageFilter, Is.EqualTo(expected));
            Assert.That(effect.ExecutionOrder, Is.EqualTo(SkillEffectExecutionOrder.Standard));
        }

        #endregion Constructor

        #region Apply

        [TestCase("Bow", 1, "", true)]
        [TestCase("Sword", 99, "", true)]
        [TestCase("Sword", 1, "Attack", false)]
        [TestCase("Sword", 1, "Utility", true)]
        public void Apply_Failed(string category, int maxRangeBaseValue, string dealsDamageFilter, bool itemDealsDamage)
        {
            IUnit unit = Substitute.For<IUnit>();
            ISkill skill = Substitute.For<ISkill>();
            IMapObj map = Substitute.For<IMapObj>();
            List<IUnit> units = new List<IUnit>() { unit };

            IUnitInventoryItem item = Substitute.For<IUnitInventoryItem>();
            item.Item.Category.Returns(category);
            item.Item.DealsDamage.Returns(itemDealsDamage);
            item.MaxRange.BaseValue.Returns(maxRangeBaseValue);
            item.MaxRange.ForcedModifier.Returns(0);

            unit.Inventory.GetAllItems().Returns(new List<IUnitInventoryItem>() { item });

            IEnumerable<string> parameters = new List<string>() { "Sword", "2", dealsDamageFilter };
            ItemMaxRangeModifierEffect effect = new ItemMaxRangeModifierEffect(parameters);

            effect.Apply(unit, skill, map, units);

            Assert.That(item.MaxRange.ForcedModifier, Is.EqualTo(0));
        }

        [TestCase("", true)]
        [TestCase("", false)]
        [TestCase("Attack", true)]
        [TestCase("Utility", false)]
        public void Apply_Successful(string dealsDamageFilter, bool itemDealsDamage)
        {
            string category = "Sword";
            string values = "2";

            IUnit unit = Substitute.For<IUnit>();
            ISkill skill = Substitute.For<ISkill>();
            IMapObj map = Substitute.For<IMapObj>();
            List<IUnit> units = new List<IUnit>() { unit };

            IUnitInventoryItem item = Substitute.For<IUnitInventoryItem>();
            item.Item.Category.Returns(category);
            item.Item.DealsDamage.Returns(itemDealsDamage);
            item.MaxRange.BaseValue.Returns(1);
            item.MaxRange.ForcedModifier.Returns(0);

            unit.Inventory.GetAllItems().Returns(new List<IUnitInventoryItem>() { item });

            IEnumerable<string> parameters = new List<string>() { category, values, dealsDamageFilter };
            ItemMaxRangeModifierEffect effect = new ItemMaxRangeModifierEffect(parameters);

            effect.Apply(unit, skill, map, units);

            Assert.That(item.MaxRange.ForcedModifier, Is.EqualTo(2));
        }

        #endregion Apply
    }
}
