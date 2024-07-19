namespace CHWLibrary;

/// <summary>
/// Provides data for events that report data updates.
/// </summary>
public class DataUpdateEventArgs : EventArgs
{
    /// <summary>
    /// Gets the time the data was updated.
    /// </summary>
    public DateTime UpdateTime { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataUpdateEventArgs"/> class.
    /// </summary>
    public DataUpdateEventArgs()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataUpdateEventArgs"/> class.
    /// </summary>
    /// <param name="updateTime">The update time.</param>
    public DataUpdateEventArgs(DateTime updateTime)
    {
        UpdateTime = updateTime;
    }
}