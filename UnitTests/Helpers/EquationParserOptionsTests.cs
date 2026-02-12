using RedditEmblemAPI.Services.Helpers;

namespace UnitTests.Helpers
{
    public class EquationParserOptionsTests
    {
        #region Union

        [Test]
        public void EquationParserOptions_Union_NoOptions()
        {
            EquationParserOptions options1 = new EquationParserOptions();
            Assert.That(options1.EvalUnitCombatStat, Is.False);
            Assert.That(options1.EvalUnitStat, Is.False);
            Assert.That(options1.EvalUnitLevel, Is.False);
            Assert.That(options1.EvalWeaponUtilStat_Greatest, Is.False);
            Assert.That(options1.EvalWeaponUtilStat_Sum, Is.False);
            Assert.That(options1.EvalWeaponStat, Is.False);
            Assert.That(options1.EvalBattalionStat, Is.False);

            EquationParserOptions options2 = new EquationParserOptions();
            Assert.That(options2.EvalUnitCombatStat, Is.False);
            Assert.That(options2.EvalUnitStat, Is.False);
            Assert.That(options2.EvalUnitLevel, Is.False);
            Assert.That(options2.EvalWeaponUtilStat_Greatest, Is.False);
            Assert.That(options2.EvalWeaponUtilStat_Sum, Is.False);
            Assert.That(options2.EvalWeaponStat, Is.False);
            Assert.That(options2.EvalBattalionStat, Is.False);

            EquationParserOptions union = options1.Union(options2);
            Assert.That(union.EvalUnitCombatStat, Is.False);
            Assert.That(union.EvalUnitStat, Is.False);
            Assert.That(union.EvalUnitLevel, Is.False);
            Assert.That(union.EvalWeaponUtilStat_Greatest, Is.False);
            Assert.That(union.EvalWeaponUtilStat_Sum, Is.False);
            Assert.That(union.EvalWeaponStat, Is.False);
            Assert.That(union.EvalBattalionStat, Is.False);
        }

        [Test]
        public void EquationParserOptions_Union_EvalUnitCombatStat()
        {
            EquationParserOptions options1 = new EquationParserOptions();
            EquationParserOptions options2 = new EquationParserOptions()
            {
                EvalUnitCombatStat = true
            };

            EquationParserOptions union = options1.Union(options2);
            Assert.That(union.EvalUnitCombatStat, Is.True);
            Assert.That(union.EvalUnitStat, Is.False);
            Assert.That(union.EvalUnitLevel, Is.False);
            Assert.That(union.EvalWeaponUtilStat_Greatest, Is.False);
            Assert.That(union.EvalWeaponUtilStat_Sum, Is.False);
            Assert.That(union.EvalWeaponStat, Is.False);
            Assert.That(union.EvalBattalionStat, Is.False);
        }

        [Test]
        public void EquationParserOptions_Union_EvalUnitStat()
        {
            EquationParserOptions options1 = new EquationParserOptions();
            EquationParserOptions options2 = new EquationParserOptions()
            {
                EvalUnitStat = true
            };

            EquationParserOptions union = options1.Union(options2);
            Assert.That(union.EvalUnitCombatStat, Is.False);
            Assert.That(union.EvalUnitStat, Is.True);
            Assert.That(union.EvalUnitLevel, Is.False);
            Assert.That(union.EvalWeaponUtilStat_Greatest, Is.False);
            Assert.That(union.EvalWeaponUtilStat_Sum, Is.False);
            Assert.That(union.EvalWeaponStat, Is.False);
            Assert.That(union.EvalBattalionStat, Is.False);
        }

        [Test]
        public void EquationParserOptions_Union_EvalUnitLevel()
        {
            EquationParserOptions options1 = new EquationParserOptions();
            EquationParserOptions options2 = new EquationParserOptions()
            {
                EvalUnitLevel = true
            };

            EquationParserOptions union = options1.Union(options2);
            Assert.That(union.EvalUnitCombatStat, Is.False);
            Assert.That(union.EvalUnitStat, Is.False);
            Assert.That(union.EvalUnitLevel, Is.True);
            Assert.That(union.EvalWeaponUtilStat_Greatest, Is.False);
            Assert.That(union.EvalWeaponUtilStat_Sum, Is.False);
            Assert.That(union.EvalWeaponStat, Is.False);
            Assert.That(union.EvalBattalionStat, Is.False);
        }

