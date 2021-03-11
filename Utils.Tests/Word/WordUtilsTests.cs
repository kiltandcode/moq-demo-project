using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Utils.UtilLogger;
using Utils.Word;
using static Utils.Word.WordUtils;

namespace Utils.Tests.Word
{
    [TestClass]
    public class WordUtilsTests
    {
        // Declare the properties (any dependencies and the class to test)
        private IUtilLogger _log;
        private IWordUtils _wordUtils;

        // Define constants to use for the unit test categories (...don't repeat yourself)
        private const string TEST_CATEGORY_USING_ASSERT = "Unit tests using Arrange, Act, Assert";
        private const string TEST_CATEGORY_USING_FLUENTASSERTION = "Unit tests using FluentAssertion";
        private const string TEST_CATEGORY_USING_MOCKING_MOQ = "Unit tests using mocking e.g. Moq";

        #region Use the [TestInitialize] attribute to run a method on each test execution

        [TestInitialize]
        public void Initialize()
        {
            // Initialize a mock of the logger dependency
            _log = Mock.Of<IUtilLogger>();

            // Initialize the WordUtils class with it's dependency
            _wordUtils = new WordUtils(_log);
        }

        #endregion

        #region Unit tests using Arrange, Act, Assert (not recommended)

        [TestCategory(TEST_CATEGORY_USING_ASSERT)]
        [TestMethod]
        public void Reverse_ShouldBeWordInReverse_IfWordIsValid()
        {
            // Arrange
            string word = "mountain";

            // Act
            string reverseWord = _wordUtils.Reverse(word);

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(reverseWord));
            Assert.AreEqual(word.Length, reverseWord?.Length);
            Assert.AreEqual("niatnuom", reverseWord);
        }

        [TestCategory(TEST_CATEGORY_USING_ASSERT)]
        [TestMethod]
        public void Reverse_ShouldThrowArgumentException_IfWordIsNull()
        {
            // Arrange
            string word = null;

            // Act
            Action action = () => _wordUtils.Reverse(word);
            ArgumentException exception = 
                Assert.ThrowsException<ArgumentException>(action);

            // Assert
            Assert.AreEqual(REVERSE_ERROR_ENTER_A_WORD, exception.Message);
        }

        [TestCategory(TEST_CATEGORY_USING_ASSERT)]
        [TestMethod]
        public void Reverse_ShouldThrowArgumentException_IfWordIsEmpty()
        {
            // Arrange
            string word = string.Empty;

            // Act
            Action action = () => _wordUtils.Reverse(word);
            ArgumentException exception =
                Assert.ThrowsException<ArgumentException>(action);

            // Assert
            Assert.AreEqual(REVERSE_ERROR_ENTER_A_WORD, exception.Message);
        }

        #endregion

        #region Unit tests using FluentAssertion (recommended)

        // FluentAssertions documentation can be found at:
        // https://fluentassertions.com/introduction

        [TestCategory(TEST_CATEGORY_USING_FLUENTASSERTION)]
        [TestMethod]
        public void Reverse_ShouldBeWordInReverse_IfWordIsValid_Fluent()
        {
            string reverseWord = _wordUtils.Reverse("mountain");

            reverseWord.Should().NotBeNullOrEmpty();
            reverseWord.Should().HaveLength(8);
            reverseWord.Should().Be("niatnuom");
        }

        [TestCategory(TEST_CATEGORY_USING_FLUENTASSERTION)]
        [TestMethod]
        public void Reverse_ShouldThrowArgumentException_IfWordIsNull_Fluent()
        {
            Action action = () => _wordUtils.Reverse(null);

            action.Should().Throw<ArgumentException>()
                .WithMessage(REVERSE_ERROR_ENTER_A_WORD);
        }

        [TestCategory(TEST_CATEGORY_USING_FLUENTASSERTION)]
        [TestMethod]
        public void Reverse_ShouldThrowArgumentException_IfWordIsEmpty_Fluent()
        {
            Action action = () => _wordUtils.Reverse(string.Empty);

            action.Should().Throw<ArgumentException>()
                .WithMessage(REVERSE_ERROR_ENTER_A_WORD);
        }

        #endregion

        #region Unit tests using mocking (Moq)

        // Moq documentation can be found at:
        // https://github.com/Moq/moq4/wiki/Quickstart

        // Using "Verify" in the Moq library
        [TestCategory(TEST_CATEGORY_USING_MOCKING_MOQ)]
        [TestMethod]
        public void Reverse_ShouldInvokeOnce_IsLogEnabled()
        {
            _wordUtils.Reverse("mountain");

            Mock.Get(_log).Verify(x => x.IsLogEnabled(), Times.Once);
        }

        // Using "Setup" in the Moq library
        [TestCategory(TEST_CATEGORY_USING_MOCKING_MOQ)]
        [TestMethod]
        public void Reverse_ShouldInvokeOnce_LogInformationMethod()
        {
            Mock.Get(_log)
                .Setup(x => x.IsLogEnabled())
                .Returns(true);

            _wordUtils.Reverse("mountain");

            Mock.Get(_log).Verify(x => x.Log(
                LogLevel.Information, 
                It.IsAny<EventId>(), 
                It.IsAny<FormattedLogValues>(), 
                It.IsAny<Exception>(), 
                It.IsAny<Func<object, Exception, string>>()), 
                Times.Once);
        }

        // Using "Callback" in the Moq library
        [TestCategory(TEST_CATEGORY_USING_MOCKING_MOQ)]
        [TestMethod]
        public void Reverse_ShouldInvokeOnce_AddToWordCache()
        {
            string word = "mountain";
            string reverseWord = "niatnuom";

            Dictionary<string, string> wordCache = new Dictionary<string, string>();

            Mock.Get(_log)
                .Setup(x => x.AddToWordCache(word, reverseWord))
                .Callback<string, string>((w, r) => wordCache.Add(w, r));

            _wordUtils.Reverse(word);

            wordCache.Count.Should().BeGreaterThan(0);

            Mock.Get(_log).Verify(x => x.AddToWordCache(
                It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        #endregion
    }
}
