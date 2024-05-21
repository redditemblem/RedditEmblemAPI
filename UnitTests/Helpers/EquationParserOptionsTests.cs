using RedditEmblemAPI.Services.Helpers;

namespace UnitTests.Helpers
{
    [TestClass]
    public class EquationParserOptionsTests
    {
        #region Union

        [TestMethod]
        public void EquationParserOptions_Union_NoOptions()
        {
            EquationParserOptions options1 = new EquationParserOptions();
            EquationParserOptions options2 = new EquationParserOptions();

            EquationParserOptions union = options1.Union(options2);
            Assert.IsFalse(union.EvalUnitCombatStat);
            Assert.IsFalse(union.EvalUnitStat);
            Assert.IsFalse(union.EvalUnitLevel);
            Assert.IsFalse(union.EvalWeaponUtilStat_Greatest);
            Assert.IsFalse(union.EvalWeaponUtilStat_Sum);
            Assert.IsFalse(union.EvalWeaponStat);
            Assert.IsFalse(union.EvalBattalionStat);
        }

        [TestMethod]
        public void EquationParserOptions_Union_EvalUnitCombatStat()
        {
            EquationParserOptions options1 = new EquationParserOptions();
            EquationParserOptions options2 = new EquationParserOptions()
            {
                EvalUnitCombatStat = true
            };

            EquationParserOptions union = options1.Union(options2);
            Assert.IsTrue(union.EvalUnitCombatStat);
            Assert.IsFalse(union.EvalUnitStat);
            Assert.IsFalse(union.EvalUnitLevel);
            Assert.IsFalse(union.EvalWeaponUtilStat_Greatest);
            Assert.IsFalse(union.EvalWeaponUtilStat_Sum);
            Assert.IsFalse(union.EvalWeaponStat);
            Assert.IsFalse(union.EvalBattalionStat);
        }

        [TestMethod]
        public void EquationParserOptions_Union_EvalUnitStat()
        {
            EquationParserOptions options1 = new EquationParserOptions();
            EquationParserOptions options2 = new EquationParserOptions()
            {
                EvalUnitStat = true
            };

            EquationParserOptions union = options1.Union(options2);
            Assert.IsFalse(union.EvalUnitCombatStat);
            Assert.IsTrue(union.EvalUnitStat);
            Assert.IsFalse(union.EvalUnitLevel);
            Assert.IsFalse(union.EvalWeaponUtilStat_Greatest);
            Assert.IsFalse(union.EvalWeaponUtilStat_Sum);
            Assert.IsFalse(union.EvalWeaponStat);
            Assert.IsFalse(union.EvalBattalionStat);
        }

        [TestMethod]
        public void EquationParserOptions_Union_EvalUnitLevel()
        {
            EquationParserOptions options1 = new EquationParserOptions();
            EquationParserOptions options2 = new EquationParserOptions()
            {
                EvalUnitLevel = true
            };

            EquationParserOptions union = options1.Union(options2);
            Assert.IsFalse(union.EvalUnitCombatStat);
            Assert.IsFalse(union.EvalUnitStat);
            Assert.IsTrue(union.EvalUnitLevel);
            Assert.IsFalse(union.EvalWeaponUtilStat_Greatest);
            Assert.IsFalse(union.EvalWeaponUtilStat_Sum);
            Assert.IsFalse(union.EvalWeaponStat);
            Assert.IsFalse(union.EvalBattalionStat);
        }

        [TestMethod]
        public void EquationParserOptions_Union_EvalWeaponUtilStat_Greatest()
        {
            EquationParserOptions options1 = new EquationParserOptions();
            EquationParserOptions options2 = new EquationParserOptions()
            {
                EvalWeaponUtilStat_Greatest = true
            };

            EquationParserOptions union = options1.Union(options2);
            Assert.IsFalse(union.EvalUnitCombatStat);
            Assert.IsFalse(union.EvalUnitStat);
            Assert.IsFalse(union.EvalUnitLevel);
            Assert.IsTrue(union.EvalWeaponUtilStat_Greatest);
            Assert.IsFalse(union.EvalWeaponUtilStat_Sum);
            Assert.IsFalse(union.EvalWeaponStat);
            Assert.IsFalse(union.EvalBattalionStat);
        }

        [TestMethod]
        public void EquationParserOptions_Union_EvalWeaponUtilStat_Sum()
        {
            EquationParserOptions options1 = new EquationParserOptions();
            EquationParserOptions options2 = new EquationParserOptions()
            {
                EvalWeaponUtilStat_Sum = true
            };

            EquationParserOptions union = options1.Union(options2);
            Assert.IsFalse(union.EvalUnitCombatStat);
            Assert.IsFalse(union.EvalUnitStat);
            Assert.IsFalse(union.EvalUnitLevel);
            Assert.IsFalse(union.EvalWeaponUtilStat_Greatest);
            Assert.IsTrue(union.EvalWeaponUtilStat_Sum);
            Assert.IsFalse(union.EvalWeaponStat);
            Assert.IsFalse(union.EvalBattalionStat);
        }

        [TestMethod]
        public void EquationParserOptions_Union_EvalWeaponStat()
        {
            EquationParserOptions options1 = new EquationParserOptions();
            EquationParserOptions options2 = new EquationParserOptions()
            {
                EvalWeaponStat = true
            };

            EquationParserOptions union = options1.Union(options2);
            Assert.IsFalse(union.EvalUnitCombatStat);
            Assert.IsFalse(union.EvalUnitStat);
            Assert.IsFalse(union.EvalUnitLevel);
            Assert.IsFalse(union.EvalWeaponUtilStat_Greatest);
            Assert.IsFalse(union.EvalWeaponUtilStat_Sum);
            Assert.IsTrue(union.EvalWeaponStat);
            Assert.IsFalse(union.EvalBattalionStat);
        }

        [TestMethod]
        public void EquationParserOptions_Union_EvalBattalionStat()
        {
            EquationParserOptions options1 = new EquationParserOptions();
            EquationParserOptions options2 = new EquationParserOptions()
            {
                EvalBattalionStat = true
            };

            EquationParserOptions union = options1.Union(options2);
            Assert.IsFalse(union.EvalUnitCombatStat);
            Assert.IsFalse(union.EvalUnitStat);
            Assert.IsFalse(union.EvalUnitLevel);
            Assert.IsFalse(union.EvalWeaponUtilStat_Greatest);
            Assert.IsFalse(union.EvalWeaponUtilStat_Sum);
            Assert.IsFalse(union.EvalWeaponStat);
            Assert.IsTrue(union.EvalBattalionStat);
        }

        [TestMethod]
        public void EquationParserOptions_Union_AllOptions()
        {
            EquationParserOptions options1 = new EquationParserOptions();
            EquationParserOptions options2 = new EquationParserOptions()
            {
                EvalUnitCombatStat = true,
                EvalUnitStat = true,
                EvalUnitLevel = true,
                EvalWeaponUtilStat_Greatest = true,
                EvalWeaponUtilStat_Sum = true,
                EvalWeaponStat = true,
                EvalBattalionStat = true
            };

            EquationParserOptions union = options1.Union(options2);
            Assert.IsTrue(union.EvalUnitCombatStat);
            Assert.IsTrue(union.EvalUnitStat);
            Assert.IsTrue(union.EvalUnitLevel);
            Assert.IsTrue(union.EvalWeaponUtilStat_Greatest);
            Assert.IsTrue(union.EvalWeaponUtilStat_Sum);
            Assert.IsTrue(union.EvalWeaponStat);
            Assert.IsTrue(union.EvalBattalionStat);
        }

        #endregion Union
    }
}