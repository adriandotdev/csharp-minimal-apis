using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Namotion.Reflection;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductEntityTypeConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CategoryEntityTypeConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntityTypeConfiguration).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder
        .UseSeeding((context, _) =>
        {
            context.Set<Supplier>().Add(
                new Supplier
                {
                    Id = 1,
                    Name = "Nestle",
                    ContactPerson = "John Doe",
                    PhoneNumber = "+639120000000",
                    Email = "johndoe@gmail.com",
                    Address = "Caloocan City"
                });

            context.Set<Supplier>().Add(
                new Supplier
                {
                    Id = 2,
                    Name = "Meralco",
                    ContactPerson = "Wizz Kher",
                    PhoneNumber = "+639180000000",
                    Email = "wiz@gmail.com",
                    Address = "Davao City"
                });

            context.Set<Supplier>().Add(
                new Supplier
                {
                    Id = 3,
                    Name = "Meralco 3",
                    ContactPerson = "Johnny Wow",
                    PhoneNumber = "+639150000000",
                    Email = "johnnywow@gmail.com",
                    Address = "Isabela City"
                });

            context.SaveChanges();
        })
        .UseAsyncSeeding(async (context, _, cancellationToken) =>
        {
            context.Set<Supplier>().Add(
               new Supplier
               {
                   Id = 1,
                   Name = "Nestle",
                   ContactPerson = "John Doe",
                   PhoneNumber = "+639120000000",
                   Email = "johndoe@gmail.com",
                   Address = "Caloocan City"
               });

            context.Set<Supplier>().Add(
                new Supplier
                {
                    Id = 2,
                    Name = "Meralco",
                    ContactPerson = "Wizz Kher",
                    PhoneNumber = "+639180000000",
                    Email = "wiz@gmail.com",
                    Address = "Davao City"
                });
                
            context.Set<Supplier>().Add(
                new Supplier
                {
                    Id = 3,
                    Name = "Meralco 3",
                    ContactPerson = "Johnny Wow",
                    PhoneNumber = "+639150000000",
                    Email = "johnnywow@gmail.com",
                    Address = "Isabela City"
                });

            await context.SaveChangesAsync(cancellationToken);
        });
    }
}