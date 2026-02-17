using NSubstitute;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Models.Units
{
    public class UnitBattalionTests
    {
        #region SetUp

        IDictionary<string, IBattalion> BATTALIONS;

        [SetUp]
        public void SetUp()
        {
            string batt1Name = "Battalion 1";
            IBattalion batt1 = Substitute.For<IBattalion>();
            batt1.Name.Returns(batt1Name);

            this.BATTALIONS = new Dictionary<string, IBattalion>();
            this.BATTALIONS.Add(batt1Name, batt1);
        }

        #endregion SetUp

        [Test]
        public void Constructor_RequiredFields_IndexOutOfBounds()
        {
            UnitBattalionConfig config = new UnitBattalionConfig()
            {
                Battalion = 0,
                Endurance = 1,
                GambitUses = 2
            };

            IEnumerable<string> data = new List<string>();

            Assert.Throws<RequiredValueNotProvidedException>(() => new UnitBattalion(config, data, BATTALIONS));
        }

        [Test]
        public void Constructor_RequiredFields_UnmatchedBattalion()
        {
            UnitBattalionConfig config = new UnitBattalionConfig()
            {
                Battalion = 0,
                Endurance = 1,
                GambitUses = 2
            };

            IEnumerable<string> data = new List<string>()
            {
                "Battalion 2"
            };

            Assert.Throws<UnmatchedBattalionException>(() => new UnitBattalion(config, data, BATTALIONS));
        }

        [Test]
        public void Constructor_RequiredFields()
        {
            string battName = "Battalion 1";

            UnitBattalionConfig config = new UnitBattalionConfig()
            {
                Battalion = 0,
                Endurance = 1,
                GambitUses = 2
            };

            IEnumerable<string> data = new List<string>()
            {
                battName,
                "1",
                "3"
            };

            IUnitBattalion batt = new UnitBattalion(config, data, BATTALIONS);
            IBattalion expectedMatch = BATTALIONS[battName];

            Assert.That(batt, Is.Not.Null);
            Assert.That(batt.BattalionObj, Is.EqualTo(expectedMatch));
            Assert.That(batt.Endurance, Is.EqualTo(1));
            Assert.That(batt.GambitUses, Is.EqualTo(3));

            expectedMatch.Received(1).FlagAsMatched();
        }
    }
}
