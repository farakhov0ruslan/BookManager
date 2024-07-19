using System.Text.Json;
using CHWLibrary;
using CustomExceptions;
using InOutProcessing;
using MenuProcessing;

namespace _12_Farahov_CHW_3_2;

// Реализована работа с XML форматом. Файл с XML данными лежит в папке с проектом и называется 12V.xml
static class Program
{
    static void Main()
    {
        bool flagExitProgram = false; // Переменная для закрытия программы.
        bool flagNewData = false; // Переменная для повтора программы с теми же исходными данными.
        List<Author>? authorsList = new List<Author>();
        while (!flagExitProgram)
        {
            if (!flagNewData) // Считывание новых даных.
            {
                PrintMenu.InputCorrectNumMenu(2, "start", out int startNumMenu);
                try
                {
                    // Вызов метода с реализацией логики считвания корректных данных из файла.
                    MenuChoise.StartMenu(startNumMenu, out authorsList);
                }
                catch (WrongFileFormatException e) // Отлавливание ошибки неверного формата файла.
                {
                    IOController.WriteLine(e.Message, ConsoleColor.Red);
                    continue;
                }
                catch (JsonException) // Отлавливание ошибки связанной с чтением JSON файла.
                {
                    IOController.WriteLine("В переданном файле данные заданы некорректно, введите другой файл.",
                        ConsoleColor.Red);
                    continue;
                }
                // Отлавливание ошибки связанной с чтением XML файла.
                catch (System.Runtime.Serialization.SerializationException)
                {
                    IOController.WriteLine("В переданном файле данные заданы некорректно, введите другой файл.",
                        ConsoleColor.Red);
                    continue;
                }
                catch (Exception e)
                {
                    IOController.WriteLine(e.Message, ConsoleColor.Red);
                    continue;
                }
            }

            IOController.WriteLine("Данные считаные успешно!", ConsoleColor.Green);

            // Вызов метода для печати меню с сортировкой и иземенением а также для получения корректного номера выбора.
            PrintMenu.InputCorrectNumMenu(3, "change_data",
                out int numChangeDataMenu);
            while (true)
            {
                try
                {
                    // Вызов метода с реализацией логики изменения данных в зависимости от номера выбора.
                    MenuChoise.ChangeDataMenu(numChangeDataMenu, authorsList!, out authorsList);
                    break;
                }
                catch (WrongInputTypeException e) // Отлавливание ошибки неверно введёного значения для изменения поля.
                {
                    IOController.WriteLine(e.Message, ConsoleColor.Red);
                }
                catch (Exception e) // Отлавливание непредвиденной ошибки.
                {
                    IOController.WriteLine(e, ConsoleColor.Red);
                }
            }

            // Вызов метода для печати последнего меню и получение корректного номера выбора.
            PrintMenu.InputCorrectNumMenu(3, "end", out int numEndMenu);
            // Вызов метода с реализацией логики продолжения или завержения программы в зависимости от номера выбора.
            MenuChoise.EndMenu(numEndMenu, out flagExitProgram, out flagNewData);
        }
    }
}