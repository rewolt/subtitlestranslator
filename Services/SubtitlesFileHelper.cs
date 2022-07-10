namespace SubtitlesTranslator.Services;

internal class SubtitlesFileHelper
{
    public static string[]? ReadFromFile(string path)
    {
        var file = new FileInfo(path);
        file.Refresh();
        if (file.Extension != ".srt")
        {
            Console.WriteLine("This is not a '.srt' file!\nPress any key to exit.");
            Console.ReadKey();
            return null;
        }

        return File.ReadAllLines(file.FullName);
    }

    public static string WriteToFile(string text, Translator.LanguageKey languageKey, string path)
    {
        var newFileNamePath = CreateNewFileName(path, languageKey);
        var file = new FileInfo(newFileNamePath);
        
        using var sw = file.CreateText();
        sw.Write(text);
        sw.Flush();

        return file.FullName;
    }

    private static string CreateNewFileName(string path, Translator.LanguageKey languageKey)
    {
        var fileName = Path.GetFileNameWithoutExtension(path);
        var extension = Path.GetExtension(path);

        var newFileName = $"{fileName}-{languageKey}";
        return Path.Combine(Path.GetDirectoryName(path)!, newFileName + extension);
    }
}
