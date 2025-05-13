using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Items;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public int Minimum { get; private set; }

        /// <summary>
        /// Unparsed, raw data for the minimum number of tiles an item can reach.
        /// </summary>
        [JsonIgnore]
        public string MinimumRaw { get; private set; }

        /// <summary>
        /// Flag indicating whether or not the Minimum range requires calculation.
        /// </summary>
        public bool MinimumRequiresCalculation { get; private set; }

        /// <summary>
        /// The maximum number of tiles an item can reach.
        /// </summary>
        public int Maximum { get; private set; }

        /// <summary>
        /// Unparsed, raw cell value for the maximum number of tiles an item can reach.
        /// </summary>
        [JsonIgnore]
        public string MaximumRaw { get; private set; }

        /// <summary>
        /// Flag indicating whether or not the Maximum range requires calculation.
        /// </summary>
        public bool MaximumRequiresCalculation { get; private set; }

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
        /// Initializes the class with the passed in <paramref name="minimum"/> and <paramref name="maximum"/> values.
        /// </summary>
        /// <param name="minimum">A numerical string value.</param>
        /// <param name="maximum">A numerical string value.</param>
        /// <exception cref="MinimumGreaterThanMaximumException"></exception>
        public ItemRange(ItemRangeConfig config, IEnumerable<string> data)
        {
            this.Minimum = RangeValueHandler_Minimum(data, config.Minimum);
            this.Maximum = RangeValueHandler_Maximum(data, config.Maximum);

            if (this.Minimum == 0 && this.Maximum > 0 && !this.MinimumRequiresCalculation)
                throw new ItemRangeMinimumNotSetException("Minimum Range", "Maximum Range");
            if (this.Minimum > this.Maximum && !this.MaximumRequiresCalculation)
                throw new MinimumGreaterThanMaximumException("Minimum Range", "Maximum Range");

            this.Shape = GetItemRangeShape(DataParser.OptionalString(data, config.Shape, "Range Shape"));
            this.CanOnlyUseBeforeMovement = DataParser.OptionalBoolean_YesNo(data, config.CanOnlyUseBeforeMovement, "Can Only Use Before Movement");
        }

        private int RangeValueHandler_Minimum(IEnumerable<string> data, int index)
        {
            try
            {
                this.MinimumRaw = data.ElementAtOrDefault<string>(index) ?? string.Empty;
                return DataParser.OptionalInt_Positive(data, index, "Minimum Range");
            }
            catch (PositiveIntegerException)
            {
                //Check if this value needs to be calculated. If yes, return 0. If no, throw the error again.
                if (this.MinimumRaw.Contains("{") || this.MinimumRaw.Contains("}"))
                {
                    this.MinimumRequiresCalculation = true;
                    return 0;
                }

                throw;
            }
        }

        private int RangeValueHandler_Maximum(IEnumerable<string> data, int index)
        {
            try
            {
                this.MaximumRaw = data.ElementAtOrDefault<string>(index) ?? string.Empty;
                return DataParser.OptionalInt_Positive(data, index, "Maximum Range");
            }
            catch (PositiveIntegerException)
            {
                //Check if this value needs to be calculated. If yes, return 0. If no, throw the error again.
                if (this.MaximumRaw.Contains("{") || this.MaximumRaw.Contains("}"))
                {
                    this.MaximumRequiresCalculation = true;
                    return 0;
                }

                throw;
            }
        }

        private ItemRangeShape GetItemRangeShape(string shape)
        {
            if (string.IsNullOrEmpty(shape))
                return ItemRangeShape.Standard;

            object shapeEnum;
            if (!Enum.TryParse(typeof(ItemRangeShape), shape, out shapeEnum))
                throw new UnmatchedItemRangeShapeException(shape, Enum.GetNames<ItemRangeShape>());

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