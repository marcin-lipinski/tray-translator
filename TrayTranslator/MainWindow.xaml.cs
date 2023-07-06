using Hardcodet.Wpf.TaskbarNotification;
using OibraryExample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using TrayTranslator.Models;
using TrayTranslator.StartupHelpers;



namespace TrayTranslator;

public partial class MainWindow : Window
{
    private readonly IExampleOfDI exampleOfDI;
    private readonly IAbstractFactory<ChildForm> factory;
    private readonly IHttpClientFactory httpClientFactory;

    public MainWindow(IExampleOfDI exampleOfDI, IAbstractFactory<ChildForm> factory, IHttpClientFactory httpClientFactory)
    {
        InitializeComponent();
        this.exampleOfDI = exampleOfDI;
        this.factory = factory;
        this.httpClientFactory = httpClientFactory;
    }

    private async void getData_Click(object sender, RoutedEventArgs e)
    {
        var client = httpClientFactory.CreateClient("translatorApi");
        var mes = TranslationRequestMessage.Get("Hello, world!", "en", "es");

        var response = await client.SendAsync(mes);
        var content = await response.Content.ReadFromJsonAsync<Response>();
        Data.Text = content.Data.Translations.First().TranslatedText;

       MyIcon.Visibility = Visibility.Hidden;
    }

    private void openChildForm_Click(object sender, RoutedEventArgs e)
    {
        factory.Create().Show();
        Data.Text = string.Empty;
        MyIcon.Visibility = Visibility.Visible;
    }
}


public class Response
{
    public Data Data { get; set; }
}

public class Data
{
    public Translation[] Translations { get; set; }
}

public class Translation
{
    public string TranslatedText { get; set; }
}


