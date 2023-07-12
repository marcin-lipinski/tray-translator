namespace Translator;

public record TranslationResult(string Text, State State);

public enum State
{
    Success,
    Failed
}
