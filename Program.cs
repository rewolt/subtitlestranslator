using SubtitlesTranslator.Services;

Console.BackgroundColor = ConsoleColor.DarkBlue;
Console.WriteLine("Subtitles translator v0.1");
Console.WriteLine("Uses deepl.com. Many thanks!!");
Console.ResetColor();
Console.WriteLine("Supported languages:");
Translator.PrintSupportedLanguages();
Console.WriteLine();

if (args.Length == 0)
{
    Console.WriteLine("Please drag & drop '.srt' file on SubtitlesTranslator.exe icon to translate.\nPress any key to exit.");
    Console.ReadKey();
    return;
}

var subtitlesFilePath = args[0];

var apiKey = ApiKeyReader.GetApiKey();
if (apiKey == null)
    return;

var subtitles = SubtitlesFileHelper.ReadFromFile(subtitlesFilePath);
if (subtitles == null)
    return;

var inputLang = LanguageSelector.GetInputLanguage();
if (inputLang == null)
    return;

var targetlang = LanguageSelector.GetTargetLanguage();
if (targetlang == null)
    return;

Console.WriteLine($"\nTranslating from {inputLang} to {targetlang}...");

var translator = new Translator(apiKey);
var translatedSubtitles = await translator.Translate(subtitles, inputLang.Value, targetlang.Value);

var translatedSubtitlesFilePath = SubtitlesFileHelper.WriteToFile(translatedSubtitles, targetlang.Value, subtitlesFilePath);

Console.WriteLine($"\nTranslated subtitles saved at '{translatedSubtitlesFilePath}'.\nPress any key to exit.");
Console.ReadKey();