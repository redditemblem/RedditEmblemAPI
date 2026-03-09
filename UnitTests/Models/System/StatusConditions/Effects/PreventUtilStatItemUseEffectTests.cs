using NSubstitute;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Models.System.StatusConditions.Effects
{
    public class PreventUtilStatItemUseEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            IEnumerable<string> parameters = new List<string>();

            Assert.Throws<StatusConditionEffectMissingParameterException>(() => new PreventUtilStatItemUseEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new PreventUtilStatItemUseEffect(parameters));
        }

        [Test]
        public void Constructor()
        {
            IEnumerable<string> parameters = new List<string>() { "Str,Mag" };

            PreventUtilStatItemUseEffect effect = new PreventUtilStatItemUseEffect(parameters);

            Assert.That(effect.UtilizedStats, Is.EqualTo(new List<string>() { "Str", "Mag" }));
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
            sword.Item.UtilizedStats.Returns(new List<string>() { "Str" });
            sword.IsUsePrevented = false;

            IUnitInventoryItem staff = Substitute.For<IUnitInventoryItem>();
            staff.Item.UtilizedStats.Returns(new List<string>() { "Mag" });
            staff.IsUsePrevented = false;

            unit.Inventory.GetAllItems().Returns(new List<IUnitInventoryItem>() { sword, staff });

            IEnumerable<string> parameters = new List<string>() { "Str" };
            PreventUtilStatItemUseEffect effect = new PreventUtilStatItemUseEffect(parameters);

            Assert.That(sword.IsUsePrevented, Is.False);
            Assert.That(staff.IsUsePrevented, Is.False);

            effect.Apply(unit, status, tags);

            Assert.That(sword.IsUsePrevented, Is.True);
            Assert.That(staff.IsUsePrevented, Is.False);
        }

        #endregion Apply
    }
}
