using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace SEOAnalyzer.Application.Tests
{
    [TestClass]
    public class WordCountServiceTest
    {
        private WordCountService _service;

        [TestInitialize]
        public void Initialize()
        {
            _service = new WordCountService();
        }

        [TestMethod]
        public void GetWordCount_SpaceSeparator_WordSplit()
        {
            // Arrange
            string text = "One Two Three";
            HashSet<string> stopWords = new HashSet<string>();

            // Act
            var result = _service.GetWordCount(text, stopWords);
            var totalCount = result.Sum(w => w.Value);

            // Assert
            Assert.AreEqual(result.Count, 3);
            Assert.AreEqual(totalCount, 3);
        }

        [TestMethod]
        public void GetWordCount_PeriodSeparatorWithSpacesBeforeAndAfter_TrimmedWordSplit()
        {
            // Arrange
            string text = "  one.two.three   ";
            HashSet<string> stopWords = new HashSet<string>();

            // Act
            var result = _service.GetWordCount(text, stopWords);
            var totalCount = result.Sum(w => w.Value);

            // Assert
            Assert.AreEqual(result.Count, 3);
            Assert.AreEqual(totalCount, 3);
        }

        [TestMethod]
        public void GetWordCount_EmptyText_EmptyResult()
        {
            // Arrange
            string text = "";
            HashSet<string> stopWords = new HashSet<string>();

            // Act
            var result = _service.GetWordCount(text, stopWords);

            // Assert
            Assert.AreEqual(result.Count, 0);
        }

        [TestMethod]
        public void GetWordCount_DulicateWordsWithMixedCases_CorrectFrequency()
        {
            // Arrange
            string text = "One one.TWO";
            HashSet<string> stopWords = new HashSet<string>();

            // Act
            var result = _service.GetWordCount(text, stopWords);
            var totalCount = result.Sum(w => w.Value);

            // Assert
            Assert.AreEqual(result.Count, 2);
            Assert.AreEqual(totalCount, 3);
        }

        [TestMethod]
        public void GetWordCount_StopWords_CorrectFrequency()
        {
            // Arrange
            string text = "One one.TWO";
            HashSet<string> stopWords = new HashSet<string>() { "two" };

            // Act
            var result = _service.GetWordCount(text, stopWords);
            var totalCount = result.Sum(w => w.Value);

            // Assert
            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(totalCount, 2);
        }

        [TestMethod]
        public void GetWordCount_NullStopWords_CorrectFrequency()
        {
            // Arrange
            string text = "One one.TWO";
            HashSet<string> stopWords = new HashSet<string>();

            // Act
            var result = _service.GetWordCount(text, stopWords);
            var totalCount = result.Sum(w => w.Value);

            // Assert
            Assert.AreEqual(result.Count, 2);
            Assert.AreEqual(totalCount, 3);
        }

        [TestMethod]
        public void GetWordCountFromList_SimilarWordInOriginalList_CorrectFrequency()
        {
            // Arrange
            string firstText = "One one.TWO";
            string secondText = "one&three!four   ";
            HashSet<string> stopWords = new HashSet<string>();

            // Act
            var firstResult = _service.GetWordCount(firstText, stopWords);
            var secondResult = _service.GetWordCountFromList(secondText, firstResult, stopWords);
            var totalCount = secondResult.Sum(w => w.Value);

            // Assert
            Assert.AreEqual(secondResult.Count, 1);
            Assert.AreEqual(totalCount, 1);
        }

        // TODO: GetExternalLinkCount mock HtmlDocument
    }
}
