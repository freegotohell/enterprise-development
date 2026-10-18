namespace AutoService.Domain.Shared.Results;

/// <summary>
/// Represents the status of PUT
/// </summary>
public enum UpdateStatus
{
    /// <summary>
    /// Entity was successfully updated
    /// </summary>
    Success = 0,

    /// <summary>
    /// Entity was not found
    /// </summary>
    NotFound = 1,

    /// <summary>
    /// One or more referenced entities are missing or incompatible
    /// </summary>
    InvalidReference = 2,

    /// <summary>
    /// Update operation failed
    /// </summary>
    Failed = 3
}