using System.Runtime.Serialization;
using System.Text.Json;
using System.Xml;
using CHWLibrary;
using DataProcessing;
using InOutProcessing;
using SaveProcessing;
using Checker;
using CustomExceptions;

namespace MenuProcessing;

/// <summary>
/// Provides methods for user interface and menu processing.
/// </summary>
public static class MenuChoise
{
    /// <summary>
    /// Displays main menu and loads author data from file. 
    /// </summary>
    /// <param name="numMenu">Selected menu number</param>
    /// <param name="data">Output loaded author data</param>
    public static void StartMenu(int numMenu, out List<Author>? data)
    {
        IOController.PrintSeparators();
        InputProcessing.StringCorrectnessTemplate correctPathToFile = InputProcessing.CorrectPathToFile;
        string[] addConditions = Array.Empty<string>();
        switch (numMenu) // Switch конструкция для выбора с каким расширением работать(нужна для масштабирования).
        {
            case 1:
                addConditions = new[] { "json" };
                break;
            case 2:
                addConditions = new[] { "xml" };
                break;
        }

        string expansion = addConditions[0];
        // Получаем корректный путь до файла.
        string path = InputProcessing.GetCorrectStringFromConsole("Введите путь до файла c расширением: ",
            correctPathToFile, addConditions);

        // Проверка что полученный файл соответсвует нужному нам шаблону.
        if (!JsonFileCorrectnessController.Check(path))
        {
            throw new WrongFileFormatException(
                "Файл по пути, который вы ввели, содержит неккоректный формат данных, повторите ввод.");
        }

        // Получаем данные из файла.
        if (expansion == "json")
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                data = JsonSerializer.Deserialize<List<Author>>(fileStream);
            }
        }
        else
        {
            // Получаем данные из XML.
            DataContractSerializer serializer = new DataContractSerializer(typeof(List<Author>));
            using (FileStream fs = File.OpenRead(path))
            {
                // Десериализуем данные
                data = (List<Author>)(serializer.ReadObject(fs) ?? new List<Author>());
            }
        }


        // Инициализируем статические данные в классе для сохранения для дальнещей работы с ними.
        AutoSaver.Expansion = expansion;
        AutoSaver.FilePath = path;

        // Подписываем обработчики на данные из из файла.
        EventSubscribe.AllHandlersSubscribe(data);
    }

    /// <summary>
    /// Allows user to edit a book of an author.
    /// </summary>
    /// <param name="authorData">List of authors</param>
    /// <param name="indexAuthor">Index of author to edit</param>
    /// <returns>Updated author list</returns>
    private static List<Author>? ChangeBookField(List<Author>? authorData, int indexAuthor)
    {
        // Переменная списка книг у выбранного пользователем автора.
        var books = authorData![indexAuthor].Books;
        if (books != null)
        {
            // Метод печатающий меню для выбора книги из списка книг автора, передаёт индекс этой книги.
            PrintMenu.InputCorrectChoiceBook(books, out int indexBook);

            /* Метод печатающий меню для выбора поля книги для изменения из списка возможных изменений,
            передаёт название этого поля. */
            PrintMenu.InputCorrectStrForMenu(InputProcessing.CorrectChangeBookField,
                "change_book_fields", out string changeBookFiled);

            // Метод возвращаеющий непустое значение, полученное от пользователя.
            string newValue = InputProcessing.GetCorrectStringFromConsole("Новое значение: ",
                InputProcessing.NotNullStr);

            if (changeBookFiled.ToLower() == "earnings")
            {
                /* Если пользователь ввёл изменение дохода книги, вызываем события для
                 пересчёта доходов у всех авторов с такой-же книгой */
                authorData = books[indexBook].OnChangeEarnings(authorData, newValue);
            }
            else // Иначе просто изменяем в списке всех авторов, автора у которого будет новая книга с новым значением.
            {
                authorData[indexAuthor] = EditingFields.EditFeld(authorData[indexAuthor],
                    changeBookFiled, newValue, books[indexBook]);
            }
        }
        else
        {
            IOController.WriteLine("У этого автора нет списка книг.", ConsoleColor.Red);
        }

        return authorData;
    }

    /// <summary>
    /// Prints author data table and optional book details.
    /// </summary>
    /// <param name="dataAuthors">List of authors to print</param>
    private static void PrintAuthorsDataTable(List<Author>? dataAuthors)
    {
        OutputProcessing.PrintAuthorsTable(dataAuthors); // Выводим таблицу авторов.
        IOController.WriteLine("Хотите узнать дополнительные сведения о книгах?", ConsoleColor.Cyan);

        // Метод который получает от пользователя ответ, хочет ли он получить доп сведения о книгах какого то автора.
        PrintMenu.InputCorrectNumMenu(2, "books_info", out int numBooksInfoMenu);
        if (numBooksInfoMenu == 1)
        {
            IOController.WriteLine("Введите номер автора, о которым хотите получить доп информацию" +
                                   " (номера - левый столбец таблицы)", ConsoleColor.Cyan);
            if (dataAuthors != null)
            {
                int indexAuthor = InputProcessing.GetCorrectIntFromConsole("Номер автора: ",
                    1, dataAuthors.Count) - 1;
                OutputProcessing.PrintBooksTable(dataAuthors[indexAuthor].Books);
            }
        }
    }

    /// <summary>
    /// Displays change data menu and processes user choices.
    /// </summary>
    /// <param name="numMenu">Selected menu number</param>
    /// <param name="sourceList">Original author list</param>
    /// <param name="changeDataList">Output modified author list</param>
    public static void ChangeDataMenu(int numMenu, List<Author> sourceList, out List<Author>? changeDataList)
    {
        changeDataList = new List<Author>(sourceList); // Список с отсортированными(изменёнными данными).
        IOController.PrintSeparators();
        switch (numMenu)
        {
            // Сортировка списка авторов.
            case 1:
                // Метод для получения корректного имени поля по которому сортировать.
                PrintMenu.InputCorrectStrForMenu(InputProcessing.CorrectSortField, "sort_fields",
                    out string sortField);

                PrintMenu.InputCorrectNumMenu(2, "sort_order", out int numSortOrderMenu);
                bool sortOrder = numSortOrderMenu == 1; // Простым сравнием получае bool значение порядка сортировки.
                changeDataList.Sort((x, y) => x.CompareTo(y, sortField, sortOrder));
                break;

            // Изменения значения автора или поля.
            case 2:
                // Метод для получения корректного индекса автора, которого выберет ползователь.
                PrintMenu.InputCorrectChoiceAuthor(changeDataList, out int indexAuthor);

                // Метод для получения корректного варианта для изменения поля или автора, или поля у одной из книг. 
                PrintMenu.InputCorrectStrForMenu(InputProcessing.CorrectChangeAuthorField,
                    "change_autor_fields", out string changeAuthorFiled);

                // Если изменение поля автора(name единственное разрещенное поля для изменения).
                if (changeAuthorFiled.ToLower() == "name")
                {
                    // Получаем непустое новое значение для изменения от пользователя.
                    string newName = InputProcessing.GetCorrectStringFromConsole("Новое значение: ",
                        InputProcessing.NotNullStr);

                    // Метод для получения копии обьекта автора с новым именем.
                    changeDataList[indexAuthor] = EditingFields.EditFeld(changeDataList[indexAuthor],
                        "authorname", newName);
                }
                else
                {
                    /* Вызываем методя для изменении одного из полей книги, и получаем список с
                     измененным полем книги и автором это книги (Или изменённым доходом у некоторых авторов). */
                    changeDataList = ChangeBookField(changeDataList, indexAuthor);
                }

                AutoSaver.Authors = changeDataList;
                changeDataList?[indexAuthor].OnUpdated(); // Вызываем возможное сохранения данных.
                break;
            case 3:
                changeDataList = sourceList;
                break;
        }

        AutoSaver.Authors = changeDataList;
        // Печатаем таблицу списка авторов, и взависимости от овтета, таблицу списка книг одного из авторов.
        PrintAuthorsDataTable(changeDataList);
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