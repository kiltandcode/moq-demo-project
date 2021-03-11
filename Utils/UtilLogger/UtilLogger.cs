using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Utils.Options;

namespace Utils.UtilLogger
{
    public class UtilLogger<T> : Logger<T>, IUtilLogger
    {
        private readonly IOptions<LogOptions> _logOptions;

        public UtilLogger(
            IOptions<LogOptions> logOptions,
            ILoggerFactory logFactory)
            : base(logFactory)
        {
            _logOptions = logOptions;
        }

        private Dictionary<string, string> WordCache { get; set; } = new Dictionary<string, string>();

        public bool IsLogEnabled() 
            => _logOptions.Value.IsLogEnabled;

        public void AddToWordCache(string word, string reverseWord) 
            => WordCache.Add(word, reverseWord);
    }
}
