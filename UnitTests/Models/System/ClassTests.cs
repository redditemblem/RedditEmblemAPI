using RedditEmblemAPI.Models.Configuration.System.Classes;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    [TestClass]
    public class ClassTests
    {
        #region Constants

        private const string INPUT_NAME = "Class Test";
        private const string INPUT_MOVEMENT_TYPE = "Movement Type Test";

        #endregion Constants

        [TestMethod]
        public void ClassConstructor_RequiredFields_WithInputNull()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                MovementType = 1
            };

            List<string> data = new List<string>() { };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new Class(config, data));
        }

        [TestMethod]
        public void ClassConstructor_RequiredFields_WithInput_InvalidMovementType()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                MovementType = 1
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new Class(config, data));
        }

        [TestMethod]
        public void ClassConstructor_RequiredFields()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                MovementType = 1
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_MOVEMENT_TYPE
            };

            Class cls = new Class(config, data);

            Assert.AreEqual<string>(INPUT_NAME, cls.Name);
            Assert.AreEqual<string>(INPUT_MOVEMENT_TYPE, cls.MovementType);
        }

        #region OptionalField_Tags

        [TestMethod]
        public void ClassConstructor_OptionalField_Tags_EmptyString()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                MovementType = 1,
                Tags = new List<int> { 2, 3 }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_MOVEMENT_TYPE,
                string.Empty,
                string.Empty
            };

            Class cls = new Class(config, data);

            CollectionAssert.AreEqual(new List<string>() { }, cls.Tags);
        }

        [TestMethod]
        public void ClassConstructor_OptionalField_Tags()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                MovementType = 1,
                Tags = new List<int> { 2, 3 }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_MOVEMENT_TYPE,
                "Tag 1,Tag 2",
                "Tag 3"
            };

            Class cls = new Class(config, data);

            CollectionAssert.AreEqual(new List<string>() { "Tag 1", "Tag 2", "Tag 3"}, cls.Tags);
        }

        #endregion OptionalField_Tags

        #region OptionalField_TextFields

        [TestMethod]
        public void ClassConstructor_OptionalField_TextFields_EmptyString()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                MovementType = 1,
                TextFields = new List<int> { 2, 3 }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_MOVEMENT_TYPE,
                string.Empty,
                string.Empty
            };

            Class cls = new Class(config, data);

            CollectionAssert.AreEqual(new List<string>() { }, cls.TextFields);
        }

        [TestMethod]
        public void ClassConstructor_OptionalField_TextFields()
        {
            ClassesConfig config = new ClassesConfig()
            {
                Name = 0,
                MovementType = 1,
                TextFields = new List<int> { 2, 3 }
            };

            List<string> data = new List<string>()
            {
                INPUT_NAME,
                INPUT_MOVEMENT_TYPE,
                "Text Field 1",
                "Text Field 2"
            };

            Class cls = new Class(config, data);

            CollectionAssert.AreEqual(new List<string>() { "Text Field 1", "Text Field 2" }, cls.TextFields);
        }

        #endregion OptionalField_TextFields
    }
}
