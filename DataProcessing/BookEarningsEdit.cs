using CHWLibrary;

namespace DataProcessing;

/// <summary>
/// Provides methods for recalculating book earnings.
/// </summary>
public static class BookEarningsEdit
{
    /// <summary>
    /// Recalculates the earnings for a book across associated authors.
    /// </summary>
    /// <param name="sender">The book that had a change in earnings.</param>
    /// <param name="changeAuthorsData">List of authors to update.</param>
    /// <param name="newEarningStr">New earning value as string.</param>
    /// <returns>Updated list of authors.</returns>
    public static List<Author> RecalculateEarnings(Book sender, List<Author> changeAuthorsData, string newEarningStr)
    {
        for (int i = 0; i < changeAuthorsData.Count; i++)
        {
            foreach (var b in changeAuthorsData[i].Books ?? new List<Book>())
            {
                // Если у какого то автора такая же книга, на его место ставим его копию с новым доходом.
                if (b == sender)
                {
                    changeAuthorsData[i] = EditingFields.EditFeld(changeAuthorsData[i], "earnings",
                        newEarningStr, sender);
                }
            }
        }

        return changeAuthorsData;
    }
}