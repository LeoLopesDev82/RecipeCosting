namespace RecipeCosting.Api.Models.Results;

/// <summary>
/// How a delete ended.
/// </summary>
public enum DeletionOutcome
{
    /// <summary>The record was removed.</summary>
    Deleted,

    /// <summary>No record carries that identifier.</summary>
    NotFound,

    /// <summary>Something else still depends on the record, so it stays.</summary>
    InUse,
}
