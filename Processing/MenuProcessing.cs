namespace Processing;

public static class MenuProcessing
{
    // Делегат для печати нужного меню по кодовому названию метода.
    private delegate void PrintMethod();

    /// <summary>
    /// The method returns a delegate that is used to output a specific MenuProccesing method.
    /// </summary>
    /// <param name="field">The code name of the method by which the menu output method is returned.</param>
    /// <returns>Deleagat </returns>
    /// <exception cref="ArgumentException">There is no code value for the "field" method</exception>
    private static PrintMethod GetMethod(string field)
    {
        PrintMethod printMethod = field switch
        {
            "end" => PrintEndMenu,
            "start" => PrintStartMenu,
            // "change_data" => PrintChangeDataMenu,
            // "sort_order" => PrintSortOrderMenu,
            // "save_with_source" => PrintSaveMenuWithSourceFile,
            // "save_without_source" => PrintSaveMenuWithoutSourceFile,
            // "yaml_or_json" => PrintYamlrOrJsonMenu,
            _ => throw new ArgumentException("Invalid method name passed")
        };

        return printMethod;
    }

    /// <summary>
    /// Prints the last program continuation selection menu.
    /// </summary>
    private static void PrintEndMenu()
    {
        IOController.PrintSeparators();
        IOController.WriteLine("Введите номер действия:",
            ConsoleColor.Cyan);
        IOController.Write("\t1. ", ConsoleColor.Magenta);
        IOController.WriteLine("Продолжить с исходными данными.",
            ConsoleColor.DarkCyan);
        IOController.Write("\t2. ", ConsoleColor.Magenta);
        IOController.WriteLine("Ввести новые данные.",
            ConsoleColor.DarkCyan);
        IOController.Write("\t3. ", ConsoleColor.Magenta);
        IOController.WriteLine("Закрыть программу.",
            ConsoleColor.DarkCyan);
    }

    /// <summary>
    /// Prints the start menu for input filepath readout.
    /// </summary>
    private static void PrintStartMenu()
    {
        IOController.PrintSeparators();
        IOController.WriteLine("Выберите с каким форматом работать.", ConsoleColor.Cyan);
        IOController.Write("\t1. ", ConsoleColor.Magenta);
        IOController.WriteLine("Продолжить с json.",
            ConsoleColor.DarkCyan);
    }


    /// <summary>
    /// The method that allows you to get the correct menu number.
    /// </summary>
    /// <param name="countMenuChoice">
    /// The number of sequence numbers of the selection in the menu.
    /// </param>
    /// <param name="nameMethod">The code name of the menu printing method.</param>
    /// <param name="numMenu">
    /// The value returned out is the number that the user has selected.
    /// </param>
    public static void InputCorrectNumMenu(int countMenuChoice, string nameMethod, out int numMenu)
    {
        while (true) // Цикл для корректного номера выбора меню.
        {
            GetMethod(nameMethod).Invoke(); // Печатем нужное нам меню с помощью делегата.
            IOController.Write("Номер действия = ", ConsoleColor.Magenta);
            if (!int.TryParse(IOController.ReadLine(), out numMenu) || numMenu > countMenuChoice || numMenu < 1)
            {
                Console.WriteLine();
                IOController.WriteLine("Некоректный номер действия, попробуйте ещё раз.",
                    ConsoleColor.Red);
                continue;
            }

            Console.WriteLine();
            break;
        }
    }
}