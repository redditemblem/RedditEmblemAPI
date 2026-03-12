using NSubstitute;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Models.System.StatusConditions.Effects
{
    public class AddTagEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            IEnumerable<string> parameters = new List<string>();

            Assert.Throws<StatusConditionEffectMissingParameterException>(() => new AddTagEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new AddTagEffect(parameters));
        }

        [Test]
        public void Constructor()
        {
            IEnumerable<string> parameters = new List<string>() { "Tag1,Tag2" };

            AddTagEffect effect = new AddTagEffect(parameters);

            Assert.That(effect.Tags, Is.EquivalentTo(new List<string>() { "Tag1", "Tag2" }));
        }

        #endregion Constructor

        #region Apply

        [Test]
        public void Apply_TagAlreadyExists()
        {
            IUnit unit = Substitute.For<IUnit>();
            unit.Tags.Returns(new List<string>() { "Tag1" });

            IUnitStatus status = Substitute.For<IUnitStatus>();

            ITag tag = Substitute.For<ITag>();
            tag.Name.Returns("Tag1");

            IDictionary<string, ITag> tags = new Dictionary<string, ITag>();
            tags.Add("Tag1", tag);

            IEnumerable<string> parameters = new List<string>() { "Tag1" };
            AddTagEffect effect = new AddTagEffect(parameters);

            effect.Apply(unit, status, tags);

            Assert.That(unit.Tags, Is.Not.Empty);
            Assert.That(unit.Tags, Is.EqualTo(new List<string>() { "Tag1" }));
            tag.DidNotReceive().FlagAsMatched();
        }

        [Test]
        public void Apply()
        {
            IUnit unit = Substitute.For<IUnit>();
            unit.Tags.Returns(new List<string>());

            IUnitStatus status = Substitute.For<IUnitStatus>();

            ITag tag = Substitute.For<ITag>();
            tag.Name.Returns("Tag1");

            IDictionary<string, ITag> tags = new Dictionary<string, ITag>();
            tags.Add("Tag1", tag);

            IEnumerable<string> parameters = new List<string>() { "Tag1" };
            AddTagEffect effect = new AddTagEffect(parameters);

            effect.Apply(unit, status, tags);

            Assert.That(unit.Tags, Is.Not.Empty);
            Assert.That(unit.Tags, Is.EqualTo(new List<string>() { "Tag1" }));
            tag.Received(1).FlagAsMatched();
        }

        #endregion Apply
    }
}