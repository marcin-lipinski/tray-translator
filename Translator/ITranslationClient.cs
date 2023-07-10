using System.Threading.Tasks;

namespace Translator;

public interface ITranslationClient
{
    Task<string> Translate(string sourceText, string sourceLanguage, string targetLanguage);
}