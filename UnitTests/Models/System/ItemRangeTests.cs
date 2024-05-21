using RedditEmblemAPI.Models.Configuration.System.Items;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    [TestClass]
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

        [TestMethod]
        public void ItemRangeConstructor_RequiredFields_Null()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            { 
                Minimum = 0,
                Maximum = 1
            };

            List<string> data = new List<string>() { };

            ItemRange output = new ItemRange(config, data);
            Assert.AreEqual<int>(0, output.Minimum);
            Assert.AreEqual<int>(0, output.Maximum);
        }

        [TestMethod]
        public void ItemRangeConstructor_RequiredFields_WithInvalidMinRange()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            List<string> data = new List<string>() { "-1", "0" };

            Assert.ThrowsException<PositiveIntegerException>(() => new ItemRange(config, data));
        }

        [TestMethod]
        public void ItemRangeConstructor_RequiredFields_WithInvalidMaxRange()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            List<string> data = new List<string>() { "0", "-1" };

            Assert.ThrowsException<PositiveIntegerException>(() => new ItemRange(config, data));
        }

        [TestMethod]
        public void ItemRangeConstructor_RequiredFields_WithInvalidRangeSet()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            List<string> data = new List<string>() { "2", "1" };

            Assert.ThrowsException<MinimumGreaterThanMaximumException>(() => new ItemRange(config, data));
        }

        [TestMethod]
        public void ItemRangeConstructor_RequiredFields()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            List<string> data = new List<string>() { ITEM_RANGE_VAL_1, ITEM_RANGE_VAL_1 };

            ItemRange range = new ItemRange(config, data);
            Assert.AreEqual<int>(1, range.Minimum);
            Assert.AreEqual<int>(1, range.Maximum);
        }

        #region RequiredField_CalculatedMinMaxRanges

        [TestMethod]
        public void ItemRangeConstructor_RequiredFields_CalculatedMinimumRange_NoVariables()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            List<string> data = new List<string>() { "1 * 2", ITEM_RANGE_VAL_1 };

            Assert.ThrowsException<PositiveIntegerException>(() => new ItemRange(config, data));
        }

        [TestMethod]
        public void ItemRangeConstructor_RequiredFields_CalculatedMinimumRange()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            string formula = "{UnitStat[Str]}*2";

            List<string> data = new List<string>() { formula, ITEM_RANGE_VAL_1 };

            ItemRange range = new ItemRange(config, data);
            Assert.IsTrue(range.MinimumRequiresCalculation);
            Assert.AreEqual<int>(0, range.Minimum);
            Assert.AreEqual<string>(formula, range.MinimumRaw);
        }

        [TestMethod]
        public void ItemRangeConstructor_RequiredFields_CalculatedMaximumRange_NoVariables()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            List<string> data = new List<string>() { ITEM_RANGE_VAL_1, "1 * 2" };

            Assert.ThrowsException<PositiveIntegerException>(() => new ItemRange(config, data));
        }

        [TestMethod]
        public void ItemRangeConstructor_RequiredFields_CalculatedMaximumRange()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            string formula = "{UnitStat[Str]}*2";

            List<string> data = new List<string>() { ITEM_RANGE_VAL_1, formula };

            ItemRange range = new ItemRange(config, data);
            Assert.IsTrue(range.MaximumRequiresCalculation);
            Assert.AreEqual<int>(0, range.Maximum);
            Assert.AreEqual<string>(formula, range.MaximumRaw);
        }

        #endregion RequiredField_CalculatedMinMaxRanges

        #region OptionalField_Shape

        [TestMethod]
        public void ItemRangeConstructor_OptionalField_Shape_EmptyString()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1,
                Shape = 2
            };

            List<string> data = new List<string>() { ITEM_RANGE_VAL_1, ITEM_RANGE_VAL_1, string.Empty };

            ItemRange range = new ItemRange(config, data);
            Assert.AreEqual<ItemRangeShape>(ItemRangeShape.Standard, range.Shape);
        }

        [TestMethod]
        public void ItemRangeConstructor_OptionalField_Shape_InvalidValue()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1,
                Shape = 2
            };

            List<string> data = new List<string>() { ITEM_RANGE_VAL_1, ITEM_RANGE_VAL_1, "NotAShape" };

            Assert.ThrowsException<UnmatchedItemRangeShapeException>(() => new ItemRange(config, data));
        }

        [TestMethod]
        public void ItemRangeConstructor_OptionalField_Shape_Standard()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1,
                Shape = 2
            };

            List<string> data = new List<string>() { ITEM_RANGE_VAL_1, ITEM_RANGE_VAL_1, ITEM_RANGE_SHAPE_STANDARD };

            ItemRange range = new ItemRange(config, data);
            Assert.AreEqual<ItemRangeShape>(ItemRangeShape.Standard, range.Shape);
        }

        [TestMethod]
        public void ItemRangeConstructor_OptionalField_Shape_Standard_Lowercase()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1,
                Shape = 2
            };

            List<string> data = new List<string>() { ITEM_RANGE_VAL_1, ITEM_RANGE_VAL_1, ITEM_RANGE_SHAPE_STANDARD.ToLower() };

            Assert.ThrowsException<UnmatchedItemRangeShapeException>(() => new ItemRange(config, data));
        }

        [TestMethod]
        public void ItemRangeConstructor_OptionalField_Shape_Standard_Uppercase()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1,
                Shape = 2
            };

            List<string> data = new List<string>() { ITEM_RANGE_VAL_1, ITEM_RANGE_VAL_1, ITEM_RANGE_SHAPE_STANDARD.ToUpper() };

            Assert.ThrowsException<UnmatchedItemRangeShapeException>(() => new ItemRange(config, data));
        }

        [TestMethod]
        public void ItemRangeConstructor_OptionalField_Shape_Square()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1,
                Shape = 2
            };

            List<string> data = new List<string>() { ITEM_RANGE_VAL_1, ITEM_RANGE_VAL_1, ITEM_RANGE_SHAPE_SQUARE };

            ItemRange range = new ItemRange(config, data);
            Assert.AreEqual<ItemRangeShape>(ItemRangeShape.Square, range.Shape);
        }

        [TestMethod]
        public void ItemRangeConstructor_OptionalField_Shape_Cross()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1,
                Shape = 2
            };

            List<string> data = new List<string>() { ITEM_RANGE_VAL_1, ITEM_RANGE_VAL_1, ITEM_RANGE_SHAPE_CROSS };

            ItemRange range = new ItemRange(config, data);
            Assert.AreEqual<ItemRangeShape>(ItemRangeShape.Cross, range.Shape);
        }

        [TestMethod]
        public void ItemRangeConstructor_OptionalField_Shape_Saltire()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1,
                Shape = 2
            };

            List<string> data = new List<string>() { ITEM_RANGE_VAL_1, ITEM_RANGE_VAL_1, ITEM_RANGE_SHAPE_SALTIRE };

            ItemRange range = new ItemRange(config, data);
            Assert.AreEqual<ItemRangeShape>(ItemRangeShape.Saltire, range.Shape);
        }

        [TestMethod]
        public void ItemRangeConstructor_OptionalField_Shape_Star()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1,
                Shape = 2
            };

            List<string> data = new List<string>() { ITEM_RANGE_VAL_1, ITEM_RANGE_VAL_1, ITEM_RANGE_SHAPE_STAR };

            ItemRange range = new ItemRange(config, data);
            Assert.AreEqual<ItemRangeShape>(ItemRangeShape.Star, range.Shape);
        }

        #endregion OptionalField_Shape

        #region OptionalField_CanOnlyUseBeforeMovement

        [TestMethod]
        public void ItemRangeConstructor_OptionalField_CanOnlyUseBeforeMovement_EmptyString()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1,
                CanOnlyUseBeforeMovement = 2
            };

            List<string> data = new List<string>() { ITEM_RANGE_VAL_1, ITEM_RANGE_VAL_1, string.Empty };

            ItemRange range = new ItemRange(config, data);
            Assert.IsFalse(range.CanOnlyUseBeforeMovement);
        }

        [TestMethod]
        public void ItemRangeConstructor_OptionalField_CanOnlyUseBeforeMovement_No()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1,
                CanOnlyUseBeforeMovement = 2
            };

            List<string> data = new List<string>() { ITEM_RANGE_VAL_1, ITEM_RANGE_VAL_1, "No" };

            ItemRange range = new ItemRange(config, data);
            Assert.IsFalse(range.CanOnlyUseBeforeMovement);
        }

        [TestMethod]
        public void ItemRangeConstructor_OptionalField_CanOnlyUseBeforeMovement_Yes()
        {
            ItemRangeConfig config = new ItemRangeConfig()
            {
                Minimum = 0,
                Maximum = 1,
                CanOnlyUseBeforeMovement = 2
            };

            List<string> data = new List<string>() { ITEM_RANGE_VAL_1, ITEM_RANGE_VAL_1, "Yes" };

            ItemRange range = new ItemRange(config, data);
            Assert.IsTrue(range.CanOnlyUseBeforeMovement);
        }

        #endregion OptionalField_CanOnlyUseBeforeMovement
    }
}
