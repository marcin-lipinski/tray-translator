using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Translator.Models;

namespace Translator;

public class TranslationClient : ITranslationClient
{
    private readonly IHttpClientFactory httpClientFactory;

    public TranslationClient(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<string> Translate(string sourceText, string sourceLanguage, string targetLanguage)
    {
        var client = httpClientFactory.CreateClient("translatorApi");
        var message = CreateHttpRequestMessage(sourceText, sourceLanguage, targetLanguage);

        try
        {
            var response = await client.SendAsync(message);
            var content = await response.Content.ReadFromJsonAsync<Response>();
            if (content.Data == null) throw new Exception();

            var translation = content.Data.Translations.First().TranslatedText;
            return translation;
        }
        catch (Exception)
        {
            return "There was an error with the translator";
        }        
    }
    

    private HttpRequestMessage CreateHttpRequestMessage(string text, string sourceLanguage, string targetLanguage)
    {
        return new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "q", text },
                { "target", targetLanguage },
                { "source", sourceLanguage },
            })
        };
    }
}
