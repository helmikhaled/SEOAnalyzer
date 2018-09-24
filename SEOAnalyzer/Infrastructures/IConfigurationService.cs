using System.Collections.Generic;

namespace SEOAnalyzer.Infrastructures
{
    public interface IConfigurationService
    {
        HashSet<string> GetStopWords();
    }
}