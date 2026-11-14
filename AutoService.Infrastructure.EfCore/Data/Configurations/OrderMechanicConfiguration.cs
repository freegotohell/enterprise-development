using AutoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoService.Infrastructure.EfCore.Data.Configurations;

public class OrderMechanicConfiguration : IEntityTypeConfiguration<OrderMechanic>
{
    public void Configure(EntityTypeBuilder<OrderMechanic> builder)
    {
        builder.ToTable("order_mechanics");

        builder.HasKey(orderMechanic => orderMechanic.Id);

        builder.Property(orderMechanic => orderMechanic.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(orderMechanic => orderMechanic.RepairOrderId).HasColumnName("repair_order_id");

        builder.Property(orderMechanic => orderMechanic.MechanicId).HasColumnName("mechanic_id");
    }
}
