public record GetProductsResponse (
    ICollection<Product> products,
    Pagination pagination
);