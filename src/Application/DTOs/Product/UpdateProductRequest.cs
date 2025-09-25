public record UpdateProductRequest(
    string? Name,
    string? Description,
    string? SKU,
    string? BarCode,
    int CategoryId,
    decimal? Price,
    decimal? CostPrice,
    string? ImageUrl
);