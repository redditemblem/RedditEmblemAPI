using NSubstitute;
using RedditEmblemAPI.Helpers.Ranges;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Models.Output.System.Skills.Effects;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange;
using RedditEmblemAPI.Models.Output.System.StatusConditions;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Helpers.Ranges
{
    public class UnitRangeParametersTests
    {
        [Test]
        public void Constructor()
        {
            IUnit unit = Substitute.For<IUnit>();
            unit.GetFullSkillsList().Returns(new List<ISkill>());
            unit.StatusConditions.Returns(new List<IUnitStatus>());

            UnitRangeParameters parms = new UnitRangeParameters(unit);

            Assert.That(parms.Unit, Is.EqualTo(unit));
            Assert.That(parms.IgnoresAffiliations, Is.False);
            Assert.That(parms.MoveCostModifiers, Is.Empty);
            Assert.That(parms.MoveCostSets_Skills, Is.Empty);
            Assert.That(parms.MoveCostSets_Statuses, Is.Empty);
            Assert.That(parms.WarpCostModifiers, Is.Empty);
            Assert.That(parms.WarpCostSets, Is.Empty);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void Constructor_IgnoresAffiliations(bool expected)
        {
            IUnit unit = Substitute.For<IUnit>();
            ISkill skill = Substitute.For<ISkill>();

            var effect = Substitute.For<IIgnoreUnitAffiliations, ISkillEffect>();
            effect.IsActive(unit).Returns(expected);
            skill.Effects.Returns(new List<ISkillEffect>() { (ISkillEffect)effect });

            unit.GetFullSkillsList().Returns(new List<ISkill>() { skill });
            unit.StatusConditions.Returns(new List<IUnitStatus>());

            UnitRangeParameters parms = new UnitRangeParameters(unit);

            Assert.That(parms.Unit, Is.EqualTo(unit));
            Assert.That(parms.IgnoresAffiliations, Is.EqualTo(expected));
            Assert.That(parms.MoveCostModifiers, Is.Empty);
            Assert.That(parms.MoveCostSets_Skills, Is.Empty);
            Assert.That(parms.MoveCostSets_Statuses, Is.Empty);
            Assert.That(parms.WarpCostModifiers, Is.Empty);
            Assert.That(parms.WarpCostSets, Is.Empty);
        }

        [Test]
        public void Constructor_MoveCostModifiers()
        {
            IUnit unit = Substitute.For<IUnit>();
            ISkill skill = Substitute.For<ISkill>();

            var effect = Substitute.For<ITerrainTypeMovementCostModifierEffect, ISkillEffect>();
            skill.Effects.Returns(new List<ISkillEffect>() { (ISkillEffect)effect });

            unit.GetFullSkillsList().Returns(new List<ISkill>() { skill });
            unit.StatusConditions.Returns(new List<IUnitStatus>());

            UnitRangeParameters parms = new UnitRangeParameters(unit);

            Assert.That(parms.Unit, Is.EqualTo(unit));
            Assert.That(parms.IgnoresAffiliations, Is.False);
            Assert.That(parms.MoveCostModifiers, Is.Not.Empty);
            Assert.That(parms.MoveCostSets_Skills, Is.Empty);
            Assert.That(parms.MoveCostSets_Statuses, Is.Empty);
            Assert.That(parms.WarpCostModifiers, Is.Empty);
            Assert.That(parms.WarpCostSets, Is.Empty);
        }

        [Test]
        public void Constructor_MoveCostSets_Skills()
        {
            IUnit unit = Substitute.For<IUnit>();
            ISkill skill = Substitute.For<ISkill>();

            var effect = Substitute.For<ITerrainTypeMovementCostSetEffect_Skill, ISkillEffect>();
            skill.Effects.Returns(new List<ISkillEffect>() { (ISkillEffect)effect });

            unit.GetFullSkillsList().Returns(new List<ISkill>() { skill });
            unit.StatusConditions.Returns(new List<IUnitStatus>());

            UnitRangeParameters parms = new UnitRangeParameters(unit);

            Assert.That(parms.Unit, Is.EqualTo(unit));
            Assert.That(parms.IgnoresAffiliations, Is.False);
            Assert.That(parms.MoveCostModifiers, Is.Empty);
            Assert.That(parms.MoveCostSets_Skills, Is.Not.Empty);
            Assert.That(parms.MoveCostSets_Statuses, Is.Empty);
            Assert.That(parms.WarpCostModifiers, Is.Empty);
            Assert.That(parms.WarpCostSets, Is.Empty);
        }

        [Test]
        public void Constructor_MoveCostSets_Statuses()
        {
            IUnit unit = Substitute.For<IUnit>();
            IUnitStatus unitStatus = Substitute.For<IUnitStatus>();

            IStatusCondition status = Substitute.For<IStatusCondition>();
            var effect = Substitute.For<ITerrainTypeMovementCostSetEffect_Status, IStatusConditionEffect>();
            status.Effects.Returns(new List<IStatusConditionEffect>() { (IStatusConditionEffect)effect });
            unitStatus.Status.Returns(status);

            unit.GetFullSkillsList().Returns(new List<ISkill>());
            unit.StatusConditions.Returns(new List<IUnitStatus>() { unitStatus });

            UnitRangeParameters parms = new UnitRangeParameters(unit);

            Assert.That(parms.Unit, Is.EqualTo(unit));
            Assert.That(parms.IgnoresAffiliations, Is.False);
            Assert.That(parms.MoveCostModifiers, Is.Empty);
            Assert.That(parms.MoveCostSets_Skills, Is.Empty);
            Assert.That(parms.MoveCostSets_Statuses, Is.Not.Empty);
            Assert.That(parms.WarpCostModifiers, Is.Empty);
            Assert.That(parms.WarpCostSets, Is.Empty);
        }

        [Test]
        public void Constructor_WarpCostModifiers()
        {
            IUnit unit = Substitute.For<IUnit>();
            ISkill skill = Substitute.For<ISkill>();

            var effect = Substitute.For<IWarpMovementCostModifierEffect, ISkillEffect>();
            skill.Effects.Returns(new List<ISkillEffect>() { (ISkillEffect)effect });

            unit.GetFullSkillsList().Returns(new List<ISkill>() { skill });
            unit.StatusConditions.Returns(new List<IUnitStatus>());

            UnitRangeParameters parms = new UnitRangeParameters(unit);

            Assert.That(parms.Unit, Is.EqualTo(unit));
            Assert.That(parms.IgnoresAffiliations, Is.False);
            Assert.That(parms.MoveCostModifiers, Is.Empty);
            Assert.That(parms.MoveCostSets_Skills, Is.Empty);
            Assert.That(parms.MoveCostSets_Statuses, Is.Empty);
            Assert.That(parms.WarpCostModifiers, Is.Not.Empty);
            Assert.That(parms.WarpCostSets, Is.Empty);
        }

        [Test]
        public void Constructor_WarpCostSets()
        {
            IUnit unit = Substitute.For<IUnit>();
            ISkill skill = Substitute.For<ISkill>();

            var effect = Substitute.For<IWarpMovementCostSetEffect, ISkillEffect>();
            skill.Effects.Returns(new List<ISkillEffect>() { (ISkillEffect)effect });

            unit.GetFullSkillsList().Returns(new List<ISkill>() { skill });
            unit.StatusConditions.Returns(new List<IUnitStatus>());

            UnitRangeParameters parms = new UnitRangeParameters(unit);

            Assert.That(parms.Unit, Is.EqualTo(unit));
            Assert.That(parms.IgnoresAffiliations, Is.False);
            Assert.That(parms.MoveCostModifiers, Is.Empty);
            Assert.That(parms.MoveCostSets_Skills, Is.Empty);
            Assert.That(parms.MoveCostSets_Statuses, Is.Empty);
            Assert.That(parms.WarpCostModifiers, Is.Empty);
            Assert.That(parms.WarpCostSets, Is.Not.Empty);
        }
    }
}
