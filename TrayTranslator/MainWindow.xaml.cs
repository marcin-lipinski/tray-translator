using System.Threading.Tasks;
using System.Windows;
using Translator;

namespace TrayTranslator;

public partial class MainWindow : Window
{
    private readonly ITranslationClient _translationClient;

    public MainWindow(ITranslationClient translationClient)
    {
        InitializeComponent();
        _translationClient = translationClient;
        ReadAddLanguagesToComboBox();
        SetWindowCords();
        AddEventsHandlers();
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
        SourceLanguageComboBox.SelectionChanged += async (sender, e) => await DelayedCheck();
        TargetLanguageComboBox.SelectionChanged += async (sender, e) => await DelayedCheck();
        SourceText.TextChanged += async (sender, e) => await DelayedCheck();
    }

    private void ReadAddLanguagesToComboBox()
    {
        foreach (var language in _translationClient.Languages)
        {
            SourceLanguageComboBox.Items.Add(language);
            TargetLanguageComboBox.Items.Add(language);
        }
    }

    private async Task DelayedCheck()
    {
        var registeredText = SourceText.Text;
        await Task.Delay(500);
        if (SourceText.Text.Equals(registeredText))
        {
            var sourceLanguage = SourceLanguageComboBox.Text;
            var targetLanguage = TargetLanguageComboBox.Text;
            var result = await _translationClient.Translate(registeredText, sourceLanguage, targetLanguage);

            if (result.State == State.Failed) TargetText.Foreground = System.Windows.Media.Brushes.DarkSlateGray;
            else TargetText.Foreground = System.Windows.Media.Brushes.Black;

            TargetText.Text = result.Text.Length == 0 ? TargetText.Text : result.Text;
        }
    }

    private void SwitchLanguages_Button(object sender, RoutedEventArgs er)
    {
        var temporaryText = TargetLanguageComboBox.Text;
        TargetLanguageComboBox.Text = SourceLanguageComboBox.Text;
        SourceLanguageComboBox.Text = temporaryText;
    }
}