public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? SKU { get; set; }

    public string? BarCode { get; set; }

    public int CategoryId { get; set; }

    public decimal Price { get; set; }

    public decimal CostPrice { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int CurrentStock { get; set; } = 0;

    public int LowStockThreshold { get; set; } = 0;

    // Navigations
    public ICollection<ProductSupplier> ProductSuppliers { get; set; } = [];

    public Category Category { get; set; } = null!;
}