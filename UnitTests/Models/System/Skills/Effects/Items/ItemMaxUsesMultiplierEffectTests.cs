using NSubstitute;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Models.Output.System.Skills.Effects;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Models.System.Skills.Effects.Items
{
    public class ItemMaxUsesMultiplierEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            IEnumerable<string> parameters = new List<string>();

            Assert.Throws<SkillEffectMissingParameterException>(() => new ItemMaxUsesMultiplierEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new ItemMaxUsesMultiplierEffect(parameters));
        }

        [Test]
        public void Constructor_2EmptyStrings()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.Throws<OneOrGreaterDecimalException>(() => new ItemMaxUsesMultiplierEffect(parameters));
        }

        [Test]
        public void Constructor_Multiplier_0_5()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, "0.5" };

            Assert.Throws<OneOrGreaterDecimalException>(() => new ItemMaxUsesMultiplierEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyCategory()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, "1" };

            Assert.Throws<RequiredValueNotProvidedException>(() => new ItemMaxUsesMultiplierEffect(parameters));
        }

        [Test]
        public void Constructor()
        {
            string category = "Category";
            string multiplier = "1.5";

            IEnumerable<string> parameters = new List<string>()
            {
                category,
                multiplier
            };

            ItemMaxUsesMultiplierEffect effect = new ItemMaxUsesMultiplierEffect(parameters);

            Assert.That(effect.Categories, Is.EqualTo(new List<string>() { category }));
            Assert.That(effect.Multiplier, Is.EqualTo(1.5m));
            Assert.That(effect.ExecutionOrder, Is.EqualTo(SkillEffectExecutionOrder.Standard));
        }

        #endregion Constructor

        #region Apply

        [Test]
        public void Apply()
        {
            string category = "Sword";
            string multiplier = "2";

            IUnit unit = Substitute.For<IUnit>();
            ISkill skill = Substitute.For<ISkill>();
            IMapObj map = Substitute.For<IMapObj>();
            List<IUnit> units = new List<IUnit>() { unit };

            IUnitInventoryItem unmatchedCategory = Substitute.For<IUnitInventoryItem>();
            unmatchedCategory.Item.Category.Returns("Bow");
            unmatchedCategory.MaxUses.Returns(5);

            IUnitInventoryItem infiniteUses = Substitute.For<IUnitInventoryItem>();
            infiniteUses.Item.Category.Returns(category);
            infiniteUses.MaxUses.Returns(0);

            IUnitInventoryItem valid = Substitute.For<IUnitInventoryItem>();
            valid.Item.Category.Returns(category);
            valid.MaxUses.Returns(5);

            unit.Inventory.GetAllItems().Returns(new List<IUnitInventoryItem>() { unmatchedCategory, infiniteUses, valid });

            IEnumerable<string> parameters = new List<string>(){ category, multiplier };
            ItemMaxUsesMultiplierEffect effect = new ItemMaxUsesMultiplierEffect(parameters);

            effect.Apply(unit, skill, map, units);

            Assert.That(unmatchedCategory.MaxUses, Is.EqualTo(5));
            Assert.That(infiniteUses.MaxUses, Is.EqualTo(0));
            Assert.That(valid.MaxUses, Is.EqualTo(10));
        }

        #endregion Apply
    }
}