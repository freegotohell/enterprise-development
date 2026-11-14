using AutoService.Domain.Entities;
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

        builder.Property(workType => workType.Name).HasColumnName("name").IsRequired();

        builder.Property(workType => workType.Category).HasColumnName("category").HasConversion<int>();

        builder.Property(workType => workType.Cost).HasColumnName("cost").HasPrecision(10, 2);

        builder.Property(workType => workType.Duration).HasColumnName("duration").HasColumnType("int").HasConversion(duration => checked((int)duration.TotalMinutes), minutes => TimeSpan.FromMinutes(minutes));

        builder.Property(workType => workType.Description).HasColumnName("description").IsRequired();

        builder.HasMany(workType => workType.Orders).WithOne(orderWork => orderWork.WorkType).HasForeignKey(orderWork => orderWork.WorkTypeId).OnDelete(DeleteBehavior.Restrict);
    }
}
