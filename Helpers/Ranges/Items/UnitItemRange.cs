using RedditEmblemAPI.Models.Output.System;

namespace RedditEmblemAPI.Helpers.Ranges.Items
{
    /// <summary>
    /// Represents the range parameters of a specific <c>UnitInventoryItem</c>.
    /// </summary>
    public readonly struct UnitItemRange
    {
        public int MinRange { get; }
        public int MaxRange { get; }
        public ItemRangeShape Shape { get; }
        public bool CanOnlyUseBeforeMovement { get; }
        public bool DealsDamage { get; }
        public bool AllowMeleeRange { get; }

        public UnitItemRange(decimal minRange, decimal maxRange, ItemRangeShape shape, bool canOnlyBeUsedBeforeMovement, bool dealsDamage, bool allowMeleeRange)
        {
            MinRange = (int)decimal.Floor(minRange);
            MaxRange = (int)decimal.Floor(maxRange);
            Shape = shape;
            CanOnlyUseBeforeMovement = canOnlyBeUsedBeforeMovement;
            DealsDamage = dealsDamage;
            AllowMeleeRange = allowMeleeRange;
        }
    }
}
