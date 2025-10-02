using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder
            .HasKey(p => p.Id);

        builder
            .Property(p => p.Name)
            .IsRequired();

        builder
            .Property(p => p.Description)
            .IsRequired(false);
    }
}