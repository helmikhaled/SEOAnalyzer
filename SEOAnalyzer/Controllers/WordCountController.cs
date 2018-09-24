using SEOAnalyzer.Application;
using SEOAnalyzer.Infrastructures;
using SEOAnalyzer.ViewModels;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace SEOAnalyzer.Controllers
{
    public class WordCountController : ApiController
    {
        private readonly IWordCountService _wordCountService;
        private readonly IHtmlDocumentRetriever _htmlDocumentRetriever;
        private readonly IConfigurationService _configurationService;

        // TODO: change poor man's IOC
        public WordCountController() : this(new WordCountService(), 
            new HtmlDocumentRetriever(),
            new ConfigurationService())
        {
        }

        public WordCountController(IWordCountService wordCountService, 
            IHtmlDocumentRetriever htmlDocumentRetriever, 
            IConfigurationService configurationService)
        {
            _wordCountService = wordCountService;
            _htmlDocumentRetriever = htmlDocumentRetriever;
            _configurationService = configurationService;
        }

        [HttpPost]
        public IHttpActionResult GetWordCountByText([FromBody]InputViewModel model)
        {
            try
            {
                if (model == null || string.IsNullOrEmpty(model.Source))
                    return BadRequest("Invalid text.");

                var result = new AnalysisViewModel()
                {
                    Source = model.Source,
                    Words = _wordCountService.GetWordCount(model.Source, _configurationService.GetStopWords())
                };

                return Ok(result);
            }
            catch (Exception)
            {
                // TODO: Logger
                throw;
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> GetWordCountByUrlAsync([FromBody]InputViewModel model)
        {
            try
            {
                if (model == null || string.IsNullOrEmpty(model.Source))
                    return BadRequest("Invalid url.");

                await _htmlDocumentRetriever.SetHtmlDocumentAsync(model.Source);

                if (!_htmlDocumentRetriever.IsRetrievalSuccessful)
                    return BadRequest($"Error retrieving {model.Source}.");

                var result = new AnalysisViewModel() { Source = model.Source };

                result.Words = _wordCountService.GetWordCount(_htmlDocumentRetriever.BodyContent, _configurationService.GetStopWords());

                if (_htmlDocumentRetriever.HasMetaKeyword)
                {
                    result.MetaKeywords = _wordCountService.GetWordCountFromList(_htmlDocumentRetriever.MetaKeywordContent, result.Words, _configurationService.GetStopWords());
                }

                result.ExternalLinks = _wordCountService.GetExternalLinkCount(model.Source, _htmlDocumentRetriever.HtmlDocument);

                return Ok(result);
            }
            catch (Exception)
            {
                // TODO: Logger
                throw;
            }
        }
    }
}
