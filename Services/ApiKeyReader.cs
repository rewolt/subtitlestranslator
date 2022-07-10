namespace SubtitlesTranslator.Services;

internal class ApiKeyReader
{
    const string _fileName = "ApiKey.txt";

    public static string? GetApiKey()
    {
        var file = new FileInfo(_fileName);
        file.Refresh();

        if (!file.Exists)
        {
            CreateFile(file);
            Console.WriteLine($"'{_fileName}' was created. Please, paste deepl.com API key there and run program once again.\nPress any key to exit.");
            Console.ReadKey();
            return null;
        }

        var apiKey = File.ReadAllLines(file.FullName).Where(x => !x.StartsWith("#")).First();

        if (ValidateApiKey(apiKey))
            return apiKey;
        else
        {
            Console.WriteLine("API key is not valid.\nPress any key to exit.");
            Console.ReadKey();
            return null;
        }
    }

    private static void CreateFile(FileInfo file)
    {
        using var sw = new StreamWriter(file.OpenWrite());
        sw.WriteLine("# Here you need to paste your api key to deepl.com.");
        sw.WriteLine("# It can be pasted above in uncommented line (without '#' char).");
        sw.Flush();
    }

    private static bool ValidateApiKey(string apiKey) => !string.IsNullOrWhiteSpace(apiKey);
}
