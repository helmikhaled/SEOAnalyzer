using HtmlAgilityPack;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SEOAnalyzer.Application
{
    public class HtmlDocumentRetriever : IHtmlDocumentRetriever
    {
        private const string MetaNodePath = "//meta[contains(@name, 'keyword')]";

        public async Task SetHtmlDocumentAsync(string url)
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri(url);
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStreamAsync().Result;
                    var reader = new StreamReader(result, Encoding.UTF8);
                    var htmlText = reader.ReadToEnd();
                    var htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(htmlText);

                    if (IsValid(htmlDocument))
                    {
                        RemoveComments(htmlDocument);

                        HtmlDocument = htmlDocument;
                    }
                }
            }
        }

        public bool IsRetrievalSuccessful => HtmlDocument != null;

        public HtmlDocument HtmlDocument { get; private set; }

        public string BodyContent =>
            HtmlDocument.DocumentNode.SelectSingleNode("//body").InnerText;

        public bool HasMetaKeyword => HtmlDocument.DocumentNode.SelectSingleNode(MetaNodePath) != null;

        public string MetaKeywordContent =>
            HtmlDocument.DocumentNode.SelectSingleNode(MetaNodePath).GetAttributeValue("content", string.Empty);

        private void RemoveComments(HtmlDocument htmlDocument) =>
            htmlDocument.DocumentNode.Descendants()
                    .Where(h => h.NodeType == HtmlNodeType.Comment)
                    .ToList()
                    .ForEach(n => n.Remove());

        private bool IsValid(HtmlDocument document) =>
            document != null &&
                document.DocumentNode.SelectSingleNode("html/head") != null &&
                document.DocumentNode.SelectSingleNode("html/body") != null;
    }
}
