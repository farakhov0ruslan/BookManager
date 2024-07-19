using System.Text.Json;
using System.Text.Json.Nodes;
using CHWLibrary;

namespace Processing;

/// <summary>
/// Provides methods for user interface and menu processing.
/// </summary>
public class MenuChoiseProccesing
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="discrepancyResponse"></param>
    /// <param name="containsExpansions"></param>
    /// <returns></returns>
    private static bool CorrectPathToFile(string path, out string discrepancyResponse,
        params string[] containsExpansions)
    {
        discrepancyResponse = string.Empty;
        if (!File.Exists(path))
        {
            discrepancyResponse = "Введённый вами путь до файла не существует, повторите ввод.";
            return false;
        }

        foreach (string expansion in containsExpansions)
        {
            if (!path.EndsWith($".{expansion}"))
            {
                discrepancyResponse =
                    $"Введённый вами путь до файла не cодержит нужного расширения {expansion}, повторите ввод.";
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="numMenu"></param>
    /// <param name="data"></param>
    public static void StartMenu(int numMenu, out List<Author>? data)
    {
        IOController.PrintSeparators();
        InputProcessing.StringCorrectnessTemplate correctPathToFile = CorrectPathToFile;
        string[] addConditions = Array.Empty<string>();
        switch (numMenu) // Switch конструкция для корректной работы программы в дальнейшем.
        {
            case 1:
                addConditions = new[] { "json" };
                break;
        }

        string path = InputProcessing.GetCorrectStringFromConsole("Введите путь до файла c расширением:",
            correctPathToFile, addConditions);
        using (FileStream fileStream = new FileStream(path, FileMode.Open))
        {
            data = JsonSerializer.Deserialize<List<Author>>(fileStream);
        }
    }

    /// <summary>
    /// Handles end menu choice.
    /// </summary>
    /// <param name="numMenu">Menu option number.</param>
    /// <param name="flag1">Output flag to exit app.</param>
    /// <param name="flag2">Output flag to reload data.</param> 
    public static void EndMenu(int numMenu, out bool flag1, out bool flag2)
    {
        flag1 = flag2 = false;
        switch (numMenu) // Switch конструкция для корректной работы программы в дальнейшем.
        {
            case 1: // Программа работает с теми же данными.
                flag2 = true;
                Console.Clear();
                break;
            case 2: // Программа работает с новыми данными - всё повторяется. Ничего не меняем.
                Console.Clear();
                break;
            case 3: // Программа закрывается.
                flag1 = true;
                break;
        }
    }
}