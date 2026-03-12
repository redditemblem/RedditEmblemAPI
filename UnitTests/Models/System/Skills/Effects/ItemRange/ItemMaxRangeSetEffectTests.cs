using NSubstitute;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Models.Output.System.Skills.Effects;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Models.System.Skills.Effects.ItemRange
{
    public class ItemMaxRangeSetEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            IEnumerable<string> parameters = new List<string>();

            Assert.Throws<SkillEffectMissingParameterException>(() => new ItemMaxRangeSetEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new ItemMaxRangeSetEffect(parameters));
        }

        [Test]
        public void Constructor_2EmptyStrings()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.Throws<NonZeroPositiveIntegerException>(() => new ItemMaxRangeSetEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyCategories()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, "1" };

            Assert.Throws<RequiredValueNotProvidedException>(() => new ItemMaxRangeSetEffect(parameters));
        }

        [Test]
        public void Constructor()
        {
            string category = "Category";
            string value = "2";

            IEnumerable<string> parameters = new List<string>()
            {
                category,
                value
            };

            ItemMaxRangeSetEffect effect = new ItemMaxRangeSetEffect(parameters);

            Assert.That(effect.Categories, Is.EqualTo(new List<string>() { category }));
            Assert.That(effect.Value, Is.EqualTo(2));
            Assert.That(effect.ExecutionOrder, Is.EqualTo(SkillEffectExecutionOrder.AfterFinalStatCalculations));
        }

        #endregion Constructor

        #region Apply

        [TestCase("Bow", 1, 0)]
        [TestCase("Sword", 99, 0)]
        [TestCase("Sword", 6, 0)]
        [TestCase("Sword", 5, 0)]
        [TestCase("Sword", 4, 1)]
        [TestCase("Sword", 3, 2)]
        public void Apply(string category, int maxRangeBaseValue, int expected)
        {
            IUnit unit = Substitute.For<IUnit>();
            ISkill skill = Substitute.For<ISkill>();
            IMapObj map = Substitute.For<IMapObj>();
            List<IUnit> units = new List<IUnit>() { unit };

            IUnitInventoryItem item = Substitute.For<IUnitInventoryItem>();
            item.Item.Category.Returns(category);
            item.MaxRange.BaseValue.Returns(maxRangeBaseValue);
            item.MaxRange.ForcedModifier.Returns(0);

            unit.Inventory.GetAllItems().Returns(new List<IUnitInventoryItem>() { item });

            IEnumerable<string> parameters = new List<string>() { "Sword", "5" };
            ItemMaxRangeSetEffect effect = new ItemMaxRangeSetEffect(parameters);

            effect.Apply(unit, skill, map, units);

            Assert.That(item.MaxRange.ForcedModifier, Is.EqualTo(expected));
        }

        #endregion Apply
    }
}
