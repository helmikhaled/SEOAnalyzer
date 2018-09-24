using HtmlAgilityPack;
using System.Threading.Tasks;

namespace SEOAnalyzer.Application
{
    public interface IHtmlDocumentRetriever
    {
        Task SetHtmlDocumentAsync(string url);
        bool IsRetrievalSuccessful { get; }
        HtmlDocument HtmlDocument { get; }
        string BodyContent { get; }
        bool HasMetaKeyword { get; }
        string MetaKeywordContent { get; }
    }
}