using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace TrayTranslator.StartupHelpers;

public static class ServiceExtensions
{
    public static void AddFormFactory<TForm>(this IServiceCollection services) where TForm : class
    {
        services.AddTransient<TForm>();
        services.AddSingleton<Func<TForm>>(x => () => x.GetService<TForm>()!);
        services.AddSingleton<IAbstractFactory<TForm>, AbstractFactory<TForm>>();
    }

    public static void AddTranslationClient(this IServiceCollection services, string name)
    {
        var appsettingsUri = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName);

        var config = new ConfigurationBuilder()
            .SetBasePath(appsettingsUri)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

        var apiUri = config.GetValue<string>("translateApiUri")!;

        services.AddHttpClient(name, client =>
        {
            client.BaseAddress = new Uri(apiUri);
            client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "c59b703d6emsh4913a0e90fbfc7bp15ef3bjsnd80a5991c64e");
            client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "google-translate1.p.rapidapi.com");
        });
    }
}
