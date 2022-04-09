using System.Text.Json.Serialization;

namespace TibiaResults.Models
{
    internal class Highscore
    {
        [JsonPropertyName("highscore_list")]
        public IEnumerable<HighscoreEntry>? HighscoreList { get; init; }
    }
}
