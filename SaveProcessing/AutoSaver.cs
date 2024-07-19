using System.Runtime.Serialization;
using System.Text.Json;
using System.Xml;
using CHWLibrary;


namespace SaveProcessing;

/// <summary>
/// Provides automated data saving functionality.
/// </summary>
/// <remarks>
/// Before using this class, the following public static members must be initialized:
/// <list type="bullet">
/// <item><description><see cref="Authors"/> - List of authors to save.</description></item> 
/// <item><description><see cref="Expansion"/> - File extension for saved data.</description></item>
/// <item><description><see cref="FilePath"/> - File path to save data to.</description></item>
/// </list> 
/// </remarks>
public static class AutoSaver
{
    /// <summary>
    /// List of authors to save, must be initialized before use.
    /// </summary>
    public static List<Author>? Authors { private get; set; }

    private static DateTime _lastUpdate = DateTime.Now;

    /// <summary> 
    /// File extension for saved data, must be initialized before use. 
    /// </summary>
    public static string? Expansion { private get; set; }

    private static string? _filePath;

    /// <summary>
    /// File path to save data to, must be initialized before use.
    /// </summary>
    public static string FilePath
    {
        get => _filePath ?? string.Empty;
        set => _filePath = value.Replace($".{Expansion}", $"_tmp.{Expansion}");
    }

    /// <summary>
    /// Handles data update events and triggers save if time elapsed is less than 15 seconds. 
    /// </summary>
    /// <param name="sender">Object that raised the event.</param>  
    /// <param name="args">Event arguments with update time.</param>
    public static void HandleUpdate(object? sender, DataUpdateEventArgs args)
    {
        if ((args.UpdateTime - _lastUpdate).TotalSeconds <= 15)
        {
            if (Expansion == "json")
            {
                SaveToJsonFile();
            }
            else
            {
                SaveToXmlFile();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(
                "Была сохранена коллекция объектов! Сохранённый файлл лежит по пути рядом с переданным в начале.");
        }

        _lastUpdate = args.UpdateTime;
    }

    /// <summary>
    /// Serializes the author data and saves to a JSON file. 
    /// </summary>
    /// <remarks>
    /// The file path is based on the <see cref="FilePath"/> property.
    /// Saved file location is printed to the console in green text.
    /// </remarks>  
    private static void SaveToJsonFile()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string json = JsonSerializer.Serialize(Authors, options);
        File.WriteAllText(FilePath, json);
    }

    private static void SaveToXmlFile()
    {
        XmlWriterSettings settings = new XmlWriterSettings() { Indent = true };
        DataContractSerializer serializer = new DataContractSerializer(typeof(List<Author>));

        using XmlWriter w = XmlWriter.Create(FilePath, settings);
        {
            serializer.WriteObject(w, Authors);
        }
    }
}