using NSubstitute;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Models.System.StatusConditions.Effects
{
    public class RemoveTagEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            IEnumerable<string> parameters = new List<string>();

            Assert.Throws<StatusConditionEffectMissingParameterException>(() => new RemoveTagEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new RemoveTagEffect(parameters));
        }

        [Test]
        public void Constructor()
        {
            IEnumerable<string> parameters = new List<string>() { "Tag1,Tag2" };

            RemoveTagEffect effect = new RemoveTagEffect(parameters);

            Assert.That(effect.Tags, Is.EqualTo(new List<string>() { "Tag1", "Tag2" }));
        }

        #endregion Constructor

        #region Apply

        [Test]
        public void Apply()
        {
            IUnit unit = Substitute.For<IUnit>();
            IUnitStatus status = Substitute.For<IUnitStatus>();
            IDictionary<string, ITag> tags = new Dictionary<string, ITag>();

            unit.Tags.Returns(new List<string>() { "Tag1", "Tag2", "Tag3" });

            IEnumerable<string> parameters = new List<string>() { "Tag1,Tag2" };
            RemoveTagEffect effect = new RemoveTagEffect(parameters);

            effect.Apply(unit, status, tags);

            Assert.That(unit.Tags, Is.EqualTo(new List<string>() { "Tag3" }));
        }

        #endregion Apply
    }
}