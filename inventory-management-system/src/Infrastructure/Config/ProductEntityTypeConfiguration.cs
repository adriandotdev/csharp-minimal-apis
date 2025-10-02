using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2);

        builder
            .Property(p => p.CostPrice)
            .HasColumnType("decimal(18, 2)")
            .HasPrecision(18, 2);

        builder
            .Property(p => p.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder
            .Property(p => p.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Optional Properties / Columns
        builder
            .Property(p => p.Description)
            .IsRequired(false);

        builder.Property(p => p.SKU)
        .IsRequired(false);

        builder.Property(p => p.BarCode)
            .IsRequired(false);

        builder.Property(p => p.ImageUrl)
        .IsRequired(false);

        builder
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(p => p.CurrentStock)
            .HasDefaultValueSql("0");

        builder
            .Property(p => p.LowStockThreshold)
            .HasDefaultValueSql("0");
    }
}