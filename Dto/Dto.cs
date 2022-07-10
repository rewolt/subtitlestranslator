using Newtonsoft.Json;

namespace SubtitlesTranslator.Dto
{
    record ResponseTranslations
    {
        [JsonProperty("translations")]
        public IEnumerable<Translation>? Translations { get; init; }
    }

    record Translation
    {
        [JsonProperty("detected_source_language")]
        public string? DetectedSourceLanguage { get; init; }
        [JsonProperty("text")]
        public string? Text { get; init; }
    }
}
