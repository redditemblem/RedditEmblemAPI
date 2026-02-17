using RedditEmblemAPI.Models.Output;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Models.Units
{
    public class UnitInventoryItemStatTests
    {
        [Test]
        public void Constructor_Default()
        {
            IUnitInventoryItemStat stat = new UnitInventoryItemStat();

            Assert.That(stat, Is.Not.Null);
            Assert.That(stat.BaseValue, Is.Zero);
            Assert.That(stat.FinalValue, Is.Zero);
            Assert.That(stat.Modifiers, Is.Empty);
            Assert.That(stat.ForcedModifier, Is.Zero);
        }

        [Test]
        public void Constructor_BaseValue()
        {
            decimal baseValue = 5m;

            IUnitInventoryItemStat stat = new UnitInventoryItemStat(baseValue);

            Assert.That(stat, Is.Not.Null);
            Assert.That(stat.BaseValue, Is.EqualTo(baseValue));
            Assert.That(stat.FinalValue, Is.EqualTo(baseValue));
            Assert.That(stat.Modifiers, Is.Empty);
            Assert.That(stat.ForcedModifier, Is.Zero);
        }

        [Test]
        public void Constructor_BaseValue_InvertDisplayColors()
        {
            decimal baseValue = 5m;
            bool invertDisplayColors = false;

            IUnitInventoryItemStat stat = new UnitInventoryItemStat(baseValue, invertDisplayColors);

            Assert.That(stat, Is.Not.Null);
            Assert.That(stat.BaseValue, Is.EqualTo(baseValue));
            Assert.That(stat.FinalValue, Is.EqualTo(baseValue));
            Assert.That(stat.Modifiers, Is.Empty);
            Assert.That(stat.ForcedModifier, Is.Zero);
        }

        [Test]
        public void Constructor_NamedStatValue()
        {
            decimal baseValue = 5m;
            INamedStatValue namedStat = new NamedStatValue(baseValue);

            IUnitInventoryItemStat itemStat = new UnitInventoryItemStat(namedStat);

            Assert.That(itemStat, Is.Not.Null);
            Assert.That(itemStat.BaseValue, Is.EqualTo(baseValue));
            Assert.That(itemStat.FinalValue, Is.EqualTo(baseValue));
            Assert.That(itemStat.Modifiers, Is.Empty);
            Assert.That(itemStat.ForcedModifier, Is.Zero);
        }

        [Test]
        public void Calculation_Of_Final_Value()
        {
            decimal baseValue = 1m;
            IUnitInventoryItemStat stat = new UnitInventoryItemStat(baseValue);

            Assert.That(stat, Is.Not.Null);
            Assert.That(stat.BaseValue, Is.EqualTo(baseValue));
            Assert.That(stat.FinalValue, Is.EqualTo(baseValue));
            Assert.That(stat.Modifiers, Is.Empty);
            Assert.That(stat.ForcedModifier, Is.Zero);

            stat.Modifiers.Add("Stat 1", 1);
            stat.Modifiers.Add("Stat 2", 2);
            stat.Modifiers.Add("Stat 3", 3);

            Assert.That(stat.Modifiers.Count, Is.EqualTo(3));
            Assert.That(stat.FinalValue, Is.EqualTo(7));

            stat.ForcedModifier = 3;

            Assert.That(stat.Modifiers.Count, Is.EqualTo(3));
            Assert.That(stat.FinalValue, Is.EqualTo(4));
        }
    }
}
