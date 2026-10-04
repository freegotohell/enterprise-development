using AutoService.Domain.Shared.Enums;

namespace AutoService.Domain.Shared.Mapping;

/// <summary>
/// Mapping work categories and mechanic specializations
/// </summary>
public static class MechanicSpecializationMapping
{
    /// <summary>
    /// Determines whether a mechanic specialization is suitable for a work category
    /// </summary>
    public static bool Matches(MechanicSpecialization specialization, WorkCategory category)
    {
        return category switch
        {
            WorkCategory.Engine => specialization == MechanicSpecialization.Engine,

            WorkCategory.Transmission => specialization == MechanicSpecialization.Transmission,

            WorkCategory.Electrical => specialization == MechanicSpecialization.Electrical,

            WorkCategory.Diagnostics => specialization == MechanicSpecialization.Diagnostics,

            WorkCategory.BodyRepair => specialization == MechanicSpecialization.BodyRepair,

            WorkCategory.Maintenance => true,

            _ => false
        };
    }
}