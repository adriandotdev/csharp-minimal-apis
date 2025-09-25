public record CreateProductResponse(

     int Id,
     string Name,
     string Description,
     int CategoryId,
     decimal Price,
     decimal CostPrice
);