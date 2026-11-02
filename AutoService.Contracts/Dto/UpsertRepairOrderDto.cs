using System.ComponentModel.DataAnnotations;

namespace AutoService.Contracts.Dto;

/// <summary>
/// Data required to create or update a repair order
/// </summary>
public class UpsertRepairOrderDto : IValidatableObject
{
    /// <summary>
    /// Id of the client associated with the repair order
    /// </summary>
    [Range(1, int.MaxValue)]
    public int ClientId { get; init; }

    /// <summary>
    /// Id of the car associated with the repair order
    /// </summary>
    [Range(1, int.MaxValue)]
    public int CarId { get; init; }

    /// <summary>
    /// Date and time when the car was admitted for repair
    /// </summary>
    [Required]
    public DateTime? AdmissionDate { get; init; }

    /// <summary>
    /// Date and time when the repair order was completed
    /// </summary>
    public DateTime? ReleaseDate { get; init; }

    /// <summary>
    /// Ids of mechanics to assign to the repair order
    /// </summary>
    [MinLength(1)]
    public IReadOnlyList<int> MechanicIds { get; init; } = [];

    /// <summary>
    /// Ids of work types to include in the repair order
    /// </summary>
    [MinLength(1)]
    public IReadOnlyList<int> WorkTypeIds { get; init; } = [];

    /// <inheritdoc />
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!AdmissionDate.HasValue)
        {
            yield return new ValidationResult("Admission date is required", [nameof(AdmissionDate)]);
            yield break;
        }
        if (AdmissionDate > DateTime.Now)
        {
            yield return new ValidationResult("Admission date cannot be in the future", [nameof(AdmissionDate)]);
        }
        if (ReleaseDate.HasValue && ReleaseDate.Value < AdmissionDate)
        {
            yield return new ValidationResult("Release date cannot be earlier than admission date", [nameof(ReleaseDate)]);
        }
    }
}