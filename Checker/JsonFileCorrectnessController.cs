using System.Text.RegularExpressions;

namespace Checker;

/// <summary>
/// Static class that contains method to validate correctness of JSON file
/// </summary>
public static class JsonFileCorrectnessController
{
    /// <summary>
    /// Checks if given JSON file has correct structure
    /// </summary>
    /// <param name="filePath">Path to JSON file to validate</param>
    /// <returns>True if JSON is correct, false otherwise</returns>
    public static bool Check(string filePath)
    {
        string text; // Переменная для содержания json файла.
        using (var fileStream = new StreamReader(filePath))
        {
            text = fileStream.ReadToEnd();
        }

        // Шаблон для регулярного выражения, которые исчёт вхождения название полей в строке из json файла.
        string pattern = @"\b(authorId|name|earnings|books|bookId|title|genre|publicationYear)\b";
        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

        var matches = regex.Matches(text);
        Dictionary<string, int> dictionary = new Dictionary<string, int>();
        var keys = new List<string> // Список ключей - названия полей.
            { "authorId", "name", "earnings", "books", "bookId", "title", "genre", "publicationYear" };
        foreach (var key in keys)
        {
            dictionary[key] = matches.Count(m => m.Value == key);
        }

        // Конструкция которая проверяет, что данные из json файла корректны и соотвествуют необходимому формату.
        if (dictionary["authorId"] == dictionary["name"] && dictionary["name"] == dictionary["books"] &&
            dictionary["authorId"] != 0)
        {
            if (dictionary["bookId"] == dictionary["title"] && dictionary["bookId"] == dictionary["genre"] &&
                dictionary["earnings"] == dictionary["bookId"] + dictionary["authorId"])
            {
                return true;
            }

            return false;
        }

        return false;
    }
}