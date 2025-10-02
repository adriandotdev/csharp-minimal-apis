public class ProductSupplier
{

    public int Id { get; set; }

    public int ProductId { get; set; }

    public int SupplierId { get; set; }

    // Navigations
    public Product Product { get; set; } = null!;

    public Supplier Supplier { get; set; } = null!;
}