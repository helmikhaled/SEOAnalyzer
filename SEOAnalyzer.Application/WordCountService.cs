using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SEOAnalyzer.Application
{
    public class WordCountService : IWordCountService
    {
        private readonly IEnumerable<char> _invalidLinkCharacters = new List<char> { '@', '(', ')' };
        private readonly char[] _wordSeparators = new char[] { ' ', '.', ',', ':', ';', '\t', '\n', '&', '?', '!', '"' };

        /// <summary>
        /// Get list of words together with their frequencies.
        /// Words are split by word separators, and words that
        /// appears as a stop word are ignored. 
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="stopWords">stop words</param>
        /// <returns></returns>
        public IDictionary<string, int> GetWordCount(string text, HashSet<string> stopWords)
        {
            return GetWordCount(text, w => IsEligbleWord(w, stopWords));
        }

        /// <summary>
        /// Get list of words together with their frequencies.
        /// Words must appear in the pre defined list.
        /// Words are split by word separators, and words that
        /// appears as a stop word are ignored.
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="wordDictionary">pre-defined list</param>
        /// <param name="stopWords">stop words</param>
        /// <returns></returns>
        public IDictionary<string, int> GetWordCountFromList(string text, IDictionary<string, int> wordDictionary, HashSet<string> stopWords)
        {
            return GetWordCount(text, w => IsEligbleWord(w, stopWords) && wordDictionary.ContainsKey(w.ToLower()));
        }

        /// <summary>
        /// Get list of external links from the document.
        /// Links are not coming from the same host as url.
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="document">document</param>
        /// <returns></returns>
        public IEnumerable<string> GetExternalLinkCount(string url, HtmlDocument document)
        {
            var externalLinks = new List<string>();

            if (string.IsNullOrEmpty(url))
                return externalLinks;

            var uri = new Uri(url);
            var body = document.DocumentNode.SelectSingleNode("//body");

            body.SelectNodes("//a")
                .Select(h => h.GetAttributeValue("href", "#"))
                .Where(link => IsEligibleLink(link, uri))
                .ToList()
                .ForEach(link => externalLinks.Add(link));

            return externalLinks;
        }
        
        private IDictionary<string, int> GetWordCount(string text, Func<string, bool> wordSelector)
        {
            var wordCountDictionary = new Dictionary<string, int>();

            if (IsNullOrEmptyOrWhiteSpace(text))
                return wordCountDictionary;

            var words = text.Trim()
                    .Split(_wordSeparators)
                    .Where(wordSelector)
                    .Select(w => w.ToLower())
                    .ToList();

            foreach (var word in words)
            {
                if (wordCountDictionary.ContainsKey(word))
                {
                    wordCountDictionary[word] += 1;
                }
                else
                {
                    wordCountDictionary.Add(word, 1);
                }

            }

            return wordCountDictionary;
        }

        private static bool IsNullOrEmptyOrWhiteSpace(string word) =>
            string.IsNullOrEmpty(word) || string.IsNullOrWhiteSpace(word);

        private static bool IsEligbleWord(string word, HashSet<string> stopWords) =>
            (stopWords != null && !stopWords.Contains(word.ToLower())) &&
            !IsNullOrEmptyOrWhiteSpace(word);

        private bool IsEligibleLink(string link, Uri uri) =>
            !_invalidLinkCharacters.Any(link.Contains) &&
            !link.Contains(uri.Host) && link.IndexOf('/') != 0 && link.IndexOf('#') != 0;
    }
}
