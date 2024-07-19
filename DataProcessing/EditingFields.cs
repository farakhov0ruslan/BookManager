using CHWLibrary;
using CustomExceptions;

namespace DataProcessing;

/// <summary>
/// Provides methods for editing author and book data fields.
/// </summary>
public static class EditingFields
{
    /// <summary>
    /// Edits a field of an author and associated books.
    /// </summary>
    /// <param name="oldAuthor">The original author object.</param>
    /// <param name="nameField">The name of the field to edit.</param>
    /// <param name="newValue">The new value for the field.</param>
    /// <param name="oldBooks">The original books associated with the author.</param>
    /// <returns>New author object with updated data.</returns>
    /// <exception cref="WrongInputTypeException">Thrown when new value has invalid format.</exception> 
    public static Author EditFeld(Author oldAuthor, string nameField, string newValue, params Book[] oldBooks)
    {
        // Устанавливаем значения по умолчанию.
        List<Book> newBooks = new List<Book>();
        Book newBook;
        Book oldBook = new Book();
        Author newAuthor;
        // Собираем данные от прошлого Автора.
        (string authorId, string authorName, double authorEarnings, List<Book> authorBooks) =
            (oldAuthor.GetAuthorField("authorid"), oldAuthor.GetAuthorField("name"), oldAuthor.Earnings,
                oldAuthor.Books ?? new List<Book>());
        if (oldBooks.Length > 0) // Если передали oldBooks значит это Book для изменения его поля.
        {
            newBooks = new List<Book>(authorBooks);
            oldBook = oldBooks[0];
        }

        switch (nameField.ToLower()) // Switch конструкция без учёта регистра.
        {
            case "authorname":
                newAuthor = new Author(authorId, newValue, authorEarnings, authorBooks);
                EventSubscribe.AutoSaverSubscribeAuthor(newAuthor); // Подписка Автора на сохранение данных.
                return newAuthor;

            case "title":
                newBook = new Book(oldBook.BookId, newValue, oldBook.PublicationYear, oldBook.Genre,
                    oldBook.Earnings); // Создаём копию книги с изменёным заголовком.
                newBooks[newBooks.IndexOf(oldBook)] = newBook;
                break;

            case "publication year":
                if (int.TryParse(newValue, out int newValueInt))
                {
                    // Создаём копию книги с изменёным годом публикации.
                    newBook = new Book(oldBook.BookId, oldBook.Title, newValueInt, oldBook.Genre, oldBook.Earnings);
                    newBooks[newBooks.IndexOf(oldBook)] = newBook;
                    break;
                }

                // Если введёное пользователем значение не int, выбрасываем ошибку.
                throw new WrongInputTypeException("Введённое значение не соответвует int формату," +
                                                  " повторите попытку.");

            case "genre":
                newBook = new Book(oldBook.BookId, oldBook.Title, oldBook.PublicationYear, newValue,
                    oldBook.Earnings); // Создаём копию книги с изменёным жанром.
                newBooks[newBooks.IndexOf(oldBook)] = newBook;
                break;

            case "earnings": // Вызывается только при исполнении события изменение дохода киниги и автора.
                if (double.TryParse(newValue, out double newValueDouble))
                {
                    newBook = new Book(oldBook.BookId, oldBook.Title, oldBook.PublicationYear, oldBook.Genre,
                        newValueDouble); // Создаём копию книги с изменёным доходом.
                    newBooks[newBooks.IndexOf(oldBook)] = newBook;
                    authorEarnings = newBooks.Sum(x => x.Earnings);
                    break;
                }

                // Если введёное пользователем значение не int, выбрасываем ошибку.
                throw new WrongInputTypeException(
                    "Введённое значение не соответвует double формату, повторите попытку.");
        }

        // Создаём нового автора и подписываем его на все события.
        newAuthor = new Author(authorId, authorName, authorEarnings, newBooks);
        EventSubscribe.AllHandlersSubscribe(new List<Author>(new[] { newAuthor }));
        return newAuthor;
    }
}