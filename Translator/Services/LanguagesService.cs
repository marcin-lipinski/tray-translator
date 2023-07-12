using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Translator.Models;

namespace Translator.Services;

internal class LanguagesService
{
    private Dictionary<string, Language> _languages;
    public LanguagesService()
    {
        LoadLanguages();
    }

    public string LanguageShortName(string language) => _languages[language].ShortName;
    public List<string> Languages
    {
        get => _languages.Keys.OrderBy(a => a).ToList();

    }

    private async void LoadLanguages()
    {
        await Task.Run(() =>
        {
            var uri = Path.Combine(TranslationClient.DirectoryPath, "languages.json");
            using var reader = new StreamReader(uri);
            var json = reader.ReadToEnd();
            _languages = JsonSerializer.Deserialize<List<Language>>(json).ToDictionary(lang => lang.FullName);
        });
    }
}
