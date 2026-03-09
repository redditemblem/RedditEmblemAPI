using NSubstitute;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Models.Output.System.Skills.Effects;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.EquippedItem;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Models.System.Skills.Effects.EquippedItem
{
    public class EquippedCategoryCombatStatModifierEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            IEnumerable<string> parameters = new List<string>();

            Assert.Throws<SkillEffectMissingParameterException>(() => new EquippedCategoryCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new EquippedCategoryCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_2EmptyString()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new EquippedCategoryCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_3EmptyString()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, string.Empty, string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new EquippedCategoryCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_NoCategories()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, "Stat1", "0" };

            Assert.Throws<RequiredValueNotProvidedException>(() => new EquippedCategoryCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_NoStats()
        {
            IEnumerable<string> parameters = new List<string>() { "Category", string.Empty, "0" };

            Assert.Throws<RequiredValueNotProvidedException>(() => new EquippedCategoryCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_NoValues()
        {
            IEnumerable<string> parameters = new List<string>() { "Category", "Stat1", string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new EquippedCategoryCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor()
        {
            string category = "Category";
            string stats = "Stat1";
            string values = "1";

            IEnumerable<string> parameters = new List<string>() { category, stats, values };

            EquippedCategoryCombatStatModifierEffect effect = new EquippedCategoryCombatStatModifierEffect(parameters);

            Assert.That(effect.Categories, Is.EqualTo(new List<string>() { category }));
            Assert.That(effect.Modifiers.ContainsKey(stats), Is.True);
            Assert.That(effect.Modifiers[stats], Is.EqualTo(1));
            Assert.That(effect.ExecutionOrder, Is.EqualTo(SkillEffectExecutionOrder.Standard));
        }

        #endregion Constructor

        #region Apply

        [Test]
        public void Apply_NoPrimaryEquippedItem()
        {
            IUnit unit = Substitute.For<IUnit>();
            ISkill skill = Substitute.For<ISkill>();
            IMapObj map = Substitute.For<IMapObj>();
            List<IUnit> units = new List<IUnit>() { unit };

            IUnitInventoryItem item = null;
            unit.Inventory.GetPrimaryEquippedItem().Returns(item);

            IEnumerable<string> parameters = new List<string>() { "Sword", "Stat1", "1" };
            EquippedCategoryCombatStatModifierEffect effect = new EquippedCategoryCombatStatModifierEffect(parameters);

            effect.Apply(unit, skill, map, units);

            unit.Stats.DidNotReceiveWithAnyArgs().ApplyCombatStatModifiers(Arg.Any<IDictionary<string, int>>(), skill.Name, true);
        }

        [Test]
        public void Apply_WrongItemCategory()
        {
            IUnit unit = Substitute.For<IUnit>();
            ISkill skill = Substitute.For<ISkill>();
            IMapObj map = Substitute.For<IMapObj>();
            List<IUnit> units = new List<IUnit>() { unit };

            IUnitInventoryItem item = Substitute.For<IUnitInventoryItem>();
            item.Item.Category.Returns("Bow");

            unit.Inventory.GetPrimaryEquippedItem().Returns(item);

            IEnumerable<string> parameters = new List<string>() { "Sword", "Stat1", "1" };
            EquippedCategoryCombatStatModifierEffect effect = new EquippedCategoryCombatStatModifierEffect(parameters);

            effect.Apply(unit, skill, map, units);

            unit.Stats.DidNotReceiveWithAnyArgs().ApplyCombatStatModifiers(Arg.Any<IDictionary<string, int>>(), skill.Name, true);
        }

        [Test]
        public void Apply()
        {
            IUnit unit = Substitute.For<IUnit>();
            ISkill skill = Substitute.For<ISkill>();
            IMapObj map = Substitute.For<IMapObj>();
            List<IUnit> units = new List<IUnit>() { unit };

            IUnitInventoryItem item = Substitute.For<IUnitInventoryItem>();
            item.Item.Category.Returns("Sword");

            unit.Inventory.GetPrimaryEquippedItem().Returns(item);

            IEnumerable<string> parameters = new List<string>() { "Sword", "Stat1", "1" };
            EquippedCategoryCombatStatModifierEffect effect = new EquippedCategoryCombatStatModifierEffect(parameters);

            effect.Apply(unit, skill, map, units);

            unit.Stats.Received(1).ApplyCombatStatModifiers(Arg.Is<IDictionary<string, int>>(m => m["Stat1"] == 1), skill.Name, true);
        }

        #endregion Apply
    }
}
