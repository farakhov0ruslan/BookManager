namespace CustomExceptions;

/// <summary>
/// The exception that is thrown when a wrong user input type is passeda.
/// </summary>
public class WrongInputTypeException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WrongInputTypeException"/> class.
    /// </summary>
    public WrongInputTypeException()
    {
    }

    /// <summary> 
    /// Initializes a new instance of the <see cref="WrongInputTypeException"/>
    /// class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public WrongInputTypeException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WrongInputTypeException"/> class with a specified error 
    /// message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="exception">The exception that is the cause of the current exception,
    /// or a null reference if no inner exception is specified.</param>
    public WrongInputTypeException(string message, Exception exception) : base(message, exception)
    {
    }
}