namespace TibiaResults.Models
{
    internal class ApplicationSettings
    {
        public Uri? BlobContainerUri { get; init; }

        public string? LocalPath { get; init; }

        public IEnumerable<string>? Characters { get; init; }

        public DateTime? From { get; init; }

        public DateTime? To { get; init; }
    }
}
