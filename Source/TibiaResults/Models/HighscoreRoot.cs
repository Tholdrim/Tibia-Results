using System.Text.Json.Serialization;

namespace TibiaResults.Models
{
    internal class HighscoreRoot
    {
        [JsonPropertyName("highscores")]
        public Highscore? Highscores { get; init; }
    }
}
