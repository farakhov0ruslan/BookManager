namespace DataProcessing;

using CHWLibrary;
using SaveProcessing;

/// <summary>
/// Provides methods for subscribing to events.
/// </summary>
public static class EventSubscribe
{
    /// <summary>
    /// Subscribes an auto-save handler to the book's update event. 
    /// </summary> 
    private static void AutoSaverSubscribeBook(Book book)
    {
        book.Updated += AutoSaver.HandleUpdate;
    }

    /// <summary>
    /// Subscribes an auto-save handler to the author's update event.
    /// </summary>
    public static void AutoSaverSubscribeAuthor(Author author)
    {
        author.Updated += AutoSaver.HandleUpdate;
    }

    /// <summary> 
    /// Subscribes a book earnings recalculation handler 
    /// to the book's earnings change event.
    /// </summary>
    private static void BookEarningsEditSubscribe(Book book)
    {
        book.BookEarningsChange += BookEarningsEdit.RecalculateEarnings;
    }


    /// <summary>
    /// Subscribes data update handlers to all books and 
    /// authors in the provided list.
    /// </summary>
    /// <param name="authors">List of authors to subscribe.</param>
    public static void AllHandlersSubscribe(List<Author>? authors)
    {
        foreach (var author in authors ?? new List<Author>())
        {
            AutoSaverSubscribeAuthor(author); // Подписываем сохранение на автора.
            foreach (var book in author.Books ?? new List<Book>())
            {
                AutoSaverSubscribeBook(book); // Подписываем сохранение на кингу.
                BookEarningsEditSubscribe(book); // Подписываем на измененеие дохода на книгу.
            }
        }
    }
}