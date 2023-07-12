using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace Translator.Extensions;

public static class ServiceExtensions
{
    public static void AddTranslationClient(this IServiceCollection services, string name)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(TranslationClient.DirectoryPath)
            .AddJsonFile("apiDetails.json", optional: false, reloadOnChange: true)
            .Build();

        var apiUri = config.GetValue<string>("apiUri");
        var apiKey = config.GetValue<string>("apiKey");
        var apiHost = config.GetValue<string>("apiHost");

        services.AddHttpClient(name, client =>
        {
            client.BaseAddress = new Uri(apiUri);
            client.DefaultRequestHeaders.Add("X-RapidAPI-Key", apiKey);
            client.DefaultRequestHeaders.Add("X-RapidAPI-Host", apiHost);
        });
    }
}
