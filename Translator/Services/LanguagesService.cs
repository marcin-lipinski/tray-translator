using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Translator.Models;

namespace Translator.Services
{
    internal class LanguagesService
    {
        private Dictionary<string, Language> _languages;
        public List<string> Languages
        {
            get => _languages.Keys.ToList();
        }

        public LanguagesService() 
        {
            LoadLanguages();
        }

        private async void LoadLanguages()
        {
            var uri = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName, "apiDetails.json");
            using var reader = new StreamReader(uri);
            var json = await reader.ReadToEndAsync();
            _languages = JsonSerializer.Deserialize<List<Language>>(json).ToDictionary(lang => lang.fullName);
        }
    }
}
