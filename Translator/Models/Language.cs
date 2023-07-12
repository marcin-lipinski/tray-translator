using System.Text.Json.Serialization;

namespace Translator.Models;

public class Language
{
    [JsonPropertyName("fullName")] public string FullName { get; set; }
    [JsonPropertyName("shortName")] public string ShortName { get; set; }
}