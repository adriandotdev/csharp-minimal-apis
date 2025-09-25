public record CreateProductRequest(
    string Name,
    string? Description,
    string? SKU,
    string? BarCode,
    int CategoryId,
    decimal Price,
    decimal CostPrice,
    string? ImageUrl
);