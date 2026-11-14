using AutoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoService.Infrastructure.EfCore.Data.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("clients");

        builder.HasKey(client => client.Id);

        builder.Property(client => client.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(client => client.FullName).HasColumnName("full_name").IsRequired();

        builder.Property(client => client.Phone).HasColumnName("phone").IsRequired();

        builder.HasMany(client => client.Cars).WithOne(car => car.Client).HasForeignKey(car => car.ClientId).OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(client => client.Orders).WithOne(order => order.Client).HasForeignKey(order => order.ClientId).OnDelete(DeleteBehavior.Restrict);
    }
}
