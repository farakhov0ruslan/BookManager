namespace InOutProcessing;

/// <summary>
/// Class for handling user input
/// </summary>
public static class InputProcessing
{
    /// <summary>
    /// Delegate to check if string matches expected pattern 
    /// </summary>
    /// <param name="str">Input string</param>
    /// <param name="patternMismatchResponse">Response if string does not match pattern</param>
    /// <param name="additionalConditions">An additional array of strings for additional comparisons.</param> 
    public delegate bool StringCorrectnessTemplate(string str, out string patternMismatchResponse,
        params string[] additionalConditions);

    /// <summary>
    /// Gets a valid file path from console and checks extension against allowed values
    /// Keeps asking until user provides valid input
    /// </summary>
    /// <param name="prompt">Prompt text for user input</param>
    /// <param name="correctnessTemplate">Delegate to check input correctness</param>
    /// <param name="additionalConditions">An additional array of strings for additional comparisons.</param>
    /// <returns>String matching expected pattern</returns>
    public static string GetCorrectStringFromConsole(string prompt, StringCorrectnessTemplate correctnessTemplate,
        params string[] additionalConditions)
    {
        while (true)
        {
            IOController.Write(prompt, ConsoleColor.Magenta);
            string answer = IOController.ReadLine() ?? string.Empty;
            // Проверка на соотвестствие строки допустимому шаблону.
            if (!correctnessTemplate.Invoke(answer, out string mismatchResponse, additionalConditions))
            {
                IOController.WriteLine(mismatchResponse, ConsoleColor.Red);
            }
            else
            {
                return answer;
            }
        }
    }

    /// <summary>
    /// Gets an integer input between a min and max value
    /// Keeps asking until user provides valid input
    /// </summary>
    /// <param name="prompt">Prompt text for user input</param>
    /// <param name="start">Minimum allowed value</param>
    /// <param name="end">Maximum allowed value</param> 
    /// <returns>Integer between start and end values</returns>
    public static int GetCorrectIntFromConsole(string prompt, int start, int end)
    {
        while (true)
        {
            IOController.Write(prompt, ConsoleColor.Magenta);
            string answer = (IOController.ReadLine() ?? string.Empty);
            // Проверка на соотвестствие строки допустимому шаблону.
            if (!int.TryParse(answer, out int result) || result < start || result > end)
            {
                IOController.WriteLine("Вы ввели неверное значение, повторите ввод.", ConsoleColor.Red);
            }
            else
            {
                return result;
            }
        }
    }

    /// <summary>
    /// Checks if a file path is correct and contains allowed extensions
    /// </summary>
    /// <param name="path">File path to validate</param>
    /// <param name="discrepancyResponse">Error message if invalid</param>
    /// <param name="containsExpansions">Array of allowed file extensions</param>
    /// <returns>True if valid, False if invalid</returns>
    public static bool CorrectPathToFile(string path, out string discrepancyResponse,
        params string[] containsExpansions)
    {
        discrepancyResponse = string.Empty;
        if (Array.Exists(containsExpansions, x => x == "xml"))
        {
            IOController.WriteLine("Тестовый XML файл лежит рядом с исполняемым файлом и называется 12V.xml" +
                                   " .Можно ввести просто 12.xml", ConsoleColor.Cyan);
        }

        if (!File.Exists(path))
        {
            discrepancyResponse = "Введённый вами путь до файла не существует, повторите ввод.";
            return false;
        }


        foreach (string expansion in containsExpansions)
        {
            if (!path.EndsWith($".{expansion}")) // Проверка на соответствие пути до файла переданному расширению.
            {
                discrepancyResponse =
                    $"Введённый вами путь до файла не cодержит нужного расширения {expansion}, повторите ввод.";
                return false;
            }
        }

        return true;
    }


    /// <summary>
    /// Checks if a string matches allowed author sort fields
    /// </summary>
    /// <param name="str">String to validate</param>
    /// <param name="discrepancyResponse">Error message if invalid</param>
    /// <param name="containsAdditional">Array of additional allowed values</param>
    /// <returns>True if valid, False if invalid</returns>    
    public static bool CorrectSortField(string str, out string discrepancyResponse, params string[] containsAdditional)
    {
        string[] fields = { "authorid", "name", "earnings" }; // Поля Автора для сортировки.
        discrepancyResponse = string.Empty;
        // Проверка на соответвие ответа пользователя str и одного из полей для сортировки.
        if (!Array.Exists(fields, x => x == str.ToLower()))
        {
            discrepancyResponse = "Введённое вами поле неверно, повторите ввод.";
            return false;
        }

        return true;
    }

    /// <summary>
    /// Checks if a string matches allowed author fields to edit
    /// </summary>
    /// <param name="str">String to validate</param>
    /// <param name="discrepancyResponse">Error message if invalid</param>
    /// <param name="containsAdditional">Array of additional allowed values</param>
    /// <returns>True if valid, False if invalid</returns>
    public static bool CorrectChangeAuthorField(string str, out string discrepancyResponse,
        params string[] containsAdditional)
    {
        string[] variants = { "name", "books" }; // Возможные варианты для изменения автора.
        discrepancyResponse = string.Empty;
        // Проверка на соответвие ответа пользователя str и одного из варинатов для изменения.
        if (!Array.Exists(variants, x => x == str.ToLower()))
        {
            discrepancyResponse = "Введённое вами поле неверно, повторите ввод.";
            return false;
        }

        return true;
    }

    /// <summary>
    /// Checks if a string matches allowed book fields to edit 
    /// </summary>
    /// <param name="str">String to validate</param>
    /// <param name="discrepancyResponse">Error message if invalid</param>
    /// <param name="containsAdditional">Array of additional allowed values</param>
    /// <returns>True if valid, False if invalid</returns>  
    public static bool CorrectChangeBookField(string str, out string discrepancyResponse,
        params string[] containsAdditional)
    {
        // Возможные варианты для изменения книги.
        string[] fields = { "title", "publication year", "genre", "earnings" };
        discrepancyResponse = string.Empty;
        // Проверка на соответвие ответа пользователя str и одного из варинатов для изменения.
        if (!Array.Exists(fields, x => x == str.ToLower()))
        {
            discrepancyResponse = "Введённое вами поле неверно, повторите ввод.";
            return false;
        }

        return true;
    }


    /// <summary>
    /// Checks if a string is not null or empty
    /// </summary>
    /// <param name="str">String to validate</param>
    /// <param name="discrepancyResponse">Error message if invalid</param>
    /// <param name="containsAdditional">Array of additional allowed values</param> 
    /// <returns>True if valid, False if invalid</returns>
    public static bool NotNullStr(string? str, out string discrepancyResponse, params string[] containsAdditional)
    {
        discrepancyResponse = string.Empty;
        // Проверка на соответвие ответа пользователя str и null значения.
        if (str == string.Empty)
        {
            discrepancyResponse = "Вы ввели пустое значение, повторите ввод.";
            return false;
        }

        return true;
    }
}