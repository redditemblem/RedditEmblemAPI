using NSubstitute;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Models.Output.System.Skills.Effects;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Models.System.Skills.Effects.ItemRange
{
    public class ItemAllowMeleeRangeEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            IEnumerable<string> parameters = new List<string>();

            Assert.Throws<SkillEffectMissingParameterException>(() => new ItemAllowMeleeRangeEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new ItemAllowMeleeRangeEffect(parameters));
        }

        [Test]
        public void Constructor()
        {
            string category = "Category";
            IEnumerable<string> parameters = new List<string>() { category };

            ItemAllowMeleeRangeEffect effect = new ItemAllowMeleeRangeEffect(parameters);

            Assert.That(effect.Categories, Is.EqualTo(new List<string>() { category }));
            Assert.That(effect.ExecutionOrder, Is.EqualTo(SkillEffectExecutionOrder.Standard));
        }

        #endregion Constructor

        #region Apply

        [TestCase("Bow", 1, false, false)]
        [TestCase("Sword", 0, false, false)]
        [TestCase("Sword", 1, false, true)]
        [TestCase("Sword", 0, true, true)]
        public void Apply(string category, int minRangeBaseValue, bool minimumRequiresCalculation, bool expected)
        {
            IUnit unit = Substitute.For<IUnit>();
            ISkill skill = Substitute.For<ISkill>();
            IMapObj map = Substitute.For<IMapObj>();
            List<IUnit> units = new List<IUnit>() { unit };

            IUnitInventoryItem item = Substitute.For<IUnitInventoryItem>();
            item.Item.Category.Returns(category);
            item.MinRange.BaseValue.Returns(minRangeBaseValue);
            item.Item.Range.MinimumRequiresCalculation.Returns(minimumRequiresCalculation);
            item.AllowMeleeRange = false;

            unit.Inventory.GetAllItems().Returns(new List<IUnitInventoryItem>() { item });

            IEnumerable<string> parameters = new List<string>() { "Sword" };
            ItemAllowMeleeRangeEffect effect = new ItemAllowMeleeRangeEffect(parameters);

            effect.Apply(unit, skill, map, units);

            Assert.That(item.AllowMeleeRange, Is.EqualTo(expected));
        }

        #endregion Apply
    }
}
