using RedditEmblemAPI.Helpers.Ranges;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Helpers.Ranges
{
    public class UnitItemRangeTests
    {
        [Test]
        public void Constructor()
        {
            decimal minRange = 2.5m;
            decimal maxRange = 3.5m;
            ItemRangeShape shape = ItemRangeShape.Square;
            bool canOnlyBeUsedBeforeMovement = true;
            bool dealsDamage = true;
            bool allowMeleeRange = true;

            UnitItemRange range = new UnitItemRange(minRange, maxRange, shape, canOnlyBeUsedBeforeMovement, dealsDamage, allowMeleeRange);

            Assert.That(range.MinRange, Is.EqualTo(2));
            Assert.That(range.MaxRange, Is.EqualTo(3));
            Assert.That(range.Shape, Is.EqualTo(shape));
            Assert.That(range.CanOnlyUseBeforeMovement, Is.EqualTo(canOnlyBeUsedBeforeMovement));
            Assert.That(range.DealsDamage, Is.EqualTo(dealsDamage));
            Assert.That(range.AllowMeleeRange, Is.EqualTo(allowMeleeRange));
        }
    }
}
