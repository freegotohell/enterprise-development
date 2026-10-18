namespace AutoService.Domain.Shared.Results;

/// <summary>
/// Represents the result of PUT
/// </summary>
/// <typeparam name="T">The type of the updated entity DTO.</typeparam>
public sealed record UpdateResult<T>(UpdateStatus Status, T? Value = default);