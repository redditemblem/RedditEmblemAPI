using RedditEmblemAPI.Models.Configuration.System.CombatArts;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    [TestClass]
    public class CombatArtRangeTests
    {
        #region Constants

        private const string ART_RANGE_VAL_1 = "1";

        #endregion Constants

        [TestMethod]
        public void CombatArtRangeConstructor_RequiredFields_Null()
        {
            CombatArtRangeConfig config = new CombatArtRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            List<string> data = new List<string>() { };

            CombatArtRange output = new CombatArtRange(config, data);
            Assert.AreEqual<int>(0, output.Minimum);
            Assert.AreEqual<int>(0, output.Maximum);
        }

        [TestMethod]
        public void CombatArtRangeConstructor_RequiredFields_WithInvalidMinRange()
        {
            CombatArtRangeConfig config = new CombatArtRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            List<string> data = new List<string>() { "-1", "0" };

            Assert.ThrowsException<PositiveIntegerException>(() => new CombatArtRange(config, data));
        }

        [TestMethod]
        public void CombatArtRangeConstructor_RequiredFields_WithInvalidMaxRange()
        {
            CombatArtRangeConfig config = new CombatArtRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            List<string> data = new List<string>() { "0", "-1" };

            Assert.ThrowsException<PositiveIntegerException>(() => new CombatArtRange(config, data));
        }

        [TestMethod]
        public void CombatArtRangeConstructor_RequiredFields_WithInvalidRangeSet()
        {
            CombatArtRangeConfig config = new CombatArtRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            List<string> data = new List<string>() { "2", "1" };

            Assert.ThrowsException<MinimumGreaterThanMaximumException>(() => new CombatArtRange(config, data));
        }

        [TestMethod]
        public void CombatArtRangeConstructor_RequiredFields_MinRangeUnset()
        {
            CombatArtRangeConfig config = new CombatArtRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            List<string> data = new List<string>() { "0", "1" };

            Assert.ThrowsException<ItemRangeMinimumNotSetException>(() => new CombatArtRange(config, data));
        }

        [TestMethod]
        public void CombatArtRangeConstructor_RequiredFields_MaxRangeUnset()
        {
            CombatArtRangeConfig config = new CombatArtRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            List<string> data = new List<string>() { "1", "0" };

            Assert.ThrowsException<ItemRangeMinimumNotSetException>(() => new CombatArtRange(config, data));
        }

        [TestMethod]
        public void CombatArtRangeConstructor_RequiredFields()
        {
            CombatArtRangeConfig config = new CombatArtRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            List<string> data = new List<string>() { ART_RANGE_VAL_1, ART_RANGE_VAL_1 };

            CombatArtRange range = new CombatArtRange(config, data);
            Assert.AreEqual<int>(1, range.Minimum);
            Assert.AreEqual<int>(1, range.Maximum);
        }
    }
}
