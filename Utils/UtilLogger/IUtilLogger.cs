using Microsoft.Extensions.Logging;

namespace Utils.UtilLogger
{
    public interface IUtilLogger : ILogger
    {
        bool IsLogEnabled();

        void AddToWordCache(string word, string reverseWord);
    }
}
