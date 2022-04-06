using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Items;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System
{
    /// <summary>
    /// Object representing an <c>Item</c>'s range.
    /// </summary>
    public class ItemRange
    {
        /// <summary>
        /// The minimum number of tiles an item can reach.
        /// </summary>
        public int Minimum { get; set; }

        /// <summary>
        /// The maximum number of tiles an item can reach.
        /// </summary>
        public int Maximum { get; set; }

        /// <summary>
        /// The shape of the item's range
        /// </summary>
        public ItemRangeShape Shape { get; private set; }

        /// <summary>
        /// Flag that indicates when an item can only be used before a unit has moved.
        /// </summary>
        [JsonIgnore]
        public bool CanOnlyUseBeforeMovement { get; private set; }

        /// <summary>
        /// Initializes the class with the passed in values.
        /// </summary>
        /// <exception cref="PositiveIntegerException"></exception>
        /// <exception cref="MinimumGreaterThanMaximumException"></exception>
        public ItemRange(int minimum, int maximum, string shape, bool canOnlyUseBeforeMovement)
        {
            if (minimum < 0)
                throw new PositiveIntegerException("Minimum Range", minimum.ToString());
            if (maximum < 0)
                throw new PositiveIntegerException("Maximum Range", maximum.ToString());
            if (minimum > maximum)
                throw new MinimumGreaterThanMaximumException("Minimum Range", "Maximum Range");
            if (maximum > 15 && maximum != 99)
                throw new ItemRangeMaximumTooLargeException(15);

            this.Minimum = minimum;
            this.Maximum = maximum;
            this.Shape = GetItemRangeShape(shape);
            this.CanOnlyUseBeforeMovement = canOnlyUseBeforeMovement;
        }


        /// <summary>
        /// Initializes the class with the passed in <paramref name="minimum"/> and <paramref name="maximum"/> values.
        /// </summary>
        /// <param name="minimum">A numerical string value.</param>
        /// <param name="maximum">A numerical string value.</param>
        /// <exception cref="MinimumGreaterThanMaximumException"></exception>
        public ItemRange(RangeConfig config, IList<string> data)
            : this(DataParser.Int_Positive(data, config.Minimum, "Minimum Range"),
                   DataParser.Int_Positive(data, config.Maximum, "Maximum Range"),
                   DataParser.OptionalString(data, config.Shape, "Range Shape"),
                   DataParser.OptionalBoolean_YesNo(data, config.CanOnlyUseBeforeMovement, "Can Only Use Before Movement"))
        { }

        private ItemRangeShape GetItemRangeShape(string shape)
        {
            if (string.IsNullOrEmpty(shape))
                return ItemRangeShape.Standard;

            object shapeEnum;
            if (!Enum.TryParse(typeof(ItemRangeShape), shape, out shapeEnum))
                throw new UnmatchedItemRangeShapeException(shape);

            return (ItemRangeShape)shapeEnum;
        }
    }

    public enum ItemRangeShape
    {
        Standard = 0,
        Square = 1,
        Cross = 2,
        Saltire = 3,
        Star = 4
    }
}