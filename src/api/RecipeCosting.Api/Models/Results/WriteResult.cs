namespace RecipeCosting.Api.Models.Results;

/// <summary>
/// How a write against an existing record ended.
/// </summary>
public enum WriteOutcome
{
    /// <summary>The record was replaced.</summary>
    Written,

    /// <summary>No record carries the identifier that was asked for.</summary>
    NotFound,

    /// <summary>Someone else replaced the record after this caller read it.</summary>
    Stale,
}

/// <summary>
/// The outcome of a write, and the record when there is one to return.
/// </summary>
/// <typeparam name="T">What the caller gets back when the write succeeds.</typeparam>
public class WriteResult<T>
{
    private WriteResult(WriteOutcome outcome, T? value)
    {
        Outcome = outcome;
        Value = value;
    }

    /// <summary>How the write ended.</summary>
    public WriteOutcome Outcome { get; }

    /// <summary>The record as it was stored, when the write succeeded.</summary>
    public T? Value { get; }

    /// <summary>The record was replaced.</summary>
    public static WriteResult<T> Written(T value) => new(WriteOutcome.Written, value);

    /// <summary>No record carries that identifier.</summary>
    public static WriteResult<T> NotFound() => new(WriteOutcome.NotFound, default);

    /// <summary>The caller was working from a copy someone else has replaced.</summary>
    public static WriteResult<T> Stale() => new(WriteOutcome.Stale, default);
}
