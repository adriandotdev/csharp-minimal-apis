using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Namotion.Reflection;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.ID);

        builder
            .Property(u => u.Name)
            .IsRequired();

        builder
            .Property(u => u.Email)
            .IsRequired();

        builder
            .Property(u => u.Username)
            .IsRequired();

        builder
            .Property(u => u.Password)
            .HasColumnType("VARCHAR(255)")
            .IsRequired();

        builder
            .Property(u => u.Roles)
            .HasConversion<string>()
            .IsRequired();

        builder
            .Property(u => u.Status)
            .HasConversion<string>()
            .IsRequired();
            
        builder
            .Property(u => u.LastLoginAt)
            .HasDefaultValueSql("NULL");

        builder
            .Property(u => u.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder
            .Property(u => u.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}