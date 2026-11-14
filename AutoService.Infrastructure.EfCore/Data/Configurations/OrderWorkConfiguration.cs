using AutoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoService.Infrastructure.EfCore.Data.Configurations;

public class OrderWorkConfiguration : IEntityTypeConfiguration<OrderWork>
{
    public void Configure(EntityTypeBuilder<OrderWork> builder)
    {
        builder.ToTable("order_works");

        builder.HasKey(orderWork => orderWork.Id);

        builder.Property(orderWork => orderWork.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(orderWork => orderWork.RepairOrderId).HasColumnName("repair_order_id");

        builder.Property(orderWork => orderWork.WorkTypeId).HasColumnName("work_type_id");
    }
}
