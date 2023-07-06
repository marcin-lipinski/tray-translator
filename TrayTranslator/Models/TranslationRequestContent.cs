using System.Collections.Generic;
using System.Net.Http;

namespace TrayTranslator.Models;

public class TranslationRequestMessage
{
    public static HttpRequestMessage Get(string text, string sourceLanguage, string targetLanguage)
    {
        var message = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "q", text },
                { "target", targetLanguage },
                { "source", sourceLanguage },
            })
        };

        return message;
    }
}
