using System.Text.Json.Serialization;

namespace TibiaResults.Models
{
    internal class ApplicationSettings
    {
        [JsonPropertyName("blobContainerUri")]
        public string? BlobContainerUri { get; init; }

        [JsonPropertyName("localPath")]
        public string? LocalPath { get; init; }

        [JsonPropertyName("characters")]
        public IEnumerable<string>? Characters { get; init; }

        [JsonPropertyName("from")]
        public string? From { get; init; }

        [JsonPropertyName("to")]
        public string? To { get; init; }
    }
}
