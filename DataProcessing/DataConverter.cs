using CHWLibrary;

namespace DataProcessing;

/// <summary>
/// Provides methods for converting data between types.
/// </summary>
public static class DataConverter
{
    /// <summary>
    /// Converts a list of authors to a jagged string array.
    /// </summary>
    /// <param name="data">List of authors.</param>
    /// <param name="fields">Author fields to include.</param>
    /// <returns>Jagged string array of author data.</returns>
    public static string[][] AuthorsListToJaggedArrayStr(List<Author> data, string[] fields)
    {
        string[][] result = new string[data.Count][];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = new string[fields.Length];
            for (int j = 0; j < fields.Length; j++)
            {
                result[i][j] = data[i].GetAuthorField(fields[j].ToLower());
            }
        }

        return result;
    }

    /// <summary> 
    /// Converts a list of books to a jagged string array.
    /// </summary>
    /// <param name="data">List of books.</param>
    /// <param name="fields">Book fields to include.</param>
    /// <returns>Jagged string array of book data.</returns>    
    public static string[][] BooksListToJaggedArrayStr(List<Book> data, string[] fields)
    {
        string[][] result = new string[data.Count][];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = new string[fields.Length];
            for (int j = 0; j < fields.Length; j++)
            {
                result[i][j] = data[i].GetBookField(fields[j].ToLower());
            }
        }

        return result;
    }
}