using NSubstitute;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Models.System.StatusConditions.Effects
{
    public class PreventCategoryItemUseEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            IEnumerable<string> parameters = new List<string>();

            Assert.Throws<StatusConditionEffectMissingParameterException>(() => new PreventCategoryItemUseEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new PreventCategoryItemUseEffect(parameters));
        }

        [Test]
        public void Constructor()
        {
            IEnumerable<string> parameters = new List<string>() { "Category 1, Category 2" };

            PreventCategoryItemUseEffect effect = new PreventCategoryItemUseEffect(parameters);

            Assert.That(effect.Categories, Is.EqualTo(new List<string> { "Category 1", "Category 2" }));
        }

        #endregion Constructor

        #region Apply

        [Test]
        public void Apply()
        {
            IUnit unit = Substitute.For<IUnit>();
            IUnitStatus status = Substitute.For<IUnitStatus>();
            IDictionary<string, ITag> tags = new Dictionary<string, ITag>();

            IUnitInventoryItem sword = Substitute.For<IUnitInventoryItem>();
            sword.Item.Category.Returns("Sword");
            sword.IsUsePrevented = false;

            IUnitInventoryItem bow = Substitute.For<IUnitInventoryItem>();
            bow.Item.Category.Returns("Bow");
            bow.IsUsePrevented = false;

            unit.Inventory.GetAllItems().Returns(new List<IUnitInventoryItem>() { sword, bow });

            IEnumerable<string> parameters = new List<string>() { "Sword" };
            PreventCategoryItemUseEffect effect = new PreventCategoryItemUseEffect(parameters);

            Assert.That(sword.IsUsePrevented, Is.False);
            Assert.That(bow.IsUsePrevented, Is.False);

            effect.Apply(unit, status, tags);

            Assert.That(sword.IsUsePrevented, Is.True);
            Assert.That(bow.IsUsePrevented, Is.False);
        }

        #endregion Apply
    }
}
