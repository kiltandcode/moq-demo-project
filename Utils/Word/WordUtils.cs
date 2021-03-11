using System;
using Microsoft.Extensions.Logging;
using Utils.UtilLogger;

namespace Utils.Word
{
    public class WordUtils : IWordUtils
    {
        private readonly IUtilLogger _log;

        public const string REVERSE_ERROR_ENTER_A_WORD = "Please enter a word to reverse.";

        public WordUtils(IUtilLogger log)
        {
            _log = log;
        }

        /// <summary>
        /// Reverse a word (e.g. "mountain" to "niatnuom")
        /// </summary>
        /// <param name="word">The word to reverse.</param>
        /// <returns>The word in reverse.</returns>
        public string Reverse(string word)
        {
            if (string.IsNullOrEmpty(word))
                throw new ArgumentException(REVERSE_ERROR_ENTER_A_WORD);

            char[] charArray = word.ToCharArray();
            Array.Reverse(charArray);
            string reverseWord = new string(charArray);

            if (_log.IsLogEnabled())
                _log.LogInformation($"The word \"{word}\" was reversed as \"{reverseWord}\"");

            // Add the word and the reversed word to the in-memory cache (a dictionary object)
            _log.AddToWordCache(word, reverseWord);

            return reverseWord;
        }
    }
}
