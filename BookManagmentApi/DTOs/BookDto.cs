namespace BookManagmentApi.DTOs
{
    public class BookDto
    {
        public required string Title { get; init; }
        public required string AuthorName { get; init; }
        public short PublicationYear { get; init; }
        public int ViewsCount { get; init; }
        public double PopularityScore { get; init; }
    }
}
