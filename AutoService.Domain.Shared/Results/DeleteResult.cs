namespace AutoService.Domain.Shared.Results;
/// <summary>
/// Represents the result of DELETE
/// </summary>
public enum DeleteResult
{
    /// <summary>
    /// Entity existed and successfully deleted
    /// </summary>
    Deleted = 0,
    /// <summary>
    /// No such entity
    /// </summary>
    NotFound = 1,
    /// <summary>
    /// Entity exists but cannot be deleted due to related data
    /// </summary>
    HasRelatedEntities = 2
}