using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Translator.Models;
using Translator.Services;

namespace Translator;

public class TranslationClient : ITranslationClient
{
    public static readonly string DirectoryPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.Parent!.FullName, "Translator");
    private readonly IHttpClientFactory httpClientFactory;
    private readonly LanguagesService languagesService;

    public TranslationClient(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
        languagesService = new LanguagesService();
    }

    public List<string> Languages
    {
        get => languagesService.Languages;
    }

    public async Task<TranslationResult> Translate(string sourceText, string sourceLanguage, string targetLanguage)
    {
        var client = httpClientFactory.CreateClient("translatorApi");
        var sourceLanguageShortName = languagesService.LanguageShortName(sourceLanguage);
        var targetLanguageShortName = languagesService.LanguageShortName(targetLanguage);
        var message = CreateHttpRequestMessage(sourceText, sourceLanguageShortName, targetLanguageShortName);

        try
        {
            var response = await client.SendAsync(message);
            var content = await response.Content.ReadFromJsonAsync<Response>();
            if (content.Data == null) throw new Exception();

            var translation = content.Data.Translations.First().TranslatedText;
            return new(translation, State.Success);
        }
        catch (Exception)
        {
            return new("There was an error with the translator", State.Failed); ;
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