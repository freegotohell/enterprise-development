using AutoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoService.Infrastructure.EfCore.Data.Configurations;

public class MechanicConfiguration : IEntityTypeConfiguration<Mechanic>
{
    public void Configure(EntityTypeBuilder<Mechanic> builder)
    {
        builder.ToTable("mechanics");

        builder.HasKey(mechanic => mechanic.Id);

        builder.Property(mechanic => mechanic.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(mechanic => mechanic.PassportNumber).HasColumnName("passport_number").HasMaxLength(10).IsRequired();
        builder.HasIndex(entity => entity.PassportNumber).IsUnique();

        builder.Property(mechanic => mechanic.FullName).HasColumnName("full_name").HasMaxLength(200).IsRequired();

        builder.Property(mechanic => mechanic.Specialization).HasColumnName("specialization").HasConversion<int>();

        builder.Property(mechanic => mechanic.Experience).HasColumnName("experience");

        builder.HasMany(mechanic => mechanic.Orders).WithOne(orderMechanic => orderMechanic.Mechanic).HasForeignKey(orderMechanic => orderMechanic.MechanicId).OnDelete(DeleteBehavior.Restrict);
    }
}
