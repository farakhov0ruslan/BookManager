using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CHWLibrary;

public interface IToJson
{
    string ToJson();
}

/// <summary>
/// Represents a book written by an author.
/// </summary>
[DataContract]
[Serializable]
public class Book : IToJson, IEquatable<Book>
{
    /* В этой версии у книги стоят private set. Он используется только лишь для XML десериализации.
     По другому она не работает. */

    /// <summary>
    /// Unique ID of the book.
    /// </summary>
    [DataMember(Name = "bookId")]
    [JsonPropertyName("bookId")]
    public string BookId { get; private set; } = string.Empty;

    /// <summary>
    /// Title of the book.
    /// </summary>
    [DataMember(Name = "title")]
    [JsonPropertyName("title")]
    public string Title { get; private set; } = string.Empty;

    /// <summary>
    /// Publication year of the book.
    /// </summary>
    [DataMember(Name = "publicationYear")]
    [JsonPropertyName("publicationYear")]
    public int PublicationYear { get; private set; }

    /// <summary>
    /// Genre of the book.
    /// </summary>
    [DataMember(Name = "genre")]
    [JsonPropertyName("genre")]
    public string Genre { get; private set; } = string.Empty;

    /// <summary>
    /// Total earnings from the book.
    /// </summary>
    [DataMember(Name = "earnings")]
    [JsonPropertyName("earnings")]
    public double Earnings { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Book"/> class.
    /// </summary>
    [JsonConstructor]
    public Book(string bookId, string title, int publicationYear, string genre, double earnings)
    {
        BookId = bookId;
        Title = title;
        PublicationYear = publicationYear;
        Genre = genre;
        Earnings = earnings;
    }


    /// <summary>
    /// Default constructor for <see cref="Book"/> class.
    /// </summary>
    public Book()
    {
    }


    /// <summary>
    /// Gets the value of the specified field.
    /// </summary>
    /// <param name="field">The field name.</param>
    /// <returns>The value as a string.</returns>
    /// <exception cref="ArgumentException">Invalid field name passed.</exception>
    public string GetBookField(string field)
    {
        string result = field switch
        {
            "bookid" => BookId,
            "title" => Title,
            "publicationyear" => PublicationYear.ToString(),
            "genre" => Genre,
            "earnings" => Earnings.ToString("F3"),
            _ => throw new ArgumentException("A non-correct class field has been passed")
        };
        return result;
    }


    /// <summary>
    /// Delegate for changing book earnings.
    /// </summary>
    /// <param name="changeBook">The book to change.</param>
    /// <param name="authors">The list of authors.</param>
    /// <param name="newEarningStr">New earning value as string.</param>
    /// <returns>Updated list of authors.</returns>
    public delegate List<Author> ChangeBookEarnings(Book changeBook, List<Author> authors, string newEarningStr);

    /// <summary>
    /// Event that is invoked when book earnings change.
    /// </summary>
    public event ChangeBookEarnings? BookEarningsChange;

    /// <summary>
    /// Raises the <see cref="BookEarningsChange"/> event.
    /// </summary>
    public List<Author>? OnChangeEarnings(List<Author> authors, string newEarningStr)
    {
        /* При вызове этого метода подписчик пройдётся по списку authors, и если обнаружит у себя кнгиу такую же как this
         то создай копию этой книги с новым доходом, а также пересчетает свой доход.*/
        return BookEarningsChange?.Invoke(this, authors, newEarningStr);
    }


    /// <summary>
    /// Event that is raised when book data is updated.
    /// </summary>
    public event EventHandler<DataUpdateEventArgs>? Updated;

    /// <summary>
    /// Raises the <see cref="Updated"/> event. 
    /// </summary>
    public void OnUpdated(DataUpdateEventArgs e)
    {
        /* При вызове этого метода(изменение какого то значение) подписчик с помощью даты в
         DataUpdateEventArgs сохранит все данные,если после последнего изменения прошло меньше 15 секунд.*/
        Updated?.Invoke(this, e);
    }

    /// <summary>
    /// Defines equality operator between two <see cref="Book"/> objects.
    /// </summary>
    public static bool operator ==(Book? book1, Book? book2)
    {
        if (book1 is null || book2 is null)
        {
            return false;
        }

        return book1.BookId == book2.BookId;
    }

    /// <summary>
    /// Defines inequality operator between two <see cref="Book"/> objects.
    /// </summary>
    public static bool operator !=(Book? book1, Book? book2)
    {
        return !(book1 == book2);
    }

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    public bool Equals(Book? other)
    {
        return this == other;
    }

    /// <summary> 
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    public override bool Equals(object? obj)
        => obj is Book objS && Equals(objS);

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    public override int GetHashCode() => BookId.GetHashCode();


    /// <summary>
    /// Converts this book to a JSON string representation.
    /// </summary>
    /// <returns>JSON string representing this book.</returns>
    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
}