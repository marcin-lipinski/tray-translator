using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OibraryExample;
using System.Windows;
using TrayTranslator.StartupHelpers;
using System.Net.Http;

namespace TrayTranslator;

public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, servicies) =>
            {
                servicies.AddSingleton<MainWindow>();
                servicies.AddTransient<IExampleOfDI, ExampleOfDI>();
                servicies.AddFormFactory<ChildForm>();
                servicies.AddTranslationClient("translatorApi");
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

        var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
        startupForm.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();
        base.OnExit(e);
    }
}
