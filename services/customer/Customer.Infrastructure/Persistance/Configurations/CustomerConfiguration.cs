using Customer.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Customer.Infrastructure.Persistence.Configurations;

public sealed class CustomerConfiguration
    : IEntityTypeConfiguration<CustomerEntity>
{
    public void Configure(
        EntityTypeBuilder<CustomerEntity> builder)
    {
        builder.ToTable("customers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnName("email")
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.PhoneNumber)
            .HasColumnName("phone_number")
            .HasMaxLength(30);

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.OwnsMany(
            x => x.Addresses,
            addressBuilder =>
            {
                addressBuilder.ToTable("customer_addresses");

                addressBuilder.WithOwner()
                    .HasForeignKey("customer_id");

                addressBuilder.HasKey(x => x.Id);

                addressBuilder.Property(x => x.Id)
                    .HasColumnName("id");

                addressBuilder.Property(x => x.Line1)
                    .HasColumnName("line1")
                    .HasMaxLength(200)
                    .IsRequired();

                addressBuilder.Property(x => x.City)
                    .HasColumnName("city")
                    .HasMaxLength(100)
                    .IsRequired();

                addressBuilder.Property(x => x.Country)
                    .HasColumnName("country")
                    .HasMaxLength(100)
                    .IsRequired();

                addressBuilder.Property(x => x.PostalCode)
                    .HasColumnName("postal_code")
                    .HasMaxLength(30)
                    .IsRequired();
            });

        builder.Navigation(x => x.Addresses)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
