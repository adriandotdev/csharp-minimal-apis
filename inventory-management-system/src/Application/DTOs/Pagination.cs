public record Pagination (
    int TotalRecords,
    int CurrentPage,
    int TotalPages,
    int? NextPage,
    int? PrevPage
);
