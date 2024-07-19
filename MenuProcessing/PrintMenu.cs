using CHWLibrary;
using InOutProcessing;

namespace MenuProcessing;

/// <summary>
/// Provides methods to print various menus and get coorect answer.
/// </summary>
public static class PrintMenu
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
            "change_data" => PrintChangeDataMenu,
            "sort_order" => PrintSortOrderMenu,
            "books_info" => PrintBooksInfoMenu,
            "sort_fields" => PrintSortFileds,
            "change_autor_fields" => PrintChangeAuthorFileds,
            "change_book_fields" => PrintChangeBookFileds,
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
        IOController.WriteLine("Продолжить с работу программы с изменныными данными.",
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
        IOController.WriteLine("Продолжить с JSON.",
            ConsoleColor.DarkCyan);
        IOController.Write("\t2. ", ConsoleColor.Magenta);
        IOController.WriteLine("Продолжить с XML.",
            ConsoleColor.DarkCyan);
    }

    /// <summary>
    /// Prints the change data menu.
    /// </summary>
    private static void PrintChangeDataMenu()
    {
        IOController.PrintSeparators();
        IOController.WriteLine("Введите номер действия:",
            ConsoleColor.Cyan);
        IOController.Write("\t1. ", ConsoleColor.Magenta);
        IOController.WriteLine("Отсортировать.",
            ConsoleColor.DarkCyan);
        IOController.Write("\t2. ", ConsoleColor.Magenta);
        IOController.WriteLine("Изменить значение.",
            ConsoleColor.DarkCyan);
        IOController.Write("\t3. ", ConsoleColor.Magenta);
        IOController.WriteLine("Вывести данные.",
            ConsoleColor.DarkCyan);
    }

    /// <summary>
    /// Prints the sorting order selection menu.
    /// </summary>
    private static void PrintSortOrderMenu()
    {
        IOController.PrintSeparators();
        IOController.WriteLine($"Выберите в каком порядке сортировать", ConsoleColor.Cyan);
        IOController.WriteLine("Введите номер действия:",
            ConsoleColor.Cyan);
        IOController.Write("\t1. ", ConsoleColor.Magenta);
        IOController.WriteLine("Прямой порядок сортировки.",
            ConsoleColor.DarkCyan);
        IOController.Write("\t2. ", ConsoleColor.Magenta);
        IOController.WriteLine("Обратный порядок сортировки.",
            ConsoleColor.DarkCyan);
    }

    /// <summary>
    /// Prints the additional info about books selection menu.
    /// </summary>
    private static void PrintBooksInfoMenu()
    {
        IOController.PrintSeparators();
        IOController.WriteLine("Введите номер действия:",
            ConsoleColor.Cyan);
        IOController.Write("\t1. ", ConsoleColor.Magenta);
        IOController.WriteLine("Вывести доп сведения о книгах автора.",
            ConsoleColor.DarkCyan);
        IOController.Write("\t2. ", ConsoleColor.Magenta);
        IOController.WriteLine("Не выводить доп сведения.",
            ConsoleColor.DarkCyan);
    }

    /// <summary>
    /// Prints available fields to sort by.
    /// </summary>
    private static void PrintSortFileds()
    {
        IOController.PrintSeparators();
        string[] authorsHeaders = { "AuthorId", "Name", "Earnings" };
        IOController.WriteLine("Возможные поля для сортировки :", ConsoleColor.Cyan);
        // Выводим заголовки.
        Console.Write('\t');
        IOController.WriteLine(string.Join("\n\t", authorsHeaders),
            ConsoleColor.DarkCyan);
        IOController.WriteLine($"Введите поле для сортировки из предложенных варитантов " +
                               $"(Регистр не важен).", ConsoleColor.Cyan);
    }

    /// <summary> 
    /// Prints available author fields to edit.
    /// </summary>
    private static void PrintChangeAuthorFileds()
    {
        string[] fieldsName = { "Name", "Books" };
        IOController.WriteLine("Возможные поля для изменения (books даст даст выбор" +
                               " для изменения значения одной из книг) :", ConsoleColor.Cyan);
        // Выводим заголовки.
        Console.Write('\t');
        IOController.WriteLine(string.Join("\n\t", fieldsName), ConsoleColor.DarkCyan);
    }

    /// <summary>
    /// Prints available book fields to edit.  
    /// </summary>
    private static void PrintChangeBookFileds()
    {
        string[] fieldsName = { "Title", "Publication Year", "Genre", "Earnings" };
        IOController.WriteLine("Возможные поля для изменения (Изменение Earnings запустит пересчёт" +
                               " дохода у всех авторов за эту книгу) :", ConsoleColor.Cyan);
        Console.Write('\t');
        IOController.WriteLine(string.Join("\n\t", fieldsName), ConsoleColor.DarkCyan);
    }

    /// <summary>
    /// Prints author selection info.
    /// </summary>
    /// <param name="authors">List of authors</param>
    private static void PrintAuthorInfo(List<Author> authors)
    {
        IOController.WriteLine("Введите номер автора, предложенные варинаты - имена авторов",
            ConsoleColor.Cyan);
        for (int i = 0; i < authors.Count; i++)
        {
            IOController.Write($"\t{i + 1}. ", ConsoleColor.Magenta);
            IOController.WriteLine(authors[i].Name ?? string.Empty, ConsoleColor.DarkCyan);
        }
    }

    /// <summary>
    /// Prints book selection info.
    /// </summary>
    /// <param name="books">List of authors</param>
    private static void PrintBooksInfo(List<Book> books)
    {
        IOController.PrintSeparators();
        IOController.WriteLine("Введите номер книги, предложенные варинаты - название книги и год выпуска.",
            ConsoleColor.Cyan);
        for (int i = 0; i < books.Count; i++)
        {
            IOController.Write($"\t{i + 1}. ", ConsoleColor.Magenta);
            IOController.WriteLine($"{books[i].Title} {books[i].PublicationYear}", ConsoleColor.DarkCyan);
        }
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
        GetMethod(nameMethod).Invoke(); // Печатем нужное нам меню с помощью делегата.1
        numMenu = InputProcessing.GetCorrectIntFromConsole("Номер действия = ", 1, countMenuChoice);
    }

    /// <summary>
    /// Prints a menu and gets valid string input using a validation delegate. 
    /// </summary>
    /// <param name="template">Validation delegate in InputProcessing.StringCorrectnessTemplate to check input</param>
    /// <param name="nameMethod">Name of method to print menu</param>  
    /// <param name="correctStr">Output validated string</param>
    public static void InputCorrectStrForMenu(InputProcessing.StringCorrectnessTemplate template, string nameMethod,
        out string correctStr)
    {
        GetMethod(nameMethod).Invoke(); // Печатем нужное нам меню с помощью делегата.
        correctStr = InputProcessing.GetCorrectStringFromConsole("Поле = ", template);
    }

    /// <summary>
    /// Prints author selections and gets valid author index input.
    /// </summary>
    /// <param name="authors">List of authors</param>
    /// <param name="numAuthor">Output selected author index</param>
    public static void InputCorrectChoiceAuthor(List<Author> authors, out int numAuthor)
    {
        PrintAuthorInfo(authors);
        numAuthor = InputProcessing.GetCorrectIntFromConsole("Номер автора = ", 1, authors.Count) - 1;
    }

    /// <summary>
    /// Prints book selections and gets valid book index input. 
    /// </summary>
    /// <param name="books">List of books</param>
    /// <param name="numBook">Output selected book index</param>  
    public static void InputCorrectChoiceBook(List<Book> books, out int numBook)
    {
        PrintBooksInfo(books);
        numBook = InputProcessing.GetCorrectIntFromConsole("Номер книги = ", 1, books.Count) - 1;
    }
}