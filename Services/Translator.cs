using RestSharp;
using SubtitlesTranslator.Dto;
using System.Text;

namespace SubtitlesTranslator.Services;

internal class Translator
{
    private readonly RestClient _restClient;
    private readonly string _authKey;

    public Translator(string authKey)
    {
        _restClient = new("https://api-free.deepl.com");
        _authKey = authKey;
    }

    public async Task<string> Translate(string[] subtitles, LanguageKey sourceLang, LanguageKey targetLang)
    {
        var progress = "  0%";
        Console.Write("Progress: " + progress);

        var sb = new StringBuilder();
        for (int i = 0; i < subtitles.Length; i++)
        {
            if (IsSectionHeader(subtitles[i]) || IsSectionFooter(subtitles[i]) || IsSectionTime(subtitles[i]))
            {
                sb.AppendLine(subtitles[i]);
                continue;
            }

            var translated = await UseExternalTranslator(subtitles[i], sourceLang, targetLang);
            sb.AppendLine(translated);

            progress = GetProgress(i, subtitles.Length, progress);
        }

        return sb.ToString();
    }

    public static void PrintSupportedLanguages()
    {
        foreach (var lang in Languages)
            Console.WriteLine($"{lang.Key} - {lang.Value}");
    }

    private async Task<string?> UseExternalTranslator(string textToTranslate, LanguageKey sourceLang, LanguageKey targetLang)
    {
        var request = new RestRequest("v2/translate", Method.Post);
        request.AddQueryParameter("auth_key", _authKey);
        request.AddQueryParameter("text", textToTranslate);
        request.AddQueryParameter("source_lang", sourceLang.ToString());
        request.AddQueryParameter("target_lang", targetLang.ToString());

        var response = await _restClient.PostAsync<ResponseTranslations>(request);
        return response!.Translations!.First().Text;
    }

    private static bool IsSectionHeader(string possibleHeader) => int.TryParse(possibleHeader, out _);

    private static bool IsSectionFooter(string possibleFooter) => possibleFooter == "\r\n";

    private static bool IsSectionTime(string possibleTime) => possibleTime.Contains(" --> ");

    private static string GetProgress(int counter, int total, string? lastProgress = null)
    {
        var progress = (double)counter / (double)total * 100;
        var roundedUp = Math.Ceiling(progress);
        var percent = string.Format("{0,3}%", roundedUp);

        if (percent != lastProgress)
        {
            Console.Write("\b\b\b\b");
            Console.Write(percent);
        }

        return percent;
    }

    public enum LanguageKey 
    {
        BG, CS, DA, DE, EL, EN, ES, ET, FI, FR, HU, ID, IT, JA, LT, LV, NL, PL, PT, RO, RU, SK, SL, SV, TR, ZH
    }

    public static readonly IEnumerable<KeyValuePair<LanguageKey, string>> Languages = new[]
    {
        new KeyValuePair<LanguageKey, string>(LanguageKey.BG, "Bulgarian"),
        new KeyValuePair<LanguageKey, string>(LanguageKey.CS, "Czech"),
        new KeyValuePair<LanguageKey, string>(LanguageKey.DA, "Danish"),
        new KeyValuePair<LanguageKey, string>(LanguageKey.DE, "German"),
        new KeyValuePair<LanguageKey, string>(LanguageKey.EL, "Greek"),
        new KeyValuePair<LanguageKey, string>(LanguageKey.EN, "English"),
        new KeyValuePair<LanguageKey, string>(LanguageKey.ES, "Spanish"),
        new KeyValuePair<LanguageKey, string>(LanguageKey.ET, "Estonian"),
        new KeyValuePair<LanguageKey, string>(LanguageKey.FI, "Finnish"),
        new KeyValuePair<LanguageKey, string>(LanguageKey.FR, "French"),
        new KeyValuePair<LanguageKey, string>(LanguageKey.HU, "Hungarian"),
        new KeyValuePair<LanguageKey, string>(LanguageKey.ID, "Indonesian"),
        new KeyValuePair<LanguageKey, string>(LanguageKey.IT, "Italian"),
        new KeyValuePair<LanguageKey, string>(LanguageKey.JA, "Japanese"),
        new KeyValuePair<LanguageKey, string>(LanguageKey.LT, "Lithuanian"),
        new KeyValuePair<LanguageKey, string>(LanguageKey.LV, "Latvian"),
        new KeyValuePair<LanguageKey, string>(LanguageKey.NL, "Dutch"),
        new KeyValuePair<LanguageKey, string>(LanguageKey.PL, "Polish"),
        new KeyValuePair<LanguageKey, string>(LanguageKey.PT, "Portuguese (all Portuguese varieties mixed)"),
        new KeyValuePair<LanguageKey, string>(LanguageKey.RO, "Romanian"),
        new KeyValuePair<LanguageKey, string>(LanguageKey.RU, "Russian"),
        new KeyValuePair<LanguageKey, string>(LanguageKey.SK, "Slovak"),
        new KeyValuePair<LanguageKey, string>(LanguageKey.SL, "Slovenian"),
        new KeyValuePair<LanguageKey, string>(LanguageKey.SV, "Swedish"),
        new KeyValuePair<LanguageKey, string>(LanguageKey.TR, "Turkish"),
        new KeyValuePair<LanguageKey, string>(LanguageKey.ZH, "Chinese")
    };
}
