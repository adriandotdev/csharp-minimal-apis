using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SupplierEntityTypeConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.HasKey(s => s.Id);

        builder
            .Property(s => s.Name)
            .IsRequired();

        builder
            .Property(s => s.ContactPerson)
            .IsRequired();

        builder
            .Property(s => s.PhoneNumber)
            .IsRequired();

        builder
            .Property(s => s.Email)
            .IsRequired();

        builder
            .Property(s => s.Address)
            .IsRequired();

    }
}