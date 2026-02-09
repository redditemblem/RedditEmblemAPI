using RedditEmblemAPI.Models.Configuration.System.Items;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    public class ItemRangeTests
    {
        #region Constants

        private const string ITEM_RANGE_VAL_1 = "1";

        private const string ITEM_RANGE_SHAPE_STANDARD = "Standard";
        private const string ITEM_RANGE_SHAPE_SQUARE = "Square";
        private const string ITEM_RANGE_SHAPE_CROSS = "Cross";
        private const string ITEM_RANGE_SHAPE_SALTIRE = "Saltire";
        private const string ITEM_RANGE_SHAPE_STAR = "Star";

        #endregion Constants

        [Test]
        public void Constructor_RequiredFields_Null()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            IEnumerable<string> data = new List<string>() { };

            IItemRange output = new ItemRange(config, data);

            Assert.That(output.Minimum, Is.EqualTo(0));
            Assert.That(output.Maximum, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_RequiredFields_WithInvalidMinRange()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            IEnumerable<string> data = new List<string>() { "-1", "0" };

            Assert.Throws<PositiveIntegerException>(() => new ItemRange(config, data));
        }

        [Test]
        public void Constructor_RequiredFields_WithInvalidMaxRange()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            IEnumerable<string> data = new List<string>() { "0", "-1" };

            Assert.Throws<PositiveIntegerException>(() => new ItemRange(config, data));
        }

        [Test]
        public void Constructor_RequiredFields_WithInvalidRangeSet()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            IEnumerable<string> data = new List<string>() { "2", "1" };

            Assert.Throws<MinimumGreaterThanMaximumException>(() => new ItemRange(config, data));
        }

        [Test]
        public void Constructor_RequiredFields()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            IEnumerable<string> data = new List<string>() { ITEM_RANGE_VAL_1, ITEM_RANGE_VAL_1 };

            IItemRange range = new ItemRange(config, data);

            Assert.That(range.Minimum, Is.EqualTo(1));
            Assert.That(range.Maximum, Is.EqualTo(1));
        }

        #region RequiredField_CalculatedMinMaxRanges

        [Test]
        public void Constructor_RequiredFields_CalculatedMinimumRange_NoVariables()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            IEnumerable<string> data = new List<string>() { "1 * 2", ITEM_RANGE_VAL_1 };

            Assert.Throws<PositiveIntegerException>(() => new ItemRange(config, data));
        }

        [Test]
        public void Constructor_RequiredFields_CalculatedMinimumRange()
        {
            string formula = "{UnitStat[Str]}*2";

            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            IEnumerable<string> data = new List<string>() { formula, ITEM_RANGE_VAL_1 };

            IItemRange range = new ItemRange(config, data);

            Assert.That(range.MinimumRequiresCalculation, Is.True);
            Assert.That(range.Minimum, Is.EqualTo(0));
            Assert.That(range.MinimumRaw, Is.EqualTo(formula));
        }

        [Test]
        public void Constructor_RequiredFields_CalculatedMaximumRange_NoVariables()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            IEnumerable<string> data = new List<string>() { ITEM_RANGE_VAL_1, "1 * 2" };

            Assert.Throws<PositiveIntegerException>(() => new ItemRange(config, data));
        }

        [Test]
        public void Constructor_RequiredFields_CalculatedMaximumRange()
        {
            string formula = "{UnitStat[Str]}*2";

            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            IEnumerable<string> data = new List<string>() { ITEM_RANGE_VAL_1, formula };

            IItemRange range = new ItemRange(config, data);

            Assert.That(range.MaximumRequiresCalculation, Is.True);
            Assert.That(range.Maximum, Is.EqualTo(0));
            Assert.That(range.MaximumRaw, Is.EqualTo(formula));
        }

        #endregion RequiredField_CalculatedMinMaxRanges

        #region OptionalField_Shape

        [Test]
        public void Constructor_OptionalField_Shape_EmptyString()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1,
                Shape = 2
            };

            IEnumerable<string> data = new List<string>() { ITEM_RANGE_VAL_1, ITEM_RANGE_VAL_1, string.Empty };

            IItemRange range = new ItemRange(config, data);

            Assert.That(range.Shape, Is.EqualTo(ItemRangeShape.Standard));
        }

        [Test]
        public void Constructor_OptionalField_Shape_InvalidValue()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1,
                Shape = 2
            };

            IEnumerable<string> data = new List<string>() { ITEM_RANGE_VAL_1, ITEM_RANGE_VAL_1, "NotAShape" };

            Assert.Throws<UnmatchedItemRangeShapeException>(() => new ItemRange(config, data));
        }

        [Test]
        public void Constructor_OptionalField_Shape_Standard()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1,
                Shape = 2
            };

            IEnumerable<string> data = new List<string>() { ITEM_RANGE_VAL_1, ITEM_RANGE_VAL_1, ITEM_RANGE_SHAPE_STANDARD };

            IItemRange range = new ItemRange(config, data);

            Assert.That(range.Shape, Is.EqualTo(ItemRangeShape.Standard));
        }

        [Test]
        public void Constructor_OptionalField_Shape_Standard_Lowercase()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1,
                Shape = 2
            };

            IEnumerable<string> data = new List<string>() { ITEM_RANGE_VAL_1, ITEM_RANGE_VAL_1, ITEM_RANGE_SHAPE_STANDARD.ToLower() };

            Assert.Throws<UnmatchedItemRangeShapeException>(() => new ItemRange(config, data));
        }

        [Test]
        public void Constructor_OptionalField_Shape_Standard_Uppercase()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1,
                Shape = 2
            };

            IEnumerable<string> data = new List<string>() { ITEM_RANGE_VAL_1, ITEM_RANGE_VAL_1, ITEM_RANGE_SHAPE_STANDARD.ToUpper() };

            Assert.Throws<UnmatchedItemRangeShapeException>(() => new ItemRange(config, data));
        }

        [Test]
        public void Constructor_OptionalField_Shape_Square()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1,
                Shape = 2
            };

            IEnumerable<string> data = new List<string>() { ITEM_RANGE_VAL_1, ITEM_RANGE_VAL_1, ITEM_RANGE_SHAPE_SQUARE };

            IItemRange range = new ItemRange(config, data);

            Assert.That(range.Shape, Is.EqualTo(ItemRangeShape.Square));
        }

        [Test]
        public void Constructor_OptionalField_Shape_Cross()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1,
                Shape = 2
            };

            IEnumerable<string> data = new List<string>() { ITEM_RANGE_VAL_1, ITEM_RANGE_VAL_1, ITEM_RANGE_SHAPE_CROSS };

            IItemRange range = new ItemRange(config, data);

            Assert.That(range.Shape, Is.EqualTo(ItemRangeShape.Cross));
        }

        [Test]
        public void Constructor_OptionalField_Shape_Saltire()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1,
                Shape = 2
            };

            IEnumerable<string> data = new List<string>() { ITEM_RANGE_VAL_1, ITEM_RANGE_VAL_1, ITEM_RANGE_SHAPE_SALTIRE };

            IItemRange range = new ItemRange(config, data);

            Assert.That(range.Shape, Is.EqualTo(ItemRangeShape.Saltire));
        }

        [Test]
        public void Constructor_OptionalField_Shape_Star()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1,
                Shape = 2
            };

            IEnumerable<string> data = new List<string>() { ITEM_RANGE_VAL_1, ITEM_RANGE_VAL_1, ITEM_RANGE_SHAPE_STAR };

            IItemRange range = new ItemRange(config, data);

            Assert.That(range.Shape, Is.EqualTo(ItemRangeShape.Star));
        }

        #endregion OptionalField_Shape

        #region OptionalField_CanOnlyUseBeforeMovement

        [Test]
        public void Constructor_OptionalField_CanOnlyUseBeforeMovement_EmptyString()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1,
                CanOnlyUseBeforeMovement = 2
            };

            IEnumerable<string> data = new List<string>() { ITEM_RANGE_VAL_1, ITEM_RANGE_VAL_1, string.Empty };

            IItemRange range = new ItemRange(config, data);

            Assert.That(range.CanOnlyUseBeforeMovement, Is.False);
        }

        [Test]
        public void Constructor_OptionalField_CanOnlyUseBeforeMovement_No()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1,
                CanOnlyUseBeforeMovement = 2
            };

            IEnumerable<string> data = new List<string>() { ITEM_RANGE_VAL_1, ITEM_RANGE_VAL_1, "No" };

            IItemRange range = new ItemRange(config, data);

            Assert.That(range.CanOnlyUseBeforeMovement, Is.False);
        }

        [Test]
        public void Constructor_OptionalField_CanOnlyUseBeforeMovement_Yes()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1,
                CanOnlyUseBeforeMovement = 2
            };

            IEnumerable<string> data = new List<string>() { ITEM_RANGE_VAL_1, ITEM_RANGE_VAL_1, "Yes" };

            IItemRange range = new ItemRange(config, data);

            Assert.That(range.CanOnlyUseBeforeMovement, Is.True);
        }

        #endregion OptionalField_CanOnlyUseBeforeMovement
    }
}
