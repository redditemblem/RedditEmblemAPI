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
        public int MaxPathCostBeforeUse { get; }
        public bool DealsDamage { get; }
        public bool AllowMeleeRange { get; }

        public UnitItemRange(decimal minRange, decimal maxRange, ItemRangeShape shape, int maxPathCostBeforeUse, bool dealsDamage, bool allowMeleeRange)
        {
            MinRange = (int)decimal.Floor(minRange);
            MaxRange = (int)decimal.Floor(maxRange);
            Shape = shape;
            MaxPathCostBeforeUse = maxPathCostBeforeUse;
            DealsDamage = dealsDamage;
            AllowMeleeRange = allowMeleeRange;
        }
    }
}
