using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Translator;

namespace TrayTranslator;

public partial class MainWindow : Window
{
    private readonly ITranslationClient _translationClient;

    public MainWindow(ITranslationClient translationClient)
    {
        InitializeComponent();
        ReadAddLanguagesToComboBox();
        SetWindowCords();
        AddEventsHandlers();
        _translationClient = translationClient;
    }

    private void SetWindowCords()
    {
        Left = SystemParameters.PrimaryScreenWidth - (Width + 10);
        Top = SystemParameters.PrimaryScreenHeight - (Height + 10 + SystemParameters.PrimaryScreenHeight - SystemParameters.FullPrimaryScreenHeight - SystemParameters.WindowCaptionHeight);
    }

    private void AddEventsHandlers()
    {
        CloseMenuItem.Click += (sender, e) => Application.Current.Shutdown();
        ShowMenuItem.Click += (sender, e) => Visibility = Visibility.Visible;
        HideButton.MouseLeftButtonDown += (sender, e) => Visibility = Visibility.Collapsed;
        HideButton.MouseLeave += (sender, e) => HideButtonShadow.Visibility = Visibility.Hidden;
        HideButton.MouseEnter += (sender, e) => HideButtonShadow.Visibility = Visibility.Visible;
    }

    private async void ReadAddLanguagesToComboBox()
    {
        _languages = await _translationClient.Languages;
        foreach (var language in _languages)
        {
            SourceLanguageComboBox.Items.Add(language.Value.fullName);
            TargetLanguageComboBox.Items.Add(language.Value.fullName);
        }
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
            var sourceLanguage = _languages[SourceLanguageComboBox.Text].shortName;
            var targetLanguage = _languages[TargetLanguageComboBox.Text].shortName;

            var result =  await _translationClient.Translate(registeredText, sourceLanguage, targetLanguage);
            if (result.State == State.Failed) TargetText.Foreground = System.Windows.Media.Brushes.DarkSlateGray;
            else TargetText.Foreground = System.Windows.Media.Brushes.Black;

            return result.Text;
        }
        return string.Empty;
    }
}