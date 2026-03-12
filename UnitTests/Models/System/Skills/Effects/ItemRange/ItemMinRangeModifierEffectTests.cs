using NSubstitute;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Models.Output.System.Skills.Effects;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Models.System.Skills.Effects.ItemRange
{
    public class ItemMinRangeModifierEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            IEnumerable<string> parameters = new List<string>();

            Assert.Throws<SkillEffectMissingParameterException>(() => new ItemMinRangeModifierEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new ItemMinRangeModifierEffect(parameters));
        }

        [Test]
        public void Constructor_2EmptyStrings()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.Throws<NegativeIntegerException>(() => new ItemMinRangeModifierEffect(parameters));
        }

        [Test]
        public void Constructor_Value_1()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, "1" };

            Assert.Throws<NegativeIntegerException>(() => new ItemMinRangeModifierEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyCategories()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, "-1" };

            Assert.Throws<RequiredValueNotProvidedException>(() => new ItemMinRangeModifierEffect(parameters));
        }

        [Test]
        public void Constructor()
        {
            string category = "Category";
            string value = "-2";

            IEnumerable<string> parameters = new List<string>()
            {
                category,
                value
            };

            ItemMinRangeModifierEffect effect = new ItemMinRangeModifierEffect(parameters);

            Assert.That(effect.Categories, Is.EqualTo(new List<string>() { category }));
            Assert.That(effect.Value, Is.EqualTo(-2));
            Assert.That(effect.ExecutionOrder, Is.EqualTo(SkillEffectExecutionOrder.AfterFinalStatCalculations));
        }

        #endregion Constructor

        #region Apply

        [TestCase("Bow", 1, 0)]
        [TestCase("Sword", 0, 0)]
        [TestCase("Sword", 1, 0)]
        [TestCase("Sword", 2, -1)]
        [TestCase("Sword", 3, -1)]
        public void Apply(string category, int minRangeBaseValue, int expected)
        {
            IUnit unit = Substitute.For<IUnit>();
            ISkill skill = Substitute.For<ISkill>();
            IMapObj map = Substitute.For<IMapObj>();
            List<IUnit> units = new List<IUnit>() { unit };

            IUnitInventoryItem item = Substitute.For<IUnitInventoryItem>();
            item.Item.Category.Returns(category);
            item.MinRange.BaseValue.Returns(minRangeBaseValue);
            item.MinRange.ForcedModifier.Returns(0);

            unit.Inventory.GetAllItems().Returns(new List<IUnitInventoryItem>() { item });

            IEnumerable<string> parameters = new List<string>() { "Sword", "-1" };
            ItemMinRangeModifierEffect effect = new ItemMinRangeModifierEffect(parameters);

            effect.Apply(unit, skill, map, units);

            Assert.That(item.MinRange.ForcedModifier, Is.EqualTo(expected));
        }

        #endregion Apply
    }
}
