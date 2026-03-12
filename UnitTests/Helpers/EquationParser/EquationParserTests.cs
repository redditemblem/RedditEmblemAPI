using NSubstitute;
using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Helpers
{
    public class EquationParserTests
    {
        [Test]
        public void Evaluate_PEMDAS()
        {
            string equation = "4 + 5 - 3 * (1 + 2) * 6";
            IUnit unit = null;
            EquationParserOptions options = new EquationParserOptions();

            decimal actual = EquationParser.Evaluate(equation, unit, options);

            Assert.That(actual, Is.EqualTo(-45m));
        }

        [TestCase("{UnitCombatStat}")]
        [TestCase("{UnitCombatStat[Mt]")]
        [TestCase("UnitCombatStat[Mt]}")]
        [TestCase("{UnitStat}")]
        [TestCase("{UnitStat[Str]")]
        [TestCase("UnitStat[Str]}")]
        [TestCase("{UnitLevel")]
        [TestCase("UnitLevel}")]
        [TestCase("{WeaponUtilStat_Greatest")]
        [TestCase("WeaponUtilStat_Greatest}")]
        [TestCase("{WeaponUtilStat_Sum")]
        [TestCase("WeaponUtilStat_Sum}")]
        [TestCase("{WeaponStat}")]
        [TestCase("{WeaponStat[Wt]")]
        [TestCase("WeaponStat[Wt]}")]
        [TestCase("{BattalionStat}")]
        [TestCase("{BattalionStat[Mt]")]
        [TestCase("BattalionStat[Mt]}")]
        [TestCase("{NotARealVariable}")]
        public void Evaluate_UnrecognizedEquationVariable(string equation)
        {
            IUnit unit = Substitute.For<IUnit>();
            EquationParserOptions options = new EquationParserOptions()
            {
                EvalUnitCombatStat = true,
                EvalUnitStat = true,
                EvalUnitLevel = true,
                EvalWeaponUtilStat_Greatest = true,
                EvalWeaponUtilStat_Sum = true,
                EvalWeaponStat = true,
                EvalBattalionStat = true
            };

            Assert.Throws<UnrecognizedEquationVariableException>(() => EquationParser.Evaluate(equation, unit, options));
        }


        [TestCase("")]
        public void Evaluate_EquationEvaluationError(string equation)
        {
            IUnit unit = Substitute.For<IUnit>();
            EquationParserOptions options = new EquationParserOptions()
            {
                EvalUnitCombatStat = true,
                EvalUnitStat = true,
                EvalUnitLevel = true,
                EvalWeaponUtilStat_Greatest = true,
                EvalWeaponUtilStat_Sum = true,
                EvalWeaponStat = true,
                EvalBattalionStat = true
            };

            Assert.Throws<EquationEvaluationErrorException>(() => EquationParser.Evaluate(equation, unit, options));
        }

        [TestCase("{UnitCombatStat[Mt]}", 5)]
        [TestCase("{UnitCombatStat[Hit]}", 6)]
        [TestCase("{UnitCombatStat[Mt]} + {UnitCombatStat[Hit]}", 11)]
        [TestCase("{UnitCombatStat[Mt]} - {UnitCombatStat[Hit]}", -1)]
        [TestCase("{UnitCombatStat[Hit]} - {UnitCombatStat[Mt]}", 1)]
        public void Evaluate_UnitCombatStat(string equation, decimal expected)
        {
            EquationParserOptions options = new EquationParserOptions()
            {
                EvalUnitCombatStat = true
            };

            IModifiedStatValue mt = Substitute.For<IModifiedStatValue>();
            mt.FinalValue.Returns(5);

            IModifiedStatValue hit = Substitute.For<IModifiedStatValue>();
            hit.FinalValue.Returns(6);

            IUnit unit = Substitute.For<IUnit>();
            unit.Stats.MatchCombatStatName("Mt").Returns(mt);
            unit.Stats.MatchCombatStatName("Hit").Returns(hit);

            decimal actual = EquationParser.Evaluate(equation, unit, options);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase("{UnitStat[Str]}", 5)]
        [TestCase("{UnitStat[Mag]}", 6)]
        [TestCase("{UnitStat[Str,Mag]}", 6)]
        [TestCase("{UnitStat[Str]} + {UnitStat[Mag]}", 11)]
        [TestCase("{UnitStat[Str]} - {UnitStat[Mag]}", -1)]
        [TestCase("{UnitStat[Mag]} - {UnitStat[Str]}", 1)]
        public void Evaluate_UnitStat(string equation, decimal expected)
        {
            EquationParserOptions options = new EquationParserOptions()
            {
                EvalUnitStat = true
            };

            IModifiedStatValue str = Substitute.For<IModifiedStatValue>();
            str.FinalValue.Returns(5);

            IModifiedStatValue mag = Substitute.For<IModifiedStatValue>();
            mag.FinalValue.Returns(6);

            IUnit unit = Substitute.For<IUnit>();
            unit.Stats.MatchGeneralStatName("Str").Returns(str);
            unit.Stats.MatchGeneralStatName("Mag").Returns(mag);

            decimal actual = EquationParser.Evaluate(equation, unit, options);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Evaluate_UnitLevel()
        {
            string equation = "{UnitLevel}";
            int level = 10;

            EquationParserOptions options = new EquationParserOptions()
            {
                EvalUnitLevel = true
            };

            IUnit unit = Substitute.For<IUnit>();
            unit.Stats.Level.Returns(level);

            decimal actual = EquationParser.Evaluate(equation, unit, options);

            Assert.That(actual, Is.EqualTo(level));
        }

        [Test]
        public void Evaluate_WeaponUtilStat_Greatest_NoPrimaryEquippedItem()
        {
            string equation = "{WeaponUtilStat_Greatest}";
            EquationParserOptions options = new EquationParserOptions()
            {
                EvalWeaponUtilStat_Greatest = true
            };

            IUnitInventoryItem item = null;
            IUnitEmblem emblem = null;

            IUnit unit = Substitute.For<IUnit>();
            unit.Inventory.GetPrimaryEquippedItem().Returns(item);
            unit.Emblem.Returns(emblem);

            decimal actual = EquationParser.Evaluate(equation, unit, options);

            Assert.That(actual, Is.Zero);
        }

        [Test]
        public void Evaluate_WeaponUtilStat_Greatest()
        {
            string equation = "{WeaponUtilStat_Greatest}";
            EquationParserOptions options = new EquationParserOptions()
            {
                EvalWeaponUtilStat_Greatest = true
            };

            IUnitInventoryItem item = Substitute.For<IUnitInventoryItem>();
            item.Item.UtilizedStats.Returns(new List<string>() { "Str", "Mag" });

            IModifiedStatValue str = Substitute.For<IModifiedStatValue>();
            str.FinalValue.Returns(5);

            IModifiedStatValue mag = Substitute.For<IModifiedStatValue>();
            mag.FinalValue.Returns(6);

            IUnit unit = Substitute.For<IUnit>();
            unit.Inventory.GetPrimaryEquippedItem().Returns(item);
            unit.Stats.MatchGeneralStatName("Str").Returns(str);
            unit.Stats.MatchGeneralStatName("Mag").Returns(mag);

            decimal actual = EquationParser.Evaluate(equation, unit, options);

            Assert.That(actual, Is.EqualTo(6));
        }

        [Test]
        public void Evaluate_WeaponUtilStat_Sum_NoPrimaryEquippedItem()
        {
            string equation = "{WeaponUtilStat_Sum}";
            EquationParserOptions options = new EquationParserOptions()
            {
                EvalWeaponUtilStat_Sum = true
            };

            IUnitInventoryItem item = null;
            IUnitEmblem emblem = null;

            IUnit unit = Substitute.For<IUnit>();
            unit.Inventory.GetPrimaryEquippedItem().Returns(item);
            unit.Emblem.Returns(emblem);

            decimal actual = EquationParser.Evaluate(equation, unit, options);

            Assert.That(actual, Is.Zero);
        }

        [Test]
        public void Evaluate_WeaponUtilStat_Sum()
        {
            string equation = "{WeaponUtilStat_Sum}";
            EquationParserOptions options = new EquationParserOptions()
            {
                EvalWeaponUtilStat_Sum = true
            };

            IUnitInventoryItem item = Substitute.For<IUnitInventoryItem>();
            item.Item.UtilizedStats.Returns(new List<string>() { "Str", "Mag" });

            IModifiedStatValue str = Substitute.For<IModifiedStatValue>();
            str.FinalValue.Returns(5);

            IModifiedStatValue mag = Substitute.For<IModifiedStatValue>();
            mag.FinalValue.Returns(6);

            IUnit unit = Substitute.For<IUnit>();
            unit.Inventory.GetPrimaryEquippedItem().Returns(item);
            unit.Stats.MatchGeneralStatName("Str").Returns(str);
            unit.Stats.MatchGeneralStatName("Mag").Returns(mag);

            decimal actual = EquationParser.Evaluate(equation, unit, options);

            Assert.That(actual, Is.EqualTo(11));
        }

        [Test]
        public void Evaluate_WeaponStat_NoPrimaryEquippedItem()
        {
            string equation = "{WeaponStat[Mt]}";
            EquationParserOptions options = new EquationParserOptions()
            {
                EvalWeaponStat = true
            };

            IUnitInventoryItem item = null;
            IUnitEmblem emblem = null;

            IUnit unit = Substitute.For<IUnit>();
            unit.Inventory.GetPrimaryEquippedItem().Returns(item);
            unit.Emblem.Returns(emblem);

            decimal actual = EquationParser.Evaluate(equation, unit, options);

            Assert.That(actual, Is.Zero);
        }

        [TestCase("{WeaponStat[Mt]}", 5)]
        [TestCase("{WeaponStat[Hit]}", 6)]
        [TestCase("{WeaponStat[Mt,Hit]}", 6)]
        [TestCase("{WeaponStat[Mt]} + {WeaponStat[Hit]}", 11)]
        [TestCase("{WeaponStat[Mt]} - {WeaponStat[Hit]}", -1)]
        [TestCase("{WeaponStat[Hit]} - {WeaponStat[Mt]}", 1)]
        public void Evaluate_WeaponStat(string equation, decimal expected)
        {
            EquationParserOptions options = new EquationParserOptions()
            {
                EvalWeaponStat = true
            };

            IUnitInventoryItemStat mt = Substitute.For<IUnitInventoryItemStat>();
            mt.FinalValue.Returns(5);

            IUnitInventoryItemStat hit = Substitute.For<IUnitInventoryItemStat>();
            hit.FinalValue.Returns(6);

            IUnitInventoryItem item = Substitute.For<IUnitInventoryItem>();
            item.MatchStatName("Mt").Returns(mt);
            item.MatchStatName("Hit").Returns(hit);

            IUnit unit = Substitute.For<IUnit>();
            unit.Inventory.GetPrimaryEquippedItem().Returns(item);

            decimal actual = EquationParser.Evaluate(equation, unit, options);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Evaluate_BattalionStat_NoEquippedBattalion()
        {
            string equation = "{BattalionStat[Mt]}";
            EquationParserOptions options = new EquationParserOptions()
            {
                EvalBattalionStat = true
            };

            IBattalion battalion = null;

            IUnit unit = Substitute.For<IUnit>();
            unit.Battalion.BattalionObj.Returns(battalion);

            decimal actual = EquationParser.Evaluate(equation, unit, options);

            Assert.That(actual, Is.Zero);
        }

        [TestCase("{BattalionStat[Mt]}", 5)]
        [TestCase("{BattalionStat[Hit]}", 6)]
        [TestCase("{BattalionStat[Mt,Hit]}", 6)]
        [TestCase("{BattalionStat[Mt]} + {BattalionStat[Hit]}", 11)]
        [TestCase("{BattalionStat[Mt]} - {BattalionStat[Hit]}", -1)]
        [TestCase("{BattalionStat[Hit]} - {BattalionStat[Mt]}", 1)]
        public void Evaluate_BattalionStat(string equation, decimal expected)
        {
            EquationParserOptions options = new EquationParserOptions()
            {
                EvalBattalionStat = true
            };

            IBattalion battalion = Substitute.For<IBattalion>();
            battalion.MatchStatName("Mt").Returns(5);
            battalion.MatchStatName("Hit").Returns(6);

            IUnit unit = Substitute.For<IUnit>();
            unit.Battalion.BattalionObj.Returns(battalion);

            decimal actual = EquationParser.Evaluate(equation, unit, options);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
