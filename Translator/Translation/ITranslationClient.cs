using System.Collections.Generic;
using System.Threading.Tasks;
using Translator.Models;

namespace Translator;

public interface ITranslationClient
{
    Task<TranslationResult> Translate(string sourceText, string sourceLanguage, string targetLanguage);
    publList<string> Languages;
}