        [Test]
        public void EquationParserOptions_Union_EvalWeaponUtilStat_Greatest()
        {
            EquationParserOptions options1 = new EquationParserOptions();
            EquationParserOptions options2 = new EquationParserOptions()
            {
                EvalWeaponUtilStat_Greatest = true
            };

            EquationParserOptions union = options1.Union(options2);
            Assert.That(union.EvalUnitCombatStat, Is.False);
            Assert.That(union.EvalUnitStat, Is.False);
            Assert.That(union.EvalUnitLevel, Is.False);
            Assert.That(union.EvalWeaponUtilStat_Greatest, Is.True);
            Assert.That(union.EvalWeaponUtilStat_Sum, Is.False);
            Assert.That(union.EvalWeaponStat, Is.False);
            Assert.That(union.EvalBattalionStat, Is.False);
        }

        [Test]
        public void EquationParserOptions_Union_EvalWeaponUtilStat_Sum()
        {
            EquationParserOptions options1 = new EquationParserOptions();
            EquationParserOptions options2 = new EquationParserOptions()
            {
                EvalWeaponUtilStat_Sum = true
            };

            EquationParserOptions union = options1.Union(options2);
            Assert.That(union.EvalUnitCombatStat, Is.False);
            Assert.That(union.EvalUnitStat, Is.False);
            Assert.That(union.EvalUnitLevel, Is.False);
            Assert.That(union.EvalWeaponUtilStat_Greatest, Is.False);
            Assert.That(union.EvalWeaponUtilStat_Sum, Is.True);
            Assert.That(union.EvalWeaponStat, Is.False);
            Assert.That(union.EvalBattalionStat, Is.False);
        }

        [Test]
        public void EquationParserOptions_Union_EvalWeaponStat()
        {
            EquationParserOptions options1 = new EquationParserOptions();
            EquationParserOptions options2 = new EquationParserOptions()
            {
                EvalWeaponStat = true
            };

            EquationParserOptions union = options1.Union(options2);
            Assert.That(union.EvalUnitCombatStat, Is.False);
            Assert.That(union.EvalUnitStat, Is.False);
            Assert.That(union.EvalUnitLevel, Is.False);
            Assert.That(union.EvalWeaponUtilStat_Greatest, Is.False);
            Assert.That(union.EvalWeaponUtilStat_Sum, Is.False);
            Assert.That(union.EvalWeaponStat, Is.True);
            Assert.That(union.EvalBattalionStat, Is.False);
        }

        [Test]
        public void EquationParserOptions_Union_EvalBattalionStat()
        {
            EquationParserOptions options1 = new EquationParserOptions();
            EquationParserOptions options2 = new EquationParserOptions()
            {
                EvalBattalionStat = true
            };

            EquationParserOptions union = options1.Union(options2);
            Assert.That(union.EvalUnitCombatStat, Is.False);
            Assert.That(union.EvalUnitStat, Is.False);
            Assert.That(union.EvalUnitLevel, Is.False);
            Assert.That(union.EvalWeaponUtilStat_Greatest, Is.False);
            Assert.That(union.EvalWeaponUtilStat_Sum, Is.False);
            Assert.That(union.EvalWeaponStat, Is.False);
            Assert.That(union.EvalBattalionStat, Is.True);
        }

        [Test]
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
            Assert.That(union.EvalUnitCombatStat, Is.True);
            Assert.That(union.EvalUnitStat, Is.True);
            Assert.That(union.EvalUnitLevel, Is.True);
            Assert.That(union.EvalWeaponUtilStat_Greatest, Is.True);
            Assert.That(union.EvalWeaponUtilStat_Sum, Is.True);
            Assert.That(union.EvalWeaponStat, Is.True);
            Assert.That(union.EvalBattalionStat, Is.True);
        }

        #endregion Union
    }
}