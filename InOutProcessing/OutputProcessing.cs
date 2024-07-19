using CHWLibrary;
using DataProcessing;

namespace InOutProcessing;

/// <summary>
/// Class containing methods for formatted data output to the console.
/// </summary>
public static class OutputProcessing
{
    /// <summary>
    /// Prints a single row of table data to the console.
    /// </summary>
    /// <param name="rowData">List of data strings to print</param>
    /// <param name="alignment">Dictionary with alignment length for each column</param>
    /// <param name="color">Color to use for main data</param>
    /// <param name="isHeader">Whether this row contains header text</param>
    private static void PrintRow(List<string?> rowData, Dictionary<int, int> alignment, ConsoleColor color,
        bool isHeader = false)
    {
        IOController.Write('|', ConsoleColor.Green);
        for (int i = 0; i < rowData.Count; i++)
        {
            IOController.Write($" {(rowData[i] ?? string.Empty).PadRight(alignment[i])} ",
                i == 0 && !isHeader ? ConsoleColor.Magenta : color);
            IOController.Write('|', ConsoleColor.Green);
        }

        Console.WriteLine();
    }

    /// <summary>
    /// Prints a separator line to the console.
    /// </summary>
    /// <param name="count">Total character count of separator line</param>  
    private static void PrintSeparatorLine(int count)
    {
        IOController.WriteLine("+" + new string('-', count - 1) + "+", ConsoleColor.Green);
    }

    /// <summary>
    /// Gets the maximum length of elements in the specified column.
    /// </summary>
    /// <param name="data">2D array of data</param>
    /// <param name="indexColumn">Index of column to check</param>
    /// <returns>Maximum element length</returns>
    private static int GetMaximumElementsLenght(string[]?[] data, int indexColumn)
    {
        int maxLenght = 0;
        for (int i = 0; i < data.Length; i++)
        {
            if (maxLenght < (data[i]?[indexColumn] ?? string.Empty).Length)
            {
                maxLenght = (data[i]?[indexColumn] ?? string.Empty).Length;
            }
        }

        return maxLenght;
    }

    /// <summary>
    /// Prints a 2D array of data as a table.
    /// </summary>
    /// <param name="data">2D array of data to print</param>
    /// <param name="columnHeaders">Column header texts</param>
    /// <param name="rowHeaders">Row header texts</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if data row lengths do not match column headers
    /// </exception>
    private static void PrintTable(string[][] data, string[] columnHeaders, string[] rowHeaders)
    {
        if (!Array.TrueForAll(data, x => x.Length + 1 == columnHeaders.Length))
        {
            throw new ArgumentOutOfRangeException(nameof(data), "The data lenght is incorrec");
        }

        int countSymbols = 0;
        // Словарь, содержащий длину выравнивания для каждого столбца.
        Dictionary<int, int> aligment = new Dictionary<int, int>();
        // Цикл для перебора индексов столбоцоы
        for (int i = 0; i < columnHeaders.Length; i++)
        {
            if (i > 0) // Если не первый столбец, добавляем выравнивание или по заголовку или по максимальному элементу.
            {
                aligment.Add(i, Math.Max(GetMaximumElementsLenght(data, i - 1), columnHeaders[i].Length));
            }
            /* Если первый столбец, добавляем выравнивание или
             по заголовку или по максимальному элементу заголовка строк. */
            else
            {
                aligment.Add(i,
                    Math.Max((rowHeaders.MaxBy(x => x.Length) ?? string.Empty).Length, columnHeaders[i].Length));
            }

            countSymbols += aligment[i] + 3;
        }

        PrintSeparatorLine(countSymbols);
        PrintRow(columnHeaders.ToList()!, aligment, ConsoleColor.DarkMagenta, true); // Печатаем заголовки.
        PrintSeparatorLine(countSymbols);
        for (int i = 0; i < data.Length; i++)
        {
            ConsoleColor color = i % 2 == 1 ? ConsoleColor.DarkGreen : ConsoleColor.DarkCyan;
            var rowElementsList = new List<string?>(data[i]);
            rowElementsList.Insert(0, rowHeaders[i]); // Вставляем заголовок строки.
            PrintRow(rowElementsList, aligment, color);
        }

        PrintSeparatorLine(countSymbols);
    }

    /// <summary>
    /// Prints a list of authors in table format.
    /// </summary>
    /// <param name="authors">List of authors to print</param>
    public static void PrintAuthorsTable(List<Author>? authors)
    {
        string[] authorsHeadersToPrint = { "index\\fields", "AuthorId", "Name", "Earnings" };
        if (authors != null)
        {
            // Масив из индексов списка авторов (начиная с 1).
            string[] indexAuthorsStr = Array.ConvertAll(Enumerable.Range(1, authors.Count).ToArray(),
                input => input.ToString());
            // Ковенртируем список авторов в двумерный массив строк для печати.
            string[][] printDataAuthors =
                DataConverter.AuthorsListToJaggedArrayStr(authors, authorsHeadersToPrint[1..]);
            PrintTable(printDataAuthors, authorsHeadersToPrint, indexAuthorsStr);
        }
    }

    /// <summary> 
    /// Prints a list of books in table format.
    /// </summary>
    /// <param name="books">List of books to print</param>  
    public static void PrintBooksTable(List<Book>? books)
    {
        string[] bookHeadersToPrint = { "index\\fields", "Title", "PublicationYear", "Genre", "Earnings" };
        if (books != null && books.Count != 0) // Проверка на непустой список книг Автора.
        {
            // Ковенртируем список книг в двумерный массив строк для печати.
            string[][] printDataBooks =
                DataConverter.BooksListToJaggedArrayStr(books, bookHeadersToPrint[1..]);
            string[] indexBooksStr = Array.ConvertAll( // Масив из индексов списка книг (начиная с 1).
                Enumerable.Range(1, books.Count).ToArray(), input => input.ToString());
            PrintTable(printDataBooks, bookHeadersToPrint, indexBooksStr);
        }
        else
        {
            IOController.WriteLine("У этого автора нет списка книг", ConsoleColor.Red);
        }
    }
}