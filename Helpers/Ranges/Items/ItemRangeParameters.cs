using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.System;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Helpers.Ranges.Items
{
    public readonly struct ItemRangeParameters
    {
        public ICoordinate StartingCoordinate { get; }
        public IEnumerable<ICoordinate> IgnoreTiles { get; }
        public IEnumerable<UnitItemRange> Ranges { get; }
        public IDictionary<ItemRangeShape, bool> ContainsRangeShape { get; }
        public int LargestRange { get; }
        public OrdinalDirection RangeDirection { get; }
        public int AffiliationGrouping { get; }

        public ItemRangeParameters(ICoordinate startCoord, IEnumerable<ICoordinate> ignoreTiles, IEnumerable<UnitItemRange> ranges, OrdinalDirection direction, int affiliationGrouping)
        {
            StartingCoordinate = startCoord;
            IgnoreTiles = ignoreTiles;
            Ranges = ranges;
            ContainsRangeShape = BuildContainsRangeShapeDictionary();
            LargestRange = Ranges.Select(r => r.Shape == ItemRangeShape.Square || r.Shape == ItemRangeShape.Saltire || r.Shape == ItemRangeShape.Star ? r.MaxRange * 2 : r.MaxRange).OrderByDescending(r => r).FirstOrDefault();
            RangeDirection = direction;
            AffiliationGrouping = affiliationGrouping;

            //Safeguard just in case. We shouldn't ever get a 99 range here.
            if (LargestRange >= 99)
                throw new ArgumentException("Safeguard reached. Attempting to calculate a 99 range when none should ever exist at this point.");
        }

        private IDictionary<ItemRangeShape, bool> BuildContainsRangeShapeDictionary()
        {
            IDictionary<ItemRangeShape, bool> shapes = new Dictionary<ItemRangeShape, bool>();

            shapes.Add(ItemRangeShape.Standard, this.Ranges.Any(r => r.Shape == ItemRangeShape.Standard));
            shapes.Add(ItemRangeShape.Square, this.Ranges.Any(r => r.Shape == ItemRangeShape.Square));
            shapes.Add(ItemRangeShape.Cross, this.Ranges.Any(r => r.Shape == ItemRangeShape.Cross));
            shapes.Add(ItemRangeShape.Saltire, this.Ranges.Any(r => r.Shape == ItemRangeShape.Saltire));
            shapes.Add(ItemRangeShape.Star, this.Ranges.Any(r => r.Shape == ItemRangeShape.Star));

            return shapes;
        }
    }
}
