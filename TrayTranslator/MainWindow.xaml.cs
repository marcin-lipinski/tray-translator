using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using Translator;

namespace TrayTranslator;

public partial class MainWindow : Window
{
    private readonly ITranslationClient _translationClient;

    public MainWindow(ITranslationClient translationClient)
    {
        InitializeComponent();
        AddLanguagesToComboBox();
        _translationClient = translationClient;

        Left = SystemParameters.PrimaryScreenWidth - (Width + 10);
        Top = SystemParameters.PrimaryScreenHeight - (Height + 10 + SystemParameters.PrimaryScreenHeight - SystemParameters.FullPrimaryScreenHeight - SystemParameters.WindowCaptionHeight);
        ExitButtonEllipse.Cursor = Cursors.Hand;
        ExitButtonText.Cursor = Cursors.Hand;
    }

    private async void AddLanguagesToComboBox()
    {
        var uri = System.IO.Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.Parent!.FullName, "Translator", "languages.json");
        using var reader = new StreamReader(uri);
        var json = await reader.ReadToEndAsync();
        var languages = JsonSerializer.Deserialize<List<Language>>(json).ToDictionary(lang => lang.fullName);
        foreach (var language in languages)
        {
            SourceLanguageComboBox.Items.Add(language.Value.fullName);
            TargetLanguageComboBox.Items.Add(language.Value.fullName);
        }
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);

        if (e.Cancel == false)
        {
            e.Cancel = true;
            Hide();
        }
    }

    private void closeButton_Click(object sender, RoutedEventArgs e)
    {
        Hide();
    }

    private async void SourceText_TextChanged(object sender, TextChangedEventArgs e)
    {
        var currentText = SourceText.Text;
        var targetText = await DelayedCheck(currentText);
        TargetText.Text = targetText.Length == 0 ? TargetText.Text : targetText;
    }

    private async Task<string> DelayedCheck(string registeredText)
    {
        await Task.Delay(750);
        if (SourceText.Text.Equals(registeredText))
        {
            var sourceLanguage = SourceLanguageComboBox.Text;
            var targetLanguage = TargetLanguageComboBox.Text;
            return await _translationClient.Translate(registeredText, sourceLanguage, targetLanguage);
        }
        return string.Empty;
    }

    private void MenuItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        Close();
    }

    private void ExitButtonText_MouseEnter(object sender, MouseEventArgs e)
    {
        ExitButtonEllipseShadow.Visibility = Visibility.Visible;
    }

    private void ExitButtonText_MouseLeave(object sender, MouseEventArgs e)
    {
        ExitButtonEllipseShadow.Visibility = Visibility.Hidden;
    }
}

class Language
{
    public string fullName { get; set; }
    public string shortName { get; set; }
}