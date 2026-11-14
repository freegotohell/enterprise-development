using AutoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoService.Infrastructure.EfCore.Data.Configurations;

public class RepairOrderConfiguration : IEntityTypeConfiguration<RepairOrder>
{
    public void Configure(EntityTypeBuilder<RepairOrder> builder)
    {
        builder.ToTable("repair_orders");

        builder.HasKey(order => order.Id);

        builder.Property(order => order.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(order => order.ClientId).HasColumnName("client_id");

        builder.Property(order => order.CarId).HasColumnName("car_id");

        builder.Property(order => order.AdmissionDate).HasColumnName("admission_date");

        builder.Property(order => order.ReleaseDate).HasColumnName("release_date");

        builder.HasMany(order => order.Mechanics).WithOne(orderMechanic => orderMechanic.RepairOrder).HasForeignKey(orderMechanic => orderMechanic.RepairOrderId).OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(order => order.Works).WithOne(orderWork => orderWork.RepairOrder).HasForeignKey(orderWork => orderWork.RepairOrderId).OnDelete(DeleteBehavior.Cascade);
    }
}
