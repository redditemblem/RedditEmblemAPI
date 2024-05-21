﻿using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange;

namespace UnitTests.Models.System.Skills.Effects.ItemRange
{
    [TestClass]
    public class ItemMinRangeModifierEffectTests
    {
        #region Constructor

        [TestMethod]
        public void ItemMinRangeModifierEffect_Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new ItemMinRangeModifierEffect(parameters));
        }

        [TestMethod]
        public void ItemMinRangeModifierEffect_Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new ItemMinRangeModifierEffect(parameters));
        }

        [TestMethod]
        public void ItemMinRangeModifierEffect_Constructor_2EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.ThrowsException<NegativeIntegerException>(() => new ItemMinRangeModifierEffect(parameters));
        }

        [TestMethod]
        public void ItemMinRangeModifierEffect_Constructor_Value_1()
        {
            List<string> parameters = new List<string>() { string.Empty, "1" };

            Assert.ThrowsException<NegativeIntegerException>(() => new ItemMinRangeModifierEffect(parameters));
        }

        [TestMethod]
        public void ItemMinRangeModifierEffect_Constructor_EmptyCategories()
        {
            List<string> parameters = new List<string>() { string.Empty, "-1" };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new ItemMinRangeModifierEffect(parameters));
        }

        #endregion Constructor
    }
}
