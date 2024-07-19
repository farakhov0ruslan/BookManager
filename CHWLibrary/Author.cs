using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using CustomExceptions;


namespace CHWLibrary;

/// <summary>
/// Represents an author of books.
/// </summary>
[DataContract]
[Serializable]
public class Author : IToJson, IComparable<Author>
{
    /* В этой версии у автора стоят private set. Он используется только лишь для XML десериализации.
     По другому она не работает. */

    /// <summary>
    /// The unique ID of the author.
    /// </summary>
    [DataMember(Name = "authorId", Order = 0)]
    [JsonPropertyName("authorId")]
    public string? AuthorId { get; private set; }

    /// <summary>
    /// The name of the author.
    /// </summary>
    [DataMember(Name = "name", Order = 1)]
    [JsonPropertyName("name")]
    public string? Name { get; private set; }

    /// <summary>
    /// The total earnings of the author.
    /// </summary>
    [DataMember(Name = "earnings", Order = 2)]
    [JsonPropertyName("earnings")]
    public double Earnings { get; private set; }


    /// <summary>
    /// The books written by this author.
    /// </summary>
    [DataMember(Name = "books", Order = 3)]
    [JsonPropertyName("books")]
    public List<Book>? Books { get; private set; }


    /// <summary>
    /// Initializes a new instance of the <see cref="Author"/> class.
    /// </summary>
    [JsonConstructor]
    public Author(string? authorId, string? name, double earnings, List<Book>? books)
    {
        AuthorId = authorId;
        Name = name;
        Earnings = earnings;
        Books = books;
    }

    /// <summary>
    /// Default constructor for <see cref="Author"/> class.
    /// Initializes empty values.
    /// </summary>
    public Author()
    {
        AuthorId = string.Empty;
        Name = string.Empty;
        Books = new List<Book>();
    }

    /// <summary>
    /// Gets the specified field value of the author.
    /// </summary>
    /// <param name="field">The name of the field to get.</param>
    /// <returns>The value of the field as a string.</returns>
    /// <exception cref="ArgumentException">Thrown when an invalid field name is passed.</exception>
    public string GetAuthorField(string field)
    {
        string result = field switch
        {
            "authorid" => AuthorId ?? string.Empty,
            "name" => Name ?? string.Empty,
            "earnings" => Earnings.ToString("F3"),
            _ => throw new ArgumentException("A non-correct class field has been passed")
        };
        return result;
    }


    /// <summary>
    /// Compares this author to another author based on the specified sort field and order.
    /// </summary>
    /// <param name="other">The other author to compare to.</param>
    /// <param name="sortField">The field to sort by.</param>
    /// <param name="reverse">Whether to sort in descending order.</param>
    /// <returns>A value indicating the relative order of the authors.</returns>
    public int CompareTo(Author? other, string? sortField, bool reverse = false)
    {
        int reverseInt = reverse ? 1 : -1;
        if (other == null)
        {
            return reverseInt * 1;
        }

        sortField = (sortField ?? string.Empty).ToLower();

        switch (sortField)
        {
            case "name":
                return reverseInt * string.Compare((Name ?? string.Empty).ToLower(),
                    (other.Name ?? string.Empty).ToLower(),
                    StringComparison.Ordinal);

            case "authorid":
                return reverseInt * string.Compare(AuthorId, other.AuthorId, StringComparison.Ordinal);

            case "earnings":
                return reverseInt * Earnings.CompareTo(other.Earnings);

            default:
                throw new InvalidInputException("Invalid sort field");
        }
    }


    /// <summary>
    /// Compares this author to another author by author ID.
    /// </summary>
    /// <param name="other">The other author to compare to.</param>    
    /// <returns>A value indicating the relative order of the author IDs.</returns>
    public int CompareTo(Author? other)
    {
        if (other == null)
        {
            return 1;
        }

        return string.Compare(AuthorId, other.AuthorId, StringComparison.Ordinal);
    }

    /// <summary>
    /// Event that is raised when author data is updated.
    /// </summary>
    public event EventHandler<DataUpdateEventArgs>? Updated;

    /// <summary>
    /// Raises the <see cref="Updated"/> event.
    /// </summary>
    public void OnUpdated()
    {
        /* При вызове этого метода(изменение какого то значение) подписчик с помощью даты в
         DataUpdateEventArgs сохранит все данные,если после последнего изменения прошло меньше 15 секунд.*/
        Updated?.Invoke(this, new DataUpdateEventArgs(DateTime.Now));
    }


    /// <summary>
    /// Converts this author to a JSON string representation.
    /// </summary>
    /// <returns>JSON string representing this author.</returns>
    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
}