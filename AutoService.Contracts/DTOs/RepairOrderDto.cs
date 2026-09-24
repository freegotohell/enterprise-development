namespace AutoService.Contracts.DTOs;

/// <summary>
/// Repair order returned by API
/// </summary>
public class RepairOrderDto
{
    /// <summary>
    /// Unique id of the repair order
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Id of the client associated with the repair order
    /// </summary>
    public int ClientId { get; set; }

    /// <summary>
    /// Id of the car associated with the repair order
    /// </summary>
    public int CarId { get; set; }

    /// <summary>
    /// Date and time when the car was admitted for repair
    /// </summary>
    public DateTime AdmissionDate { get; set; }

    /// <summary>
    /// Date and time when the repair order was completed
    /// </summary>
    public DateTime? ReleaseDate { get; set; }

    /// <summary>
    /// Ids of mechanics assigned to the repair order
    /// </summary>
    public List<int> MechanicIds { get; set; } = [];

    /// <summary>
    /// Ids of work types included in the repair order
    /// </summary>
    public List<int> WorkTypeIds { get; set; } = [];
}