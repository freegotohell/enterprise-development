using AutoService.Domain.Entities;
using AutoService.Domain.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoService.Infrastructure.EfCore.Data.Configurations;

public class WorkTypeConfiguration : IEntityTypeConfiguration<WorkType>
{
    public void Configure(EntityTypeBuilder<WorkType> builder)
    {
        builder.ToTable("work_types");

        builder.HasKey(workType => workType.Id);

        builder.Property(workType => workType.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(workType => workType.Name).HasColumnName("name").HasMaxLength(200).IsRequired();

        builder.Property(workType => workType.Category).HasColumnName("category").HasConversion<int>();

        builder.Property(workType => workType.Cost).HasColumnName("cost").HasPrecision(10, 2);

        builder.Property(workType => workType.Duration).HasColumnName("duration").HasColumnType("int").HasConversion(duration => checked((int)duration.TotalMinutes), minutes => TimeSpan.FromMinutes(minutes));

        builder.Property(workType => workType.Description).HasColumnName("description").HasMaxLength(2000).IsRequired();

        builder.HasMany(workType => workType.Orders).WithOne(orderWork => orderWork.WorkType).HasForeignKey(orderWork => orderWork.WorkTypeId).OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new WorkType
            {
                Id = 1,
                Name = "Oil And Filter Change",
                Category = WorkCategory.Maintenance,
                Cost = 2500,
                Duration = TimeSpan.FromHours(1),
                Description = "Engine Oil And Oil Filter Replacement"
            },

            new WorkType
            {
                Id = 2,
                Name = "Engine Diagnostics",
                Category = WorkCategory.Diagnostics,
                Cost = 5000,
                Duration = TimeSpan.FromHours(2),
                Description = "Computerized Engine Diagnostic Testing"
            },

            new()
            {
                Id = 3,
                Name = "Engine Repair",
                Category = WorkCategory.Engine,
                Cost = 15000,
                Duration = TimeSpan.FromHours(6),
                Description = "Repair Of Engine Components And Systems"
            },
            new()
            {
                Id = 4,
                Name = "Clutch Replacement",
                Category = WorkCategory.Transmission,
                Cost = 20000,
                Duration = TimeSpan.FromHours(6),
                Description = "Clutch Assembly Replacement"
            },

            new()
            {
                Id = 5,
                Name = "Electrical Diagnostics",
                Category = WorkCategory.Electrical,
                Cost = 4000,
                Duration = TimeSpan.FromHours(2),
                Description = "Electrical System Diagnostic And Troubleshooting"
            },

            new()
            {
                Id = 6,
                Name = "Electrical Wiring Repair",
                Category = WorkCategory.Electrical,
                Cost = 7000,
                Duration = TimeSpan.FromHours(3),
                Description = "Repair And Restoration Of Electrical Wiring"
            },

            new()
            {
                Id = 7,
                Name = "Body Repair",
                Category = WorkCategory.BodyRepair,
                Cost = 25000,
                Duration = TimeSpan.FromHours(8),
                Description = "Repair Of Vehicle Body Damage"
            },

            new()
            {
                Id = 8,
                Name = "Routine Vehicle Maintenance",
                Category = WorkCategory.Maintenance,
                Cost = 8000,
                Duration = TimeSpan.FromHours(3),
                Description = "Scheduled Vehicle Maintenance And Inspection"
            },

            new()
            {
                Id = 9,
                Name = "Engine Overhaul",
                Category = WorkCategory.Engine,
                Cost = 50000,
                Duration = TimeSpan.FromHours(12),
                Description = "Complete Engine Rebuild And Overhaul"
            },

            new()
            {
                Id = 10,
                Name = "Transmission Diagnostics",
                Category = WorkCategory.Transmission,
                Cost = 6000,
                Duration = TimeSpan.FromHours(2),
                Description = "Transmission System Diagnostic And Inspection"
            }
        );
    }
}
