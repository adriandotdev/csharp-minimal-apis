using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProductSupplierTypeConfiguration : Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<ProductSupplier>
{
    public void Configure(EntityTypeBuilder<ProductSupplier> builder)
    {
        builder.HasKey(ps => ps.Id);

        builder
            .HasOne(ps => ps.Product)
            .WithMany(p => p.ProductSuppliers)
            .HasForeignKey(ps => ps.ProductId);

        builder
            .HasOne(ps => ps.Supplier)
            .WithMany(s => s.ProductSuppliers)
            .HasForeignKey(ps => ps.SupplierId);
    }
}