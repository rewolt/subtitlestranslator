namespace SubtitlesTranslator.Services;

internal static class LanguageSelector
{
    public static Translator.LanguageKey? GetInputLanguage()
    {
        Console.Write("Input subtitles language (i.e. EN): ");
        return GetLanguage();
    }

    public static Translator.LanguageKey? GetTargetLanguage()
    {
        Console.Write("Target subtitles language (i.e. PL): ");
        return GetLanguage();
    }

    private static Translator.LanguageKey? GetLanguage()
    {
        var selected = Console.ReadLine();

        var result = Enum.TryParse(selected, true, out Translator.LanguageKey langKey);
        if (!result)
        {
            Console.WriteLine($"Provided language key '{selected}' was not recognized.\nPress any key to exit.");
            Console.ReadKey();
            return null;
        }
        else
            return langKey;
    }
}
