using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace Translator.Extensions;

public static class ServiceExtensions
{
    public static void AddTranslationClient(this IServiceCollection services, string name)
    {
        var appsettingsUri = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.Parent!.FullName);

        var config = new ConfigurationBuilder()
            .SetBasePath(appsettingsUri)
            .AddJsonFile("Translator\\appsettings.json", optional: false, reloadOnChange: true)
            .Build();

    var apiUri = config.GetValue<string>("translateApiUri");
        var apiKey = config.GetValue<string>("translateApiKey");
        var apiHost = config.GetValue<string>("translateApiHost");

        services.AddHttpClient(name, client =>
        {
            client.BaseAddress = new Uri(apiUri);
            client.DefaultRequestHeaders.Add("X-RapidAPI-Key", apiKey);
            client.DefaultRequestHeaders.Add("X-RapidAPI-Host", apiHost);
        });
    }
}
