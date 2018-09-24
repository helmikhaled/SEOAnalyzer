using System.Collections.Generic;
using HtmlAgilityPack;

namespace SEOAnalyzer.Application
{
    public interface IWordCountService
    {
        IEnumerable<string> GetExternalLinkCount(string url, HtmlDocument document);
        IDictionary<string, int> GetWordCount(string text, HashSet<string> stopWords);
        IDictionary<string, int> GetWordCountFromList(string text, IDictionary<string, int> wordDictionary, HashSet<string> stopWords);
    }
}