using System.Collections.Generic;

namespace SEOAnalyzer.ViewModels
{
    public class AnalysisViewModel
    {
        public string Source { get; set; }

        public IDictionary<string, int> Words { get; set; }

        public IDictionary<string, int> MetaKeywords { get; set; }

        public IEnumerable<string> ExternalLinks { get; set; }
    }
}