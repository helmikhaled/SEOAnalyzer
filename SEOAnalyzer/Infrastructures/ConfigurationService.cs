using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace SEOAnalyzer.Infrastructures
{
    public class ConfigurationService : IConfigurationService
    {
        private const string StopWordsPath = "~\\App_Data\\stop-words.json";

        public HashSet<string> GetStopWords()
        {
            var result = new List<string>();
            var path = HttpContext.Current.Server.MapPath(StopWordsPath);
            using (StreamReader reader = new StreamReader(path))
            {
                var content = reader.ReadToEnd();
                var words = JsonConvert.DeserializeObject<string[]>(content);

                result.AddRange(words);
            }

            return new HashSet<string>(result);
        }
    }
}