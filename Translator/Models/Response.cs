namespace Translator.Models;

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

