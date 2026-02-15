using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Models.Units
{
    public class UnitSpriteDataTests
    {
        [Test]
        public void Constructor_RequiredFields_IndexOutOfBounds()
        {
            UnitsConfig config = new UnitsConfig()
            {
                SpriteURL = 0
            };

            IEnumerable<string> data = new List<string>();

            Assert.Throws<RequiredValueNotProvidedException>(() => new UnitSpriteData(config, data));
        }

        [Test]
        public void Constructor_RequiredFields_InvalidInput()
        {
            UnitsConfig config = new UnitsConfig()
            {
                SpriteURL = 0
            };

            IEnumerable<string> data = new List<string>()
            {
                "NotAUrl"
            };

            Assert.Throws<URLException>(() => new UnitSpriteData(config, data));
        }

        [Test]
        public void Constructor_RequiredFields()
        {
            UnitsConfig config = new UnitsConfig()
            {
                SpriteURL = 0
            };

            IEnumerable<string> data = new List<string>()
            {
                UnitTestConsts.IMAGE_URL
            };

            IUnitSpriteData sprite = new UnitSpriteData(config, data);

            Assert.That(sprite, Is.Not.Null);
            Assert.That(sprite.SpriteURL, Is.EqualTo(UnitTestConsts.IMAGE_URL));
            Assert.That(sprite.PortraitURL, Is.Empty);
            Assert.That(sprite.HasMoved, Is.False);
            Assert.That(sprite.Aura, Is.Empty);
        }

        #region OptionalField_PortraitURL

        [Test]
        public void Constructor_OptionalField_PortraitURL_EmptyString()
        {
            UnitsConfig config = new UnitsConfig()
            {
                SpriteURL = 0,
                PortraitURL = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                UnitTestConsts.IMAGE_URL,
                string.Empty
            };

            IUnitSpriteData sprite = new UnitSpriteData(config, data);

            Assert.That(sprite, Is.Not.Null);
            Assert.That(sprite.PortraitURL, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_PortraitURL_InvalidInput()
        {
            UnitsConfig config = new UnitsConfig()
            {
                SpriteURL = 0,
                PortraitURL = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                UnitTestConsts.IMAGE_URL,
                "NotAUrl"
            };

            Assert.Throws<URLException>(() => new UnitSpriteData(config, data));
        }

        [Test]
        public void Constructor_OptionalField_PortraitURL()
        {
            UnitsConfig config = new UnitsConfig()
            {
                SpriteURL = 0,
                PortraitURL = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                UnitTestConsts.IMAGE_URL,
                UnitTestConsts.IMAGE_URL
            };

            IUnitSpriteData sprite = new UnitSpriteData(config, data);

            Assert.That(sprite, Is.Not.Null);
            Assert.That(sprite.PortraitURL, Is.EqualTo(UnitTestConsts.IMAGE_URL));
        }

        #endregion OptionalField_PortraitURL

        #region OptionalField_HasMoved

        [TestCase("", false)]
        [TestCase("No", false)]
        [TestCase("Yes", true)]
        public void Constructor_OptionalField_HasMoved_ValidInputs(string input, bool expected)
        {
            UnitsConfig config = new UnitsConfig()
            {
                SpriteURL = 0,
                HasMoved = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                UnitTestConsts.IMAGE_URL,
                input
            };

            IUnitSpriteData sprite = new UnitSpriteData(config, data);

            Assert.That(sprite, Is.Not.Null);
            Assert.That(sprite.HasMoved, Is.EqualTo(expected));
        }


        #endregion OptionalField_HasMoved
    }
}
