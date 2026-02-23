using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.System;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Helpers.Ranges
{
    public readonly struct ItemRangeParameters
    {
        public ICoordinate StartCoord { get; }
        public IEnumerable<ICoordinate> IgnoreTiles { get; }
        public IEnumerable<UnitItemRange> Ranges { get; }
        public int LargestRange { get; }
        public CompassDirection RangeDirection { get; }
        public int AffiliationGrouping { get; }

        public ItemRangeParameters(ICoordinate startCoord, IEnumerable<ICoordinate> ignoreTiles, IEnumerable<UnitItemRange> ranges, CompassDirection direction, int affiliationGrouping)
        {
            IgnoreTiles = ignoreTiles;
            StartCoord = startCoord;
            Ranges = ranges;
            LargestRange = Ranges.Select(r => r.Shape == ItemRangeShape.Square || r.Shape == ItemRangeShape.Saltire || r.Shape == ItemRangeShape.Star ? r.MaxRange * 2 : r.MaxRange).OrderByDescending(r => r).FirstOrDefault();
            RangeDirection = direction;
            AffiliationGrouping = affiliationGrouping;

            //Safeguard just in case. We shouldn't ever get a 99 range here.
            if (LargestRange >= 99)
                throw new ArgumentException("Safeguard reached. Attempting to calculate a 99 range when none should ever exist at this point.");
        }
    }
}
