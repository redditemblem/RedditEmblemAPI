﻿using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.UnitStats;

namespace UnitTests.Models.System.Skills.Effects.UnitStats
{
    [TestClass]
    public class HPDifferenceStatModifierEffectTests
    {
        #region Constructor

        [TestMethod]
        public void HPDifferenceStatModifierEffect_Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new HPDifferenceStatModifierEffect(parameters));
        }

        [TestMethod]
        public void HPDifferenceStatModifierEffect_Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new HPDifferenceStatModifierEffect(parameters));
        }

        [TestMethod]
        public void HPDifferenceStatModifierEffect_Constructor_2EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty, };

            Assert.ThrowsException<NonZeroPositiveDecimalException>(() => new HPDifferenceStatModifierEffect(parameters));
        }

        [TestMethod]
        public void HPDifferenceStatModifierEffect_Constructor_Multiplier_Neg1()
        {
            List<string> parameters = new List<string>() { "-1", string.Empty };

            Assert.ThrowsException<NonZeroPositiveDecimalException>(() => new HPDifferenceStatModifierEffect(parameters));
        }

        [TestMethod]
        public void HPDifferenceStatModifierEffect_Constructor_Multiplier_0()
        {
            List<string> parameters = new List<string>() { "0", string.Empty };

            Assert.ThrowsException<NonZeroPositiveDecimalException>(() => new HPDifferenceStatModifierEffect(parameters));
        }

        [TestMethod]
        public void HPDifferenceStatModifierEffect_Constructor_EmptyStats()
        {
            List<string> parameters = new List<string>() { "1", string.Empty };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new HPDifferenceStatModifierEffect(parameters));
        }

        #endregion Constructor
    }
}
