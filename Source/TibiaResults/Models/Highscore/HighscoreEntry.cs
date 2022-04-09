using System.Text.Json.Serialization;

namespace TibiaResults.Models
{
    internal class HighscoreEntry
    {
        [JsonPropertyName("rank")]
        public int Rank { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; init; } = null!;

        [JsonPropertyName("level")]
        public int Level { get; init; }

        [JsonPropertyName("value")]
        public long Value { get; init; }
    }
}
