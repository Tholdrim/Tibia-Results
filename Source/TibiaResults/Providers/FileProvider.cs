using TibiaResults.Helpers;
using TibiaResults.Interfaces;
using TibiaResults.Models;

namespace TibiaResults.Providers
{
    internal class FileProvider : IHighscoreProvider
    {
        public FileProvider(string localPath)
        {
            LocalPath = localPath;
        }

        private string LocalPath { get; }

        public Task<Highscore?> GetHighscoreAsync(string identifier, DateOnly date) => ReadHighscoreAsync(identifier, date);

        private async Task<Highscore?> ReadHighscoreAsync(string identifier, DateOnly date)
        {
            var fileName = Path.Combine(LocalPath, identifier, $"{date:yyyy-MM-dd}.json");
            var highscoresRoot = await FileHelper.DeserializeFromFileAsync<HighscoreRoot>(fileName);

            return highscoresRoot?.Highscores;
        }
    }
}
