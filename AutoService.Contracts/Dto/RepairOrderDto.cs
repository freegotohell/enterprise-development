namespace AutoService.Contracts.Dto;

/// <summary>
/// Repair order returned by API
/// </summary>
public class RepairOrderDto
{
    /// <summary>
    /// Unique id of the repair order
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Id of the client associated with the repair order
    /// </summary>
    public int ClientId { get; init; }

    /// <summary>
    /// Id of the car associated with the repair order
    /// </summary>
    public int CarId { get; init; }

    /// <summary>
    /// Date and time when the car was admitted for repair
    /// </summary>
    public DateTime AdmissionDate { get; init; }

    /// <summary>
    /// Date and time when the repair order was completed
    /// </summary>
    public DateTime? ReleaseDate { get; init; }

    /// <summary>
    /// Ids of mechanics assigned to the repair order
    /// </summary>
    public IReadOnlyList<int> MechanicIds { get; init; } = [];

    /// <summary>
    /// Ids of work types included in the repair order
    /// </summary>
    public IReadOnlyList<int> WorkTypeIds { get; init; } = [];
}