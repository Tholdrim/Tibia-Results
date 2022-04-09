namespace TibiaResults.Models
{
    internal class CategoryResult
    {
        private CategoryResult()
        {
        }

        public bool IsAvailable { get; init; }

        public bool IsEmpty => !Entries.Any();

        public IEnumerable<CategoryResultEntry> Entries { get; init; } = null!;

        public static CategoryResult Create(IEnumerable<CategoryResultEntry>? entries = null) => new()
        {
            IsAvailable = true,
            Entries = entries ?? Enumerable.Empty<CategoryResultEntry>()
        };

        public static CategoryResult CreateNotAvailable() => new()
        {
            IsAvailable = false,
            Entries = Enumerable.Empty<CategoryResultEntry>()
        };
    }
}
