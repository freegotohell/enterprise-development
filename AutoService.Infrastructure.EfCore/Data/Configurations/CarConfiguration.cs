using AutoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoService.Infrastructure.EfCore.Data.Configurations;

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.ToTable("cars");

        builder.HasKey(car => car.Id);

        builder.Property(car => car.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(car => car.LicensePlate).HasColumnName("license_plate").HasMaxLength(16).IsRequired();
        builder.HasIndex(entity => entity.LicensePlate).IsUnique();

        builder.Property(car => car.Brand).HasColumnName("brand").HasMaxLength(100).IsRequired();

        builder.Property(car => car.Model).HasColumnName("model").HasMaxLength(100).IsRequired();

        builder.Property(car => car.Year).HasColumnName("year");

        builder.Property(car => car.ClientId).HasColumnName("client_id");

        builder.HasMany(car => car.Orders).WithOne(order => order.Car).HasForeignKey(order => order.CarId).OnDelete(DeleteBehavior.Restrict);
    }
}
